namespace PatniListi.Web.Areas.Identity.Pages.Account
{
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using PatniListi.Data.Models;

    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return this.RedirectToPage("/Index");
            }

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound($"Несъществуващ потребител '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await this.userManager.ConfirmEmailAsync(user, code);
            this.StatusMessage = result.Succeeded ? "Благодарим ви, че потвърдихте имейла си. " : " Грешка при потвърждаването на имейла ви.";
            return this.Page();
        }
    }
}
