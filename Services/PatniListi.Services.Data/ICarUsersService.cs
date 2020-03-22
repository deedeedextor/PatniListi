namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICarUsersService
    {
        Task UpdateAsync(string carId, string companyId, IEnumerable<string> collection);

        Task SetIsDeletedAsync(string id, string fullName);

        Task<List<T>> GetAllAsync<T>(string id);

        IEnumerable<SelectListItem> GetAllUsersForCar(string carId);

        Task<T> GetByIdAsync<T>(string id);
    }
}
