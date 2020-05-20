using EasySoccer.Mobile.API.ApiResponses;
using EasySoccer.Mobile.Infra;
using Prism.Commands;

namespace EasySoccer.Mobile.Models
{
    public class CompanyModel : CompanyResponse
    {
        public DelegateCommand<CompanyModel> SelectSoccerPicthCommand { get; set; }

        public CompanyModel(CompanyResponse baseItem)
        {
            this.Active = baseItem.Active;
            this.City = baseItem.City;
            this.CNPJ = baseItem.CNPJ;
            this.CompleteAddress = baseItem.CompleteAddress;
            this.CreatedDate = baseItem.CreatedDate;
            this.Description = baseItem.Description;
            this.Distance = baseItem.Distance;
            this.Id = baseItem.Id;
            this.Image = baseItem.Image;
            this.Latitude = baseItem.Latitude;
            this.Logo = baseItem.Logo;
            this.Longitude = baseItem.Longitude;
            this.Name = baseItem.Name;
            this.WorkOnHoliDays = baseItem.WorkOnHoliDays;
            this.Image = Application.Instance.GetImage(this.Logo, Infra.Enums.BlobContainerEnum.Company);
        }
        public CompanyModel()
        {

        }
    }
}
