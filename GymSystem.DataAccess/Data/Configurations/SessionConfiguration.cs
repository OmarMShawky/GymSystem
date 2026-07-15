namespace GymSystem.DataAccess.Data.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable(h =>
        {
            h.HasCheckConstraint("SessionDateCheck", "StartDate < EndDate");
            h.HasCheckConstraint("SessionCapacityCheck", "Capacity between 1 and 25");
        });
    }
}
