using EasySoccer.Mobile.API.ApiResponses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.Mobile.Models
{
    public class AvaliableSchedulesModel : AvaliableSchedulesResponse
    {
        public AvaliableSchedulesModel()
        {

        }

        public AvaliableSchedulesModel(AvaliableSchedulesResponse item)
        {
            this.PossibleSoccerPitchs = item.PossibleSoccerPitchs;
            this.SelectedDate = item.SelectedDate;
            this.SelectedHourEnd = item.SelectedHourEnd;
            this.SelectedHourStart = item.SelectedHourStart;
            this.IsCurrentSchedule = item.IsCurrentSchedule;
        }

        public string SelectedHourStartEnd
        {
            get
            {
                return this.SelectedHourStart.Hours.ToString() + ":" + this.SelectedHourStart.Minutes.ToString() + " - " + this.SelectedHourEnd.Hours.ToString() + ":" + this.SelectedHourEnd.Minutes.ToString();
            }

        }
    }
}
