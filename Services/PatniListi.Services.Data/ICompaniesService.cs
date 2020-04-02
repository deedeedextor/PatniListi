namespace PatniListi.Services.Data
{
    using System.Threading.Tasks;

    using PatniListi.Web.ViewModels.Administration.Companies;

    public interface ICompaniesService
    {
        Task<string> CreateAsync(string companyName);

        Task<string> GetByNameAsync(string companyName);

        Task EditAsync(string id, string name, string bulstat, string vatNumber, string address, string phoneNumber);

        int GetUsersCount(string companyName);

        Task<T> GetDetailsAsync<T>(string id);
    }
}
