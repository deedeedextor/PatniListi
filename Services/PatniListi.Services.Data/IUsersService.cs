namespace PatniListi.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Web.ViewModels.Administration.Users;

    public interface IUsersService
    {
        Task CreateAsync(UserInputViewModel input);

        Task AddRoleToUser(string userId, string roleName);

        Task<T> GetByIdAsync<T>(string userId);

        Task<T> GetDetailsAsync<T>(string userId);

        IQueryable<T> GetAll<T>(string companyId);
    }
}
