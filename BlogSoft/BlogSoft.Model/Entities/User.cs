using BlogSoft.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSoft.Model.Entities
{
    public class User : CoreEntity
    {
        public User()
        {
            this.Posts = new HashSet<Post>();
            this.Shares = new HashSet<Share>();
            this.PostReactions = new HashSet<PostReaction>();
            this.Comments = new HashSet<Comment>();
        }

        public static object Claims { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Share> Shares { get; set; }
        public virtual ICollection<PostReaction> PostReactions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
