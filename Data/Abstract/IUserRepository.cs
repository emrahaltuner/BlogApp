using BlogApp.Entity;

namespace BlogApp.Data.Abstrack
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }

        void CreateUsers(User user);
    }
}