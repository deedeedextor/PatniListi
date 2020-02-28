namespace PatniListi.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PatniListi.Data.Models;

    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder
                .HasMany(c => c.Users)
                .WithOne(u => u.Company)
                .HasForeignKey(c => c.CompanyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
