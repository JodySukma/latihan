using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Latihan.Data;
using Latihan.Models;

namespace Latihan.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> db;

        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            this.db = roleManager;
        }

        // GET: Role
        [HttpGet]
        public IActionResult Index()
        {
            var items = new List<RoleViewModel>();
            items = db.Roles.Select(r => new RoleViewModel
            {
                RoleID = r.Id,
                RoleName = r.Name,
                Description = r.Description
            }).ToList();
            return View(items);
        }

        // GET: Role/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            RoleViewModel item = new RoleViewModel();
            ApplicationRole role = await db.FindByIdAsync(id);
            if (role != null)
            {
                item.RoleID = role.Id;
                item.RoleName = role.Name;
                item.Description = role.Description;
            }
            return View(item);
        }

        // GET: Role/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel item)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = new ApplicationRole();
                role.Id = item.RoleID;
                role.Name = item.RoleName;
                role.Description = item.Description;
                var result = await db.CreateAsync(role);
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            RoleViewModel item = new RoleViewModel();
            ApplicationRole role = await db.FindByIdAsync(id);
            if (role != null)
            {
                item.RoleID = role.Id;
                item.RoleName = role.Name;
                item.Description = role.Description;
            }
            return View(item);
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("RoleID,RoleName,Description")] RoleViewModel item)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = await db.FindByIdAsync(item.RoleID);
                if (role != null)
                {
                    role.Id = item.RoleID;
                    role.Name = item.RoleName;
                    role.Description = item.Description;
                    var result = await db.UpdateAsync(role);
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            RoleViewModel item = new RoleViewModel();
            ApplicationRole role = await db.FindByIdAsync(id);
            if (role != null)
            {
                item.RoleID = role.Id;
                item.RoleName = role.Name;
                item.Description = role.Description;
            }
            return View(item);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = await db.FindByIdAsync(id);
                var result = await db.DeleteAsync(role);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
