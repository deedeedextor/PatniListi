namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICarUsersService
    {
        Task UpdateAsync(string carId, string companyId, IEnumerable<string> collection);
    }
}
