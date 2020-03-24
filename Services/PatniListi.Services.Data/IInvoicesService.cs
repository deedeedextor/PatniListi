namespace PatniListi.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Web.ViewModels.Administration.Invoices;

    public interface IInvoicesService
    {
        IQueryable<T> GetAll<T>(string id);

        Task<T> GetByIdAsync<T>(string id);

        Task CreateAsync(InvoiceInputViewModel input);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(InvoiceEditViewModel input, string fullName);

        Task<bool> DeleteAsync(string id, string fullName);
    }
}
