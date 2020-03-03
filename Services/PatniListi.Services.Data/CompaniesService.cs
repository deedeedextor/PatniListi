namespace PatniListi.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;

    public class CompaniesService : ICompaniesService
    {
        private readonly IRepository<Company> companiesRepository;

        public CompaniesService(IRepository<Company> companiesRepository)
        {
            this.companiesRepository = companiesRepository;
        }

        public int GetUsersCount(string companyName)
        {
            var count = this.companiesRepository
                .All()
                .Where(c => c.Name == companyName)
                .Select(c => c.Users.Count())
                .SingleOrDefault();

            return count;
        }

        public async Task<string> Create(string companyName)
        {
            var company = new Company
            {
                Name = companyName,
            };

            await this.companiesRepository.AddAsync(company);
            await this.companiesRepository.SaveChangesAsync();

            return company.Id;
        }

        public async Task Edit(string name, string bulstat, string phoneNumber, string username)
        {
            var company = new Company
            {
                Name = name,
                Bulstat = bulstat,
                PhoneNumber = phoneNumber,
                ModifiedBy = username,
            };

            this.companiesRepository.Update(company);
            await this.companiesRepository.SaveChangesAsync();
        }

        public string GetByName(string companyName)
        {
            return this.companiesRepository
                .All()
                .Where(c => c.Name == companyName)
                .Select(c => c.Id)
                .FirstOrDefault();
        }
    }
}
