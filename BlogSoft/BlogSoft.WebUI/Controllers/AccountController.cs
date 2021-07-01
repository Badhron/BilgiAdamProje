using BlogSoft.Core.Service;
using BlogSoft.Model.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogSoft.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICoreService<User> UserService;

        public AccountController(ICoreService<User> us)
        {
            UserService = us;
        }

        [HttpPost]
        public async Task<IActionResult> Register(User item)
        {
            if (!UserService.Any(x => (x.Email == item.Email || x.UserName == item.UserName)))
            {
                item.Role = "User";
                item.ImageUrl = "\\images\\avatar.png";

                bool result = UserService.Add(item);
                if (result)
                {
                    var logged = UserService.GetByDefault(x => (x.Email == item.Email || x.UserName == item.UserName) && x.Password == item.Password);
                    var claims = new List<Claim>()
                    {
                        new Claim("ID", logged.ID.ToString()),
                        new Claim(ClaimTypes.Role, logged.Role),
                        new Claim("Role", logged.Role),
                        new Claim(ClaimTypes.Name, logged.UserName),
                        new Claim(ClaimTypes.Email, logged.Email),
                        new Claim("Image", "")
                    };
                    
                    var userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(principal);

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(User item)
        {

            if (UserService.Any(x => (x.Email == item.Email || x.UserName == item.Email) && x.Password == item.Password))
            {

                var logged = UserService.GetByDefault(x => (x.Email == item.Email || x.UserName == item.Email) && x.Password == item.Password);

                var claims = new List<Claim>()
                {
                    new Claim("ID", logged.ID.ToString()),
                    new Claim(ClaimTypes.Role, logged.Role),
                    new Claim("Role", logged.Role),
                    new Claim(ClaimTypes.Name, logged.UserName),
                    new Claim(ClaimTypes.Email, logged.Email),
                };

                var userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(principal);

                if(logged.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin", new { area = "Administrator" });
                }

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
