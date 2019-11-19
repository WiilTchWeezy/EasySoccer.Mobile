using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.Infra.Facebook;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.Mobile.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private INavigationService _navigationService;
        public DelegateCommand FacebookLoginCommand { get; set; }
        public LoginViewModel(INavigationService navigationService)
        {
            FacebookLoginCommand = new DelegateCommand(FacebookLogin);
            _navigationService = navigationService;
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
                    if(loginResponse != null && string.IsNullOrEmpty(loginResponse.Token) == false)
                    {
                        await _navigationService.NavigateAsync("/NavigationPage/SoccerPitchSearch");
                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }
    }
}
