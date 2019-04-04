using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Latihan.Models;
using Latihan.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Latihan.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // GET: User
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = new List<UserViewModel>();
            var userList = userManager.Users.ToList();
            foreach (var user in userList)
            {
                var item = new UserViewModel();
                item.UserName = user.UserName;
                item.Email = user.Email;
                item.Fullname = user.Fullname;
                string roleNameList = string.Empty;
                var roleList = await userManager.GetRolesAsync(user);
                foreach (var role in roleList)
                {
                    roleNameList = roleNameList + role + ", ";
                }
                item.RoleName = roleNameList.Substring(0, roleNameList.Length - 2); items.Add(item);
            }
            return View(items);
        }

        // GET: User/Details/5
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await userManager.FindByNameAsync(id);
            UserViewModel item = new UserViewModel();
            item.UserName = user.UserName;
            item.Email = user.Email;
            item.Fullname = user.Fullname;
            string roleNameList = string.Empty;
            var roleList = await userManager.GetRolesAsync(user);
            foreach (var role in roleList)
            {
                roleNameList = roleNameList + role + ", ";
            }
            item.RoleName = roleNameList.Substring(0, roleNameList.Length - 2); return View(item);
        }

        // GET: User/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Roles = new MultiSelectList(roleManager.Roles.ToList(), "Id", "Name");
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateFormViewModel item)
        {
            ViewBag.Roles = new MultiSelectList(roleManager.Roles.ToList(), "Id", "Name");
            if (ModelState.IsValid) { ApplicationUser user = new ApplicationUser();
                user.Fullname = item.FullName;
                user.UserName = item.UserName;
                user.Email = item.Email;
                IdentityResult result = await userManager.CreateAsync(user, item.Password);
                if (result.Succeeded)
                {
                    result = await userManager.AddToRolesAsync(user, item.RoleID);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        // GET: User/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Roles = new MultiSelectList(roleManager.Roles.ToList(), "Id", "Name");
            var user = await userManager.FindByNameAsync(id);
            UserEditFormViewModel item = new UserEditFormViewModel();
            item.UserName = user.UserName;
            item.Email = user.Email;
            item.FullName = user.Fullname;
            var roles = await userManager.GetRolesAsync(user);
            item.RoleID = roles.ToArray();
            return View(item);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserName, RoleID, Email, Password, PasswordConfirm, FullName")] UserEditFormViewModel item)
        {
            ViewBag.Roles = new MultiSelectList(roleManager.Roles.ToList(), "Id", "Name");
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(item.UserName);
                user.Email = item.Email;
                user.Fullname = item.FullName;
                var existingRoles = await userManager.GetRolesAsync(user);
                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.RemoveFromRolesAsync(user, existingRoles);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddToRolesAsync(user, item.RoleID);
                        if (result.Succeeded)
                        { return RedirectToAction("Index");
                        }
                    }
                }
            }
            return View();
        }

        // GET: User/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByNameAsync(id);
            UserViewModel item = new UserViewModel();
            item.UserName = user.UserName;
            item.Email = user.Email;
            item.Fullname = user.Fullname;
            string roleNameList = string.Empty;
            var roleList = await userManager.GetRolesAsync(user);
            foreach (var role in roleList)
            {
                roleNameList = roleNameList + role + ", ";
            }
            item.RoleName = roleNameList.Substring(0, roleNameList.Length - 2);
            return View(item);
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(id);
                var result = await userManager.DeleteAsync(user);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
