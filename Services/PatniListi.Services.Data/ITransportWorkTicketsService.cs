namespace PatniListi.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Web.ViewModels.Administration.TransportWorkTickets;

    public interface ITransportWorkTicketsService
    {
        public IQueryable<T> GetAll<T>(string id);

        Task CreateAsync(TransportWorkTicketInputViewModel input);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(TransportWortkTicketEditViewModel input, string fullName);

        Task<bool> DeleteAsync(string id, string fullName);
    }
}
