using BlogApp.Data.Abstrack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class TagsMenu : ViewComponent
    {
        private ITagRepository _tagReposittory;
        public TagsMenu(ITagRepository tagReposittory)
        {
            _tagReposittory = tagReposittory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _tagReposittory.Tags.ToListAsync());
        }
    }
}