namespace PatniListi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Data;
    using PatniListi.Services.Mapping;
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

            var user = AutoMapperConfig.MapperInstance.Map<ApplicationUser>(input);
            var result = await this.userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                return this.View(input);
            }

            await this.userManager.AddToRoleAsync(user, "Driver");
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

            var user = await this.userManager.FindByIdAsync(input.Id);

            user.UserName = input.Username;
            user.Email = input.Email;
            user.PasswordHash = input.PasswordHash;
            user.FullName = input.FullName;
            user.CompanyId = input.CompanyId;
            user.LastLoggingDate = input.LastLoggingDate;
            user.NormalizedEmail = input.NormalizedEmail;
            user.NormalizedUserName = input.NormalizedUserName;
            user.CreatedOn = input.CreatedOn;
            user.ConcurrencyStamp = input.ConcurrencyStamp;
            user.SecurityStamp = input.SecurityStamp;

            var result = await this.userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return this.View(input);
            }

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
    }
}
