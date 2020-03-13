namespace PatniListi.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Web.ViewModels.Models.Invoices;

    public class InvoicesService : IInvoicesService
    {
        private readonly IRepository<Invoice> invoicesRepository;

        public InvoicesService(IRepository<Invoice> invoicesRepository)
        {
            this.invoicesRepository = invoicesRepository;
        }

        /*public async Task<bool> CreateAsync<T>(InvoiceInputViewModel input)
        {
            var invoice = new Invoice
            {
                Number = input.Number,
                Date = input.Date,
                Location = input.Location,
                Quantity = input.Quantity,
                Price = input.Price,
            };

            throw new System.NotImplementedException();
        }*/

        public Task EditAsync(string number, string date, string location, string fuelType, decimal price, double quantity, string username, string userId, string carId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IQueryable<InvoiceViewModel>> GetAllAsync(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
