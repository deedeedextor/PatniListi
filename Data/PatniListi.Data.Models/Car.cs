namespace PatniListi.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Data.Common.Models;
    using PatniListi.Data.Models.Enums;

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
        [MaxLength(40)]
        public string Model { get; set; }

        [Display(Name = "Номер")]
        [Required]
        [RegularExpression(@"^[A-Z 0-9 A-Z]+$")]
        [Remote("ValidateLicensePlate", "Validation", "", AdditionalFields = "Id", ErrorMessage = "Регистрационният номер е зает.")]
        [MaxLength(10)]
        public string LicensePlate { get; set; }

        [Required]
        public Fuel FuelType { get; set; }

        [Display(Name = "Начални километри")]
        [Required]
        [Range(0, 1000000000.00)]
        public double StartKilometers { get; set; }

        [Display(Name = "Среден разход")]
        [Required]
        [Range(3, 20)]
        public int AverageConsumption { get; set; }

        [Display(Name = "Капацитет на резервоара")]
        [Required]
        [Range(20, 100)]
        public int TankCapacity { get; set; }

        [Display(Name = "Налично гориво")]
        [Required]
        [Range(0, 100.00)]
        public double InitialFuel { get; set; }

        [Required]
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<CarUser> CarUsers { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual ICollection<TransportWorkTicket> TransportWorkTickets { get; set; }
    }
}
