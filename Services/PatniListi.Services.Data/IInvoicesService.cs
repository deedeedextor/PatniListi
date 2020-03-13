namespace PatniListi.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Web.ViewModels.Models.Invoices;

    public interface IInvoicesService
    {
        /*Task CreateAsync(InvoiceInputViewModel input);*/

        Task EditAsync(string number, string date, string location, string fuelType, decimal price, double quantity, string username, string userId, string carId);

        Task<IQueryable<InvoiceViewModel>> GetAllAsync(string userId);
    }
}
