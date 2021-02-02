using EasySoccer.Mobile.API.ApiResponses;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EasySoccer.Mobile.Models
{
    public class CompanySchedulesModel : CompanySchedules
    {
        public ObservableCollection<CompanySchedulesEventModel> EventsModel { get; set; }
        private INavigationService _navigationService;

        private string _companyImage;
        private long _companyId;
        private string _city;
        private string _completeAddress;
        private string _companyName;
        private DateTime _selectedDate;
        public CompanySchedulesModel(CompanySchedules companySchedules, INavigationService navigationService, string companyImage, string city, string completeAddress, long companyId, DateTime selectedDate, string companyName)
        {
            EventsModel = new ObservableCollection<CompanySchedulesEventModel>();
            this.Hour = companySchedules.Hour;
            this.HourSpan = companySchedules.HourSpan;
            this.ScheduleHourCommand = new DelegateCommand(OpenReservationConfirmation);
            _navigationService = navigationService;
            _companyId = companyId;
            _companyImage = companyImage;
            _city = city;
            _completeAddress = completeAddress;
            _selectedDate = selectedDate;
            _companyName = companyName;
            foreach (var e in companySchedules.Events)
            {
                EventsModel.Add(new CompanySchedulesEventModel(e, navigationService, companyImage, city, completeAddress, companyId, selectedDate, companyName));
            }
        }

        private void OpenReservationConfirmation()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("City", _city);
            navigationParameters.Add("Name", _companyName);
            navigationParameters.Add("CompleteAddress", _completeAddress);
            navigationParameters.Add("Image", _companyImage);
            navigationParameters.Add("CompanyId", _companyId);
            navigationParameters.Add("SelectedDate", _selectedDate);
            navigationParameters.Add("CurrentHour", this.Hour);
            _navigationService.NavigateAsync("ReservationConfirmation", navigationParameters);
        }
    }
}
