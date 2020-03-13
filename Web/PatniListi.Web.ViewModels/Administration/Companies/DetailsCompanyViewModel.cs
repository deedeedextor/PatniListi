namespace PatniListi.Web.ViewModels.Administration.Companies
{
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class DetailsCompanyViewModel : IMapFrom<Company>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Bulstat { get; set; }

        public string VatNumber => $"BG{this.Bulstat}";

        public string PhoneNumber { get; set; }
    }
}
