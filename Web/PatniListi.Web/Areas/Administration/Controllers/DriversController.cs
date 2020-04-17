namespace PatniListi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Data;
    using PatniListi.Web.Infrastructure;
    using PatniListi.Web.ViewModels.Administration.Users;

    public class DriversController : AdministrationController
    {
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public DriversController(IUsersService usersService, UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(int? pageNumber)
        {
            var companyId = this.userManager.GetUserAsync(this.User).Result?.CompanyId;

            if (companyId == null)
            {
                return this.NotFound();
            }

            var users = this.usersService
                .GetAll<UserViewModel>(companyId);

            return this.View(await PaginatedList<UserViewModel>.CreateAsync(users, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.usersService.GetDetailsAsync<UserDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            bool isCreated = await this.usersService.CreateAsync(input.Username, input.Email, input.Password, input.ConfirmPassword, input.FullName, input.CompanyId);

            if (!isCreated)
            {
                return this.View(input);
            }

            return this.RedirectToAction("All", "Drivers");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = await this.usersService.GetDetailsAsync<UserEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.usersService.EditAsync(input.Id, input.Username, input.Email, input.FullName, input.CompanyId, input.CompanyName, input.CreatedOn, input.ConcurrencyStamp);

            return this.RedirectToAction("All", "Drivers");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var viewModel = await this.usersService.GetDetailsAsync<UserDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var currentUserFullName = this.userManager.GetUserAsync(this.User).Result?.FullName;

            var deleted = await this.usersService.DeleteAsync(id, currentUserFullName);

            if (!deleted)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("All", "Drivers");
        }

        public IActionResult ValidateUsername(string username, string id)
        {
            bool exists = this.usersService.IsUsernameInUse(username);

            if (exists && id == null)
            {
                return this.Json(data: "Потребителското име е заето.");
            }
            else if (exists && id != null)
            {
                if (username == this.usersService.GetUsernameById(id))
                {
                    return this.Json(data: true);
                }
                else
                {
                    return this.Json(data: "Номерът на фактурата е зает.");
                }
            }

            return this.Json(data: true);
        }

        public IActionResult ValidateEmail(string email, string id)
        {
            bool exists = this.usersService.IsEmailInUse(email);

            if (exists && id == null)
            {
                return this.Json(data: "Имейл адресът е зает.");
            }
            else if (exists && id != null)
            {
                if (email == this.usersService.GetEmailById(id))
                {
                    return this.Json(data: true);
                }
                else
                {
                    return this.Json(data: "Имейл адресът е зает.");
                }
            }

            return this.Json(data: true);
        }
    }
}
