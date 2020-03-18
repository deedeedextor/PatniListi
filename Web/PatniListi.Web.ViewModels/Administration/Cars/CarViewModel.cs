namespace PatniListi.Web.ViewModels.Administration.Cars
{
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class CarViewModel : IMapFrom<Car>
    {
        public string Id { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }

        public string FuelType { get; set; }
    }
}
