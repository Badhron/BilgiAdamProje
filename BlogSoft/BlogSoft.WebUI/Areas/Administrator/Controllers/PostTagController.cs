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
    public class PostTagController : Controller
    {
        private readonly ICoreService<Tag> TagService;
        private readonly ICoreService<PostTag> PostTagService;
        private readonly ICoreService<Post> PostService;

        public PostTagController(ICoreService<Tag> tg, ICoreService<PostTag> pt, ICoreService<Post> ps)
        {

            TagService = tg;
            PostTagService = pt;
            PostService = ps;
        }

        public IActionResult Index()
        {
            List<PostTag> postTags = PostTagService.GetAll().ToList();
            ViewBag.postTags = postTags;
            ViewBag.tags = new SelectList(TagService.GetAll(), "ID", "Name");
            ViewBag.posts = new SelectList(PostService.GetAll(), "ID", "Name");
            return View(new PostTag());
        }

        [HttpPost]
        public async Task<IActionResult> Remove(PostTag postTag)
        {
            postTag = PostTagService.GetDefault(x => x.ID == postTag.ID).First();
            PostTagService.Remove(postTag);

            return RedirectToAction("Index", "PostTag", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Update(PostTag postTag)
        {
            PostTag original = PostTagService.GetDefault(x => x.ID == postTag.ID).First();

            PostTagService.Update(original);
            return RedirectToAction("Index", "PostTag", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Insert(PostTag postTag)
        {
            postTag.ID = Guid.NewGuid();

            PostTagService.Add(postTag);

            return RedirectToAction("Index", "PostTag", new { area = "Administrator" });
        }
    }
}
