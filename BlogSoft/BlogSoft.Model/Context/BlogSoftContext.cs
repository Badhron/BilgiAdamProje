using BlogSoft.Core.Entity;
using BlogSoft.Model.Entities;
using BlogSoft.Model.Maps;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSoft.Model.Context
{
    public class BlogSoftContext : DbContext
    {
        public BlogSoftContext(DbContextOptions<BlogSoftContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new CommentMap());
            modelBuilder.ApplyConfiguration(new PostMap());
            modelBuilder.ApplyConfiguration(new TagMap());
            modelBuilder.ApplyConfiguration(new UserMap());

            modelBuilder.Entity<Share>()
                .HasKey(i => new { i.ID, i.PostId, i.UserId });
            modelBuilder.Entity<Share>()
                .HasOne(i => i.Post)
                .WithMany(ad => ad.Shares)
                .HasForeignKey(re => re.PostId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Share>()
                .HasOne(i => i.User)
                .WithMany(ad => ad.Shares)
                .HasForeignKey(re => re.UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<PostReaction>()
                .HasKey(i => new { i.ID, i.PostId, i.UserId });
            modelBuilder.Entity<PostReaction>()
                .HasOne(i => i.Post)
                .WithMany(ad => ad.PostReactions)
                .HasForeignKey(re => re.PostId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<PostReaction>()
                .HasOne(i => i.User)
                .WithMany(ad => ad.PostReactions)
                .HasForeignKey(re => re.UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<PostTag>()
               .HasKey(i => new { i.ID, i.PostId, i.TagId });
            modelBuilder.Entity<PostTag>()
                .HasOne(i => i.Post)
                .WithMany(ad => ad.Tags)
                .HasForeignKey(re => re.PostId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<PostTag>()
                .HasOne(i => i.Tag)
                .WithMany(ad => ad.Posts)
                .HasForeignKey(re => re.TagId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Comment>()
               .HasKey(i => new { i.ID, i.PostID, i.UserID });
            modelBuilder.Entity<Comment>()
                .HasOne(i => i.User)
                .WithMany(ad => ad.Comments)
                .HasForeignKey(re => re.UserID)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Comment>()
                .HasOne(i => i.Post)
                .WithMany(ad => ad.Comments)
                .HasForeignKey(re => re.PostID)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.ClientCascade);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Share> Shares { get; set; }

        public override int SaveChanges()
        {


            var collection = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added).ToList();

            string computerName = Environment.MachineName;
            string ipAddress = "127.0.0.1"; 
            DateTime date = DateTime.Now; 

            foreach (var item in collection)
            {
                CoreEntity entity = item.Entity as CoreEntity; 

                if (item != null)
                {
                    switch (item.State) 
                    {
                        case EntityState.Added:
                            entity.CreatedComputerName = computerName;
                            entity.CreatedIP = ipAddress;
                            entity.CreatedDate = date;
                            break;
                        case EntityState.Modified:
                            entity.ModifiedComputerName = computerName;
                            entity.ModifiedIP = ipAddress;
                            entity.ModifiedDate = date;
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
