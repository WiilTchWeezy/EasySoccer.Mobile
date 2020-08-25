using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasySoccer.Mobile.ViewModels
{
    public class MySchedulesViewModel : BindableBase, INavigationAware
    {
        public ObservableCollection<SoccerPitchReservationModel> Schedules { get; set; }
        public MySchedulesViewModel()
        {
            Schedules = new ObservableCollection<SoccerPitchReservationModel>();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadSchedulesAsync();
        }

        private async void LoadSchedulesAsync()
        {
            try
            {
                var response = await ApiClient.Instance.GetUserSchedulesAsync();
                if(response != null && response.Count > 0)
                {
                    Schedules.Clear();
                    foreach (var item in response)
                    {
                        Schedules.Add(new SoccerPitchReservationModel(item));
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
