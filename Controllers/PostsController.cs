using System;
using System.Security.Claims;
using BlogApp.Data.Abstrack;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        var posts = _postRepository.Posts.Where(i => i.IsActive);
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
        .Include(p => p.User)
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
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(PostCreateViewModel model, IFormFile ImageFile)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


        if (ModelState.IsValid)
        {
            //img upload 
            var imagePath = "wwwroot/img";
            var uploadedFileName = await UploadFileAsync(ImageFile, imagePath);
            if (uploadedFileName != null)
            {
                model.Image = uploadedFileName;
            }
            _postRepository.CreatePost(
                new Post
                {
                    Title = model.Title,
                    Content = model.Content,
                    Desc = model.Desc,
                    Url = model.Url,
                    IsActive = false,
                    UserId = int.Parse(userId ?? ""),
                    PublishedOn = DateTime.Now,

                    Image = model.Image,
                }
            );
            return RedirectToAction("Index");
        }
        return View(model);
    }

    public async Task<IActionResult> List()
    {

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
        var role = User.FindFirstValue(ClaimTypes.Role);
        var posts = _postRepository.Posts;
        if (string.IsNullOrEmpty(role))
        {
            posts = posts.Where(i => i.UserId == userId);
        }
        return View(await posts.ToListAsync());
    }






    //İmage Upload methot
    private async Task<string?> UploadFileAsync(IFormFile image, string imagePath)
    {
        if (image == null)
        {
            return null;
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var ext = Path.GetExtension(image.FileName);
        if (!allowedExtensions.Contains(ext))
        {
            ModelState.AddModelError("", "Geçerli bir resim seçin");
            return null;
        }

        var randomFileName = $"{Guid.NewGuid()}{ext}";
        var path = Path.Combine(Directory.GetCurrentDirectory(), imagePath, randomFileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return randomFileName;
    }


    [Authorize]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var post = _postRepository.Posts.Include(p => p.Tags).FirstOrDefault(i => i.PostId == id);
        if (post == null)
        {
            return NotFound();
        }
        ViewBag.Tags = _tagRepository.Tags.ToList();
        return View(
            new PostCreateViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Desc = post.Desc,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive,
                Tags = post.Tags
            }
        );
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(PostCreateViewModel model, IFormFile ImageFile, int[] tagIds)
    {


        if (ModelState.IsValid)
        {
            var imagePath = "wwwroot/img";
            var uploadedFileName = await UploadFileAsync(ImageFile, imagePath);

            if (uploadedFileName != null)
            {
                model.Image = uploadedFileName;
            }

            var entitytoUpdate = new Post
            {
                PostId = model.PostId,
                Title = model.Title,
                Content = model.Content,
                Url = model.Url,
                Desc = model.Desc,
                Image = model.Image
            };
            if (User.FindFirstValue(ClaimTypes.Role) == "admin")
            {
                entitytoUpdate.IsActive = model.IsActive;
            }

            _postRepository.EditPost(entitytoUpdate, tagIds);
            return RedirectToAction("List");
        }
        ViewBag.Tags = _tagRepository.Tags.ToList();
        return View(model);
    }
}
