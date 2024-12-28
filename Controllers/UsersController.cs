using System.Security.Claims;
using BlogApp.Data.Abstrack;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

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
    }
}