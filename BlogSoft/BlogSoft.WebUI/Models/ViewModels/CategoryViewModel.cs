using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Models.ViewModels
{
    public class CategoryViewModel
    {

        public class CategoryDropdownObject
        {
            public CategoryDropdownObject(Guid value, string text)
            {
                this.value = value;
                this.text = text;
            }
            public Guid value;
            public string text;
        }

        private readonly ICoreService<Category> CategoryService;
        public List<CategoryDropdownObject> CategoryDropdown = new List<CategoryDropdownObject>();
        public string TagValues;

        public CategoryViewModel(ICoreService<Category> cs)
        {
            CategoryService = cs;
            CategoryDropdown = new List<CategoryDropdownObject>();
            int counter = 1;
            foreach (Category category in CategoryService.GetAll())
            {
                CategoryDropdown.Add(new CategoryDropdownObject(category.ID, category.Name));
                counter++;
            }
        }

    }
}
