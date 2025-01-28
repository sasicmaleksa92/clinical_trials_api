using ClinicalTrials.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicalTrials.Infrastructure.Persistance.Configuration
{
    internal class ClinicalTrialConfiguration : IEntityTypeConfiguration<ClinicalTrial>
    {
        public void Configure(EntityTypeBuilder<ClinicalTrial> builder)
        {
            builder.ToTable("ClinicalTrials");

            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.TrialId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ct => ct.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ct => ct.StartDate)
                .IsRequired();

            builder.Property(ct => ct.EndDate);

            builder.Property(ct => ct.Participants)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(ct => ct.Status)
                .IsRequired();

            builder.Property(ct => ct.DurationInDays);
                //.HasComputedColumnSql("DATEDIFF(day, StartDate, ISNULL(EndDate, GETDATE()))");
        }
    }
}
