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
        public const int CarModelMaxLength = 20;
        public const int CarModelMinLength = 2;

        public const int LicensePlateMaxLength = 10;
        public const int LicensePlateMinLength = 3;

        public const int CapacityFuelMin = 20;
        public const int CapacityFuelMax = 100;

        public const int StartKilometersMaxRange = int.MaxValue;
        public const int StartKilometersMinRange = 0;

        public const int AverageConsumptionMaxRange = 30;
        public const int AverageConsumptionMinRange = 3;

        public const double InitialFuelMaxRange = 100.00;
        public const double InitialFuelMinRange = 0;

        // Company
        public const int CompanyMaxLength = 20;
        public const int CompanyMinLength = 2;

        public const int CompanyAddressMaxLength = 30;
        public const int CompanyAddressMinLength = 3;

        // Invoice
        public const int InvoiceLocationMaxLength = 10;
        public const int InvoiceLocationMinLength = 3;

        public const double PriceMaxLength = 10.00;
        public const double PriceMinLength = 0.10;

        public const double QuantityMaxLength = double.MaxValue;
        public const double QuantityMinLength = 1.00;

        public const double TotalPriceMaxLength = double.MaxValue;
        public const double TotalPriceMinLength = 1.00;

        public const int BulstatMaxLength = 10;

        // Routes
        public const int RouteMaxLength = 20;
        public const int RouteMinLength = 2;
    }
}
