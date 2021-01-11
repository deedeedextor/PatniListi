namespace PatniListi.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using PatniListi.Data.Models;

    public interface ICompaniesService
    {
        Company GetById(string id);

        Task<string> CreateAsync(string companyName);

        Task<string> GetByNameAsync(string companyName);

        Task EditAsync(string id, string name, string bulstat, string vatNumber, string phoneNumber, string address, DateTime createdOn);

        int GetUsersCount(string companyName);

        Task<T> GetDetailsAsync<T>(string id);
    }
}
