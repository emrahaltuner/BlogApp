using BlogApp.Entity;

namespace BlogApp.Data.Abstrack
{
    public interface ITagRepository
    {
        IQueryable<Tag> Tags { get; }

        void CreateTag(Tag tag);
    }
}