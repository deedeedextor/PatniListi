namespace PatniListi.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using PatniListi.Data.Models;

    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder
                .HasMany(i => i.Invoices)
                .WithOne(c => c.Car)
                .HasForeignKey(i => i.CarId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(tr => tr.TransportWorkTickets)
                .WithOne(c => c.Car)
                .HasForeignKey(tr => tr.CarId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
