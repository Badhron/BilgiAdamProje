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
    public class TagController : Controller
    {
        private readonly ICoreService<Tag> TagService;
        private readonly ICoreService<PostTag> PostTagService;

        public TagController(ICoreService<Tag> tg, ICoreService<PostTag> pt)
        {

            TagService = tg;
            PostTagService = pt;
        }

        public IActionResult Index()
        {
            List<Tag> tag = TagService.GetAll().OrderBy(x => x.Name).ToList();
            ViewBag.tags = tag;
            return View(new Tag());
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Tag tag)
        {
            tag = TagService.GetById(tag.ID);
            PostTagService.RemoveAll(x => x.TagId == tag.ID);

            TagService.Remove(tag);

            return RedirectToAction("Index", "Tag", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Update(Tag tag)
        {
            Tag original = TagService.GetById(tag.ID);
            original.Name = tag.Name;

            TagService.Update(original);
            return RedirectToAction("Index", "Tag", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Tag tag)
        {
            tag.ID = Guid.NewGuid();

            TagService.Add(tag);

            return RedirectToAction("Index", "Tag", new { area = "Administrator" });
        }
    }
}
