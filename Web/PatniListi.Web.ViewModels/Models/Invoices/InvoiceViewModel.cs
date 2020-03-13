namespace PatniListi.Web.ViewModels.Models.Invoices
{
    using System;

    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class InvoiceViewModel : IMapFrom<Invoice>
    {
        public string Id { get; set; }

        public string Number { get; set; }

        public DateTime Date { get; set; }

        public string Location { get; set; }

        public string FuelType { get; set; }

        public decimal Price { get; set; }

        public double Quantity { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedOn { get; set; }

        public string UserFullName { get; set; }

        public string CarModel { get; set; }
    }
}
