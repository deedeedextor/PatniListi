namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<bool> DeleteAsync(string id, string fullName)
        {
            var user = await this.usersRepository
                .All()
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return false;
            }

            this.usersRepository.Delete(user);
            await this.usersRepository.SaveChangesAsync();

            return true;
        }

        public IQueryable<T> GetAll<T>(string companyId)
        {
            return this.usersRepository
                .All()
                .Where(u => u.CompanyId == companyId)
                .OrderBy(u => u.UserName)
                .To<T>();
        }

        public IEnumerable<SelectListItem> GetAll(string companyId)
        {
            return this.usersRepository
                .AllAsNoTracking()
                .Where(u => u.CompanyId == companyId)
                .OrderBy(u => u.FullName)
                .Select(u => new SelectListItem
                {
                    Value = u.FullName,
                    Text = u.FullName,
                })
                .ToList();
        }

        public IEnumerable<SelectListItem> GetUsersByCar(string carId)
        {
            return this.usersRepository
                .AllAsNoTracking()
                .OrderBy(u => u.FullName)
                .SelectMany(u => u.CarUsers)
                .Where(u => u.CarId == carId)
                .Select(u => new SelectListItem
                {
                    Value = u.ApplicationUser.FullName,
                    Text = u.ApplicationUser.FullName,
                })
                .ToList();
        }

        public async Task<T> GetByIdAsync<T>(string userId)
        {
            var viewModel = await this.usersRepository
                .All()
                .Where(u => u.Id == userId)
                .Include(u => u.Company)
                .To<T>()
                .FirstOrDefaultAsync();

            return viewModel;
        }

        public async Task<T> GetByNameAsync<T>(string fullName, string companyId)
        {
            var viewModel = await this.usersRepository
                      .All()
                      .Where(u => u.FullName == fullName && u.CompanyId == companyId)
                      .To<T>()
                      .FirstOrDefaultAsync();

            return viewModel;
        }

        public async Task<T> GetDetailsAsync<T>(string userId)
        {
            var viewModel = await this.usersRepository
                   .AllAsNoTracking()
                   .Where(u => u.Id == userId)
                   .Include(u => u.Company)
                   .Include(u => u.CarUsers)
                   .To<T>()
                   .FirstOrDefaultAsync();

            return viewModel;
        }

        public bool IsUsernameInUse(string username)
        {
            var exists = this.usersRepository
                .AllAsNoTrackingWithDeleted()
                .Any(u => u.UserName == username);

            if (exists)
            {
                return true;
            }

            return false;
        }

        public bool IsEmailInUse(string email)
        {
            var exists = this.usersRepository
                .AllAsNoTrackingWithDeleted()
                .Any(u => u.Email == email);

            if (exists)
            {
                return true;
            }

            return false;
        }

        public string GetUsernameById(string id)
        {
            return this.usersRepository
                .AllAsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => u.UserName)
                .SingleOrDefault();
        }

        public string GetEmailById(string id)
        {
            return this.usersRepository
                .AllAsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => u.Email)
                .SingleOrDefault();
        }
    }
}
