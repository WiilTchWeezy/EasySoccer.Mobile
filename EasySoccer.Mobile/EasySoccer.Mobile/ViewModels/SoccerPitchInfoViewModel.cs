using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.API.ApiResponses;
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
    public class SoccerPitchInfoViewModel : BindableBase, INavigationAware
    {

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _image;
        public string Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
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

        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    this.LoadCompanySchedulesAsync(this._selectedDate);
                }
            }
        }

        private TimeSpan _selectedTime;
        public TimeSpan SelectedTime
        {
            get { return _selectedTime; }
            set { SetProperty(ref _selectedTime, value); }
        }

        private int? _selectedSportTypeIndex = null;
        public int? SelectedSportTypeIndex
        {
            get { return _selectedSportTypeIndex; }
            set { SetProperty(ref _selectedSportTypeIndex, value); }
        }

        private int? _selectedIndexTime = null;
        public int? SelectedIndexTime
        {
            get { return _selectedIndexTime; }
            set { SetProperty(ref _selectedIndexTime, value); }
        }

        private DateTime _minimunDate = DateTime.Now;
        public DateTime MinimunDate
        {
            get { return _minimunDate; }
            set { SetProperty(ref _minimunDate, value); }
        }

        private long _companyId = 0;
        private CompanyModel _currentCompany;

        public ObservableCollection<SoccerPitchResponse> SoccerPitchs { get; set; }
        public List<SportTypeResponse> SportTypes { get; set; }
        public ObservableCollection<string> SportTypesNames { get; set; }

        public ObservableCollection<string> CompanySchedules { get; set; }

        public DelegateCommand CheckScheduleAvaliableCommand { get; set; }

        private INavigationService _navigationService;
        public SoccerPitchInfoViewModel(INavigationService navigationService)
        {
            SoccerPitchs = new ObservableCollection<SoccerPitchResponse>();
            SportTypes = new List<SportTypeResponse>();
            SportTypesNames = new ObservableCollection<string>();
            CompanySchedules = new ObservableCollection<string>();
            CheckScheduleAvaliableCommand = new DelegateCommand(CheckScheduleAvaliable);
            _navigationService = navigationService;
        }

        private async Task LoadSoccerPitchsAsync()
        {
            SoccerPitchs.Clear();
            try
            {
                var response = await ApiClient.Instance.GetSoccerPitchesAsync(_companyId);
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        item.Image = Application.Instance.GetImage(item.ImageName, Infra.Enums.BlobContainerEnum.SoccerPitch);
                        SoccerPitchs.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        private async Task LoadSportTypesAsync()
        {
            SportTypes.Clear();
            try
            {
                var response = await ApiClient.Instance.GetSportTypesAsync(_companyId);
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        SportTypesNames.Add(item.Name);
                        SportTypes.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        private async Task LoadCompanySchedulesAsync(DateTime selectedDate)
        {
            try
            {
                CompanySchedules.Clear();
                var response = await ApiClient.Instance.GetCompanySchedulesAsync((int)_companyId, (int)selectedDate.DayOfWeek);
                if (response != null && response.Data != null && response.Data.Count > 0)
                {
                    foreach (var item in response.Data)
                    {
                        if (selectedDate.Date == DateTime.Now.Date)
                        {
                            TimeSpan companyHour = TimeSpan.Zero;
                            if (TimeSpan.TryParse(item, out companyHour))
                            {
                                if (companyHour > DateTime.Now.TimeOfDay)
                                    CompanySchedules.Add(item);
                            }
                        }
                        else
                            CompanySchedules.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        private void CheckScheduleAvaliable()
        {
            if (SelectedSportTypeIndex.HasValue && SelectedIndexTime.HasValue)
            {
                try
                {
                    var navigationParameters = new NavigationParameters();
                    navigationParameters.Add(nameof(SelectedDate), SelectedDate);
                    navigationParameters.Add("SelectedTime", CompanySchedules[SelectedIndexTime.Value]);
                    navigationParameters.Add("SelectedSportType", JsonConvert.SerializeObject(SportTypes[SelectedSportTypeIndex.Value]));
                    navigationParameters.Add("CurrentCompany", JsonConvert.SerializeObject(_currentCompany));
                    _navigationService.NavigateAsync("ScheduleAvaliable", navigationParameters);
                }
                catch (Exception e)
                {

                }
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.Keys != null && parameters.Keys.Contains("selectedSoccerPitch"))
            {
                var jsonObject = parameters.GetValue<string>("selectedSoccerPitch");
                var selectedSoccerPitch = JsonConvert.DeserializeObject<CompanyModel>(jsonObject);
                if (selectedSoccerPitch != null)
                {
                    _currentCompany = selectedSoccerPitch;
                    _companyId = selectedSoccerPitch.Id;
                    Name = selectedSoccerPitch.Name;
                    Image = selectedSoccerPitch.Image;
                    City = selectedSoccerPitch.City;
                    CompleteAddress = selectedSoccerPitch.CompleteAddress;
                    LoadSoccerPitchsAsync();
                }
            }
            LoadCompanySchedulesAsync(this.SelectedDate);
            LoadSportTypesAsync();
        }
    }
}
