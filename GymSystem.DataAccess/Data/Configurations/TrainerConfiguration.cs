using GymSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Data.Configurations;

public class TrainerConfiguration : UserConfiguration<Trainer>
{
    public override void Configure(EntityTypeBuilder<Trainer> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Specialty)
               .HasConversion<string>()
               .HasMaxLength(30);
    }
}
