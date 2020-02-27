namespace PatniListi.Data.Models
{
    public class RouteTransportWorkTicket
    {
        public string RouteId { get; set; }

        public Route Route { get; set; }

        public string TransportWorkTicketId { get; set; }

        public TransportWorkTicket TransportWorkTicket { get; set; }
    }
}
