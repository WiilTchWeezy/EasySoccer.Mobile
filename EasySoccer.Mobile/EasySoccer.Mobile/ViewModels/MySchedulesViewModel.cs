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
        private INavigationService _navigationService;

        public DelegateCommand ItemTresholdCommand { get; set; }
        public MySchedulesViewModel(INavigationService navigationService)
        {
            Schedules = new ObservableCollection<SoccerPitchReservationModel>();
            _navigationService = navigationService;
            ItemTresholdCommand = new DelegateCommand(ItemTreshold);
        }

        private void ItemTreshold()
        {
            if(Schedules.Count > 0 && _hasMoreData)
            {
                LoadSchedulesAsync(false, _page, _pageSize);
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadSchedulesAsync();
        }

        private bool _isBusy = false;
        private int _page = 1;
        private int _pageSize = 10;
        private bool _hasMoreData = true;
        private async void LoadSchedulesAsync(bool clear = true, int page = 1, int pageSize = 10)
        {
            try
            {
                if (_isBusy == false)
                {
                    _isBusy = true;
                    var response = await ApiClient.Instance.GetUserSchedulesAsync(page, pageSize);
                    if (response != null && response.Count > 0)
                    {
                        _page++;
                        if (clear)
                            Schedules.Clear();
                        foreach (var item in response)
                        {
                            Schedules.Add(new SoccerPitchReservationModel(item, _navigationService));
                        }
                    }
                    else
                    {
                        _hasMoreData = false;
                    }
                    _isBusy = false;
                }
            }
            catch (Exception e)
            {
                _isBusy = false;
                UserDialogs.Instance.Alert(e.Message);
            }
        }
    }
}
