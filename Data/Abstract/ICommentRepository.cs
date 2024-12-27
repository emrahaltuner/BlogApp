using BlogApp.Entity;

namespace BlogApp.Data.Abstrack
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }

        void CreateComment(Comment comment);
    }
}