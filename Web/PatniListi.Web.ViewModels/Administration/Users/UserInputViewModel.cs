﻿namespace PatniListi.Web.ViewModels.Administration.Users
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class UserInputViewModel : IMapTo<ApplicationUser>
    {
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.UsernameMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.UsernameMinLength)]
        [Remote("ValidateUsername", "Validation", "", AdditionalFields ="Id", ErrorMessage = "Потребителското име е заето.")]
        [Display(Name = "Потребителско име")]
        public string Username { get; set; }

        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [EmailAddress(ErrorMessage = AttributesErrorMessages.EmailErrorMessage)]
        [Remote("ValidateEmail", "Validation", "", AdditionalFields = "Id", ErrorMessage = "Имейл адресът е зает.")]
        [Display(Name = "Имейл")]
        public string Email { get; set; }

        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.PasswordMaxLength, ErrorMessage = AttributesErrorMessages.PasswordErrorMessage, MinimumLength = AttributesConstraints.PasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди парола")]
        [Compare("Password", ErrorMessage = AttributesErrorMessages.ComparePasswordErrorMessage)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Име и Фамилия")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [RegularExpression(@"^[А-Я][а-я]+ [А-Я][а-я]+$", ErrorMessage = AttributesErrorMessages.FullNameErrorMessage)]
        public string FullName { get; set; }

        public string CompanyId { get; set; }

        public DateTime LastLoggingDate { get; set; }
    }
}
