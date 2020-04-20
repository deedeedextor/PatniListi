namespace PatniListi.Web.ViewModels.Administration.Cars
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class CarEditViewModel : IMapFrom<Car>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Display(Name = "Модел")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.CarModelMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.CarModelMinLength)]
        [RegularExpression(@"^[А-Я][а-я]+ [А-Я][а-я]+$", ErrorMessage = AttributesErrorMessages.InvalidErrorMessage)]
        public string Model { get; set; }

        [Display(Name = "Номер")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [RegularExpression(@"^[A-Z 0-9 A-Z]+$")]
        [StringLength(AttributesConstraints.LicensePlateMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.LicensePlateMinLength)]
        [Remote("ValidateLicensePlate", "Validation", "", AdditionalFields = "Id", ErrorMessage = "Регистрационният номер е зает.")]
        public string LicensePlate { get; set; }

        [Display(Name = "Вид гориво")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string FuelType { get; set; }

        [Display(Name = "Начални километри")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.StartKilometersMinRange, AttributesConstraints.StartKilometersMaxRange, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double StartKilometers { get; set; }

        [Display(Name = "Среден разход")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.AverageConsumptionMinRange, AttributesConstraints.AverageConsumptionMaxRange, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public int AverageConsumption { get; set; }

        [Display(Name = "Капацитет на резервоара")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.CapacityFuelMin, AttributesConstraints.CapacityFuelMax, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public int TankCapacity { get; set; }

        [Display(Name = "Налично гориво")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.InitialFuelMinRange, AttributesConstraints.InitialFuelMaxRange, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double InitialFuel { get; set; }

        [Display(Name = "Шофьори")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public IEnumerable<string> FullName { get; set; }

        [Display(Name = "Фирма")]
        public string CompanyName { get; set; }

        public string CompanyId { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<CarUserViewModel> Drivers { get; set; }

        public IEnumerable<SelectListItem> AllDrivers { get; set; }

        public IEnumerable<SelectListItem> AllTypes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Car, CarEditViewModel>()
                .ForMember(x => x.FuelType, y => y.MapFrom(x => x.FuelType.ToString()))
                .ForMember(x => x.Drivers, y => y.MapFrom(x => x.CarUsers));
        }
    }
}
