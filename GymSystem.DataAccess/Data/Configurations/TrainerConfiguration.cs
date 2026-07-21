namespace GymSystem.DataAccess.Data.Configurations;

public class TrainerConfiguration : GymUserConfiguration<Trainer>, IEntityTypeConfiguration<Trainer>
{
    public new void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.Property(t => t.Specialty)
               .HasConversion<string>()
               .HasMaxLength(30);

        base.Configure(builder);
    }
}
