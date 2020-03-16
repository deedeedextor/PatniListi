namespace PatniListi.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using PatniListi.Data.Models;

    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.logger = logger;
            this.userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user != null)
            {
                // save the last login time
                user.LastLoggingDate = DateTime.UtcNow;

                await this.userManager.UpdateAsync(user);
            }

            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return this.LocalRedirect(returnUrl);
            }
            else
            {
                return this.RedirectToPage();
            }
        }
    }
}
