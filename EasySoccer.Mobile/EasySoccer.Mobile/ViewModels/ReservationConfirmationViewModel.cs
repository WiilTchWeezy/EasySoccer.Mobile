using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.API.ApiResponses;
using EasySoccer.Mobile.API.Session;
using EasySoccer.Mobile.Infra;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasySoccer.Mobile.ViewModels
{
    public class ReservationConfirmationViewModel : BindableBase, INavigationAware
    {
        private string _image;
        public string Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
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

        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set { SetProperty(ref _companyName, value); }
        }

        private int _selectedHourIndex = -1;
        public int SelectedHourIndex
        {
            get { return _selectedHourIndex; }
            set
            {
                if (SetProperty(ref _selectedHourIndex, value))
                {
                    UpdateSoccerPitchInfo();
                }
            }
        }

        private string _currentHour;
        public string CurrentHour
        {
            get { return _currentHour; }
            set { SetProperty(ref _currentHour, value); }
        }

        private string _currentHourEnd;
        public string CurrentHourEnd
        {
            get { return _currentHourEnd; }
            set { SetProperty(ref _currentHourEnd, value); }
        }

        private bool _soccerPitchEnabled;
        public bool SoccerPitchEnabled
        {
            get { return _soccerPitchEnabled; }
            set { SetProperty(ref _soccerPitchEnabled, value); }
        }

        private string _soccerPitchName;
        public string SoccerPitchName
        {
            get { return _soccerPitchName; }
            set { SetProperty(ref _soccerPitchName, value); }
        }

        private string _soccerPitchNumberOfPlayers;
        public string SoccerPitchNumberOfPlayers
        {
            get { return _soccerPitchNumberOfPlayers; }
            set { SetProperty(ref _soccerPitchNumberOfPlayers, value); }
        }

        private string _soccerPitchImage;
        public string SoccerPitchImage
        {
            get { return _soccerPitchImage; }
            set { SetProperty(ref _soccerPitchImage, value); }
        }

        private string _soccerPitchSportType;
        public string SoccerPitchSportType
        {
            get { return _soccerPitchSportType; }
            set { SetProperty(ref _soccerPitchSportType, value); }
        }

        private bool _soccerPitchVisible;
        public bool SoccerPitchVisible
        {
            get { return _soccerPitchVisible; }
            set { SetProperty(ref _soccerPitchVisible, value); }
        }

        private int? _indexSoccerPicth;
        public int? IndexSoccerPicth
        {
            get { return _indexSoccerPicth; }
            set
            {
                if (SetProperty(ref _indexSoccerPicth, value))
                {
                    if (_indexSoccerPicth.HasValue)
                    {
                        UpdateSoccerPitchInfo();
                    }
                }
            }
        }

        private int _indexSoccerPicthPlan = -1;
        public int IndexSoccerPicthPlan
        {
            get { return _indexSoccerPicthPlan; }
            set
            {
                SetProperty(ref _indexSoccerPicthPlan, value);
            }
        }

        private string _selectedDateText;
        public string SelectedDateText
        {
            get { return _selectedDateText; }
            set { SetProperty(ref _selectedDateText, value); }
        }


        private long _companyId;
        private long _soccerPitchId = 0;
        private TimeSpan _hourStart = TimeSpan.Zero;
        private TimeSpan _hourEnd = TimeSpan.Zero;
        private DateTime _selectedDate;

        public ObservableCollection<SoccerPitchResponse> SoccerPitchsObject { get; set; }
        public ObservableCollection<SoccerPitchPlanResponse> PlansObject { get; set; }
        public ObservableCollection<string> Plans { get; set; }
        public ObservableCollection<string> SoccerPitchs { get; set; }
        public ObservableCollection<string> Hours { get; set; }

        public DelegateCommand MakeReservationCommand { get; set; }
        private INavigationService _navigationService;
        public ReservationConfirmationViewModel(INavigationService navigationService)
        {
            SoccerPitchsObject = new ObservableCollection<SoccerPitchResponse>();
            SoccerPitchs = new ObservableCollection<string>();
            Hours = new ObservableCollection<string>();
            PlansObject = new ObservableCollection<SoccerPitchPlanResponse>();
            Plans = new ObservableCollection<string>();
            _navigationService = navigationService;
            MakeReservationCommand = new DelegateCommand(MakeReservation);
        }

        private async void LoadDataAsync()
        {
            try
            {
                var response = await ApiClient.Instance.GetSoccerPitchesAsync(_companyId);
                if (response != null)
                {
                    foreach (var item in response)
                    {
                        SoccerPitchs.Add(item.Name);
                        SoccerPitchsObject.Add(item);
                    }
                    UpdateSoccerPitchInfo();
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        private void UpdateSoccerPitchInfo()
        {
            if (IndexSoccerPicth.HasValue && IndexSoccerPicth.Value >= 0 && SoccerPitchsObject.Count > IndexSoccerPicth.Value)
            {
                var currentSoccerPitch = SoccerPitchsObject[IndexSoccerPicth.Value];
                if (currentSoccerPitch != null)
                {
                    SoccerPitchVisible = true;
                    SoccerPitchName = currentSoccerPitch.Name;
                    SoccerPitchNumberOfPlayers = $"Quantidade de jogadores :{currentSoccerPitch.NumberOfPlayers}";
                    SoccerPitchImage = Application.Instance.GetImage(currentSoccerPitch.ImageName, Infra.Enums.BlobContainerEnum.SoccerPitch);
                    SoccerPitchSportType = currentSoccerPitch.SportTypeName;
                    _soccerPitchId = currentSoccerPitch.Id;
                    UpdateHours(currentSoccerPitch);
                    LoadPlansAsync();
                }
            }
            else if (_soccerPitchId > 0)
            {
                var currentSoccerPitch = SoccerPitchsObject.Where(x => x.Id == _soccerPitchId).FirstOrDefault();
                if (currentSoccerPitch != null)
                {
                    IndexSoccerPicth = SoccerPitchsObject.IndexOf(currentSoccerPitch);
                    SoccerPitchVisible = true;
                    SoccerPitchName = currentSoccerPitch.Name;
                    SoccerPitchNumberOfPlayers = $"Quantidade de jogadores :{currentSoccerPitch.NumberOfPlayers}";
                    SoccerPitchImage = Application.Instance.GetImage(currentSoccerPitch.Image, Infra.Enums.BlobContainerEnum.SoccerPitch);
                    SoccerPitchSportType = currentSoccerPitch.SportTypeName;
                    UpdateHours(currentSoccerPitch);
                    LoadPlansAsync();
                }
            }
            else
            {
                SoccerPitchVisible = false;
            }
        }

        private void UpdateHours(SoccerPitchResponse soccerPitch)
        {
            if (soccerPitch != null && this.SelectedHourIndex >= 0)
            {
                var currentHour = Hours[SelectedHourIndex];
                TimeSpan currentHourStart = TimeSpan.Zero;
                if (TimeSpan.TryParse(currentHour, out currentHourStart))
                {
                    _hourStart = currentHourStart;
                    var currentHourEnd = currentHourStart.Add(TimeSpan.FromMinutes(soccerPitch.Interval));
                    _hourEnd = currentHourEnd;
                    CurrentHourEnd = $"{currentHourEnd.Hours:00}:{currentHourEnd.Minutes:00}";
                }
            }
        }

        private async void MakeReservation()
        {
            try
            {
                if (CurrentUser.Instance.IsLoggedIn && CurrentUser.Instance.UserId.HasValue && PlansObject.Count > IndexSoccerPicthPlan)
                {
                    var plan = PlansObject[IndexSoccerPicthPlan];
                    var reservationResponse = await ApiClient.Instance.MakeReservationAsync(_soccerPitchId, CurrentUser.Instance.UserId.Value, _selectedDate, _hourStart, _hourEnd, plan.Id);
                    if (reservationResponse != null && reservationResponse.Id != Guid.Empty)
                    {
                        UserDialogs.Instance.Alert("Agendamento realizado com sucesso.");
                        var navigationParameters = new NavigationParameters();
                        navigationParameters.Add("ReservationId", reservationResponse.Id);
                        await _navigationService.GoBackToRootAsync(navigationParameters);

                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Ocorreu um erro ao realizar o agendamento.");
                    }
                }
                else
                {
                    await _navigationService.NavigateAsync("Login", useModalNavigation: true);
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        private async void LoadPlansAsync()
        {
            try
            {
                var plansResponse = await ApiClient.Instance.GetSoccerPitchPlanBySoccerPitchAsync(_soccerPitchId) ;
                if (plansResponse != null)
                {
                    PlansObject.Clear();
                    Plans.Clear();
                    foreach (var item in plansResponse)
                    {
                        PlansObject.Add(item);
                        Plans.Add(item.Name);
                        IndexSoccerPicthPlan = 0;
                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message);
            }

        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Image"))
                Image = parameters.GetValue<string>("Image");
            if (parameters.ContainsKey("Name"))
                Name = parameters.GetValue<string>("Name");
            if (parameters.ContainsKey("City"))
                City = parameters.GetValue<string>("City");
            if (parameters.ContainsKey("CompleteAddress"))
                CompleteAddress = parameters.GetValue<string>("CompleteAddress");
            if (parameters.ContainsKey("CompanyId"))
                _companyId = parameters.GetValue<long>("CompanyId");
            if (parameters.ContainsKey("SelectedDate"))
            {
                _selectedDate = parameters.GetValue<DateTime>("SelectedDate");
                SelectedDateText = _selectedDate.ToString("dd/MM/yyyy");
            }
            if (parameters.ContainsKey("CurrentHour"))
            {
                CurrentHour = parameters.GetValue<string>("CurrentHour");
                if (string.IsNullOrEmpty(CurrentHour) == false)
                {
                    var currentHour = TimeSpan.Zero;
                    if (TimeSpan.TryParse(CurrentHour, out currentHour))
                    {
                        this.Hours.Add($"{currentHour.Hours:00}:{currentHour.Minutes:00}");
                        this.Hours.Add($"{currentHour.Hours:00}:30");
                        SelectedHourIndex = 0;
                    }
                }
            }
            if (parameters.ContainsKey("SoccerPitchId"))
            {
                _soccerPitchId = parameters.GetValue<long>("SoccerPitchId");
            }
            SoccerPitchEnabled = _soccerPitchId <= 0;
            LoadDataAsync();

        }
    }
}
