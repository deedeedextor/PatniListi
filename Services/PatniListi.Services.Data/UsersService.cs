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
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository, UserManager<ApplicationUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.userManager = userManager;
        }

        public async Task<bool> AddRoleToUser(ApplicationUser user, string roleName)
        {
            if (user == null)
            {
                return false;
            }

            await this.userManager.AddToRoleAsync(user, roleName);

            return true;
        }

        public ApplicationUser GetByName(string fullName)
        {
            return this.usersRepository
                .All()
                .Where(u => u.FullName == fullName)
                .FirstOrDefault();
        }
    }
}
