namespace PatniListi.Services.Data
{
    using System.Threading.Tasks;

    public interface ICompaniesService
    {
        Task<string> Create(string companyName);

        string GetByName(string companyName);

        Task Edit(string name, string bulstat, string phoneNumber, string username);

        int GetUsersCount(string companyName);
    }
}
