using Acr.UserDialogs;
using EasySoccer.Common.Events;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.Infra.Facebook;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using Xamarin.Essentials;

namespace EasySoccer.Mobile.ViewModels
{
    public class LoginViewModel : BindableBase, INavigationAware
    {

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;

        public DelegateCommand FacebookLoginCommand { get; set; }
        public DelegateCommand RegisterCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public LoginViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            FacebookLoginCommand = new DelegateCommand(FacebookLogin);
            RegisterCommand = new DelegateCommand(Register);
            LoginCommand = new DelegateCommand(Login);
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
        }

        private async void FacebookLogin()
        {
            try
            {
                var facebookLoginResponse = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name", "birthday" }, new string[] { "email", "user_birthday" });
                if (facebookLoginResponse.Status == FacebookActionStatus.Completed)
                {
                    var facebookResponseData = JsonConvert.DeserializeObject<FacebookResponseData>(facebookLoginResponse.Data);
                    var loginResponse = await ApiClient.Instance.LoginFromFacebook(facebookResponseData.email, facebookResponseData.first_name, facebookResponseData.last_name, facebookResponseData.birthday, facebookResponseData.id);
                    if (loginResponse != null && string.IsNullOrEmpty(loginResponse.Token) == false)
                    {
                        var fcmToken = Preferences.Get("FcmToken", string.Empty);
                        if (Preferences.ContainsKey("FcmToken") && string.IsNullOrEmpty(fcmToken) == false)
                        {
                            var token = await ApiClient.Instance.InserTokenAsync(fcmToken);
                        }
                        _eventAggregator.GetEvent<UserLoggedInEvent>().Publish(true);
                        await _navigationService.GoBackAsync();
                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        private void Register()
        {
            _navigationService.NavigateAsync("RegisterUser", useModalNavigation: true);
        }

        private async void Login()
        {
            try
            {
                var loginResponse = await ApiClient.Instance.LoginAsync(Email, Password);
                if (loginResponse != null)
                {
                    _eventAggregator.GetEvent<UserLoggedInEvent>().Publish(true);
                    var fcmToken = Preferences.Get("FcmToken", string.Empty);
                    if (Preferences.ContainsKey("FcmToken") && string.IsNullOrEmpty(fcmToken) == false)
                    {
                        var token = await ApiClient.Instance.InserTokenAsync( fcmToken );
                    }
                    await _navigationService.GoBackAsync();
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Email"))
            {
                Email = parameters.GetValue<string>("Email");
            }
            if (parameters.ContainsKey("Password"))
            {
                Password = parameters.GetValue<string>("Password");
            }
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
                Login();
        }
    }
}
