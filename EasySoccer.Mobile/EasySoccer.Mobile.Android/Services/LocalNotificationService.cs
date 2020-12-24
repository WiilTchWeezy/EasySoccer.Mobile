
using Android.App;
using Android.Content;
using Android.Support.V4.App;

namespace EasySoccer.Mobile.Droid.Services
{
    public class LocalNotificationService
    {
        private const string CHANNEL_ID = "";

        public void SendLocalNotification(Context context, string title, string message)
        {
            Intent intent = new Intent(context, typeof(MainActivity));
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(context, pendingIntentId, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, CHANNEL_ID)
                                                    .SetContentTitle(title)
                                                    .SetContentText(message)
                                                    .SetDefaults((int)NotificationDefaults.Sound)
                                                    .SetSmallIcon(Resource.Drawable.ic_launcher);
            Notification notification = builder.Build();

            NotificationManager notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }
    }
}