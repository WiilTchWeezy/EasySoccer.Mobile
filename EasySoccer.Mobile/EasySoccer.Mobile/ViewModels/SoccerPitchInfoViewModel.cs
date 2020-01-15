using Acr.UserDialogs;
using EasySoccer.Mobile.API;
using EasySoccer.Mobile.API.ApiResponses;
using EasySoccer.Mobile.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.Mobile.ViewModels
{
    public class SoccerPitchInfoViewModel : BindableBase, INavigationAware
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

        public ObservableCollection<SoccerPitchResponse> SoccerPitchs { get; set; }
        public SoccerPitchInfoViewModel()
        {
            SoccerPitchs = new ObservableCollection<SoccerPitchResponse>();
        }

        private async Task LoadSoccerPitchsAsync()
        {
            SoccerPitchs.Clear();
            try
            {
                var response = await ApiClient.Instance.GetSoccerPitchesAsync(_companyId);
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        item.Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQEumvi_yIGIc2_CNCwIX7lyubcM2OqdshH7d2Lc3kV1lNG6P6f";
                        SoccerPitchs.Add(item);
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
            if (parameters.Keys != null && parameters.Keys.Contains("selectedSoccerPitch"))
            {
                var jsonObject = parameters.GetValue<string>("selectedSoccerPitch");
                var selectedSoccerPitch = JsonConvert.DeserializeObject<CompanyModel>(jsonObject);
                if (selectedSoccerPitch != null)
                {
                    _companyId = selectedSoccerPitch.Id;
                    Name = selectedSoccerPitch.Name;
                    Image = selectedSoccerPitch.Image;
                    City = selectedSoccerPitch.City;
                    CompleteAddress = selectedSoccerPitch.CompleteAddress;
                    LoadSoccerPitchsAsync();
                }
            }
        }
    }
}
