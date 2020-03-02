namespace PatniListi.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Common.Models;

    public class Invoice : BaseDeletableModel<Guid>
    {
        [Display(Name = "Номер на фактура")]
        [Required]
        [RegularExpression(@"^[0-9]+$")]
        public string Number { get; set; }

        [Display(Name = "Дата на фактура")]
        public DateTime Date { get; set; }

        [Display(Name = "Място на зареждане")]
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Location { get; set; }

        [Display(Name = "Цена на гориво без ДДС")]
        [Required]
        [Range(0.10, 10.00)]
        public decimal Price { get; set; }

        [Display(Name = "Количество")]
        [Required]
        public double Quantity { get; set; }

        [Display(Name = "ДДС")]
        [Required]
        public double VAT { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string CarId { get; set; }

        public Car Car { get; set; }
    }
}
