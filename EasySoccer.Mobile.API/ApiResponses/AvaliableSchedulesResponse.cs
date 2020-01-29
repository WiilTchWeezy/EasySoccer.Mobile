using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.Mobile.API.ApiResponses
{
    public class AvaliableSchedulesResponse
    {
        public List<SoccerPitchResponse> PossibleSoccerPitchs { get; set; }

        public DateTime SelectedDate { get; set; }

        public TimeSpan SelectedHourStart { get; set; }

        public TimeSpan SelectedHourEnd { get; set; }

        public bool IsCurrentSchedule { get; set; }
    }
}
