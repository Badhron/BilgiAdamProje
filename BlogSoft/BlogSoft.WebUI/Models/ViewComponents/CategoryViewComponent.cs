using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Models.ViewComponents
{
    [ViewComponent(Name = "Category")]
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICoreService<Category> CategoryService;

        public CategoryViewComponent(ICoreService<Category> cs)
        {
            CategoryService = cs;
        }
        public IViewComponentResult Invoke()
        {
            var categories = CategoryService.GetActive();
            return View(categories);
        }
    }
}
