namespace PatniListi.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using PatniListi.Data.Models;
    using PatniListi.Services.Data;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly ICompaniesService companiesService;
        private readonly IUsersService usersService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ICompaniesService companiesService,
            IUsersService usersService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.companiesService = companiesService;
            this.usersService = usersService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.Response.Redirect("/");
            }

            this.ReturnUrl = returnUrl;

            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                var companyId = await this.companiesService.GetByNameAsync(this.Input.CompanyName);

                if (companyId == null)
                {
                    companyId = await this.companiesService.CreateAsync(this.Input.CompanyName);
                }

                var user = new ApplicationUser { UserName = this.Input.Username, Email = this.Input.Email, FullName = this.Input.FullName, CompanyId = companyId };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);

                if (result.Succeeded)
                {
                    if (this.companiesService.GetUsersCount(this.Input.CompanyName) == 1)
                    {
                        await this.usersService.AddRoleToUser(user.Id, "Admin");
                    }
                    else
                    {
                        await this.usersService.AddRoleToUser(user.Id, "Driver");
                    }

                    this.logger.LogInformation("User created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: this.Request.Scheme);

                    await this.emailSender.SendEmailAsync(this.Input.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email });
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Полето е задължително.")]
            [StringLength(15, ErrorMessage = "Полето {0} трябва да бъде с дължина между {2} и {1} символа.", MinimumLength = 3)]
            [Display(Name = "Потребителско име")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Полето е задължително.")]
            [EmailAddress]
            [Display(Name = "Имейл")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Полето е задължително.")]
            [StringLength(100, ErrorMessage = "{0}та трябва да бъде с дължина между {2} и {1} символа.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърди парола")]
            [Compare("Password", ErrorMessage = "Полето за парола и потвърди парола трябва да съвпадат.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Име и Фамилия")]
            [Required(ErrorMessage = "Полето е задължително.")]
            [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$", ErrorMessage = "Невалидно име и фамилия.")]
            public string FullName { get; set; }

            [Display(Name = "Име на фирма")]
            [Required(ErrorMessage = "Полето е задължително.")]
            [StringLength(20, MinimumLength = 2, ErrorMessage = "Името на фирмата трябва да бъде между {2} и {1} символа.")]
            public string CompanyName { get; set; }
        }
    }
}
