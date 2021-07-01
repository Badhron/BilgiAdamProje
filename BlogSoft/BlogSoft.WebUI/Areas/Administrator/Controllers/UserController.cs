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
    public class UserController : Controller
    {

        private readonly ICoreService<User> UserService;
        private readonly ICoreService<Post> PostService;

        private readonly ICoreService<Comment> CommentService;
        private readonly ICoreService<PostReaction> PostReactionService;
        private readonly ICoreService<PostTag> PostTagService;
        private readonly ICoreService<Share> ShareService;

        public UserController(ICoreService<User> us, ICoreService<Post> ps, ICoreService<Comment> cm, ICoreService<PostReaction> pr, ICoreService<PostTag> pt, ICoreService<Share> sh)
        {

            UserService = us;
            PostService = ps;
            CommentService = cm;
            PostReactionService = pr;
            PostTagService = pt;
            ShareService = sh;
        }

        public IActionResult Index()
        {
            List<User> coreUsers = UserService.GetAll().OrderBy(x => x.Role).OrderBy(x => x.UserName).ToList();
            ViewBag.users = coreUsers;
            return View(new User());
        }

        [HttpPost]
        public async Task<IActionResult> Remove(User user)
        {
            user = UserService.GetById(user.ID);

            foreach(Post post in PostService.GetDefault(x => x.UserID == user.ID))
            {
                CommentService.RemoveAll(x => x.PostID == post.ID);
                PostReactionService.RemoveAll(x => x.PostId == post.ID);
                PostTagService.RemoveAll(x => x.PostId == post.ID);
                ShareService.RemoveAll(x => x.PostId == post.ID);
                PostService.Remove(post);
            }

            CommentService.RemoveAll(x => x.UserID == user.ID);
            PostReactionService.RemoveAll(x => x.UserId == user.ID);
            ShareService.RemoveAll(x => x.UserId == user.ID);

            UserService.Remove(user);

            return RedirectToAction("Index", "User", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            User original = UserService.GetById(user.ID);
            original.UserName = user.UserName;
            original.Email = user.Email;
            original.Password = user.Password;
            original.ImageUrl = user.ImageUrl;
            original.Role = user.Role;

            UserService.Update(original);
            return RedirectToAction("Index", "User", new { area = "Administrator" });
        }

        [HttpPost]
        public async Task<IActionResult> Insert(User user)
        {
            user.ID = Guid.NewGuid();

            UserService.Add(user);

            return RedirectToAction("Index", "User", new { area = "Administrator" });
        }
    }
}
