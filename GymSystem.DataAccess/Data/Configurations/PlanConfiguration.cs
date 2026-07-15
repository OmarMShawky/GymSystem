namespace GymSystem.DataAccess.Data.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.Property(p => p.Name)
               .HasColumnType("VARCHAR")
               .HasMaxLength(50);

        builder.HasIndex(p => p.Name)
               .IsUnique();

        builder.Property(p => p.Description)
               .HasMaxLength(200);

        builder.Property(p => p.Price)
               .HasPrecision(10, 2);

        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("PlanDurationDays", "DurationDays BETWEEN 1 AND 365");
        });

    }
}