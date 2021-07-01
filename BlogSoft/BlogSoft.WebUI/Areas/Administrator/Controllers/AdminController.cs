using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Areas.Administrator.Controllers
{
    [Area("Administrator"), Authorize]
    public class AdminController : Controller
    {
        private readonly ICoreService<Category> CategoryService;
        private readonly ICoreService<Post> PostService;
        private readonly ICoreService<User> UserService;
        private readonly ICoreService<Comment> CommentService;
        private readonly ICoreService<PostReaction> PostReactionService;
        private readonly ICoreService<Share> ShareService;
        private readonly ICoreService<Tag> TagService;
        private readonly ICoreService<PostTag> PostTagService;

        public AdminController(ICoreService<Category> cs, ICoreService<Post> ps, ICoreService<Comment> cm, ICoreService<User> us, ICoreService<PostReaction> pr, ICoreService<Share> sh, ICoreService<Tag> tg, ICoreService<PostTag> pt)
        {
            CategoryService = cs;
            PostService = ps;
            UserService = us;
            PostReactionService = pr;
            ShareService = sh;
            CommentService = cm;
            TagService = tg;
            PostTagService = pt;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
