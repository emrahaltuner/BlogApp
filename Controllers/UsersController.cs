using System.IO;
using System.Security.Claims;
using BlogApp.Data.Abstrack;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var isUser = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                if (isUser != null)
                {
                    var userClaim = new List<Claim>();
                    userClaim.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));
                    userClaim.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
                    userClaim.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));
                    userClaim.Add(new Claim(ClaimTypes.UserData, isUser.Image ?? ""));


                    if (isUser.Email == "emrah@emrah.com")
                    {
                        userClaim.Add(new Claim(ClaimTypes.Role, "admin"));
                    }
                    var claimsIdentity = new ClaimsIdentity(userClaim, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Posts");
                }
            }
            else
            {
                ModelState.AddModelError("", "Email adresi veya şifre hatalı");
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Posts");
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile Image)
        {
            //Resim validate and uploads
            var allowedExtensin = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(Image.FileName);
            var randomFileName = string.Format($"{Guid.NewGuid()}{ext}");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
            if (Image != null)
            {

                //image validate 
                if (!allowedExtensin.Contains(ext))
                {
                    ModelState.AddModelError("", "Geçerli bir resim seçin");
                }

            }
            //model validation en create
            if (ModelState.IsValid)
            {
                //uploads image to path
                if (Image != null)
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }
                    model.Image = randomFileName;
                }

                //create user model
                var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName || x.Email == model.Email);
                if (user == null)
                {
                    _userRepository.CreateUsers(new User
                    {
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Image = model.Image ?? "p1.jpg"

                    });
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı Adı yada Email kullanımda");
                }

            }
            return View(model);
        }

        public IActionResult Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = _userRepository
            .Users
            .Include(p => p.Posts)
            .Include(p => p.Comments)
            .ThenInclude(p => p.Post)
            .FirstOrDefault(p => p.UserName == username);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}