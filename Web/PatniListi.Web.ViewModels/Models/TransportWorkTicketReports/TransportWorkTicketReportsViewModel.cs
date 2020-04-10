namespace PatniListi.Web.ViewModels.Models.TransportWorkTicketReports
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class TransportWorkTicketReportsViewModel : IMapFrom<TransportWorkTicket>, IHaveCustomMappings
    {
        public TransportWorkTicketReportsViewModel()
        {
            this.Routes = new HashSet<TransportWorkTicketRoutesReportsViewModel>();
        }

        public string Id { get; set; }

        [Display(Name = "Дата на пътен лист")]
        public DateTime Date { get; set; }

        [Display(Name = "Пробег")]
        public double TravelledDistance { get; set; }

        [Display(Name = "Разход")]
        public double FuelConsumption { get; set; }

        [Display(Name = "Начален километраж")]
        public double StartKilometers { get; set; }

        [Display(Name = "Налично гориво")]
        public double FuelAvailability { get; set; }

        [Display(Name = "Остатък гориво")]
        public double Residue { get; set; }

        [Display(Name = "Краен километраж")]
        public double EndKilometers { get; set; }

        [Display(Name = "Шофьор")]
        public string ApplicationUserFullName { get; set; }

        [Display(Name = "Въведена на")]
        public DateTime CreatedOn { get; set; }

        public string CarId { get; set; }

        public string CarModel { get; set; }

        public string CarLicensePlate { get; set; }

        public string Car => $"{this.CarModel} - {this.CarLicensePlate}";

        public string CarCompanyId { get; set; }

        [Display(Name = "Фирма")]
        public string CarCompanyName { get; set; }

        public IEnumerable<TransportWorkTicketRoutesReportsViewModel> Routes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<TransportWorkTicket, TransportWorkTicketReportsViewModel>()
                .ForMember(x => x.Routes, y => y.MapFrom(x => x.RouteTransportWorkTickets));
        }
    }
}
