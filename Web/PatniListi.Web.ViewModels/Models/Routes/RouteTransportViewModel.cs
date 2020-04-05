namespace PatniListi.Web.ViewModels.Models.Routes
{
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class RouteTransportViewModel : IMapFrom<RouteTransportWorkTicket>
    {
        public string RouteId { get; set; }

        [Display(Name = "Начална точка на тръгване")]
        public string RouteStartPoint { get; set; }

        [Display(Name = "Крайна точка на пристигане")]
        public string RouteEndPoint { get; set; }

        [Display(Name = "Разстояние")]
        public double RouteDistance { get; set; }

        public string Route => $"{this.RouteStartPoint} - {this.RouteEndPoint}";

        public string TransportWorkTicketId { get; set; }
    }
}
