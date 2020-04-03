namespace PatniListi.Web.ViewModels.Models.Invoices
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class InvoiceCarViewModel : IMapFrom<CarUser>
    {
        public string CarId { get; set; }

        [Display(Name = "Модел")]
        public string CarModel { get; set; }

        [Display(Name = "Вид гориво")]
        public string CarFuelType { get; set; }

        [Display(Name = "Заредил")]
        public string ApplicationUserFullName { get; set; }

        public IEnumerable<SelectListItem> AllDrivers { get; set; }
    }
}
