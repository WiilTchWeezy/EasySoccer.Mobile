using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasySoccer.Mobile.ViewModels
{
    public class RegisterUserViewModel : BindableBase
    {

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
        }

        public DelegateCommand RegisterCommand { get; set; }
        public DelegateCommand BackCommand { get; set; }

        private INavigationService _navigationService;
        public RegisterUserViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            RegisterCommand = new DelegateCommand(Register);
            BackCommand = new DelegateCommand(Back);
        }

        private async void Register()
        {
            try
            {
                var validate = Validate();
                if (string.IsNullOrEmpty(validate))
                {
                    var registerResponse = await ApiClient.Instance.CreateUserAsync(Name, Phone, null, Email, Password);
                    if (registerResponse != null)
                    {
                        UserDialogs.Instance.Alert("Usuário cadastrado com sucesso!");
                        var navigationParameters = new NavigationParameters();
                        navigationParameters.Add("Email", Email);
                        navigationParameters.Add("Password", Password);
                        await _navigationService.NavigateAsync("Login", navigationParameters);
                    }
                }
                else
                {
                    UserDialogs.Instance.Alert("Campos inválidos \n" + validate);
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        private string Validate()
        {
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(Name))
                sb.AppendLine("Nome");
            if (string.IsNullOrEmpty(Email))
                sb.AppendLine("Email");
            if (string.IsNullOrEmpty(Password))
                sb.AppendLine("Senha");
            if (!string.IsNullOrEmpty(Password) && !Password.Equals(ConfirmPassword))
            {
                sb.AppendLine("Senha e a confirmação da senha estão diferentes.");
            }
            return sb.ToString();
        }

        private void Back()
        {
            _navigationService.GoBackAsync();
        }
    }
}
