namespace PatniListi.Services.Data
{
    using System;
    using System.Threading.Tasks;

    public interface ICompaniesService
    {
        Task<string> CreateAsync(string companyName);

        Task<string> GetByNameAsync(string companyName);

        Task EditAsync(string id, string name, string bulstat, string vatNumber, string address, string phoneNumber, DateTime createdOn);

        int GetUsersCount(string companyName);

        Task<T> GetDetailsAsync<T>(string id);
    }
}
