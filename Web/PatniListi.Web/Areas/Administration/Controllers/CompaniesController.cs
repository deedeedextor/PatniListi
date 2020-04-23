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

        public async Task<IActionResult> Details(string id)
        {
            var company = await this.companiesService.GetDetailsAsync<DetailsCompanyViewModel>(id);

            if (company == null)
            {
                return this.NotFound();
            }

            return this.View(company);
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

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var company = this.companiesService.GetById(input.Id);

            company.Name = input.Name;
            company.Bulstat = input.Bulstat;
            company.VatNumber = input.VatNumber;
            company.PhoneNumber = input.PhoneNumber;
            company.Address = input.Address;
            company.CreatedOn = input.CreatedOn;

            await this.companiesService.EditAsync(company);
            return this.RedirectToAction("Details", "Companies", new { input.Id });
        }
    }
}
