namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IUsersService
    {
        IEnumerable<SelectListItem> GetAll(string companyId);

        IEnumerable<SelectListItem> GetUsersByCar(string carId);

        Task<bool> CreateAsync(string username, string email, string password, string confirmPassword, string fullName, string companyId);

        Task AddRoleToUser(string userId, string roleName);

        Task<T> GetByIdAsync<T>(string userId);

        bool IsUsernameInUse(string username);

        bool IsEmailInUse(string email);

        Task<T> GetByNameAsync<T>(string fullName, string companyId);

        Task<T> GetDetailsAsync<T>(string userId);

        IQueryable<T> GetAll<T>(string companyId);

        Task EditAsync(string id, string username, string email, string fullName, string companyId, string companyName, DateTime createdOn, string concurrencyStamp);

        Task<bool> DeleteAsync(string id, string fullName);
    }
}
