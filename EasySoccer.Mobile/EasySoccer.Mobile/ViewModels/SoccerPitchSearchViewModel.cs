using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.API.ApiResponses;
using EasySoccer.Mobile.API.Infra.Exceptions;
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
        public ObservableCollection<CompanyResponse> SoccerPitchs { get; set; }
        public SoccerPitchSearchViewModel()
        {
            SoccerPitchs = new ObservableCollection<CompanyResponse>();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var companiesResponse = await ApiClient.Instance.GetCompaniesAsync();
                if (companiesResponse != null && companiesResponse.Count > 0)
                {
                    foreach (var item in companiesResponse)
                    {
                        item.Image = "https://img.stpu.com.br/?img=https://s3.amazonaws.com/pu-mgr/default/a0R0f00000vgXC1EAM/591de07de4b021f908345636.jpg&w=710&h=462";
                        SoccerPitchs.Add(item);
                    }
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
            LoadDataAsync();
        }
    }
}
