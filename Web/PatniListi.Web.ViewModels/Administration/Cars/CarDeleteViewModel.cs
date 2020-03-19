namespace PatniListi.Web.ViewModels.Administration.Cars
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.Users;

    public class CarDeleteViewModel : IMapFrom<Car>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Display(Name = "Модел")]
        public string Model { get; set; }

        [Display(Name = "Номер")]
        public string LicensePlate { get; set; }

        [Display(Name = "Вид гориво")]
        public string FuelType { get; set; }

        [Display(Name = "Фирма")]
        public string CompanyName { get; set; }

        [Display(Name = "Шофьори")]
        public IEnumerable<UserCarViewModel> AllDrivers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Car, CarDeleteViewModel>()
                .ForMember(x => x.FuelType, y => y.MapFrom(x => x.FuelType.ToString()))
                .ForMember(x => x.AllDrivers, y => y.MapFrom(x => x.CarUsers));
        }
    }
}
