using BlogApp.Data.Abstrack;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete
{
    public class EfPostRepository : IPostRepository
    {
        private BlogContext _context;
        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Post> Posts => _context.Posts;


        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void EditPost(Post post)
        {
            var entity = _context.Posts.FirstOrDefault(i => i.PostId == post.PostId);
            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Desc = post.Desc;
                entity.Url = post.Url;
                entity.Content = post.Url;
                entity.IsActive = post.IsActive;
                entity.Image = post.Image;
                _context.SaveChanges();
            }
        }

        public void EditPost(Post post, int[] tagIds)
        {
            var entity = _context.Posts.Include(i => i.Tags).FirstOrDefault(i => i.PostId == post.PostId);
            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Desc = post.Desc;
                entity.Url = post.Url;
                entity.Content = post.Url;
                entity.IsActive = post.IsActive;
                entity.Image = post.Image;

                entity.Tags = _context.Tags.Where(tag => tagIds.Contains(tag.TagId)).ToList();

                _context.SaveChanges();
            }
        }
    }
}