using BlogSoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Models.ViewModels
{
    public class PostComment
    {
        public Comment comment { get; set; }
        public User user { get; set; }

        public PostComment(Comment comment, User user)
        {
            this.comment = comment;
            this.user = user;
        }
    }
}
