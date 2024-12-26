using BlogApp.Entity;

namespace BlogApp.Data.Abstrack
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }

        void CreatePost(Post post);
    }
}