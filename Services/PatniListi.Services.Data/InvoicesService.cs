namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.Invoices;
    using PatniListi.Web.ViewModels.Administration.Users;

    public class InvoicesService : IInvoicesService
    {
        private const string InvalidUserIdErrorMessage = "Не съществуваща фактура.";

        private readonly IRepository<Invoice> invoicesRepository;
        private readonly IUsersService usersService;

        public InvoicesService(IRepository<Invoice> invoicesRepository, IUsersService usersService)
        {
            this.invoicesRepository = invoicesRepository;
            this.usersService = usersService;
        }

        public async Task CreateAsync(InvoiceInputViewModel input)
        {
            var user = await this.usersService.GetByNameAsync<UserViewModel>(input.FullName, input.CarCompanyId);

            var invoice = new Invoice
            {
                Number = input.Number,
                Date = input.Date,
                FuelType = input.CarFuelType,
                Location = input.Location,
                Price = input.Price,
                Quantity = input.Quantity,
                TotalPrice = input.TotalPrice,
                UserId = user.Id,
                CarId = input.CarId,
                CreatedBy = input.CreatedBy,
            };

            await this.invoicesRepository.AddAsync(invoice);
            await this.invoicesRepository.SaveChangesAsync();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task EditAsync(InvoiceEditViewModel input)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<T> GetAll<T>(string id)
        {
            return this.invoicesRepository
                .All()
                .Where(c => c.CarId == id)
                .OrderByDescending(c => c.CreatedOn)
                .To<T>();
        }

        public IEnumerable<SelectListItem> GetAll(string companyId)
        {
            return this.usersService.GetAll(companyId);
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            var viewModel = await this.invoicesRepository
                .All()
                .Where(c => c.CarId == id)
                .To<T>()
                .SingleOrDefaultAsync();

            if (viewModel == null)
            {
                throw new ArgumentNullException(InvalidUserIdErrorMessage, id);
            }

            return viewModel;
        }

        public Task<T> GetDetailsAsync<T>(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
