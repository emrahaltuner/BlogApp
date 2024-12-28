using System;
using System.Security.Claims;
using BlogApp.Data.Abstrack;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers;

public class PostsController : Controller
{
    private IPostRepository _postRepository;
    private ITagRepository _tagRepository;
    private ICommentRepository _commentRepository;

    public PostsController(IPostRepository postRepository, ITagRepository tagRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _tagRepository = tagRepository;
        _commentRepository = commentRepository;
    }
    public async Task<IActionResult> Index(string? tag)
    {

        var posts = _postRepository.Posts;
        if (!string.IsNullOrEmpty(tag))
        {
            posts = posts.Where(x => x.Tags.Any(t => t.Url == tag));
        }
        posts = posts.Include(t => t.Tags);
        return View(
            new PostViewModel
            {
                Posts = await posts.ToListAsync()
            });
    }
    public async Task<IActionResult> Details(string? url)
    {
        var post = await _postRepository
        .Posts
        .Include(p => p.Tags)
        .Include(c => c.Comments)
        .ThenInclude(u => u.User)
        .FirstOrDefaultAsync(p => p.Url == url);
        return View(post);
    }
    [HttpPost]
    public JsonResult AddComment(int PostId, string Text)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        var image = User.FindFirstValue(ClaimTypes.UserData);
        var entity = new Comment
        {
            Text = Text,
            PublishedOn = DateTime.Now,
            PostId = PostId,
            UserId = int.Parse(userId ?? "")
        };
        _commentRepository.CreateComment(entity);

        //return RedirectToRoute("post_details", new { url = Url });

        return Json(new
        {
            username,
            Text,
            entity.PublishedOn,
            image,
        });

    }
}
