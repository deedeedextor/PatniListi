namespace PatniListi.Web.ViewModels.Administration.Invoices
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class InvoiceViewModel : IMapFrom<Invoice>
    {
        public string Id { get; set; }

        [Display(Name = "Номер на фактура")]
        public string Number { get; set; }

        [Display(Name = "Дата на фактура")]
        public DateTime Date { get; set; }

        [Display(Name = "Място на зареждане")]
        public string Location { get; set; }

        [Display(Name = "Място на зареждане")]
        public string ApplicationUserFullName { get; set; }

        [Display(Name = "Сума")]
        public string TotalPrice { get; set; }

        public string CarId { get; set; }
    }
}
