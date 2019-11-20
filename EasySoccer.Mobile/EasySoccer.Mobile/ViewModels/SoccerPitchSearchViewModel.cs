using EasySoccer.Mobile.API.ApiResponses;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasySoccer.Mobile.ViewModels
{
	public class SoccerPitchSearchViewModel : BindableBase
	{
        public ObservableCollection<SoccerPitchResponse> SoccerPitchs { get; set; }
        public SoccerPitchSearchViewModel()
        {
            SoccerPitchs = new ObservableCollection<SoccerPitchResponse>();
            LoadDataAsync();
        }

        private void LoadDataAsync()
        {
            SoccerPitchs.Add(new SoccerPitchResponse {
                Image = "https://img.stpu.com.br/?img=https://s3.amazonaws.com/pu-mgr/default/a0R0f00000vgXC1EAM/591de07de4b021f908345636.jpg&w=710&h=462",
                Name = "Spot"
            });

            SoccerPitchs.Add(new SoccerPitchResponse
            {
                Image = "https://img.stpu.com.br/?img=https://s3.amazonaws.com/pu-mgr/default/a0R0f00000vgXC1EAM/591de07de4b021f908345636.jpg&w=710&h=462",
                Name = "All Star"
            });
        }
	}
}
