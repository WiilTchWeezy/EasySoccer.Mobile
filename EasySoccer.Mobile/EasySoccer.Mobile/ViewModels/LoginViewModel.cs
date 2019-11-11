using Acr.UserDialogs;
using EasySoccer.Mobile.Infra.Facebook;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.Mobile.ViewModels
{
	public class LoginViewModel : BindableBase
	{
        public DelegateCommand FacebookLoginCommand { get; set; }
        public LoginViewModel()
        {
            FacebookLoginCommand = new DelegateCommand(FacebookLogin);
        }

        private async void FacebookLogin()
        {
            var facebookLoginResponse = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name", "birthday" }, new string[] { "email", "user_birthday" });
            if(facebookLoginResponse.Status == FacebookActionStatus.Completed)
            {
                var facebookResponseData = JsonConvert.DeserializeObject<FacebookResponseData>(facebookLoginResponse.Data);
                UserDialogs.Instance.Alert($"Olá {facebookResponseData.first_name} {facebookResponseData.last_name}.");
            }
        }
	}
}
