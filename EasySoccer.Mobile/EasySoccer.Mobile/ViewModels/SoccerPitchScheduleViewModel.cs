using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.API.ApiResponses;
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
using System.Linq;

namespace EasySoccer.Mobile.ViewModels
{
    public class SoccerPitchScheduleViewModel : BindableBase, INavigationAware
    {
        public ObservableCollection<CompanySchedulesModel> CompanySchedules { get; set; }

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

        private DateTime _minimumDate;
        public DateTime MinimumDate
        {
            get { return _minimumDate; }
            set { SetProperty(ref _minimumDate, value); }
        }

        private string _image;
        public string Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }

        private string _completeAddress;
        public string CompleteAddress
        {
            get { return _completeAddress; }
            set { SetProperty(ref _completeAddress, value); }
        }

        private int _companyId = 0;
        private INavigationService _navigationService;
        private CompanyModel _currentCompany;
        public DelegateCommand ScheduleHour { get; set; }
        public SoccerPitchScheduleViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            CompanySchedules = new ObservableCollection<CompanySchedulesModel>();
            SelectedDate = DateTime.Now;
            MinimumDate = DateTime.Now;
            ScheduleHour = new DelegateCommand(OpenReservationConfirmation);
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
                        CompanySchedules.Add(new CompanySchedulesModel(item, _navigationService, Image, City, CompleteAddress, _companyId, SelectedDate, Name));
                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        public void OpenReservationConfirmation()
        {
            _navigationService.NavigateAsync("ReservationConfirmation");
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
            if(parameters.ContainsKey("CurrentCompany"))
            {
                var jsonObject = parameters.GetValue<string>("CurrentCompany");
                var currentCompany = JsonConvert.DeserializeObject<CompanyModel>(jsonObject);
                if(currentCompany != null)
                {
                    _currentCompany = currentCompany;
                    Image = _currentCompany.Image;
                    City = _currentCompany.City;
                    CompleteAddress = _currentCompany.CompleteAddress;
                    Name = _currentCompany.Name;
                }
            }
            LoadDataAsync();
        }
    }
}
