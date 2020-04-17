namespace PatniListi.Web.ViewModels.Models.InvoiceReports
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Common;

    public class InvoiceReportsIndexViewModel
    {
        public InvoiceReportsIndexViewModel()
        {
            this.Invoices = new HashSet<InvoiceReportsViewModel>();
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
        [Remote("ValidatePeriodBetweenDates", "InvoiceReports", AdditionalFields = "From", ErrorMessage = "Избраният период не може да бъде по-голям от месец.")]
        public DateTime To { get; set; }

        public IEnumerable<InvoiceReportsViewModel> Invoices { get; set; }
    }
}
