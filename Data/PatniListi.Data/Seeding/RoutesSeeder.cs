namespace PatniListi.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Data.Models;

    public class RoutesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Routes.Any())
            {
                return;
            }

            var routes = new List<Route>()
            {
                new Route { StartPoint = "София", EndPoint = "Пловдив", Distance = 146 },
                new Route { StartPoint = " Пловдив", EndPoint = "София", Distance = 146 },
                new Route { StartPoint = "София", EndPoint = "Перник", Distance = 35.2 },
                new Route { StartPoint = "Перник", EndPoint = "София", Distance = 40.3 },
                new Route { StartPoint = "София", EndPoint = "Кюстендил", Distance = 104 },
                new Route { StartPoint = "Кюстендил", EndPoint = "София", Distance = 104 },
                new Route { StartPoint = "София", EndPoint = "Ихтиман", Distance = 61.4 },
                new Route { StartPoint = "Ихтиман", EndPoint = "София", Distance = 54.4 },
                new Route { StartPoint = "София", EndPoint = "Варна", Distance = 441 },
                new Route { StartPoint = "Варна", EndPoint = "София", Distance = 441 },
                new Route { StartPoint = "София", EndPoint = "Бургас", Distance = 383 },
                new Route { StartPoint = "Бургас", EndPoint = "София", Distance = 383 },
                new Route { StartPoint = "София", EndPoint = "Хасково", Distance = 227 },
                new Route { StartPoint = "Хасково", EndPoint = "София", Distance = 227 },
                new Route { StartPoint = "София", EndPoint = "Стара Загора", Distance = 232 },
                new Route { StartPoint = "Стара Загора", EndPoint = "София", Distance = 232 },
                new Route { StartPoint = "Пловдив", EndPoint = "Стара Загора", Distance = 100 },
                new Route { StartPoint = "Пловдив", EndPoint = "Хасково", Distance = 95.5 },
                new Route { StartPoint = "Хасково", EndPoint = "Пловдив", Distance = 94.2 },
                new Route { StartPoint = "Пловдив", EndPoint = "Асеновград", Distance = 19.6 },
                new Route { StartPoint = "Асеновград", EndPoint = "Пловдив", Distance = 19.6 },
                new Route { StartPoint = "Пловдив", EndPoint = "Велико Търново", Distance = 209 },
                new Route { StartPoint = "Велико Търново", EndPoint = "Пловдив", Distance = 209 },
                new Route { StartPoint = "София", EndPoint = "Велико Търново", Distance = 219 },
                new Route { StartPoint = "Велико Търново", EndPoint = "София", Distance = 219 },
                new Route { StartPoint = "Пловдив", EndPoint = "Видин", Distance = 362 },
                new Route { StartPoint = "Видин", EndPoint = "Пловдив", Distance = 362 },
                new Route { StartPoint = "Пловдив", EndPoint = "Русе", Distance = 312 },
                new Route { StartPoint = "Русе", EndPoint = "Пловдив", Distance = 312 },
                new Route { StartPoint = "София", EndPoint = "Русе", Distance = 310 },
                new Route { StartPoint = "Русе", EndPoint = "София", Distance = 310 },
                new Route { StartPoint = "Пловдив", EndPoint = "Карнобат", Distance = 213 },
                new Route { StartPoint = "Карнобат", EndPoint = "Пловдив", Distance = 213 },
                new Route { StartPoint = "Хасково", EndPoint = "Кърджали", Distance = 47.1 },
                new Route { StartPoint = "Кърджали", EndPoint = "Хасково", Distance = 47.1 },
                new Route { StartPoint = "Хасково", EndPoint = "Димитровград", Distance = 16.7 },
                new Route { StartPoint = "Димитровград", EndPoint = "Хасково", Distance = 16.7 },
                new Route { StartPoint = "Хасково", EndPoint = "Стара Загора", Distance = 60.5 },
                new Route { StartPoint = "Стара Загора", EndPoint = "Хасково", Distance = 60.5 },
                new Route { StartPoint = "Варна", EndPoint = "Бургас", Distance = 213 },
                new Route { StartPoint = "Бургас", EndPoint = "Варна", Distance = 131 },
                new Route { StartPoint = "Пловдив", EndPoint = "Карнобат", Distance = 131 },
            };

            await dbContext.Routes.AddRangeAsync(routes);
        }
    }
}
