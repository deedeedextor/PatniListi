namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IUsersService
    {
        IEnumerable<SelectListItem> GetAll(string companyId);

        IEnumerable<SelectListItem> GetUsersByCar(string carId);

        Task<T> GetByIdAsync<T>(string userId);

        bool IsUsernameInUse(string username);

        bool IsEmailInUse(string email);

        Task<T> GetByNameAsync<T>(string fullName, string companyId);

        Task<T> GetDetailsAsync<T>(string userId);

        IQueryable<T> GetAll<T>(string companyId);

        Task<bool> DeleteAsync(string id, string fullName);

        string GetUsernameById(string id);

        string GetEmailById(string id);
    }
}
