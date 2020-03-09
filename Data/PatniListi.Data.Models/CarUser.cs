namespace PatniListi.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CarUser
    {
        [Required]
        public string CarId { get; set; }

        public virtual Car Car { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
