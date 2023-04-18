using MentalHealthApp.BusinessLogic.Implementation;
using MentalHealthApp.BusinessLogic.Implementation.Account;
using MentalHealthApp.BusinessLogic.Implementation.Account.Validations;
using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using MentalHealthApp.Common.DTOs;
using MentalHealthApp.Common.Extensions;
using MentalHealthApp.WebApp.Code;
using MentalHealthApp.WebApp.Code.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MentalHealthApp.WebApp.Controllers
{
    public class UserAccountController : BaseController
    {
        private readonly UserAccountService _userAccountService;
        private readonly ForumService _forumService;
        private readonly RegisterUserValidator _registerUserValidator;

        public UserAccountController(ControllerDependencies dependencies, UserAccountService userAccountService)
            :base(dependencies)
        {
            this._userAccountService = userAccountService;
            _registerUserValidator = new RegisterUserValidator();
        }

        private async Task LogIn(CurrentUserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email)
            };
            user.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                scheme: "MentalHealthAppCookies",
                principal: principal
                );
        }

        private async  Task LogOut()
        {
            await HttpContext.SignOutAsync(scheme: "MentalHealthAppCookies");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var user = _userAccountService.Login(model.Email, model.Password);
            if (user is null)
            {
                ModelState.AddModelError(nameof(LoginModel.Password), "Email/Password incorrect");
                return View(model);
            }

            if (!user.isAuthenticated)
            {
                model.AreCredentialsInvalid = true;
                return View(model);
            }
            await LogIn(user);
           // var role = _forumService.GetRole(user.Id ?? Guid.Empty);
            //if (role.Equals(RoleTypes.Pacient.ToString()))
            //{
            //    return RedirectToAction("UserProfile", "Profile");
            //}
            //else
            //{
                return RedirectToAction("Index", "Home");
            //}

        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await LogOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult DemoPage()
        {
            var model = _userAccountService.GetUsers();
            return View(model);
        }

        [HttpGet] 
        public IActionResult Register()
        {
            var model = new RegisterModel();
            return View("Register", model);
        }

        [HttpPost]

        public IActionResult Register(RegisterModel model)
        {
            //if(!ModelState.IsValid)
            //{
            //    ModelState.AddModelError(nameof(LoginModel), "Date invalide");
            //}
            _registerUserValidator.Validate(model).ThenThrow(model);
            _userAccountService.RegisterNewUser(model);
            return RedirectToAction("Index", "Home");
        }

       
    }
}
