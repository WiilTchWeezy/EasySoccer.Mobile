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

namespace EasySoccer.Mobile.ViewModels
{
    public class ScheduleAvaliableViewModel : BindableBase, INavigationAware
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

        public ObservableCollection<AvaliableSchedulesModel> AvaliableSchedules { get; set; }

        private long _companyId = 0;
        private SportTypeResponse sportType = new SportTypeResponse();
        private DateTime selectedDate = new DateTime();
        private TimeSpan selectedTime = new TimeSpan();
        private CompanyModel companyModel = new CompanyModel();
        public ScheduleAvaliableViewModel()
        {
            AvaliableSchedules = new ObservableCollection<AvaliableSchedulesModel>();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CurrentCompany"))
            {
                var jsonObject = parameters.GetValue<string>("CurrentCompany");
                var selectedSoccerPitch = JsonConvert.DeserializeObject<CompanyModel>(jsonObject);
                if (selectedSoccerPitch != null)
                {
                    _companyId = selectedSoccerPitch.Id;
                    Name = selectedSoccerPitch.Name;
                    Image = selectedSoccerPitch.Image;
                    City = selectedSoccerPitch.City;
                    CompleteAddress = selectedSoccerPitch.CompleteAddress;
                }
            }
            if (parameters.ContainsKey("SelectedSportType"))
            {
                var jsonObject = parameters.GetValue<string>("SelectedSportType");
                var selectedSportType = JsonConvert.DeserializeObject<SportTypeResponse>(jsonObject);
                if (selectedSportType != null)
                {
                    this.sportType = selectedSportType;
                }
            }
            if (parameters.ContainsKey("SelectedDate"))
                this.selectedDate = parameters.GetValue<DateTime>("SelectedDate");
            if (parameters.ContainsKey("SelectedTime"))
                this.selectedTime = parameters.GetValue<TimeSpan>("SelectedTime");
            if (parameters.ContainsKey("CurrentCompany"))
            {
                var jsonObject = parameters.GetValue<string>("CurrentCompany");
                var selectedCompany = JsonConvert.DeserializeObject<CompanyModel>(jsonObject);
                if (selectedCompany != null)
                {
                    this.companyModel = selectedCompany;
                }
            }
            this.LoadAvaliableSchedules();
        }

        private async Task LoadAvaliableSchedules()
        {
            try
            {
                var selectedDateString = this.selectedDate.ToString("yyyy-MM-dd");
                selectedDateString += " " + this.selectedTime.ToString();
                var avaliableSchedulesResponse = await ApiClient.Instance.GetAvaliableSchedulesAsync(this.companyModel.Id, selectedDateString, this.sportType.Id);
                if (avaliableSchedulesResponse != null && avaliableSchedulesResponse.Count > 0)
                {
                    foreach (var item in avaliableSchedulesResponse)
                    {
                        AvaliableSchedules.Add(new AvaliableSchedulesModel(item, this.companyModel.Id));
                    }
                }
            }
            catch (Exception E)
            {

            }
        }
    }
}
