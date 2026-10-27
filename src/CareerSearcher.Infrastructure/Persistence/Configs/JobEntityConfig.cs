using CareerSearcher.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerSearcher.Infrastructure.Persistence.Configs;

internal class JobEntityConfig : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("jobs");

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Company)
            .IsRequired();

        builder.Property(x => x.Url)
            .IsRequired();

        builder.Property(x => x.Description)
            .IsRequired();

        builder.Property(x => x.UniqueId)
            .IsRequired();
    }
}
