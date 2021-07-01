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
    public class PostController : Controller
    {

        private readonly ICoreService<Post> PostService;
        private readonly ICoreService<Category> CategoryService;
        private readonly ICoreService<Tag> TagService;

        private readonly ICoreService<Comment> CommentService;
        private readonly ICoreService<PostReaction> PostReactionService;
        private readonly ICoreService<PostTag> PostTagService;
        private readonly ICoreService<Share> ShareService;


        public PostController(ICoreService<Post> ps, ICoreService<Category> cg, ICoreService<Tag> tg, ICoreService<Comment> cm, ICoreService<PostReaction> pr, ICoreService<PostTag> pt, ICoreService<Share> sh)
        {
            PostService = ps;
            CategoryService = cg;
            TagService = tg;
            CommentService = cm;
            PostReactionService = pr;
            PostTagService = pt;
            ShareService = sh;
    }

        public IActionResult Index()
        {
            List<Post> posts = PostService.GetAll().OrderBy(x => x.Name).ToList();
            ViewBag.posts = posts;
            ViewBag.categories = new SelectList(CategoryService.GetAll(), "ID", "Name");
            return View(new Post());
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Post post)
        {
            post = PostService.GetById(post.ID);
            CommentService.RemoveAll(x => x.PostID == post.ID);
            PostReactionService.RemoveAll(x => x.PostId == post.ID);
            PostTagService.RemoveAll(x => x.PostId == post.ID);
            ShareService.RemoveAll(x => x.PostId == post.ID);
            PostService.Remove(post);

            return RedirectToAction("Index", "Post", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Update(Post post)
        {
            Post original = PostService.GetById(post.ID);
            original.Name = post.Name;
            original.CategoryID = post.CategoryID;
            original.ImagePath = post.ImagePath;
            original.Description = post.Description;
            original.ViewCount = post.ViewCount;


            PostService.Update(original);
            return RedirectToAction("Index", "Post", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Post post)
        {
            post.ID = Guid.NewGuid();
            post.UserID = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);
            PostService.Add(post);

            return RedirectToAction("Index", "Post", new { area = "Administrator" });
        }

    }
}
