using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eShop.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace eShop.Controllers
{
    public class RoleController : Controller
    {
        ApplicationDbContext _context;
        public RoleController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Role
        public ActionResult Index()
        {
            var Roles = _context.Roles.ToList();
            return View(Roles);
        }
        
        public ActionResult Create()
        {
            var Roles = new IdentityRole();
            return View(Roles);
        }
        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}