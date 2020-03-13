namespace PatniListi.Web.ViewModels.Models.Users
{
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class RoleViewModel : IMapFrom<ApplicationRole>
    {
        public string Id { get; set; }
    }
}
