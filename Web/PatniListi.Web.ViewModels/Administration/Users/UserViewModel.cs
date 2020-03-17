namespace PatniListi.Web.ViewModels.Administration.Users
{
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string CompanyName { get; set; }
    }
}
