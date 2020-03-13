namespace PatniListi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Services.Data;
    using PatniListi.Web.ViewModels.Administration.Companies;

    public class CompaniesController : AdministrationController
    {
        private readonly ICompaniesService companiesService;

        public CompaniesController(ICompaniesService companiesService)
        {
            this.companiesService = companiesService;
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var company = await this.companiesService.GetDetailsAsync<DetailsCompanyViewModel>(id);

            return this.View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Companies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                return this.RedirectToAction();
            }
            catch
            {
                return this.View();
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var company = await this.companiesService.GetDetailsAsync<CompanyEditViewModel>(id);

            if (company == null)
            {
                return this.NotFound();
            }

            return this.View(company);
        }

        // POST: Companies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.companiesService.EditAsync(input);
            return this.RedirectToAction("Details", "Companies", new { input.Id });
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int id)
        {
            return this.View();
        }

        // POST: Companies/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return this.RedirectToAction();
            }
            catch
            {
                return this.View();
            }
        }
    }
}
