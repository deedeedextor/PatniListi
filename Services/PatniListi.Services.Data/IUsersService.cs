namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Web.ViewModels.Administration.Users;

    public interface IUsersService
    {
        IEnumerable<SelectListItem> GetAll(string companyId);

        IEnumerable<SelectListItem> GetUsersByCar(string carId);

        Task CreateAsync(UserInputViewModel input);

        Task AddRoleToUser(string userId, string roleName);

        Task<T> GetByIdAsync<T>(string userId);

        Task<T> GetByNameAsync<T>(string fullName, string companyId);

        Task<T> GetDetailsAsync<T>(string userId);

        IQueryable<T> GetAll<T>(string companyId);

        Task EditAsync(UserEditViewModel input);

        Task<bool> DeleteAsync(string id, string fullName);
    }
}
