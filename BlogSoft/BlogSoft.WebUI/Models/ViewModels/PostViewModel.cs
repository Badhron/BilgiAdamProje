using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using BlogSoft.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Models.ViewModels
{
    public class PostViewModel
    {
        private ICoreService<Post> PostService;
        private ICoreService<Category> CategoryService;
        private ICoreService<User> UserService;
        private ICoreService<Comment> CommentService;
        private ICoreService<PostReaction> PostReactionService;
        private ICoreService<Share> ShareService;
        private ICoreService<Tag> TagService;
        private ICoreService<PostTag> PostTagService;

        public Post post { get; set; }
        public int likeCount { get; set; }
        public int dislikeCount { get; set; }
        public List<PostReaction> reactions { get; set; }
        public List<PostComment> comments { get; set; }
        public List<Tag> tags { get; set; }
        public string tagsString { get; set; }
        public Category category { get; set; }
        public Guid categoryID { get; set; }
        public bool isUserLiked { get; set; }
        public string thumbsStyle { get; set; }
        public User postOwner { get; set; }
        public bool isUserOwner { get; set; }

        public List<PostComment> GetPostCommentsById(Guid id)
        {
            List<PostComment> postComments = new List<PostComment>();
            List<Comment> comments = CommentService.GetDefault(x => x.PostID == id).OrderByDescending(x => x.CreatedDate).ToList();
            foreach (Comment comment in comments)
            {
                postComments.Add(new PostComment(comment, UserService.GetById(comment.UserID)));
            }
            return postComments;
        }

        public List<Tag> GetPostTags(Guid id)
        {
            List<PostTag> postTags = PostTagService.GetDefault(x => x.PostId == id).OrderByDescending(x => x.CreatedDate).ToList();
            List<Tag> tags = new List<Tag>();
            this.tagsString = "";
            foreach (PostTag postTag in postTags)
            {
                tags.Add(TagService.GetDefault(x => x.ID == postTag.TagId).First());
                tagsString += TagService.GetDefault(x => x.ID == postTag.TagId).First().Name.Replace("#","") + ",";
            }
            return tags;
        }


        public PostViewModel()
        {
            this.post = new Post();
            this.reactions = new List<PostReaction>();
            this.comments = new List<PostComment>();
            this.tags = new List<Tag>();
            this.isUserLiked = false;
            this.isUserOwner = false;
        }

        public PostViewModel(Guid id, ICoreService<Post> ps, ICoreService<Category> cs, ICoreService<User> us, ICoreService<Comment> cm, ICoreService<PostReaction> pr, ICoreService<Share> sh, ICoreService<Tag> tg, ICoreService<PostTag> pt)
        {
            this.PostService = ps;
            this.CategoryService = cs;
            this.UserService = us;
            this.CommentService = cm;
            this.PostReactionService = pr;
            this.ShareService = sh;
            this.TagService = tg;
            this.PostTagService = pt;

            this.post = ps.GetById(id);
            this.postOwner = us.GetById(post.UserID);

            this.comments = GetPostCommentsById(id);
            this.tags = GetPostTags(id);

            this.isUserOwner = false;
            this.isUserLiked = false;
            this.likeCount = 0;
            this.dislikeCount = 0;

            this.post = post;
            reactions = PostReactionService.GetDefault(x => x.PostId == this.post.ID).ToList();

            if (reactions.Count() > 0)
            {
                foreach (PostReaction reaction in reactions)
                {
                    switch (reaction.Reaction)
                    {
                        case 1:
                            this.likeCount++;
                            break;
                        case 2:
                            this.dislikeCount++;
                            break;
                        default:
                            this.likeCount++;
                            break;
                    }
                }
            }

        }

        internal void addUser(Guid guid)
        {
            if(guid == this.post.UserID)
            {
                this.isUserOwner = true;
            }
            this.isUserLiked = false;
            if (PostReactionService.Any(x => (x.PostId == post.ID && x.UserId == guid && x.Reaction == 1)))
            {
                this.isUserLiked = true;
            }
            this.thumbsStyle = this.isUserLiked ? "color:#1a73e8" : "color:grey";
        }

        internal bool insert(ICoreService<Tag> tg, ICoreService<Post> ps, ICoreService<PostTag> pt)
        {
            bool an = ps.Add(post);

            if (an)
            {
                post = ps.GetByDefault(x => (x.ID == post.ID));
                string[] tagNames = this.tagsString.Split(',');
                foreach (string tagName in tagNames)
                {
                    string[] cleanTags = tagName.Trim().Split();

                    foreach(string cleanTag in cleanTags)
                    {
                        string usTag = "#" + cleanTag;
                        if (!tg.Any(x => (x.Name == usTag)))
                        {
                            Tag newTag = new Tag();
                            newTag.Name = usTag;
                            tg.Add(newTag);
                            newTag = tg.GetByDefault(x => (x.ID == newTag.ID));
                            PostTag postTag = new PostTag();
                            postTag.ID = Guid.NewGuid();
                            postTag.PostId = post.ID;
                            postTag.TagId = newTag.ID;
                            pt.Add(postTag);
                        }
                        else
                        {
                            Tag existTag = tg.GetByDefault(x => (x.Name == usTag));
                            PostTag postTag = new PostTag();
                            postTag.PostId = post.ID;
                            postTag.TagId = existTag.ID;
                            pt.Add(postTag);
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
            
        }

        internal void update()
        {

            PostTagService.RemoveAll(x => x.PostId == this.post.ID);

            string[] tagNames = this.tagsString.Split(',');

            foreach (string tagName in tagNames)
            {
                string[] cleanTags = tagName.Trim().Split();

                foreach (string cleanTag in cleanTags)
                {
                    string usTag = "#" + cleanTag;
                    if (!TagService.Any(x => (x.Name == usTag)))
                    {
                        Tag newTag = new Tag();
                        newTag.Name = usTag;
                        TagService.Add(newTag);
                        newTag = TagService.GetByDefault(x => (x.ID == newTag.ID));
                        PostTag postTag = new PostTag();
                        postTag.ID = Guid.NewGuid();
                        postTag.PostId = post.ID;
                        postTag.TagId = newTag.ID;
                        PostTagService.Add(postTag);
                    }
                    else
                    {
                        Tag existTag = TagService.GetByDefault(x => (x.Name == usTag));
                        PostTag postTag = new PostTag();
                        postTag.PostId = post.ID;
                        postTag.TagId = existTag.ID;
                        PostTagService.Add(postTag);
                    }
                }
            }


            PostService.Update(this.post);

        }
    }
}
