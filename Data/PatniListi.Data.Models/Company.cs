﻿namespace PatniListi.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Common.Models;

    public class Company : BaseModel<string> 
    {
        public Company()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Users = new HashSet<ApplicationUser>();
            this.Cars = new HashSet<Car>();
        }

        [Display(Name = "Име на фирма")]
        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [Display(Name = "Булстат")]
        [RegularExpression(@"[0-9]{10}")]
        public string Bulstat { get; set; }

        [Display(Name = "Номер по ДДС")]
        public string VatNumber { get; set; }

        [Display(Name = "Адрес")]
        [RegularExpression(@"^[#.0-9а-яА-Я\s,-]+$")]
        [StringLength(60)]
        public string Address { get; set; }

        [Display(Name = "Телефон")]
        [RegularExpression(@"^[+359[0-9 ]+$")]
        public string PhoneNumber { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
