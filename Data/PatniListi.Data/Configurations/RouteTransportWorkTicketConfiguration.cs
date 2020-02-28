namespace PatniListi.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using PatniListi.Data.Models;

    public class RouteTransportWorkTicketConfiguration : IEntityTypeConfiguration<RouteTransportWorkTicket>
    {
        public void Configure(EntityTypeBuilder<RouteTransportWorkTicket> builder)
        {
            builder
                .HasKey(rt => new { rt.RouteId, rt.TransportWorkTicketId });

            builder
                .HasOne(r => r.Route)
                .WithMany(tr => tr.RouteTransportWorkTickets)
                .HasForeignKey(r => r.RouteId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(tr => tr.TransportWorkTicket)
                .WithMany(r => r.RouteTransportWorkTickets)
                .HasForeignKey(tr => tr.TransportWorkTicketId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
