namespace PatniListi.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Common.Models;

    public class Route : BaseDeletableModel<Guid>
    {
        public Route()
        {
            this.RouteTransportWorkTickets = new HashSet<RouteTransportWorkTicket>();
        }

        [Display(Name = "Начална точка на тръгване")]
        [Required]
        public string StartPoint { get; set; }

        [Display(Name = "Крайна точка на пристигане")]
        [Required]
        public string EndPoint { get; set; }

        [Display(Name = "Разстояние")]
        [Required]
        public double Distance { get; set; }

        public ICollection<RouteTransportWorkTicket> RouteTransportWorkTickets { get; set; }
    }
}
