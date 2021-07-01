using BlogSoft.Core.Map;
using BlogSoft.Model.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSoft.Model.Maps
{
    public class TagMap : CoreMap<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(30).IsRequired(true);

            base.Configure(builder);
        }
    }
}
