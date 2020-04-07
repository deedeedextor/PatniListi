namespace PatniListi.Web.ViewModels.Models.TransportWorkTickets
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Models.Routes;

    public class TransportWorkTicketDetailsViewModel : IMapFrom<TransportWorkTicket>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Display(Name = "За дата")]
        public DateTime Date { get; set; }

        [Display(Name = "Гориво")]
        public string CarFuelType { get; set; }

        [Display(Name = "Среден разход")]
        public int CarAverageConsumption { get; set; }

        public string CarCompanyId { get; set; }

        [Display(Name = "Шофьор")]
        public string ApplicationUserFullName { get; set; }

        [Display(Name = "Въведен")]
        public string CreatedBy { get; set; }

        public string CarId { get; set; }

        public string CarModel { get; set; }

        public string CarLicensePlate { get; set; }

        public double AllLiters { get; set; }

        public double AllTravelledDistance { get; set; }

        public double AllFuelConsumption { get; set; }

        [Display(Name = "Автомобил")]
        public string Car => $"{this.CarModel} - {this.CarLicensePlate}";

        [Display(Name = "Маршрут")]
        public IEnumerable<string> Route { get; set; }

        [Display(Name = "Растояние")]
        public string Distance { get; set; }

        [Display(Name = "Изминати километри")]
        public double TravelledDistance { get; set; }

        [Display(Name = "Издаден на")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Начален километраж")]
        public double StartKilometers { get; set; }

        [Display(Name = "Краен километраж")]
        public double EndKilometers { get; set; }

        [Display(Name = "Наличност")]
        public double FuelAvailability { get; set; }

        [Display(Name = "Разход")]
        public double FuelConsumption { get; set; }

        [Display(Name = "Остатък")]
        public double Residue { get; set; }

        public IEnumerable<SelectListItem> AllDrivers { get; set; }

        public IEnumerable<SelectListItem> AllRoutes { get; set; }

        [Display(Name = "Маршрути")]
        public IEnumerable<RouteTransportViewModel> Routes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<TransportWorkTicket, TransportWorkTicketDetailsViewModel>()
                .ForMember(x => x.Routes, y => y.MapFrom(x => x.RouteTransportWorkTickets));
        }
    }
}
