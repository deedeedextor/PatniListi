namespace PatniListi.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Common.Models;

    public class Car : BaseDeletableModel<string>
    {
        public Car()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Invoices = new HashSet<Invoice>();
            this.CarUsers = new HashSet<CarUser>();
            this.TransportWorkTickets = new HashSet<TransportWorkTicket>();
        }

        [Display(Name = "Модел")]
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Model { get; set; }

        [Display(Name = "Номер")]
        [Required]
        [RegularExpression(@"^[A-Z 0-9 A-Z]+$")]
        [StringLength(10, MinimumLength = 3)]
        public string LicensePlate { get; set; }

        [Display(Name = "Начални километри")]
        [Required]
        [Range(0, double.MaxValue)]
        public double StartKilometers { get; set; }

        [Display(Name = "Среден разход")]
        [Required]
        [Range(3, 30)]
        public int AverageConsumption { get; set; }

        [Display(Name = "Капацитет на резервоара")]
        [Required]
        [Range(20.00, 100.00)]
        public double TankCapacity { get; set; }

        [Display(Name = "Налично гориво")]
        [Required]
        [Range(0, 100.00)]
        public double InitialFuel { get; set; }

        public virtual ICollection<CarUser> CarUsers { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual ICollection<TransportWorkTicket> TransportWorkTickets { get; set; }
    }
}
