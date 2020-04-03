namespace PatniListi.Web.ViewModels.Models.Cars
{
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class CarViewModel : IMapFrom<Car>
    {
        public string Id { get; set; }

        [Display(Name = "Модел")]
        public string Model { get; set; }

        [Display(Name = "Регистрационен номер")]
        public string LicensePlate { get; set; }

        [Display(Name = "Вид гориво")]
        public string FuelType { get; set; }
    }
}
