using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using BlogSoft.WebUI.Models;
using BlogSoft.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Controllers
{
    //    [Authorize]
    public class ProfileController : Controller
    {

        private IHostingEnvironment _env;
        private readonly ICoreService<Category> CategoryService;
        private readonly ICoreService<Post> PostService;
        private readonly ICoreService<User> UserService;
        private readonly ICoreService<Comment> CommentService;
        private readonly ICoreService<PostReaction> PostReactionService;
        private readonly ICoreService<Share> ShareService;
        private readonly ICoreService<Tag> TagService;
        private readonly ICoreService<PostTag> PostTagService;

        public ProfileController(ICoreService<Category> cs, ICoreService<Post> ps, ICoreService<Comment> cm, ICoreService<User> us, ICoreService<PostReaction> pr, ICoreService<Share> sh, ICoreService<Tag> tg, ICoreService<PostTag> pt, IHostingEnvironment env)
        {
            CategoryService = cs;
            PostService = ps;
            UserService = us;
            PostReactionService = pr;
            ShareService = sh;
            CommentService = cm;
            TagService = tg;
            PostTagService = pt;
            _env = env;
        }

        public IActionResult Index()
        {
            string UserID = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value).ToString();
            return RedirectToAction("Profile", "Profile", new { id = UserID });
        }

        [HttpGet]
        public IActionResult Profile(string id)
        {

            ViewBag.categories = CategoryService.GetAll();
            ViewBag.tags = new TagViewModel(TagService);

            if (User.Identity.IsAuthenticated)
            {
                Guid UserID = new Guid(id);
                Guid ownerId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);
                ViewBag.user = UserService.GetById(ownerId);


                UserViewModel userViewModel = new UserViewModel(UserID, ownerId, PostService, CategoryService, UserService, CommentService, PostReactionService, ShareService, TagService, PostTagService);
                return View(userViewModel);
            }
            else
            {
                Guid UserID = new Guid(id);
                UserViewModel userViewModel = new UserViewModel(UserID, PostService, CategoryService, UserService, CommentService, PostReactionService, ShareService, TagService, PostTagService);
                return View(userViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(User item, List<IFormFile> files, string currentPasword, string newPasword)
        {

            Guid ownerId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);

            if(ownerId == item.ID || User.IsInRole("Admin"))
            {
                User user = UserService.GetById(item.ID);

                if (newPasword != null)
                {
                    if(currentPasword == user.Password)
                    {
                        user.Password = newPasword;
                    }
                    else
                    {
                        return RedirectToAction("Profile", "Profile", new { id = item.ID });
                    }
                }
                if(files.Count() > 0)
                {
                    bool imgResult;
                    string imgPath = Helper.ImageUpload(files, _env, out imgResult);

                    if (imgResult)
                    {
                        user.ImageUrl = imgPath;
                    }
                }

                user.Email = item.Email;
                user.UserName = item.UserName;
                UserService.Update(user);

                return RedirectToAction("Profile", "Profile", new { id = item.ID });
            }
            else
            {
                return RedirectToAction("Home", "Index");
            }

        }
    }
}
