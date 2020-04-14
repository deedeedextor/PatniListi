namespace PatniListi.Web.ViewModels.Administration.Users
{
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class UserCarViewModel : IMapFrom<CarUser>
    {
        public string UserId { get; set; }

        public string ApplicationUserFullName { get; set; }

        public string CarId { get; set; }
    }
}
