namespace PatniListi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

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

            if (company == null)
            {
                return this.NotFound();
            }

            return this.View(company);
        }

        // GET: Companies/Edit/5
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
    }
}
