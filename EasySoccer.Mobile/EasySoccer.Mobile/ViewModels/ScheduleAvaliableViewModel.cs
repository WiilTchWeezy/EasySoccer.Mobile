using EasySoccer.Mobile.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasySoccer.Mobile.ViewModels
{
    public class ScheduleAvaliableViewModel : BindableBase, INavigationAware
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _image;
        public string Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
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

        private long _companyId = 0;
        public ScheduleAvaliableViewModel()
        {

        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var jsonObject = parameters.GetValue<string>("CurrentCompany");
            var selectedSoccerPitch = JsonConvert.DeserializeObject<CompanyModel>(jsonObject);
            if (selectedSoccerPitch != null)
            {
                _companyId = selectedSoccerPitch.Id;
                Name = selectedSoccerPitch.Name;
                Image = selectedSoccerPitch.Image;
                City = selectedSoccerPitch.City;
                CompleteAddress = selectedSoccerPitch.CompleteAddress;
            }
        }
    }
}
