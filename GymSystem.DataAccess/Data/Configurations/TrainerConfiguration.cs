namespace GymSystem.DataAccess.Data.Configurations;

public class TrainerConfiguration : GymUserConfiguration<Trainer>, IEntityTypeConfiguration<Trainer>
{
    public new void Configure(EntityTypeBuilder<Trainer> builder)
    {

        builder.HasOne(t => t.Category)
               .WithMany(c => c.Trainers)
               .HasForeignKey(t => t.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        base.Configure(builder);
    }
}
