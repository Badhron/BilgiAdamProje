using BlogSoft.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSoft.Model.Entities
{
    public class Post : CoreEntity
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.Shares = new HashSet<Share>();
            this.PostReactions = new HashSet<PostReaction>();
            this.Tags = new HashSet<PostTag>();
        }


        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int ViewCount { get; set; }

        public Guid CategoryID { get; set; }
        public Category Category { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Share> Shares { get; set; }
        public virtual ICollection<PostReaction> PostReactions { get; set; }
        public virtual ICollection<PostTag> Tags { get; set; }
    }
}
