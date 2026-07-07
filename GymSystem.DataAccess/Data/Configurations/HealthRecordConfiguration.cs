using GymSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Data.Configurations;

public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
{
    public void Configure(EntityTypeBuilder<HealthRecord> builder)
    {
        builder.Property(c => c.Weight)
               .HasPrecision(3, 2);

        builder.Property(c => c.Height)
               .HasPrecision(3, 2);

        builder.Property(c => c.BloodType)
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.HasOne(m => m.Member)
               .WithOne(m => m.HealthRecord)
               .HasForeignKey<HealthRecord>(m => m.MemberId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("CK_HealthRecord_Height", "[HEIGHT] > 0");
            tb.HasCheckConstraint("CK_HealthRecord_Weight", "[WEIGHT] > 0");
        });

        builder.HasQueryFilter(h => !h.IsDeleted);
    }
}
