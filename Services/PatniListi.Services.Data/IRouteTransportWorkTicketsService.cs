namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRouteTransportWorkTicketsService
    {
        Task UpdateAsync(string transportWorkTicketId, string companyId, IEnumerable<string> collection);

        Task SetIsDeletedAsync(string transportWorkTicketId, string fullName);

        Task<List<T>> GetAllAsync<T>(string transportWorkTicketId);
    }
}
