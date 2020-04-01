namespace PatniListi.Web.ViewModels.Models.Routes
{
    using System.ComponentModel.DataAnnotations;

    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class RouteDetailsViewModel : IMapFrom<Route>
    {
        public string Id { get; set; }

        [Display(Name = "Начална точка на тръгване")]
        public string StartPoint { get; set; }

        [Display(Name = "Крайна точка на пристигане")]
        public string EndPoint { get; set; }

        [Display(Name = "Разстояние")]
        public double Distance { get; set; }

        public string Route => $"{this.StartPoint} - {this.EndPoint}";
    }
}
