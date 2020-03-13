namespace PatniListi.Data.Models
{
    using PatniListi.Data.Common.Models;

    public class ContactFormEntry : BaseModel<int>
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Ip { get; set; }
    }
}
