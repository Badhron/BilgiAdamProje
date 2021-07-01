using BlogSoft.Core.Map;
using BlogSoft.Model.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSoft.Model.Maps
{
    public class CommentMap : CoreMap<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(x => x.Detail).HasMaxLength(200).IsRequired(false);

            base.Configure(builder);
        }
    }
}
