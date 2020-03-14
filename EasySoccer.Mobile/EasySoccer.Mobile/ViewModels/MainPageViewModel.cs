using Acr.UserDialogs;
using EasySoccer.Common.Events;
using EasySoccer.Mobile.API.Session;
using EasySoccer.Mobile.Infra;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;

namespace EasySoccer.Mobile.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        public DelegateCommand<string> NavigatePageCommand { get; set; }
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;
        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            NavigatePageCommand = new DelegateCommand<string>(NavigatePage);
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UserLoggedInEvent>().Subscribe(UserHasLoggedIn);
            UserLoggedIn = CurrentUser.Instance.IsLoggedIn;
            LoginLogoutText = UserLoggedIn ? "Sair" : "Fazer Login";
            LoginLogoutParameter = UserLoggedIn ? "Logout" : "Login";
            CurrentUser.Instance.SetEventAggregator(_eventAggregator);
        }

        private void NavigatePage(string page)
        {
            if (page == "Logout")
            {
                var confirmConfig = new ConfirmConfig()
                {
                    CancelText = "Cancelar",
                    Message = "Deseja realmente sair?",
                    OkText = "Sair",
                    Title = "EasySoccer",
                    OnAction = (selection) =>
                    {
                        if (selection)
                            Application.Instance.LogOff(_navigationService);
                    }
                };
                UserDialogs.Instance.Confirm(confirmConfig);
            }
            else if (page == "Login")
            {
                _navigationService.NavigateAsync("Login", useModalNavigation: true);
            }
            else
                _navigationService.NavigateAsync("NavigationPage/" + page);
        }

        private void UserHasLoggedIn(bool payLoad)
        {
            this.UserLoggedIn = payLoad;
            LoginLogoutText = UserLoggedIn ? "Sair" : "Fazer Login";
            LoginLogoutParameter = UserLoggedIn ? "Logout" : "Login";
        }

        private bool _userLoggedIn;
        public bool UserLoggedIn
        {
            get { return _userLoggedIn; }
            set { SetProperty(ref _userLoggedIn, value); }
        }

        private string _loginLogoutText;
        public string LoginLogoutText
        {
            get { return _loginLogoutText; }
            set { SetProperty(ref _loginLogoutText, value); }
        }

        private string _loginLogoutParameter;
        public string LoginLogoutParameter
        {
            get { return _loginLogoutParameter; }
            set { SetProperty(ref _loginLogoutParameter, value); }
        }
    }
}
