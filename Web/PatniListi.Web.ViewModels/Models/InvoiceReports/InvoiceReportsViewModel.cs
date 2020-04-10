namespace PatniListi.Web.ViewModels.Models.InvoiceReports
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class InvoiceReportsViewModel : IMapFrom<Invoice>, IHaveCustomMappings
    {
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
        public double Quantity { get; set; }

        [Display(Name = "Крайна сума")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Гориво")]
        public string CarFuelType { get; set; }

        [Display(Name = "Въвел")]
        public string CreatedBy { get; set; }

        [Display(Name = "Въведена на")]
        public DateTime CreatedOn { get; set; }

        public string CarId { get; set; }

        public string CarModel { get; set; }

        public string CarLicensePlate { get; set; }

        public string Car => $"{this.CarModel} - {this.CarLicensePlate}";

        public string CarCompanyId { get; set; }

        [Display(Name = "Фирма")]
        public string CarCompanyName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Invoice, InvoiceReportsViewModel>()
                .ForMember(x => x.CarFuelType, y => y.MapFrom(x => x.Car.FuelType.ToString()));
        }
    }
}
