﻿namespace PatniListi.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Common.Attributes;
    using PatniListi.Data.Common.Models;

    public class TransportWorkTicket : BaseModel<string>, IDeletableEntity
    {
        public TransportWorkTicket()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsDeleted = false;
            this.RouteTransportWorkTickets = new HashSet<RouteTransportWorkTicket>();
        }

        [Display(Name = "Дата на тръгване")]
        [Required]
        [MyDateTime(ErrorMessage = "Не може да въвеждате предходна дата!")]
        public DateTime Date { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string CarId { get; set; }

        public Car Car { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<RouteTransportWorkTicket> RouteTransportWorkTickets { get; set; }
    }
}