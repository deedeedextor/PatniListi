namespace PatniListi.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Data.Models;
    using PatniListi.Services.Data;
    using PatniListi.Web.ViewModels;
    using PatniListi.Web.ViewModels.Models.Users;

    public class HomeController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public HomeController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction(nameof(this.IndexLoggedIn));
            }

            return this.View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> IndexLoggedIn()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var viewModel = await this.usersService.GetByIdAsync<ApplicationUserHomeViewModel>(userId);
            return this.View(viewModel);
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        public IActionResult Guidance()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
