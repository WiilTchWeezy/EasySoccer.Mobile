using EasySoccer.Mobile.API.ApiResponses;
using Prism.Commands;
using Prism.Navigation;
using System;

namespace EasySoccer.Mobile.Models
{
    public class CompanySchedulesEventModel : CompanySchedulesEvent
    {
        private INavigationService _navigationService;
        private string _companyImage;
        private long _companyId;
        private string _city;
        private string _completeAddress;
        private string _companyName;
        private DateTime _selectedDate;
        public CompanySchedulesEventModel(CompanySchedulesEvent companySchedulesEvent, INavigationService navigationService, string companyImage, string city, string completeAddress, long companyId, DateTime selectedDate, string companyName)
        {
            _navigationService = navigationService;
            _companyId = companyId;
            _companyImage = companyImage;
            _city = city;
            _completeAddress = completeAddress;
            _selectedDate = selectedDate;
            _companyName = companyName;
            this.HasReservation = companySchedulesEvent.HasReservation;
            this.PersonName = companySchedulesEvent.PersonName;
            this.ScheduleHour = companySchedulesEvent.ScheduleHour;
            this.SoccerPitch = companySchedulesEvent.SoccerPitch;
            this.SoccerPitchId = companySchedulesEvent.SoccerPitchId;
            this.ScheduleHourCommand = new DelegateCommand(OpenReservationConfirmation);
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
            navigationParameters.Add("SoccerPitchId", this.SoccerPitchId);
            var currentHourArray = this.ScheduleHour.Replace(" ", string.Empty).Split('-');
            if (currentHourArray.Length > 0) {
                navigationParameters.Add("CurrentHour", currentHourArray[0]);
            }
            _navigationService.NavigateAsync("ReservationConfirmation", navigationParameters);
        }
    }
}
