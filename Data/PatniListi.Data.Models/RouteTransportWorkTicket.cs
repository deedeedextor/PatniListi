namespace PatniListi.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RouteTransportWorkTicket
    {
        [Required]
        public string RouteId { get; set; }

        public virtual Route Route { get; set; }

        [Required]
        public string TransportWorkTicketId { get; set; }

        public virtual TransportWorkTicket TransportWorkTicket { get; set; }
    }
}
