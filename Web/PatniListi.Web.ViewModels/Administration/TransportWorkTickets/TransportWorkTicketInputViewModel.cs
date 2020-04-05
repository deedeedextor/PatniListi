namespace PatniListi.Web.ViewModels.Administration.TransportWorkTickets
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Common;
    using PatniListi.Web.ViewModels.Models.Routes;

    public class TransportWorkTicketInputViewModel
    {
        [Display(Name = "За дата")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public DateTime Date { get; set; }

        [Display(Name = "Гориво")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string CarFuelType { get; set; }

        [Display(Name = "Среден разход")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public int CarAverageConsumption { get; set; }

        public int CarTankCapacity { get; set; }

        public double CarInitialFuel { get; set; }

        public string CarCompanyId { get; set; }

        [Display(Name = "Шофьор")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string ApplicationUserFullName { get; set; }

        [Display(Name = "Въведен")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string CreatedBy { get; set; }

        public string CarId { get; set; }

        public string CarModel { get; set; }

        public string CarLicensePlate { get; set; }

        public double CarStartKilometers { get; set; }

        public int CarTransportWorkTicketsCount { get; set; }

        public double AllLiters { get; set; }

        public double AllTravelledDistance { get; set; }

        public double AllFuelConsumption { get; set; }

        [Display(Name = "Автомобил")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string Car => $"{this.CarModel} - {this.CarLicensePlate}";

        [Display(Name = "Маршрут")]
        public IEnumerable<string> Route { get; set; }

        [Display(Name = "Растояние")]
        public string Distance { get; set; }

        [Display(Name = "Изминати километри")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.PositiveMinRange, AttributesConstraints.PositiveMaxRange, ErrorMessage = AttributesErrorMessages.InvalidPositiveRangeErrorMessage)]
        public double TravelledDistance { get; set; }

        [Display(Name = "Издаден на")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Начален километраж")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.StartKilometersMinRange, AttributesConstraints.StartKilometersMaxRange, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double StartKilometers { get; set; }

        [Display(Name = "Краен километраж")]
        [Required]
        [Range(AttributesConstraints.PositiveMinRange, AttributesConstraints.PositiveMaxRange, ErrorMessage = AttributesErrorMessages.InvalidPositiveRangeErrorMessage)]
        public double EndKilometers { get; set; }

        [Display(Name = "Наличност")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public double FuelAvailability { get; set; }

        [Display(Name = "Разход")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.PositiveMinRange, AttributesConstraints.PositiveMaxRange, ErrorMessage = AttributesErrorMessages.InvalidPositiveRangeErrorMessage)]
        public double FuelConsumption { get; set; }

        [Display(Name = "Остатък")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.PositiveMinRange, AttributesConstraints.PositiveMaxRange, ErrorMessage = AttributesErrorMessages.InvalidPositiveRangeErrorMessage)]
        public double Residue { get; set; }

        public IEnumerable<SelectListItem> AllDrivers { get; set; }

        public IEnumerable<SelectListItem> AllRoutes { get; set; }

        [Display(Name = "Маршрути")]
        public ICollection<RouteViewModel> Routes { get; set; }
    }
}
