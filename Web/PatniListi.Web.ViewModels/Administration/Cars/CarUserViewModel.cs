﻿namespace PatniListi.Web.ViewModels.Administration.Cars
{
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class CarUserViewModel : IMapFrom<CarUser>
    {
        public string CarId { get; set; }

        public string CarModel { get; set; }

        public string UserId { get; set; }

        public string ApplicationUserFullName { get; set; }
    }
}
