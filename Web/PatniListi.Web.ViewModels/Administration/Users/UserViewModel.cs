namespace PatniListi.Web.ViewModels.Administration.Users
{
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        [Display(Name = "Потребител")]
        public string Username { get; set; }

        [Display(Name = "Имейл")]
        public string Email { get; set; }

        [Display(Name = "Име и фамилия")]
        public string FullName { get; set; }
    }
}
