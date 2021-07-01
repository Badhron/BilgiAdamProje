using BlogSoft.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSoft.Model.Entities
{
    public class PostTag : CoreEntity
    {
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
