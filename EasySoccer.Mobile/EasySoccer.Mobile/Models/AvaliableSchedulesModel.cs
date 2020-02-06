using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.API.ApiResponses;
using EasySoccer.Mobile.API.Session;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.Mobile.Models
{
    public class AvaliableSchedulesModel : AvaliableSchedulesResponse, INotifyPropertyChanged
    {
        private INavigationService _navigationService;
        public AvaliableSchedulesModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private long companyId = 0;
        public AvaliableSchedulesModel(AvaliableSchedulesResponse item, long companyId, INavigationService navigationService)
        {
            this.PossibleSoccerPitchs = item.PossibleSoccerPitchs;
            this.SelectedDate = item.SelectedDate;
            this.SelectedHourEnd = item.SelectedHourEnd;
            this.SelectedHourStart = item.SelectedHourStart;
            this.IsCurrentSchedule = item.IsCurrentSchedule;
            PossibleSoccerPitchNames = new ObservableCollection<string>();
            SoccerPitchPlansNames = new ObservableCollection<string>();
            SoccerPitchPlans = new List<SoccerPitchPlanResponse>();
            foreach (var soccerPitch in item.PossibleSoccerPitchs)
            {
                PossibleSoccerPitchNames.Add(soccerPitch.Name);
            }
            MakeScheduleCommand = new DelegateCommand(MakeSchedule);
            this.companyId = companyId;
            _navigationService = navigationService;
        }

        public ObservableCollection<string> PossibleSoccerPitchNames { get; set; }
        public ObservableCollection<string> SoccerPitchPlansNames { get; set; }
        public List<SoccerPitchPlanResponse> SoccerPitchPlans { get; set; }
        public DelegateCommand MakeScheduleCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private int? _selectedIndexPitch;
        public int? SelectedIndexPitch
        {
            get { return _selectedIndexPitch; }
            set
            {
                if (value != _selectedIndexPitch)
                {
                    _selectedIndexPitch = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndexPitch)));
                    if (_selectedIndexPitch.HasValue)
                        this.LoadPlansAsync(PossibleSoccerPitchs[_selectedIndexPitch.Value].Id);
                }
            }
        }

        public string SelectedHourStartEnd
        {
            get
            {
                return this.SelectedHourStart.Hours.ToString("00") + ":" + this.SelectedHourStart.Minutes.ToString("00") + " - " + this.SelectedHourEnd.Hours.ToString("00") + ":" + this.SelectedHourEnd.Minutes.ToString("00");
            }

        }

        public bool IsAlternativeSchedule
        {
            get
            {
                return !this.IsCurrentSchedule;
            }
        }

        private int? _selectedIndexPlan;
        public int? SelectedIndexPlan
        {
            get { return _selectedIndexPlan; }
            set
            {
                if (value != _selectedIndexPlan)
                {
                    _selectedIndexPlan = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndexPlan)));
                }
            }
        }

        private async Task LoadPlansAsync(long soccerPitchId)
        {
            try
            {
                var response = await ApiClient.Instance.GetSoccerPitchPlanBySoccerPitchAsync(soccerPitchId);
                if (response != null)
                {
                    foreach (var item in response)
                    {
                        this.SoccerPitchPlans.Clear();
                        this.SoccerPitchPlansNames.Clear();
                        this.SoccerPitchPlans.Add(item);
                        this.SoccerPitchPlansNames.Add(item.Name);
                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        private async void MakeSchedule()
        {
            try
            {
                if (SelectedIndexPitch.HasValue && SelectedIndexPlan.HasValue)
                {
                    var reservationRespone = await ApiClient.Instance.MakeReservationAsync(PossibleSoccerPitchs[_selectedIndexPitch.Value].Id, CurrentUser.Instance.UserId.Value, this.SelectedDate, this.SelectedHourStart, this.SelectedHourEnd, SoccerPitchPlans[this.SelectedIndexPlan.Value].Id);
                    if (reservationRespone != null)
                    {
                        UserDialogs.Instance.Alert("Agendamento realizado com sucesso.");
                        await _navigationService.NavigateAsync("/NavigationPage/SoccerPitchSearch");
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
