namespace PatniListi.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Data;
    using PatniListi.Web.Infrastructure;
    using PatniListi.Web.ViewModels.Administration.Cars;

    public class CarsController : AdministrationController
    {
        private readonly ICarsService carsService;
        private readonly UserManager<ApplicationUser> userManager;

        public CarsController(ICarsService carsService, UserManager<ApplicationUser> userManager)
        {
            this.carsService = carsService;
            this.userManager = userManager;
        }

        // GET: Cars
        public async Task<IActionResult> All(int? pageNumber)
        {
            var companyId = this.userManager.GetUserAsync(this.User).Result?.CompanyId;
            var cars = this.carsService
                .GetAll<CarViewModel>(companyId);

            return this.View(await PaginatedList<CarViewModel>.CreateAsync(cars, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        // GET: Cars/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            var viewModel = new CarInputViewModel
            {
                AllTypes = this.carsService.GetFuelType(),
            };

            return this.View(viewModel);
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.AllTypes = this.carsService.GetFuelType();
                return this.View(input);
            }

            await this.carsService.CreateAsync(input);

            return this.RedirectToAction("All", "Cars");
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cars/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}