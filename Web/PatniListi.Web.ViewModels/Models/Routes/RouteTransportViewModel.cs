namespace PatniListi.Web.ViewModels.Models.Routes
{
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class RouteTransportViewModel : IMapFrom<RouteTransportWorkTicket>
    {
        public string RouteId { get; set; }

        public virtual Route Route { get; set; }

        public string TransportWorkTicketId { get; set; }

        public virtual TransportWorkTicket TransportWorkTicket { get; set; }
    }
}
