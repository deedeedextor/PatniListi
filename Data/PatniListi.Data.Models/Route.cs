namespace PatniListi.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Common.Models;

    public class Route : BaseModel<string>, IDeletableEntity
    {
        public Route()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsDeleted = false;
            this.RouteTransportWorkTickets = new HashSet<RouteTransportWorkTicket>();
        }

        [Display(Name = "Начална точка на тръгване")]
        [Required]
        public string StartPoint { get; set; }

        [Display(Name = "Крайна точка на тръгване")]
        [Required]
        public string EndPoint { get; set; }

        [Display(Name = "Разстояние")]
        [Required]
        public double Distance { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<RouteTransportWorkTicket> RouteTransportWorkTickets { get; set; }
    }
}
