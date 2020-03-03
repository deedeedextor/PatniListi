namespace PatniListi.Services.Data
{
    using System.Threading.Tasks;

    using PatniListi.Data.Models;

    public interface IUsersService
    {
        Task AddRoleToUser(string userId, string roleName);
    }
}
