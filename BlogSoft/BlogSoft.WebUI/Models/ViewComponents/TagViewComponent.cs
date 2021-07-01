using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Models.ViewComponents
{
    [ViewComponent(Name = "Tag")]
    public class TagViewComponent : ViewComponent
    {
        private readonly ICoreService<Tag> TagService;

        public TagViewComponent(ICoreService<Tag> tg)
        {
            TagService = tg;
        }
        public IViewComponentResult Invoke()
        {
            var tags = TagService.GetActive().OrderByDescending(x => x.Posts.Count).OrderByDescending(x => x.Name).ToList();
            return View(tags);
        }
    }
}
