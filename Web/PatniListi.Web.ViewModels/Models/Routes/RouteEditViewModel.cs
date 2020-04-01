namespace PatniListi.Web.ViewModels.Models.Routes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class RouteEditViewModel : IMapFrom<Route>
    {
        public string Id { get; set; }

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

        public DateTime CreatedOn { get; set; }

        public string Route => $"{this.StartPoint} - {this.EndPoint}";
    }
}
