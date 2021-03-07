using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasySoccer.Mobile.ViewModels
{
    public class IndicateCompanyViewModel : BindableBase
    {
        public DelegateCommand SaveCommand { get; set; }
        private INavigationService _navigationService;
        public IndicateCompanyViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SaveCommand = new DelegateCommand(Save);
        }

        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set { SetProperty(ref _companyName, value); }
        }

        private string _companyPhone;
        public string CompanyPhone
        {
            get { return _companyPhone; }
            set { SetProperty(ref _companyPhone, value); }
        }

        private string _companyEmail;
        public string CompanyEmail
        {
            get { return _companyEmail; }
            set { SetProperty(ref _companyEmail, value); }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        private async void Save()
        {
            try
            {
                var response = await ApiClient.Instance.IndicateCompanyAsync(CompanyName, CompanyPhone, CompanyEmail, Comment);
                UserDialogs.Instance.Alert("Em breve entraremos em contato. Muito Obrigado!");
                await _navigationService.NavigateAsync("SoccerPitchSearch");
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }
    }
}
