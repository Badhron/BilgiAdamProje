using BlogSoft.Core.Map;
using BlogSoft.Model.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSoft.Model.Maps
{
    public class UserMap : CoreMap<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.UserName).HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Email).HasMaxLength(255).IsRequired(true);
            builder.Property(x => x.Password).HasMaxLength(20).IsRequired(true);
            builder.Property(x => x.Role).HasMaxLength(100).IsRequired(true);

            base.Configure(builder);
        }
    }
}
