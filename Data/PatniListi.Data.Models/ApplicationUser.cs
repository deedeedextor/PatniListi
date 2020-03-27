// ReSharper disable VirtualMemberCallInConstructor
namespace PatniListi.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;
    using PatniListi.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CarUsers = new HashSet<CarUser>();
            this.Invoices = new HashSet<Invoice>();
            this.TransportWorkTickets = new HashSet<TransportWorkTicket>();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        [Display(Name = "Име и фамилия")]
        [Required]
        [RegularExpression(@"^[А-Я][а-я]+ [А-Я][а-я]+$")]
        public string FullName { get; set; }

        [Required]
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public DateTime? LastLoggingDate { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<CarUser> CarUsers { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual ICollection<TransportWorkTicket> TransportWorkTickets { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
