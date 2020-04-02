namespace PatniListi.Web.ViewModels.Models.Invoices
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using PatniListi.Common;
    using PatniListi.Data.Common.Attributes;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class InvoiceInputViewModel : IMapTo<Invoice>, IMapFrom<Invoice>, IHaveCustomMappings
    {
        [Display(Name = "Номер на фактура")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [RegularExpression(@"^[0-9]+$")]
        public string Number { get; set; }

        [Display(Name = "Дата на фактура")]
        [MyDateTime]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = AttributesErrorMessages.DateТimeErrorMessage)]
        public DateTime Date { get; set; }

        [Display(Name = "Място на зареждане")]
        [Required]
        [StringLength(AttributesConstraints.InvoiceLocationMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage)]
        public string Location { get; set; }

        [Display(Name = "Вид гориво")]
        [Required]
        public string FuelType { get; set; }

        [Display(Name = "Цена на литър")]
        [Required]
        [Range(AttributesConstraints.PriceMinRange, AttributesConstraints.PriceMaxRange, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public decimal Price { get; set; }

        [Display(Name = "Количество")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public double Quantity { get; set; }

        [Display(Name = "Въведена от")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string CreatedBy { get; set; }

        [Display(Name = "Шофьор")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string ApplicationUserFullName { get; set; }

        [Display(Name = "За кола")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string CarModel { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<InvoiceInputViewModel, Invoice>()
                .ForPath(x => x.ApplicationUser.FullName, y => y.MapFrom(x => x.ApplicationUserFullName))
                .ForPath(x => x.Car.Model, y => y.MapFrom(x => x.CarModel))
                .ReverseMap();
        }
    }
}
