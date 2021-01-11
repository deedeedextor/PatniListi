namespace PatniListi.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

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
                .AllAsNoTracking()
                .Where(c => c.Name == companyName)
                .Select(c => c.Users.Count())
                .SingleOrDefault();

            return count;
        }

        public async Task<string> CreateAsync(string name)
        {
            var company = new Company
            {
                Name = name,
            };

            await this.companiesRepository.AddAsync(company);
            await this.companiesRepository.SaveChangesAsync();

            return company.Id;
        }

        public async Task EditAsync(string id, string name, string bulstat, string vatNumber, string phoneNumber, string address, DateTime createdOn)
        {
            var company = this.GetById(id);

            company.Name = name;
            company.Bulstat = bulstat;
            company.VatNumber = vatNumber;
            company.PhoneNumber = phoneNumber;
            company.Address = address;
            company.CreatedOn = createdOn;

            this.companiesRepository.Update(company);
            await this.companiesRepository.SaveChangesAsync();
        }

        public async Task<string> GetByNameAsync(string name)
        {
            return await this.companiesRepository
                .All()
                .Where(c => c.Name == name)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var viewModel = await this.companiesRepository
                .All()
                .Where(c => c.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return viewModel;
        }

        public Company GetById(string id)
        {
            return this.companiesRepository
                .AllAsNoTracking()
                .FirstOrDefault(c => c.Id == id);
        }
    }
}
