namespace PatniListi.Web.ViewModels.Administration.Cars
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.Users;

    public class CarDetailsViewModel : IMapFrom<Car>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Display(Name = "Модел")]
        public string Model { get; set; }

        [Display(Name = "Номер")]
        public string LicensePlate { get; set; }

        [Display(Name = "Вид гориво")]
        public string FuelType { get; set; }

        [Display(Name = "Начални километри")]
        public int StartKilometers { get; set; }

        [Display(Name = "Среден разход")]
        public int AverageConsumption { get; set; }

        [Display(Name = "Капацитет на резервоара")]
        public int TankCapacity { get; set; }

        [Display(Name = "Налично гориво")]
        public double InitialFuel { get; set; }

        [Display(Name = "Фирма")]
        public string CompanyName { get; set; }

        [Display(Name = "Шофьори")]
        public IEnumerable<UserCarViewModel> AllDrivers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Car, CarDetailsViewModel>()
                .ForMember(x => x.FuelType, y => y.MapFrom(x => x.FuelType.ToString()))
                .ForMember(x => x.AllDrivers, y => y.MapFrom(x => x.CarUsers));
        }
    }
}
