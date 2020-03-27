namespace PatniListi.Web.ViewModels.Administration.Companies
{
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class CompanyEditViewModel : IMapFrom<Company>
    {
        public string Id { get; set; }

        [Display(Name = "Име на фирма")]
        [Required(AllowEmptyStrings = false, ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.CompanyMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.CompanyMinLength)]
        public string Name { get; set; }

        [Display(Name = "Булстат")]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = AttributesErrorMessages.InvalidErrorMessage)]
        [StringLength(AttributesConstraints.BulstatMaxLength, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string Bulstat { get; set; }

        public string VatNumber => $"BG{this.Bulstat}";

        [Display(Name = "Адрес")]
        [RegularExpression(@"^[#.0-9а-яА-Я\s,-]+$", ErrorMessage = AttributesErrorMessages.InvalidErrorMessage)]
        [StringLength(AttributesConstraints.CompanyAddressMaxLength, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage, MinimumLength = AttributesConstraints.CompanyAddressMinLength)]
        public string Address { get; set; }

        [Display(Name = "Телефон")]
        [RegularExpression(@"^[+359 [0-9 ]+$", ErrorMessage = AttributesErrorMessages.InvalidErrorMessage)]
        public string PhoneNumber { get; set; }
    }
}
