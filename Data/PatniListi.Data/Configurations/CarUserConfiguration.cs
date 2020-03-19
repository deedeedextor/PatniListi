namespace PatniListi.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PatniListi.Data.Models;

    public class CarUserConfiguration : IEntityTypeConfiguration<CarUser>
    {
        public void Configure(EntityTypeBuilder<CarUser> builder)
        {
            builder
                .HasKey(cu => new { cu.CarId, cu.UserId });

            builder
                .HasOne(c => c.Car)
                .WithMany(u => u.CarUsers)
                .HasForeignKey(c => c.CarId)
                .IsRequired();

            builder
                .HasOne(u => u.ApplicationUser)
                .WithMany(c => c.CarUsers)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
        }
    }
}
