using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Models.ViewComponents
{
    [ViewComponent(Name = "Login")]
    public class LoginViewComponent : ViewComponent
    {
        private readonly ICoreService<User> UserService;

        public LoginViewComponent(ICoreService<User> us)
        {
            UserService = us;
        }

        public IViewComponentResult Invoke()
        {
            return View(new User());
        }
    }
}
