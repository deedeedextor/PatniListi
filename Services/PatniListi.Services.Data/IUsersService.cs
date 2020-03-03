namespace PatniListi.Services.Data
{
    using System.Threading.Tasks;

    using PatniListi.Data.Models;

    public interface IUsersService
    {
        Task<bool> AddRoleToUser(ApplicationUser user, string roleName);

        ApplicationUser GetByName(string fullName);
    }
}
