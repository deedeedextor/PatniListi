namespace PatniListi.Web.ViewModels.Administration.TransportWorkTickets
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class TransportWorkTicketViewModel : IMapFrom<TransportWorkTicket>
    {
        public string Id { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [Display(Name = "Шофьор")]
        public string ApplicationUserFullName { get; set; }

        [Display(Name = "Въведен")]
        public string CreatedBy { get; set; }

        public string CarId { get; set; }

        public string CarModel { get; set; }

        public string CarLicensePlate { get; set; }

        [Display(Name = "Автомобил")]
        public string Car => $"{this.CarModel} - {this.CarLicensePlate}";
    }
}
