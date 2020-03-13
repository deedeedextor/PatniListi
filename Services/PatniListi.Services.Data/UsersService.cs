namespace PatniListi.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Models.Users;

    public class UsersService : IUsersService
    {
        private const string InvalidUserIdErrorMessage = "Не съществуващ потребител.";
        private const string InvalidUserNameErrorMessage = "Потребител с име: {0} не съществува.";

        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<ApplicationRole> roleRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository, IDeletableEntityRepository<ApplicationRole> roleRepository, UserManager<ApplicationUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.roleRepository = roleRepository;
            this.userManager = userManager;
        }

        public async Task AddRoleToUser(string userId, string roleName)
        {
            var user = await this.GetUserAsync(userId);

            var role = await this.GetRoleByNameAsync<RoleViewModel>(roleName);

            user.Roles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = role.Id });
            await this.roleRepository.SaveChangesAsync();
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

        public async Task<T> GetRoleByNameAsync<T>(string roleName)
        {
            return await this.roleRepository
                .All()
                .Where(r => r.Name == roleName)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        private async Task<ApplicationUser> GetUserAsync(string userId)
        {
            return await this.usersRepository
                .All()
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
        }
    }
}
