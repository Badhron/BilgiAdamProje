using BlogSoft.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSoft.Model.Entities
{
    public class Tag : CoreEntity
    {
        public Tag()
        {
            this.Posts = new HashSet<PostTag>();
        }

        public string Name { get; set; }
        public virtual ICollection<PostTag> Posts { get; set; }
    }
}
