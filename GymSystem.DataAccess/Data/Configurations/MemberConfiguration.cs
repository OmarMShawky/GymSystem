namespace GymSystem.DataAccess.Data.Configurations;

public class MemberConfiguration : UserConfiguration<Member>, IEntityTypeConfiguration<Member>
{
    public new void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(m => m.Gender)
               .HasConversion<string>();

        builder.HasQueryFilter(m => !m.IsDeleted);

        base.Configure(builder);
    }
}


//using GymSystem.DataAccess.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GymSystem.DataAccess.Data.Configurations;

//public class UserConfiguration<T> : IEntityTypeConfiguration<T> where T : User
//{
//    public virtual void Configure(EntityTypeBuilder<T> builder)
//    {
//        builder.HasDiscriminator<string>("UserType")
//               .HasValue<Member>("Member")
//               .HasValue<Trainer>("Trainer");

//        builder.Property(u => u.Name)
//               .HasMaxLength(100);

//        builder.Property(u => u.Phone)
//               .HasMaxLength(20);

//        builder.Property(u => u.Email)
//               .HasMaxLength(50);

//        builder.OwnsOne(x => x.Address, t =>
//        {
//            t.Property(a => a.Street).HasColumnName("Street").HasMaxLength(50);
//            t.Property(a => a.City).HasColumnName("City").HasMaxLength(50);
//            t.Property(a => a.BuildingNumber).HasColumnName("BuildingNumber");
//        });

//        builder.HasIndex(e=>e.Email).IsUnique();

//        builder.HasIndex(e=>e.Phone).IsUnique();

//        builder.ToTable(t =>
//        {
//            t.HasCheckConstraint(
//                "User_Phone_CK",
//                "LEN([Phone]) = 11 AND [Phone] LIKE '01[0125]%'"
//                );
//        });

//        builder.HasQueryFilter(u => !u.IsDeleted);
//    }
//}
