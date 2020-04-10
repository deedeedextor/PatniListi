namespace PatniListi.Web.ViewModels.Models.TransportWorkTicketReports
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Common;

    public class TransportWorkTicketReportsIndexViewModel
    {
        public TransportWorkTicketReportsIndexViewModel()
        {
            this.TransportWorkTickets = new HashSet<TransportWorkTicketReportsViewModel>();
        }

        [Display(Name = "Автомобил")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string CarId { get; set; }

        public IEnumerable<SelectListItem> AllCars { get; set; }

        [Display(Name = "От дата")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public DateTime From { get; set; }

        [Display(Name = "До дата")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public DateTime To { get; set; }

        public IEnumerable<TransportWorkTicketReportsViewModel> TransportWorkTickets { get; set; }
    }
}
