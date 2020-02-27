// ReSharper disable VirtualMemberCallInConstructor
namespace PatniListi.Data.Models
{
    using System;

    using Microsoft.AspNetCore.Identity;
    using PatniListi.Data.Common.Models;

    public class ApplicationRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public ApplicationRole()
            : this(null)
        {
        }

        public ApplicationRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsDeleted = false;
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
