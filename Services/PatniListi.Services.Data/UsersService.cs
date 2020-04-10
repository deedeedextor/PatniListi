namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.Users;

    public class UsersService : IUsersService
    {
        private const string InvalidUserIdErrorMessage = "Не съществуващ потребител.";

        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<ApplicationRole> roleRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<UserInputViewModel> logger;
        private readonly RoleManager<ApplicationRole> roleManager;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ApplicationRole> roleRepository,
            UserManager<ApplicationUser> userManager,
            ILogger<UserInputViewModel> logger,
            RoleManager<ApplicationRole> roleManager)
        {
            this.usersRepository = usersRepository;
            this.roleRepository = roleRepository;
            this.userManager = userManager;
            this.logger = logger;
            this.roleManager = roleManager;
        }

        public async Task AddRoleToUser(string userId, string roleName)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            var role = await this.roleManager.FindByNameAsync(roleName);

            user.Roles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = role.Id });
            await this.roleRepository.SaveChangesAsync();
        }

        public async Task<bool> CreateAsync(string username, string email, string password, string confirmPassword, string fullName, string companyId)
        {
            var user = new ApplicationUser { UserName = username, Email = email, FullName = fullName, CompanyId = companyId };

            var result = await this.userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await this.AddRoleToUser(user.Id, "Driver");

                this.logger.LogInformation("User created a new account with password.");
                return true;
            }

            return false;
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

        public async Task EditAsync(string id, string username, string email, string fullName, string companyId, string companyName, DateTime createdOn, string concurrencyStamp)
        {
            var user = new ApplicationUser
            {
                Id = id,
                UserName = username,
                Email = email,
                FullName = fullName,
                CompanyId = companyId,
                CreatedOn = createdOn,
                ConcurrencyStamp = concurrencyStamp,
            };

            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAll<T>(string companyId)
        {
            return this.usersRepository
                .All()
                .Where(u => u.CompanyId == companyId)
                .OrderByDescending(u => u.CreatedOn)
                .To<T>();
        }

        public IEnumerable<SelectListItem> GetAll(string companyId)
        {
            return this.usersRepository
                .AllAsNoTracking()
                .Where(u => u.CompanyId == companyId)
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

            if (viewModel == null)
            {
                throw new ArgumentNullException(InvalidUserIdErrorMessage, userId);
            }

            return viewModel;
        }

        public async Task<T> GetByNameAsync<T>(string fullName, string companyId)
        {
            var viewModel = await this.usersRepository
                      .All()
                      .Where(u => u.FullName == fullName && u.CompanyId == companyId)
                      .To<T>()
                      .FirstOrDefaultAsync();

            if (viewModel == null)
            {
                throw new ArgumentNullException(InvalidUserIdErrorMessage, fullName);
            }

            return viewModel;
        }

        public async Task<T> GetDetailsAsync<T>(string userId)
        {
            var viewModel = await this.usersRepository
                   .All()
                   .Where(u => u.Id == userId)
                   .Include(u => u.Company)
                   .Include(u => u.CarUsers)
                   .To<T>()
                   .FirstOrDefaultAsync();

            if (viewModel == null)
            {
                throw new ArgumentNullException(InvalidUserIdErrorMessage, userId);
            }

            return viewModel;
        }
    }
}
