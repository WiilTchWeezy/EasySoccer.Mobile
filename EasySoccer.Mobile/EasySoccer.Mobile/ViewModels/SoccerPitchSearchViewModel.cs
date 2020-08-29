using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.API.ApiResponses;
using EasySoccer.Mobile.API.Infra.Exceptions;
using EasySoccer.Mobile.Infra;
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

        public DelegateCommand FilterCommand { get; set; }

        private INavigationService _navigationService;
        public SoccerPitchSearchViewModel(INavigationService navigationService)
        {
            SoccerPitchs = new ObservableCollection<CompanyModel>();
            SelectSoccerPicthCommand = new DelegateCommand<CompanyModel>(SelectSoccerPicth);
            FilterCommand = new DelegateCommand(FilterAsync);
            _navigationService = navigationService;
        }

        private async Task LoadDataAsync(string filterText, string orderField, string orderDirection)
        {
            try
            {
                SoccerPitchs.Clear();
                var companiesResponse = await ApiClient.Instance.GetCompaniesAsync(filterText, orderField, orderDirection);
                if (companiesResponse != null && companiesResponse.Count > 0)
                {
                    foreach (var item in companiesResponse)
                    {
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

        public void FilterAsync()
        {
            _navigationService.NavigateAsync("SoccerPitchFilter");
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            string orderField = string.Empty;
            string orderDirection = string.Empty;
            string filterText = string.Empty;
            if (parameters.ContainsKey("FilterText"))
                filterText = parameters.GetValue<string>("FilterText");
            if (parameters.ContainsKey("OrderField"))
                orderField = parameters.GetValue<string>("OrderField");
            if (parameters.ContainsKey("OrderDirection"))
                orderDirection = parameters.GetValue<string>("OrderDirection");
            LoadDataAsync(filterText, orderField, orderDirection);
        }
    }
}
