namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PatniListi.Data.Models;

    public interface ICarUsersService
    {
        Task UpdateAsync(string carId, string companyId, IEnumerable<string> collection);

        Task SetIsDeletedAsync(string id);

        Task<List<CarUser>> GetAllAsync(string id);
    }
}
