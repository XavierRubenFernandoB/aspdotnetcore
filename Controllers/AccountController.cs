using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreProj.Models;
using NetCoreProj.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreProj.Controllers
{
    //test
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        //FOR CLIENT VALIDATION
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email}  is already in use.");
            }
        }

        [HttpGet]
        public IActionResult RegisterView()
        {
            return View();
        }

        [HttpPost]
        // GET: /<controller>/
        public async Task<IActionResult> RegisterView(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    City = model.City
                };

                //Create the user
                var result = await userManager.CreateAsync(user, model.Password);

                //login
                if (result.Succeeded) 
                {
                    //For email confirmation link
                    var v_token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationlink = Url.Action("ConfirmEmail", "Account", new { userid = user.Id, token = v_token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, confirmationlink);

                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    ViewBag.ErrorTitle = "Registration successful";
                    ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                            "email, by clicking on the confirmation link we have emailed you";
                    return View("Error");                    
                }

                //if error display on screen
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("listview", "home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        // GET: /<controller>/
        public async Task<IActionResult> Login(LoginViewModel model, string returnurl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                //Password check done to ensure its not related to password & also avoid security hacks
                if (user != null && !user.EmailConfirmed && (await userManager.CheckPasswordAsync(user, model.Password))) 
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }

                //Create the user
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                //Success
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnurl))
                    {
                        return LocalRedirect(returnurl);
                    }
                    else
                    {
                        return RedirectToAction("listview", "home");
                    }
                }

                //Failure
               ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View();
        }

    }
}
