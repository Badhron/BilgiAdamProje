using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using BlogSoft.WebUI.Models;
using BlogSoft.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private IHostingEnvironment _env;
        private readonly ICoreService<Post> PostService;
        private readonly ICoreService<Category> CategoryService;
        private readonly ICoreService<Tag> TagService;
        private readonly ICoreService<PostTag> PostTagService;
        private readonly ICoreService<Comment> CommentService;
        private readonly ICoreService<PostReaction> PostReactionService;
        private readonly ICoreService<Share> ShareService;
        private readonly ICoreService<User> UserService;

        public PostController(ICoreService<Post> ps, ICoreService<User> us, ICoreService<Category> cs, ICoreService<Tag> tg, ICoreService<PostTag> pt, ICoreService<Comment> cm, ICoreService<PostReaction> pr, ICoreService<Share> sh, IHostingEnvironment env)
        {
            TagService = tg;
            PostService = ps;
            CategoryService = cs;
            PostTagService = pt;
            CommentService = cm;
            PostReactionService = pr;
            ShareService = sh;
            UserService = us;
            _env = env;
        }


        [HttpGet]
        public async Task<IActionResult> LikeInProfile(string id)
        {
            Guid PostId = new Guid(id);
            Guid UserId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);
            if (ModelState.IsValid)
            {
                if (PostReactionService.Any(x => (x.UserId == UserId && x.PostId == PostId)))
                {
                    PostReaction postReaction = PostReactionService.GetByDefault(x => (x.UserId == UserId && x.PostId == PostId));
                    PostReactionService.Remove(postReaction);
                }
                else
                {
                    PostReaction postReaction = new PostReaction();
                    postReaction.UserId = UserId;
                    postReaction.PostId = PostId;
                    postReaction.Reaction = 1;
                    PostReactionService.Add(postReaction);
                }
            }

            Post post = PostService.GetById(PostId);

            return RedirectToAction("Profile", "Profile", new { id = post.UserID });
        }
        
        [HttpGet]
        public async Task<IActionResult> Like(string id)
        {
            Guid PostId = new Guid(id);
            Guid UserId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);
            if (ModelState.IsValid)
            {
                if (PostReactionService.Any(x => (x.UserId == UserId && x.PostId == PostId)))
                {
                    PostReaction postReaction = PostReactionService.GetByDefault(x => (x.UserId == UserId && x.PostId == PostId));
                    PostReactionService.Remove(postReaction);
                }
                else
                {
                    PostReaction postReaction = new PostReaction();
                    postReaction.ID = Guid.NewGuid();
                    postReaction.UserId = UserId;
                    postReaction.PostId = PostId;
                    postReaction.Reaction = 1;
                    PostReactionService.Add(postReaction);
                }
            }

            return RedirectToAction("Post", "Home", new { id = PostId });
        }



        [HttpPost]
        public async Task<IActionResult> NewComment(string IDStr, string commentStr)
        {
            Guid postID = new Guid(IDStr);
            if (ModelState.IsValid && (commentStr != null || commentStr != ""))
            {
                Comment comment = new Comment();
                comment.ID = Guid.NewGuid();
                comment.UserID = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);
                comment.Detail = commentStr;
                comment.PostID = postID;
                CommentService.Add(comment);
            }

            return RedirectToAction("Post", "Home", new { id = postID });
        }


        [HttpPost]
        public async Task<IActionResult> InsertWithPostViewModel(PostViewModel postViewModel, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {

                postViewModel.post.UserID = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);
                bool imgResult;
                string imgPath = Helper.ImageUpload(files, _env, out imgResult);


                if (imgResult)
                {
                    postViewModel.post.ImagePath = imgPath;
                    bool result = postViewModel.insert(TagService, PostService, PostTagService);
                    //System.Diagnostics.Debug.WriteLine("::result::>" + result);
                }
            }

            
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string postID, string userID)
        {
            Post post = PostService.GetById(new Guid(postID));
            CommentService.RemoveAll(x => x.PostID == post.ID);
            PostReactionService.RemoveAll(x => x.PostId == post.ID);
            PostTagService.RemoveAll(x => x.PostId == post.ID);
            ShareService.RemoveAll(x => x.PostId == post.ID);

            PostService.Remove(post);

            if(userID != null && userID != "")
            {
                Guid uid = new Guid(userID);
                return RedirectToAction("Profile", "Profile", new { id = uid });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> PostEditPage(string id)
        {
            Guid PostId = new Guid(id);
            Post post = PostService.GetById(PostId);
            Guid UserID = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);

            if (post.UserID == UserID || User.IsInRole("Admin"))
            {
                PostViewModel postViewModel = new PostViewModel(post.ID, PostService, CategoryService, UserService, CommentService, PostReactionService, ShareService, TagService, PostTagService);
                postViewModel.addUser(UserID);
                ViewBag.categories = new SelectList(CategoryService.GetAll(), "ID", "Name");
                ViewBag.tags = new TagViewModel(TagService);
                return View(postViewModel);
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> Update(PostViewModel postViewModel, List<IFormFile> files)
        {
            Guid UserID = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);
            if (postViewModel.post.UserID == UserID || User.IsInRole("Admin"))
            {

                PostViewModel original = new PostViewModel(postViewModel.post.ID, PostService, CategoryService, UserService, CommentService, PostReactionService, ShareService, TagService, PostTagService);

                original.addUser(UserID);

                if (files != null)
                {
                    bool imgResult;
                    string imgPath = Helper.ImageUpload(files, _env, out imgResult);
                    if (imgResult)
                    {
                        original.post.ImagePath = imgPath;
                    }
                }

                original.post.Name = postViewModel.post.Name;
                original.tagsString = postViewModel.tagsString;
                original.post.CategoryID = postViewModel.post.CategoryID;
                original.post.Description = postViewModel.post.Description;

                original.update();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveComment(string commentID, string PostID)
        {
            Guid pID = new Guid(PostID);
            Guid cID = new Guid(commentID);
            Guid UserID = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);
            Comment comment = CommentService.GetDefault(x => x.ID == cID).First();

            if (UserID == comment.UserID || User.IsInRole("Admin"))
            {
                CommentService.Remove(comment);
            }


            return RedirectToAction("Post", "Home", new { id = pID });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateComment(string commentID, string PostID, string Detail)
        {
            Guid pID = new Guid(PostID);
            Guid cID = new Guid(commentID);
            Guid UserID = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);
            if (Detail != null || Detail != "")
            {
                Comment original = CommentService.GetDefault(x => x.ID == cID).First();
                if (UserID == original.UserID || User.IsInRole("Admin"))
                {
                    original.Detail = Detail;
                    CommentService.Update(original);
                }
            }

            
            return RedirectToAction("Post", "Home", new { id = pID });
        }
    }

}
