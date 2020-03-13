namespace PatniListi.Web.ViewModels.Models.Users
{
    using System;

    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class ApplicationUserHomeViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string CompanyBulstat { get; set; }

        public DateTime LastLoggingDate { get; set; }
    }
}
