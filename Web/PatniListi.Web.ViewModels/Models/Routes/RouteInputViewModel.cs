namespace PatniListi.Web.ViewModels.Models.Routes
{
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Common;

    public class RouteInputViewModel
    {
        [Display(Name = "Начална точка на тръгване")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [RegularExpression(@"^[А-Яа-я ]+$", ErrorMessage = AttributesErrorMessages.InvalidErrorMessage)]
        [StringLength(AttributesConstraints.RouteMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.RouteMinLength)]
        public string StartPoint { get; set; }

        [Display(Name = "Крайна точка на пристигане")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [RegularExpression(@"^[А-Яа-я ]+$", ErrorMessage = AttributesErrorMessages.InvalidErrorMessage)]
        [StringLength(AttributesConstraints.RouteMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.RouteMinLength)]
        public string EndPoint { get; set; }

        [Display(Name = "Разстояние")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public double Distance { get; set; }

        public string ReturnUrl { get; set; }
    }
}
