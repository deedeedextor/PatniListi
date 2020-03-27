namespace PatniListi.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Common.Models;

    public class ContactFormEntry : BaseModel<int>
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Content { get; set; }

        [Required]
        public string Ip { get; set; }
    }
}
