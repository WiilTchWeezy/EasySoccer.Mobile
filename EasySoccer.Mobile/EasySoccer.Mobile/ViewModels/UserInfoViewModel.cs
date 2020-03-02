using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.Mobile.ViewModels
{
    public class UserInfoViewModel : BindableBase, INavigationAware
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        public UserInfoViewModel()
        {

        }

        private async Task LoadUserInfo()
        {
            try
            {
                var userResponse = await ApiClient.Instance.GetUserInfoAsync();
                if(userResponse != null)
                {
                    Phone = userResponse.Phone;
                    Email = userResponse.Email;
                    Name = userResponse.Name;
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
            LoadUserInfo();
        }
    }
}
