namespace GymSystem.DataAccess.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name)
               .HasColumnType("VARCHAR")
               .HasMaxLength(50);

        builder.Property(c => c.Description)
               .HasMaxLength(250);
               

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
