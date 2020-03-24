namespace PatniListi.Web.ViewModels.Administration.Invoices
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class InvoiceDetailsViewModel : IMapFrom<Invoice>
    {
        private const decimal Vat = 1.2m;

        public string Id { get; set; }

        [Display(Name = "Номер на фактура")]
        public string Number { get; set; }

        [Display(Name = "Дата на фактура")]
        public DateTime Date { get; set; }

        [Display(Name = "Място на зареждане")]
        public string Location { get; set; }

        [Display(Name = "Заредил")]
        public string ApplicationUserFullName { get; set; }

        [Display(Name = "Цена на литър")]
        public decimal Price { get; set; }

        [Display(Name = "Количество")]
        public decimal Quantity { get; set; }

        [Display(Name = "Крайна сума")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Гориво")]
        public string CarFuelType { get; set; }

        [Display(Name = "Автомобил")]
        public string CarModel { get; set; }

        [Display(Name = "Въвел")]
        public string CreatedBy { get; set; }

        public string CarId { get; set; }

        public string CarCompanyId { get; set; }

        [Display(Name = "Фирма")]
        public string CarCompanyName { get; set; }

        public decimal PriceWithoutVat => this.Price / Vat;

        public decimal Sum => this.PriceWithoutVat * this.Quantity;

        public decimal SumWithVat => this.TotalPrice - this.Sum;
    }
}
