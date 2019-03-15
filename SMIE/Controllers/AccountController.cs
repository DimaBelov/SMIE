using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SMIE.DAL.Entities;
using SMIE.DAL.Interfaces;
using SMIE.Models;

namespace SMIE.Controllers
{
    public class AccountController : Controller
    {
        readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.Get(model.UserNameOrEmail, model.Password);
            if (user != null)
            {
                await Authenticate(user); // аутентификация

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Incorrect login and (or) password");
            return View(model);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if(await _userService.IsUserNameExsists(model.UserName))
                ModelState.AddModelError("", "This user name is already registered");
            else if (await _userService.IsEmailExsists(model.Email))
                ModelState.AddModelError("", "This email is already registered");
            else
            {
                await _userService.Add(new User { UserName = model.UserName, Email = model.Email, Password = model.Password });

                //await Authenticate(model.Email); // аутентификация

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim("email", user.Email)
            };

            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookies", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(AppConstants.DefaultAuthScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AppConstants.DefaultAuthScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
