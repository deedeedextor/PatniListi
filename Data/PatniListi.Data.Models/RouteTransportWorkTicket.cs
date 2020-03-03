namespace PatniListi.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RouteTransportWorkTicket
    {
        [Required]
        public string RouteId { get; set; }

        public Route Route { get; set; }

        [Required]
        public string TransportWorkTicketId { get; set; }

        public TransportWorkTicket TransportWorkTicket { get; set; }
    }
}
