using EasySoccer.Mobile.API.ApiResponses;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasySoccer.Mobile.ViewModels
{
    public class SoccerPitchScheduleViewModel : BindableBase
    {
        public ObservableCollection<CompanySchedules> CompanySchedules { get; set; }
        public SoccerPitchScheduleViewModel()
        {
            CompanySchedules = new ObservableCollection<CompanySchedules>();
            InitEvents();
        }

        private void InitEvents()
        {
            var events = new ObservableCollection<CompanySchedulesEvent>();
            events.Add(new CompanySchedulesEvent 
            {
                SelectedHour = "20:00 - 21:00",
                SoccerPitch = "Quadra 2",
                UserName = "Tarcisio Vitor"
            });
            events.Add(new CompanySchedulesEvent
            {
                SelectedHour = "20:00 - 21:00",
                SoccerPitch = "Quadra 3",
                UserName = "Tarcisio"
            });
            events.Add(new CompanySchedulesEvent
            {
                SelectedHour = "20:00 - 21:00",
                SoccerPitch = "Quadra 5",
                UserName = "Sergio Vitor"
            });
            events.Add(new CompanySchedulesEvent
            {
                SelectedHour = "20:00 - 21:00",
                SoccerPitch = "Quadra 9",
                UserName = "Tarcisio Vitor"
            });
            events.Add(new CompanySchedulesEvent
            {
                SelectedHour = "13:00 - 21:00",
                SoccerPitch = "Quadra 9",
                UserName = "Tarcisio Vitor"
            });
            CompanySchedules.Add(new API.ApiResponses.CompanySchedules 
            {
                Hour = "12:00",
                Events = events
            });
            CompanySchedules.Add(new API.ApiResponses.CompanySchedules
            {
                Hour = "13:00",
                Events = events
            });
            CompanySchedules.Add(new API.ApiResponses.CompanySchedules
            {
                Hour = "14:00",
                Events = events
            });
            CompanySchedules.Add(new API.ApiResponses.CompanySchedules
            {
                Hour = "15:00"
            });
            CompanySchedules.Add(new API.ApiResponses.CompanySchedules
            {
                Hour = "16:00"
            });
            CompanySchedules.Add(new API.ApiResponses.CompanySchedules
            {
                Hour = "17:00"
            });
        }
    }
}
