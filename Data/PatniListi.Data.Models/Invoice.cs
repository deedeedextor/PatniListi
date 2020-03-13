namespace PatniListi.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Common.Attributes;
    using PatniListi.Data.Common.Models;

    public class Invoice : BaseDeletableModel<string>
    {
        public Invoice()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Display(Name = "Номер на фактура")]
        [Required]
        [RegularExpression(@"^[0-9]+$")]
        public string Number { get; set; }

        [Display(Name = "Дата на фактура")]
        [MyDateTime]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }

        [Display(Name = "Място на зареждане")]
        [Required]
        [StringLength(20)]
        public string Location { get; set; }

        [Display(Name = "Цена на литър")]
        [Required]
        [Range(0.10, 10.00)]
        public decimal Price { get; set; }

        [Display(Name = "Количество")]
        [Required]
        [Range(1, double.MaxValue)]
        public double Quantity { get; set; }

        [Display(Name = "Гориво")]
        [Required]
        public string FuelType { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string CarId { get; set; }

        public virtual Car Car { get; set; }

        [Required]
        public string CreatedBy { get; set; }
    }
}
