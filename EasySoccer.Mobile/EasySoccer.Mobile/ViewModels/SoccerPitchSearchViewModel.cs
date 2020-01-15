using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.API.ApiResponses;
using EasySoccer.Mobile.API.Infra.Exceptions;
using EasySoccer.Mobile.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.Mobile.ViewModels
{
    public class SoccerPitchSearchViewModel : BindableBase, INavigationAware
    {
        public ObservableCollection<CompanyModel> SoccerPitchs { get; set; }

        public DelegateCommand<CompanyModel> SelectSoccerPicthCommand { get; set; }

        private INavigationService _navigationService;
        public SoccerPitchSearchViewModel(INavigationService navigationService)
        {
            SoccerPitchs = new ObservableCollection<CompanyModel>();
            SelectSoccerPicthCommand = new DelegateCommand<CompanyModel>(SelectSoccerPicth);
            _navigationService = navigationService;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                SoccerPitchs.Clear();
                var companiesResponse = await ApiClient.Instance.GetCompaniesAsync();
                if (companiesResponse != null && companiesResponse.Count > 0)
                {
                    foreach (var item in companiesResponse)
                    {
                        item.Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQjne-aTNHMNq_X8tpP-7G-T1OsPEsJ7GubYn8htIMP-3L0sv3z";
                        var companyModel = new CompanyModel(item);
                        companyModel.SelectSoccerPicthCommand = this.SelectSoccerPicthCommand;
                        SoccerPitchs.Add(companyModel);
                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        private void SelectSoccerPicth(CompanyModel selectedSoccerPitch)
        {
            var navigationParameters = new NavigationParameters();
            selectedSoccerPitch.SelectSoccerPicthCommand = null;
            navigationParameters.Add("selectedSoccerPitch", JsonConvert.SerializeObject(selectedSoccerPitch));
            _navigationService.NavigateAsync("SoccerPitchInfo", navigationParameters);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadDataAsync();
        }
    }
}
