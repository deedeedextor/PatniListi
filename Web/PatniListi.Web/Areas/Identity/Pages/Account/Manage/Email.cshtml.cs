namespace PatniListi.Web.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using PatniListi.Data.Models;

    public partial class EmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;

        public EmailModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }

        [Display(Name = "Потребител")]
        public string Username { get; set; }

        [Display(Name = "Имейл адрес")]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Несъществуващ потребител '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Несъществуващ потребител '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var email = await this.userManager.GetEmailAsync(user);
            if (this.Input.NewEmail != email)
            {
                var userId = await this.userManager.GetUserIdAsync(user);
                var code = await this.userManager.GenerateChangeEmailTokenAsync(user, this.Input.NewEmail);
                var callbackUrl = this.Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = this.Input.NewEmail, code = code },
                    protocol: this.Request.Scheme);
                await this.emailSender.SendEmailAsync(
                    this.Input.NewEmail,
                    "Confirm your email",
                    $"Моля, потвърдете акаунта си, като <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> щракнете тук </a>.");

                this.StatusMessage = "Изпратена е връзка за промяна на имейл адреса. Моля, проверете си имейла.";
                return this.RedirectToPage();
            }

            this.StatusMessage = "Имейл адресът ви е непроменен.";
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Несъществуващ потрежител '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var userId = await this.userManager.GetUserIdAsync(user);
            var email = await this.userManager.GetEmailAsync(user);
            var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = this.Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: this.Request.Scheme);
            await this.emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Моля, потвърдете акаунта си, като <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> щракнете тук </a>.");

            this.StatusMessage = "Изпратен е имейл за потвърждение. Моля, проверете си имейла.";
            return this.RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var email = await this.userManager.GetEmailAsync(user);
            this.Email = email;

            this.Input = new InputModel
            {
                NewEmail = email,
            };

            this.IsEmailConfirmed = await this.userManager.IsEmailConfirmedAsync(user);
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Моля, въведете имейл адрес.")]
            [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
            [Display(Name = "Нов имейл адрес")]
            public string NewEmail { get; set; }
        }
    }
}
