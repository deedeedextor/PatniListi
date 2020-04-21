namespace PatniListi.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Data.Models;

    public class CarsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Cars.Any())
            {
                return;
            }

            var companyIdOne = dbContext.Companies
                .Where(c => c.Name == "Авангард ЕООД")
                .Select(c => c.Id)
                .FirstOrDefault();

            var cars = new List<Car>()
            {
                new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 },
                new Car { Model = "Форд Фиеста", LicensePlate = "CO1967KA", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 0, StartKilometers = 0 },
                new Car { Model = "Шкода Октавия", LicensePlate = "PB1243BA", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Бензин, TankCapacity = 50, AverageConsumption = 5, InitialFuel = 20, StartKilometers = 120000 },
                new Car { Model = "Шкода Сюпърб", LicensePlate = "H0243AA", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Бензин, TankCapacity = 60, AverageConsumption = 5, InitialFuel = 25, StartKilometers = 198120 },
                new Car { Model = "Форд Фокус", LicensePlate = "C7243BA", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 20, StartKilometers = 250000 },
                new Car { Model = "Алфа Ромео", LicensePlate = "X6743KA", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Бензин, TankCapacity = 70, AverageConsumption = 6, InitialFuel = 30, StartKilometers = 250987 },
                new Car { Model = "Ауди А3", LicensePlate = "CT3043KK", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Бензин, TankCapacity = 50, AverageConsumption = 5, InitialFuel = 20, StartKilometers = 253451 },
                new Car { Model = "Фолцваген Голф", LicensePlate = "CT0256ВК", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Дизел, TankCapacity = 50, AverageConsumption = 5, InitialFuel = 50, StartKilometers = 269874 },
                new Car { Model = "Сеат Ибиза", LicensePlate = "Х5256ВК", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Дизел, TankCapacity = 45, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 345239 },
                new Car { Model = "Сеат Леон", LicensePlate = "Х4256ВК", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Дизел, TankCapacity = 50, AverageConsumption = 5, InitialFuel = 40, StartKilometers = 345122 },
                new Car { Model = "Фолцваген Поло", LicensePlate = "C1356ВA", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Бензин, TankCapacity = 45, AverageConsumption = 4, InitialFuel = 15, StartKilometers = 198435 },
                new Car { Model = "Фолцваген Поло", LicensePlate = "C1666ВA", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Бензин, TankCapacity = 45, AverageConsumption = 4, InitialFuel = 20, StartKilometers = 200000 },
                new Car { Model = "Ауди А8", LicensePlate = "PB6966CA", CompanyId = companyIdOne, FuelType = Models.Enums.Fuel.Бензин, TankCapacity = 90, AverageConsumption = 7, InitialFuel = 35, StartKilometers = 200456 },
            };

            await dbContext.Cars.AddRangeAsync(cars);

            var companyIdTwo = dbContext.Companies
                .Where(c => c.Name == "ЕT Саламандър")
                .Select(c => c.Id)
                .FirstOrDefault();

            var carsTwo = new List<Car>()
            {
                new Car { Model = "Форд Фиеста", LicensePlate = "CO0012KA", CompanyId = companyIdTwo, FuelType = Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 },
                new Car { Model = "Форд Фиеста", LicensePlate = "CO0067KA", CompanyId = companyIdTwo, FuelType = Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 0, StartKilometers = 0 },
                new Car { Model = "БМВ Х5", LicensePlate = "CO9167ВA", CompanyId = companyIdTwo, FuelType = Models.Enums.Fuel.Бензин, TankCapacity = 85, AverageConsumption = 8, InitialFuel = 40, StartKilometers = 378192 },
            };

            await dbContext.Cars.AddRangeAsync(carsTwo);
        }
    }
}
