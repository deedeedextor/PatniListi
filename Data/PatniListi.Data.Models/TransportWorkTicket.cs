namespace PatniListi.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Common.Attributes;
    using PatniListi.Data.Common.Models;

    public class TransportWorkTicket : BaseDeletableModel<string>
    {
        public TransportWorkTicket()
        {
            this.Id = Guid.NewGuid().ToString();
            this.RouteTransportWorkTickets = new HashSet<RouteTransportWorkTicket>();
        }

        [Display(Name = "Дата на тръгване")]
        [Required]
        [MyDateTime]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string CarId { get; set; }

        public virtual Car Car { get; set; }

        [Display(Name = "Начални километри")]
        [Required]
        [Range(0, 1000000000.00)]
        public double StartKilometers { get; set; }

        [Display(Name = "Крайни километри")]
        [Required]
        [Range(1, 1000000000.00)]
        public double EndKilometers { get; set; }

        [Display(Name = "Изминати километри")]
        [Required]
        [Range(1, 1000000000.00)]
        public double TravelledDistance { get; set; }

        [Display(Name = "Разход")]
        [Required]
        [Range(1, 1000000000.00)]
        public double FuelConsumption { get; set; }

        [Display(Name = "Остатък")]
        [Required]
        [Range(1, 1000000000.00)]
        public double Residue { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public virtual ICollection<RouteTransportWorkTicket> RouteTransportWorkTickets { get; set; }
    }
}
