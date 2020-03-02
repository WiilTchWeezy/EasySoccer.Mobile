using Acr.UserDialogs;
using EasySoccer.Mobile.Infra;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasySoccer.Mobile.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        public DelegateCommand<string> NavigatePageCommand { get; set; }
        private INavigationService _navigationService;
        public MainPageViewModel(INavigationService navigationService)
        {
            NavigatePageCommand = new DelegateCommand<string>(NavigatePage);
            _navigationService = navigationService;
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
            else
                _navigationService.NavigateAsync("NavigationPage/" + page);
        }
    }
}
