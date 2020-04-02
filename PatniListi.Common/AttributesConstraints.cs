namespace PatniListi.Common
{
    public class AttributesConstraints
    {
        // Users
        public const int UsernameMaxLength = 20;
        public const int UsernameMinLength = 3;

        public const int PasswordMaxLength = 100;
        public const int PasswordMinLength = 6;

        // Cars
        public const int CarModelMaxLength = 40;
        public const int CarModelMinLength = 2;

        public const int LicensePlateMaxLength = 10;
        public const int LicensePlateMinLength = 3;

        public const int CapacityFuelMax = 100;
        public const int CapacityFuelMin = 20;

        public const double StartKilometersMaxRange = 1000000000.00;
        public const double StartKilometersMinRange = 0;

        public const int AverageConsumptionMaxRange = 20;
        public const int AverageConsumptionMinRange = 3;

        public const double InitialFuelMaxRange = 100.00;
        public const double InitialFuelMinRange = 0;

        // Company
        public const int CompanyMaxLength = 20;
        public const int CompanyMinLength = 2;

        public const int CompanyAddressMaxLength = 30;
        public const int CompanyAddressMinLength = 3;

        // Invoice
        public const int InvoiceLocationMaxLength = 60;
        public const int InvoiceLocationMinLength = 3;

        public const double PriceMaxRange = 10.00;
        public const double PriceMinRange = 0.10;

        public const double QuantityMaxRange = 999.99;
        public const double QuantityMinRange = 1.00;

        public const double TotalPriceMaxRange = 999.99;
        public const double TotalPriceMinRange = 1.00;

        public const int BulstatMaxLength = 10;

        // Routes
        public const int RouteMaxLength = 60;
        public const int RouteMinLength = 2;

        // TransportWorkTickets
        public const double StartMaxRange = 1000000000.00;
        public const double StartMinRange = 0;

        public const double PositiveMaxRange = 1000000000.00;
        public const double PositiveMinRange = 0;
    }
}
