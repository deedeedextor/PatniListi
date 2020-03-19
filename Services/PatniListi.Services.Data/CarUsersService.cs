namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Web.ViewModels.Administration.Users;

    public class CarUsersService : ICarUsersService
    {
        private readonly IDeletableEntityRepository<CarUser> carUsersRepository;
        private readonly IUsersService usersService;

        public CarUsersService(IDeletableEntityRepository<CarUser> carUsersRepository, IUsersService usersService)
        {
            this.carUsersRepository = carUsersRepository;
            this.usersService = usersService;
        }

        public async Task UpdateAsync(string carId, string companyId, IEnumerable<string> collection)
        {
            var newDrivers = new List<UserViewModel>();

            if (collection.Count() > 0)
            {
                foreach (var name in collection)
                {
                    var user = this.usersService.GetByNameAsync<UserViewModel>(name, companyId).Result;
                    newDrivers.Add(user);
                }

                var carUsers = await this.carUsersRepository
                    .All()
                    .Where(cu => cu.CarId == carId)
                    .ToListAsync();

                if (carUsers.Count() > 0)
                {
                    foreach (var carUser in carUsers)
                    {
                        this.carUsersRepository.HardDelete(carUser);

                        await this.carUsersRepository.SaveChangesAsync();
                    }
                }

                foreach (var user in newDrivers)
                {
                    var carUser = new CarUser
                    {
                        CarId = carId,
                        UserId = user.Id,
                    };

                    await this.carUsersRepository.AddAsync(carUser);
                }

                await this.carUsersRepository.SaveChangesAsync();

            }
        }
    }
}
