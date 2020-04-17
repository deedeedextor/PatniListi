namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
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

        public async Task SetIsDeletedAsync(string carId, string fullName)
        {
            var carUsersFromDb = await this.GetAllAsync<UserCarViewModel>(carId);

            if (carUsersFromDb != null)
            {
                foreach (var cu in carUsersFromDb)
                {
                    var carUser = new CarUser
                    {
                        CarId = cu.CarId,
                        UserId = cu.UserId,
                    };

                    carUser.ModifiedBy = fullName;
                    this.carUsersRepository.Delete(carUser);

                    await this.carUsersRepository.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateAsync(string carId, string companyId, IEnumerable<string> collection)
        {
            var newDrivers = new List<UserViewModel>();

            if (collection.Count() > 0)
            {
                foreach (var name in collection)
                {
                    var user = await this.usersService.GetByNameAsync<UserViewModel>(name, companyId);
                    newDrivers.Add(user);
                }

                var carUsers = await this.GetAllAsync<UserCarViewModel>(carId);

                if (carUsers.Count() > 0)
                {
                    foreach (var cu in carUsers)
                    {
                        var carUser = new CarUser
                        {
                            CarId = cu.CarId,
                            UserId = cu.UserId,
                        };

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

        public async Task<List<T>> GetAllAsync<T>(string id)
        {
            return await this.carUsersRepository
                .All()
                .Where(cu => cu.CarId == id)
                .To<T>()
                .ToListAsync();
        }
    }
}
