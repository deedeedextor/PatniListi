namespace PatniListi.Services.Data
{
    using System.Threading.Tasks;

    using PatniListi.Data.Models;

    public interface ICompaniesService
    {
        Company GetById(string id);

        Task<string> CreateAsync(string companyName);

        Task<string> GetByNameAsync(string companyName);

        Task EditAsync(Company company);

        int GetUsersCount(string companyName);

        Task<T> GetDetailsAsync<T>(string id);
    }
}
