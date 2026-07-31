namespace GymSystem.DataAccess.Data.Configurations;

public class MemberConfiguration : GymUserConfiguration<Member>, IEntityTypeConfiguration<Member>
{
    public new void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(m => m.Gender)
               .HasConversion<string>();

        builder.HasQueryFilter(m => !m.IsDeleted);

        base.Configure(builder);
    }
}
