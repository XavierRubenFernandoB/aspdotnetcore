﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
                    // If the user is signed in and in the Admin role, then it is
                    // the Admin user that is creating a new user. So redirect the
                    // Admin user to ListRoles action
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("listview","home");
                }

                //if error display on screen
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
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
