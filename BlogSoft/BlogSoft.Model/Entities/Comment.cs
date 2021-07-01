using BlogSoft.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSoft.Model.Entities
{
    public class Comment : CoreEntity
    {
        public string Detail { get; set; }
        public Guid PostID { get; set; }
        public Post Post { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
