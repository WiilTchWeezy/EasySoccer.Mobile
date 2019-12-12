using System;

namespace EasySoccer.Mobile.API.ApiResponses
{
    public class CompanyResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CNPJ { get; set; }
        public string Logo { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool WorkOnHoliDays { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string City { get; set; }
        public string CompleteAddress { get; set; }
        public bool Active { get; set; }
        public double Distance { get; set; }

        public string Image { get; set; }
    }
}
