using System;
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

    public PostsController(IPostRepository postRepository, ITagRepository tagRepository)
    {
        _postRepository = postRepository;
        _tagRepository = tagRepository;
    }
    public async Task<IActionResult> Index(string? tag)
    {

        var posts = _postRepository.Posts;
        if (!string.IsNullOrEmpty(tag))
        {
            posts = posts.Where(x => x.Tags.Any(t => t.Url == tag));
        }

        return View(
            new PostViewModel { Posts = await posts.ToListAsync() });
    }
    public async Task<IActionResult> Details(string? url)
    {
        var post = await _postRepository.Posts.FirstOrDefaultAsync(p => p.Url == url);
        return View(post);
    }
}
