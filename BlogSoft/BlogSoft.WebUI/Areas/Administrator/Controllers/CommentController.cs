using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Areas.Administrator.Controllers
{
    [Area("Administrator"), Authorize]
    public class CommentController : Controller
    {

        private readonly ICoreService<Comment> CommentService;
        private readonly ICoreService<User> UserService;
        private readonly ICoreService<Post> PostService;

        public CommentController(ICoreService<Comment> cm, ICoreService<User> us, ICoreService<Post> ps)
        {
            CommentService = cm;
            UserService = us;
            PostService = ps;
        }

        public IActionResult Index()
        {
            List<Comment> comments = CommentService.GetAll().OrderBy(x => x.CreatedDate).ToList();
            ViewBag.comments = comments;
            ViewBag.users = new SelectList(UserService.GetAll(), "ID", "UserName");
            ViewBag.posts = new SelectList(PostService.GetAll(), "ID", "Name");
            return View(new Comment());
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Comment comment)
        {
            comment = CommentService.GetDefault(x => x.ID == comment.ID).First();

            CommentService.Remove(comment);

            return RedirectToAction("Index", "Comment", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Update(Comment comment)
        {
            Comment original = CommentService.GetDefault(x => x.ID == comment.ID).First();
            original.Detail = comment.Detail;

            CommentService.Update(original);
            return RedirectToAction("Index", "Comment", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Comment comment)
        {
            comment.ID = Guid.NewGuid();
            System.Diagnostics.Debug.WriteLine("::ID::>" + comment.ID);
            System.Diagnostics.Debug.WriteLine("::Detail::>" + comment.Detail);
            System.Diagnostics.Debug.WriteLine("::PostID::>" + comment.PostID);
            System.Diagnostics.Debug.WriteLine("::UserID::>" + comment.UserID);
            CommentService.Add(comment);

            return RedirectToAction("Index", "Comment", new { area = "Administrator" });
        }
    }
}
