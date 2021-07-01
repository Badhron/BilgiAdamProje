using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Models.ViewComponents
{
    public class TrendingViewComponent : ViewComponent
    {

        private readonly ICoreService<Post> PostService;

        public TrendingViewComponent(ICoreService<Post> ps)
        {
            PostService = ps;
        }

        public IViewComponentResult Invoke()
        {
            var posts = PostService.GetActive().OrderByDescending(x => x.ViewCount).Take(5).ToList();
            return View(posts);
        }
    }
}
