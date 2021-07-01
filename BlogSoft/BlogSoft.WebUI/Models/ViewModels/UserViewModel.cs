using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Models.ViewModels
{
    public class UserViewModel
    {
        private ICoreService<Post> postService;
        private ICoreService<Category> categoryService;
        private ICoreService<User> userService;
        private ICoreService<Comment> commentService;
        private ICoreService<PostReaction> postReactionService;
        private ICoreService<Share> shareService;
        private ICoreService<Tag> tagService;
        private ICoreService<PostTag> postTagService;

        public User user;
        public List<PostViewModel> PostModels;
        private Guid userID;

        public UserViewModel(Guid userID, Guid ownerID, ICoreService<Post> postService, ICoreService<Category> categoryService, ICoreService<User> userService, ICoreService<Comment> commentService, ICoreService<PostReaction> postReactionService, ICoreService<Share> shareService, ICoreService<Tag> tagService, ICoreService<PostTag> postTagService)
        {
            this.postService = postService;
            this.categoryService = categoryService;
            this.userService = userService;
            this.commentService = commentService;
            this.postReactionService = postReactionService;
            this.shareService = shareService;
            this.tagService = tagService;
            this.postTagService = postTagService;

            PostModels = new List<PostViewModel>();

            this.user = userService.GetById(userID);
            List<Post> posts = this.postService.GetDefault(x => x.UserID == this.user.ID).OrderByDescending(x => x.CreatedDate).ToList();

            foreach (Post post in posts)
            {
                PostViewModel postViewModel = new PostViewModel(post.ID,  postService, categoryService, userService, commentService, postReactionService, shareService, tagService, postTagService);
                postViewModel.addUser(ownerID);
                PostModels.Add(postViewModel);
            }

        }

        public UserViewModel(Guid userID, ICoreService<Post> postService, ICoreService<Category> categoryService, ICoreService<User> userService, ICoreService<Comment> commentService, ICoreService<PostReaction> postReactionService, ICoreService<Share> shareService, ICoreService<Tag> tagService, ICoreService<PostTag> postTagService)
        {
            this.userID = userID;
            this.postService = postService;
            this.categoryService = categoryService;
            this.userService = userService;
            this.commentService = commentService;
            this.postReactionService = postReactionService;
            this.shareService = shareService;
            this.tagService = tagService;
            this.postTagService = postTagService;

            PostModels = new List<PostViewModel>();

            this.user = userService.GetById(userID);
            List<Post> posts = this.postService.GetDefault(x => x.UserID == this.user.ID).OrderByDescending(x => x.CreatedDate).ToList();

            foreach (Post post in posts)
            {
                PostViewModel postViewModel = new PostViewModel(post.ID, postService, categoryService, userService, commentService, postReactionService, shareService, tagService, postTagService);
                PostModels.Add(postViewModel);
            }
        }
    }
}
