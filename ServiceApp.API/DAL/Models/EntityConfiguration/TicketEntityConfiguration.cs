using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Models.EntityConfiguration;

public class TicketEntityConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");
        builder.HasKey(t => t.Id);
        builder.Property(u => u.TicketStatus)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(u => u.Priority)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasData(
            new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "TestTicket123",
                CreatedAt = DateTime.Now,
                Description = "test ticket lorem ipsum",
                Priority = TicketPriority.Lowest,
                TicketStatus = TicketStatus.Open
            });

    }
}

