namespace PatniListi.Data.Models
{
    public class CarUser
    {
        public string CarId { get; set; }

        public Car Car { get; set; }

        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
