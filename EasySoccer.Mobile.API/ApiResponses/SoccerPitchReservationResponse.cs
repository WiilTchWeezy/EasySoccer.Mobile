using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.Mobile.API.ApiResponses
{
    public class SoccerPitchReservationResponse
    {
        public Guid Id { get; set; }

        public long SoccerPitchId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime SelectedDate { get; set; }

        public TimeSpan SelectedHourStart { get; set; }

        public TimeSpan SelectedHourEnd { get; set; }

        public int Status { get; set; }

        public long? StatusChangedUserId { get; set; }

        public string Note { get; set; }

        public long SoccerPitchSoccerPitchPlanId { get; set; }

        public Guid? OringinReservationId { get; set; }

        public string SoccerPitchName { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }

        public string Logo { get; set; }
    }
}
