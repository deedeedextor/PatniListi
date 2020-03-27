namespace PatniListi.Web.ViewModels.Administration.Invoices
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class InvoiceDeleteViewModel : IMapFrom<Invoice>
    {
        public string Id { get; set; }

        [Display(Name = "Номер на фактура")]
        public string Number { get; set; }

        [Display(Name = "Дата на фактура")]
        public DateTime Date { get; set; }

        [Display(Name = "Място на зареждане")]
        public string Location { get; set; }

        [Display(Name = "Цена на литър")]
        public decimal Price { get; set; }

        [Display(Name = "Количество")]
        public double Quantity { get; set; }

        [Display(Name = "Обща сума")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Гориво")]
        public string CarFuelType { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Заредил")]
        public string ApplicationUserFullName { get; set; }

        [Display(Name = "Автомобил")]
        public string CarModel { get; set; }

        [Display(Name = "Въвел")]
        public string CreatedBy { get; set; }

        public string CarId { get; set; }

        public string CarCompanyId { get; set; }
    }
}
