using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.Mobile.API.ApiResponses
{
    public class CompanySchedulesEvent 
    {
        public string ScheduleHour { get; set; }
        public string SoccerPitch { get; set; }
        public string PersonName { get; set; }
        public bool HasReservation { get; set; }
        public long SoccerPitchId { get; set; }

        public bool NotHaveReservation { get { return !HasReservation; }  }

        public DelegateCommand ScheduleHourCommand { get; set; }
    }
}
