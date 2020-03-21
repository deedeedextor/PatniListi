namespace PatniListi.Web.ViewModels.Administration.Users
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.Cars;

    public class UserDeleteViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Display(Name = "Потребител")]
        public string Username { get; set; }

        [Display(Name = "Имейл")]
        public string Email { get; set; }

        [Display(Name = "Име и фамилия")]
        public string FullName { get; set; }

        [Display(Name = "Фирма")]
        public string CompanyName { get; set; }

        [Display(Name = "Създаден на")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Коли")]
        public IEnumerable<CarUserViewModel> AllCars { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserDeleteViewModel>()
                .ForMember(x => x.AllCars, y => y.MapFrom(x => x.CarUsers));
        }
    }
}
