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
    public class CategoryController : Controller
    {
        private readonly ICoreService<Category> CategoryService;

        public CategoryController(ICoreService<Category> cg)
        {

            CategoryService = cg;
        }

        public IActionResult Index()
        {
            List<Category> categories = CategoryService.GetAll().OrderBy(x => x.Name).ToList();
            ViewBag.categories = categories;
            return View(new Category());
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Category category)
        {
            category = CategoryService.GetById(category.ID);

            CategoryService.Remove(category);

            return RedirectToAction("Index", "Category", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            Category original = CategoryService.GetById(category.ID);
            original.Name = category.Name;
            original.ImageUrl = category.ImageUrl;


            CategoryService.Update(original);
            return RedirectToAction("Index", "Category", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Category category)
        {
            category.ID = Guid.NewGuid();

            CategoryService.Add(category);

            return RedirectToAction("Index", "Category", new { area = "Administrator" });
        }
    }
}
