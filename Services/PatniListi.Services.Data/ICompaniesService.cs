namespace PatniListi.Services.Data
{
    using System.Threading.Tasks;

    using PatniListi.Web.ViewModels.Administration.Companies;

    public interface ICompaniesService
    {
        Task<string> CreateAsync(string companyName);

        Task<string> GetByNameAsync(string companyName);

        Task EditAsync(CompanyEditViewModel input);

        int GetUsersCount(string companyName);

        Task<T> GetDetailsAsync<T>(string id);
    }
}
