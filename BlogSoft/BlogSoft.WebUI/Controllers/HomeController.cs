using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using BlogSoft.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICoreService<Category> CategoryService;
        private readonly ICoreService<Post> PostService;
        private readonly ICoreService<User> UserService;
        private readonly ICoreService<Comment> CommentService;
        private readonly ICoreService<PostReaction> PostReactionService;
        private readonly ICoreService<Share> ShareService;
        private readonly ICoreService<Tag> TagService;
        private readonly ICoreService<PostTag> PostTagService;

        public HomeController(ICoreService<Category> cs, ICoreService<Post> ps, ICoreService<Comment> cm, ICoreService<User> us, ICoreService<PostReaction> pr, ICoreService<Share> sh, ICoreService<Tag> tg, ICoreService<PostTag> pt)
        {
            CategoryService = cs;
            PostService = ps;
            UserService = us;
            PostReactionService = pr;
            ShareService = sh;
            CommentService = cm;
            TagService = tg;
            PostTagService = pt;
        }

        public IActionResult Index()
        {
            ViewBag.posts = PostService.GetAll();
            return View();
        }

        public IActionResult Post(Guid id)
        {
            //System.Diagnostics.Debug.WriteLine("::A1::>"+id);
            

            PostViewModel post = new PostViewModel(id, PostService, CategoryService, UserService, CommentService, PostReactionService, ShareService, TagService, PostTagService);

            post.post.ViewCount++;
            PostService.Update(post.post);
            //System.Diagnostics.Debug.WriteLine("::Test::>"+post.isUserLiked);
            /*
            Post post = _ps.GetById(id);
            post.ViewCount++;
            _ps.Update(post);
            */

            if (User.Identity.IsAuthenticated)
            {
                Guid UserID = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);
                ViewBag.user = UserService.GetById(UserID);
                post.addUser(UserID);
            }


            return View(Tuple.Create<PostViewModel, User>(post, UserService.GetById(post.post.UserID)));
        }

        public IActionResult PostsByCategory(Guid id)
        {
            ViewBag.posts = PostService.GetDefault(x => x.CategoryID == id).ToList();
            return View("Index");
        }

        public IActionResult PostsByTag(Guid id)
        {
            List<PostTag> postTags = PostTagService.GetDefault(x => x.TagId == id).ToList();
            List<Post> posts = new List<Post>();
            foreach(PostTag postTag in postTags)
            {
                posts.Add(PostService.GetDefault(x => x.ID == postTag.PostId).First());
            }
            ViewBag.posts = posts;
            return View("Index");
        }

        /*
        public IActionResult Posts(Guid id)
        {
            return View(_ps.GetDefault(x => x.CategoryID == id).ToList());
        }
        */
    }
}
