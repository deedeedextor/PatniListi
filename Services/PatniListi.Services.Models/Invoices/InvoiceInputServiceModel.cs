using PatniListi.Data.Models;
using PatniListi.Services.Mapping;
using System;

namespace PatniListi.Services.Models.Invoices
{
    public class InvoiceInputServiceModel : IMapTo<Invoice>, IMapFrom<Invoice>
    {
        public string Number { get; set; }

        public DateTime Date { get; set; }

        public string Location { get; set; }

        public string FuelType { get; set; }

        public decimal Price { get; set; }

        public double Quantity { get; set; }

        public string CreatedBy { get; set; }

        public string UserFullName { get; set; }

        public string CarModel { get; set; }
    }
}
