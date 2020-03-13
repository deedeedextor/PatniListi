namespace PatniListi.Services.Data
{
    using System.Threading.Tasks;

    using PatniListi.Web.ViewModels.Models.Users;

    public interface IUsersService
    {
        Task AddRoleToUser(string userId, string roleName);

        Task<T> GetRoleByNameAsync<T>(string roleName);

        Task<T> GetByIdAsync<T>(string userId);
    }
}
