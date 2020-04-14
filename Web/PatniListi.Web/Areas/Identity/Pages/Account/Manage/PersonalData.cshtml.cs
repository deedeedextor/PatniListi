namespace PatniListi.Web.Areas.Identity.Pages.Account.Manage
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using PatniListi.Data.Models;

    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public PersonalDataModel(
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Несъществуващ потребител '{this.userManager.GetUserId(this.User)}'.");
            }

            return this.Page();
        }
    }
}
