namespace GymSystem.DataAccess.Data.Configurations;

public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
{
    public void Configure(EntityTypeBuilder<HealthRecord> builder)
    {
        builder.Property(h => h.Weight)
               .HasPrecision(3, 2);

        builder.Property(h => h.Height)
               .HasPrecision(3, 2);

        builder.Property(h => h.BloodType)
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(h => h.Notes)
               .HasMaxLength(100);

        builder.HasOne(h => h.Member)
               .WithOne(h => h.HealthRecord)
               .HasForeignKey<HealthRecord>(h => h.MemberId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("CK_HealthRecord_Height", "[HEIGHT] > 0");
            tb.HasCheckConstraint("CK_HealthRecord_Weight", "[WEIGHT] > 0");
        });

        builder.HasQueryFilter(h => !h.IsDeleted);
    }
}
