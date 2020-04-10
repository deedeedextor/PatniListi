namespace PatniListi.Web.ViewModels.Models.Invoices
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class InvoiceEditViewModel : IMapFrom<Invoice>
    {
        public string Id { get; set; }

        public string CarId { get; set; }

        public string CarCompanyId { get; set; }

        public int CarTankCapacity { get; set; }

        [Display(Name = "Налично гориво в момента")]
        public double CurrentLiters { get; set; }

        [Display(Name = "Номер на фактура")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = AttributesErrorMessages.InvalidErrorMessage)]
        public string Number { get; set; }

        [Display(Name = "Дата на фактура")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public DateTime Date { get; set; }

        [Display(Name = "Място на зареждане")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [RegularExpression(@"^[А-Яа-я ]+$")]
        [StringLength(AttributesConstraints.InvoiceLocationMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.InvoiceLocationMinLength)]
        public string Location { get; set; }

        [Display(Name = "Цена на литър")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.PriceMinRange, AttributesConstraints.PriceMaxRange, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public decimal Price { get; set; }

        [Display(Name = "Количество")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.QuantityMinRange, AttributesConstraints.QuantityMaxRange, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        [Remote("ValidateQuantity", "Invoices", ErrorMessage = "Наличното и заредено количество гориво не трябва да надвишават капацитета на резервоара", AdditionalFields = "CurrentLiters, CarTankCapacity")]
        public double Quantity { get; set; }

        [Display(Name = "Обща сума")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.TotalPriceMinRange, AttributesConstraints.TotalPriceMaxRange, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
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

        public DateTime CreatedOn { get; set; }
    }
}
