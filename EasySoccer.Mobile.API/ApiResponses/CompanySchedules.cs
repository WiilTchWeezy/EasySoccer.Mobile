using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EasySoccer.Mobile.API.ApiResponses
{
    public class CompanySchedules
    {
        public ObservableCollection<CompanySchedulesEvent> Events { get; set; }

        public string Hour { get; set; }
    }
}
