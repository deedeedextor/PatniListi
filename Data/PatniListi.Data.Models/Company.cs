namespace PatniListi.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using PatniListi.Data.Common.Models;

    public class Company : BaseModel<string>, IDeletableEntity
    {
        public Company()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Users = new HashSet<ApplicationUser>();
            this.IsDeleted = false;
        }

        [Display(Name = "Име на фирма")]
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Display(Name = "Булстат")]
        [RegularExpression("[0-9]{10}")]
        public string Bulstat { get; set; }

        [Display(Name = "Номер по ДДС")]
        public string VatNumber => $"BG{this.Bulstat}";

        [Display(Name = "Телефон")]
        [Required]
        [RegularExpression("^[+359[0-9 ]+$")]
        public string PhoneNumber { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
