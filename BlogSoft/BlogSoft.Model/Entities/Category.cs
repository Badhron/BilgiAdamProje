using BlogSoft.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSoft.Model.Entities
{
    public class Category : CoreEntity
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }

        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
