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
    [ViewComponent(Name = "EditUserModal")]
    public class EditUserModalViewComponent : ViewComponent
    {

        private readonly ICoreService<User> UserService;

        public EditUserModalViewComponent(ICoreService<User> us)
        {
            UserService = us;
        }

        public IViewComponentResult Invoke(Guid id)
        {
            //System.Diagnostics.Debug.WriteLine("::Test::>" + id);
            User user = UserService.GetById(id);
            return View(user);
        }
    }
}
