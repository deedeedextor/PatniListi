namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PatniListi.Web.ViewModels.Models.Routes;

    public interface IRouteTransportWorkTicketsService
    {
        Task UpdateAsync(string transportWorkTicketId, string companyId, IEnumerable<RouteTransportViewModel> collection);

        Task SetIsDeletedAsync(string id, string fullName);

        Task<List<T>> GetAllAsync<T>(string id);

        Task<T> GetByIdAsync<T>(string id);
    }
}
