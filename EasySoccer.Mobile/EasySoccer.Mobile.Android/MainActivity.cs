using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.API.Session;
using EasySoccer.Mobile.Droid.Services;
using Plugin.FacebookClient;
using Plugin.FirebasePushNotification;
using Prism;
using Prism.Ioc;
using System;

namespace EasySoccer.Mobile.Droid
{
    [Activity(Label = "EasySoccer", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.Forms.FormsMaterial.Init(this, bundle);
            FacebookClientManager.Initialize(this);
            UserDialogs.Init(this);
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                FirebasePushNotificationManager.DefaultNotificationChannelId = "EasySoccerNotificationChannel";
                FirebasePushNotificationManager.DefaultNotificationChannelName = "Geral";
            }

#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
              FirebasePushNotificationManager.Initialize(this,false);
#endif

            //Handle notification when app is closed here
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                var notificationService = new LocalNotificationService();
                object messageObj = string.Empty;
                object titleObj = string.Empty;
                if (p.Data.ContainsKey("message"))
                    p.Data.TryGetValue("message", out messageObj);
                if (p.Data.ContainsKey("title"))
                    p.Data.TryGetValue("title", out titleObj);
                var message = messageObj.ToString();
                var title = titleObj.ToString();

                if (string.IsNullOrEmpty(message) == false && string.IsNullOrEmpty(title) == false)
                {
                    notificationService.SendLocalNotification(this, title, message);
                }
            };

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                if (Xamarin.Essentials.Preferences.ContainsKey("FcmToken") && string.IsNullOrEmpty(Xamarin.Essentials.Preferences.Get("FcmToken", string.Empty)) == false)
                {
                    Xamarin.Essentials.Preferences.Remove("FcmToken");
                }
                Xamarin.Essentials.Preferences.Set("FcmToken", p.Token);
                if (CurrentUser.Instance.IsLoggedIn)
                {
                    try
                    {
                        ApiClient.Instance.InserTokenAsync( p.Token).GetAwaiter().GetResult();
                    }
                    catch (Exception)
                    {

                    }
                }
            };
            LoadApplication(new App(new AndroidInitializer()));
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);
            FacebookClientManager.OnActivityResult(requestCode, resultCode, intent);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

