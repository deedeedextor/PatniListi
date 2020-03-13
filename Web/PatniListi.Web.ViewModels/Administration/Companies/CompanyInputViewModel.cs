namespace PatniListi.Web.ViewModels.Administration.Companies
{
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class CompanyInputViewModel : IMapTo<Company>
    {
        [Display(Name = "Име на фирма")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.CompanyMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage)]
        public string Name { get; set; }
    }
}
