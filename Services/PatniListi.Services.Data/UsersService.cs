namespace PatniListi.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;

    public class UsersService : IUsersService
    {
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
            var user = this.GetByName(userId);
            var role = this.GetRoleByName(roleName);

            user.Roles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = role.Id });
            await this.roleRepository.SaveChangesAsync();
        }

        private ApplicationRole GetRoleByName(string roleName)
        {
            return this.roleRepository
                .All()
                .Where(r => r.Name == roleName)
                .FirstOrDefault();
        }

        private ApplicationUser GetByName(string userId)
        {
            return this.usersRepository
                .All()
                .Where(u => u.Id == userId)
                .FirstOrDefault();
        }
    }
}
