using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using BlogSoft.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Models.ViewComponents
{
    [ViewComponent(Name = "NewPostModal")]
    public class NewPostModalViewComponent : ViewComponent
    {
        private readonly ICoreService<Category> CategoryService;
        private readonly ICoreService<Post> PostService;
        private readonly ICoreService<Tag> TagService;

        public NewPostModalViewComponent(ICoreService<Category> cs, ICoreService<Post> ps, ICoreService<Tag> tg)
        {
            CategoryService = cs;
            PostService = ps;
            TagService = tg;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.categories = CategoryService.GetAll();
            ViewBag.tags = new TagViewModel(TagService);
            return View(new PostViewModel());
        }
    }
}
