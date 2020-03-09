namespace PatniListi.Common
{
    public class AttributesConstraints
    {
        // Cars
        public const int CarModelMaxLength = 20;
        public const int CarModelMinLength = 2;

        public const int LicensePlateMaxLength = 10;
        public const int LicensePlateMinLength = 3;

        public const double CapacityFuelMin = 20.00;
        public const double CapacityFuelMax = 100.00;

        // Company
        public const int CompanyMaxLength = 20;
        public const int CompanyMinLength = 2;

        // Invoice
        public const int InvoiceLocationMaxLength = 10;
        public const int InvoiceLocationMinLength = 3;

        public const double PriceMaxLength = 0.10;
        public const double PriceMinLength = 10.00;

        // Routes
        public const int RouteMaxLength = 20;
        public const int RouteMinLength = 2;
    }
}
