using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Data.Configurations;

public abstract class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(u => u.Name)
               .HasMaxLength(100);

        builder.Property(u => u.Phone)
               .HasMaxLength(11);

        builder.HasIndex(u => u.Phone)
               .IsUnique();

        builder.Property(u => u.Email)
               .HasMaxLength(50);

        builder.HasIndex(u => u.Email)
               .IsUnique();

        builder.OwnsOne(u => u.Address, a =>
        {
            a.Property(a => a.Street)
             .HasColumnName("Street")
             .HasColumnType("VARCHAR")
             .HasMaxLength(50);

            a.Property(a => a.City)
             .HasColumnName("City")
             .HasColumnType("VARCHAR")
             .HasMaxLength(50);

            a.Property(a => a.BuildingNumber)
             .HasColumnName("BuildingNumber")
             .HasColumnType("INT");
        });

        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "Phone_CK",
                "LEN([Phone]) = 11 AND [Phone] LIKE '01[0125]%'"
            );

            t.HasCheckConstraint(
                "Email_CK",
                "[Email] LIKE '%@%.%'"
            );
        });
    }
}