namespace PatniListi.Web.ViewModels.Administration.Users
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class UserEditViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.UsernameMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.UsernameMinLength)]
        [Remote("ValidateUsername", "Drivers", ErrorMessage = "Потребителското име е заето.")]
        [Display(Name = "Потребителско име")]
        public string Username { get; set; }

        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [EmailAddress(ErrorMessage = AttributesErrorMessages.EmailErrorMessage)]
        [Remote("ValidateEmail", "Drivers", ErrorMessage = "Имейл адресът е зает.")]
        [Display(Name = "Имейл")]
        public string Email { get; set; }

        [Display(Name = "Име и Фамилия")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [RegularExpression(@"^[А-Я][а-я]+ [А-Я][а-я]+$", ErrorMessage = AttributesErrorMessages.FullNameErrorMessage)]
        public string FullName { get; set; }

        [Display(Name = "Фирма")]
        public string CompanyName { get; set; }

        public string CompanyId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
