using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Models.ViewModels
{
    public class TagViewModel
    {
        public class TagDropdownObject
        {
            public TagDropdownObject(Guid value, string text)
            {
                this.value = value;
                this.text = text;
            }
            public Guid value;
            public string text;
        }

        private readonly ICoreService<Tag> TagService;
        public List<TagDropdownObject> TagDropdown = new List<TagDropdownObject>();
        public string TagValues;

        public TagViewModel(ICoreService<Tag> tg)
        {
            TagService = tg;
            TagDropdown = new List<TagDropdownObject>();
            int counter = 1;
            foreach (Tag tag in TagService.GetAll())
            {
                TagDropdown.Add(new TagDropdownObject(tag.ID, tag.Name));
                counter++;
            }
        }
    }
}
