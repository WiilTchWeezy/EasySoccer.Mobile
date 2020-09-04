using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.API.ApiResponses;
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
    public class SoccerPitchScheduleViewModel : BindableBase, INavigationAware
    {
        public ObservableCollection<CompanySchedules> CompanySchedules { get; set; }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (SetProperty(ref _selectedDate, value))
                    LoadDataAsync();
            }
        }

        private int _companyId = 0;
        public SoccerPitchScheduleViewModel()
        {
            CompanySchedules = new ObservableCollection<CompanySchedules>();
            SelectedDate = DateTime.Now;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var response = await ApiClient.Instance.GetCompanyReservationSchedulesAsync(_companyId, SelectedDate);
                if (response != null && response.Count > 0)
                {
                    CompanySchedules.Clear();
                    foreach (var item in response)
                    {
                        CompanySchedules.Add(item);
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
            if (parameters.ContainsKey("CompanyId"))
            {
                _companyId = parameters.GetValue<int>("CompanyId");
            }
            LoadDataAsync();
        }
    }
}
