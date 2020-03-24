namespace PatniListi.Web.ViewModels.Administration.Invoices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class InvoiceEditViewModel : IMapFrom<Invoice>
    {
        public string Id { get; set; }

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
        [StringLength(AttributesConstraints.InvoiceLocationMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.InvoiceLocationMinLength)]
        public string Location { get; set; }

        [Display(Name = "Цена на литър")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.PriceMinLength, AttributesConstraints.PriceMaxLength, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public decimal Price { get; set; }

        [Display(Name = "Количество")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.QuantityMinLength, AttributesConstraints.QuantityMaxLength, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public decimal Quantity { get; set; }

        [Display(Name = "Обща сума")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.TotalPriceMinLength, AttributesConstraints.TotalPriceMaxLength, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Гориво")]
        public string CarFuelType { get; set; }

        [Display(Name = "Заредил")]
        public string FullName { get; set; }

        [Display(Name = "Автомобил")]
        public string CarModel { get; set; }

        [Display(Name = "Въвел")]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CarId { get; set; }

        public string CarCompanyId { get; set; }

        public IEnumerable<SelectListItem> AllDrivers { get; set; }
    }
}
