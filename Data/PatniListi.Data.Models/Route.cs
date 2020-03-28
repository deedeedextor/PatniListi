namespace PatniListi.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Common.Models;

    public class Route : BaseDeletableModel<string>
    {
        public Route()
        {
            this.Id = Guid.NewGuid().ToString();
            this.RouteTransportWorkTickets = new HashSet<RouteTransportWorkTicket>();
        }

        [Display(Name = "Начална точка на тръгване")]
        [Required]
        [RegularExpression(@"^[А-Яа-я] +$")]
        [StringLength(60)]
        public string StartPoint { get; set; }

        [Display(Name = "Крайна точка на пристигане")]
        [Required]
        [RegularExpression(@"^[А-Яа-я] +$")]
        [StringLength(60)]
        public string EndPoint { get; set; }

        [Display(Name = "Разстояние")]
        [Required]
        public double Distance { get; set; }

        public virtual ICollection<RouteTransportWorkTicket> RouteTransportWorkTickets { get; set; }
    }
}
