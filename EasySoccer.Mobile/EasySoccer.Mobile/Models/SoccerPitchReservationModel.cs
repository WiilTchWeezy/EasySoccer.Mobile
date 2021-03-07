using EasySoccer.Mobile.API.ApiResponses;
using EasySoccer.Mobile.Infra;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EasySoccer.Mobile.Models
{
    public class SoccerPitchReservationModel : SoccerPitchReservationResponse
    {
        public DelegateCommand OpenReservationInfoCommand { get; set; }
        private INavigationService _navigationService;
        public SoccerPitchReservationModel(SoccerPitchReservationResponse item, INavigationService navigationService)
        {
            this.Id = item.Id;
            this.SelectedDate = item.SelectedDate;
            this.SelectedHourEnd = item.SelectedHourEnd;
            this.SelectedHourStart = item.SelectedHourStart;
            this.SoccerPitchId = item.SoccerPitchId;
            this.SoccerPitchName = item.SoccerPitchName;
            this.UserName = item.UserName;
            this.CompanyName = item.CompanyName;
            this.Status = item.Status;
            this.StatusDescription = item.StatusDescription;
            this.CompanyImage = Application.Instance.GetImage(item.Logo, Infra.Enums.BlobContainerEnum.Company);
            OpenReservationInfoCommand = new DelegateCommand(OpenReservationInfo);
            _navigationService = navigationService;
        }

        public SoccerPitchReservationModel(INavigationService navigationService)
        {
            OpenReservationInfoCommand = new DelegateCommand(OpenReservationInfo);
            _navigationService = navigationService;
        }
        private void OpenReservationInfo()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("ReservationId", this.Id);
            _navigationService.NavigateAsync("ReservationInfo", navigationParameters);
        }

        public string CompanyImage { get; set; }

        public string SelectedHourStartEnd
        {
            get
            {
                var cultureInfo = new CultureInfo("pt-BR");
                return cultureInfo.TextInfo.ToTitleCase(this.SelectedDate.ToString("ddd", cultureInfo)) + " · " + this.SelectedDate.ToString("dd MMMM yyyy", cultureInfo) + " (" + this.SelectedHourStart.Hours.ToString("00") + ":" + this.SelectedHourStart.Minutes.ToString("00") + " - " + this.SelectedHourEnd.Hours.ToString("00") + ":" + this.SelectedHourEnd.Minutes.ToString("00") + ")";
            }
        }

        [JsonIgnore]
        public string StatusColor
        {
            get
            {
                string color = "#f0ad4e";
                switch (Status)
                {
                    case 1:
                        color = "#f0ad4e";
                        break;
                    case 2:
                        color = "#d9534f";
                        break;
                    case 3:
                        color = "#5cb85c";
                        break;
                    case 4:
                        color = "#0275d8";
                        break;
                    default:
                        break;
                }
                return color;
            }
        }

    }
}
