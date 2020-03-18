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
        private const string InvalidUserNameErrorMessage = "Потребител с име: {0} не съществува.";

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

        public async Task CreateAsync(UserInputViewModel input)
        {
            var user = new ApplicationUser { UserName = input.Username, Email = input.Email, FullName = input.FullName, CompanyId = input.CompanyId };

            var result = await this.userManager.CreateAsync(user, input.Password);

            if (result.Succeeded)
            {
                await this.AddRoleToUser(user.Id, "Driver");

                this.logger.LogInformation("User created a new account with password.");
            }
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
                .All()
                .Where(u => u.CompanyId == companyId)
                .Select(u => new SelectListItem
                {
                    Value = u.FullName,
                    Text = u.FullName,
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
