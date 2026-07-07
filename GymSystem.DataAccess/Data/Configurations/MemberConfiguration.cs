using GymSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Data.Configurations;

public class MemberConfiguration : UserConfiguration<Member>
{
    public override void Configure(EntityTypeBuilder<Member> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Phone)
               .HasMaxLength(500);
    }
}
