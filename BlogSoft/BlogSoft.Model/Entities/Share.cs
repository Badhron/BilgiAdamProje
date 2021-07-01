using BlogSoft.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSoft.Model.Entities
{
    public class Share : CoreEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
