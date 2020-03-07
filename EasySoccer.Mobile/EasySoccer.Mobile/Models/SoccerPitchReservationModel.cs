using EasySoccer.Mobile.API.ApiResponses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EasySoccer.Mobile.Models
{
    public class SoccerPitchReservationModel : SoccerPitchReservationResponse
    {
        public SoccerPitchReservationModel(SoccerPitchReservationResponse item)
        {
            this.SelectedDate = item.SelectedDate;
            this.SelectedHourEnd = item.SelectedHourEnd;
            this.SelectedHourStart = item.SelectedHourStart;
            this.SoccerPitchId = item.SoccerPitchId;
            this.SoccerPitchName = item.SoccerPitchName;
            this.UserName = item.UserName;
            this.CompanyName = item.CompanyName;
            this.CompanyImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQjne-aTNHMNq_X8tpP-7G-T1OsPEsJ7GubYn8htIMP-3L0sv3z";
        }

        public SoccerPitchReservationModel()
        {

        }

        public string CompanyImage { get; set; }

        public string SelectedHourStartEnd
        {
            get
            {
                var cultureInfo = new CultureInfo("pt-BR");
                return cultureInfo.TextInfo.ToTitleCase(this.SelectedDate.ToString("ddd", cultureInfo)) + " · " +  this.SelectedDate.ToString("dd MMMM yyyy", cultureInfo) + " (" + this.SelectedHourStart.Hours.ToString("00") + ":" + this.SelectedHourStart.Minutes.ToString("00") + " - " + this.SelectedHourEnd.Hours.ToString("00") + ":" + this.SelectedHourEnd.Minutes.ToString("00") + ")";
            }
        }

    }
}
