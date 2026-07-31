namespace GymSystem.DataAccess.Data.Configurations;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {

        builder.HasQueryFilter(m => !m.IsDeleted);

        builder.HasIndex(m => new { m.MemberId, m.EndDate });
    }
}
