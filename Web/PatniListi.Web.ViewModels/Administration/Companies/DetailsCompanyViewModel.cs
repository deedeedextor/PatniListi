namespace PatniListi.Web.ViewModels.Administration.Companies
{
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class DetailsCompanyViewModel : IMapFrom<Company>
    {
        public string Id { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }

        [Display(Name = "Булстат")]
        public string Bulstat { get; set; }

        [Display(Name = "Номер по ДДС")]
        public string VatNumber => $"BG{this.Bulstat}";

        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }
    }
}
