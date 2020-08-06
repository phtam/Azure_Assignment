using Azure_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {

        DataContext db = new DataContext();
        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Category category)
        {
            category.Picture = null;
            db.Categories.Add(category);
            db.SaveChanges();
            
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(String txtEmail, String txtPass)
        {
            ViewBag.Mail = txtEmail;
            ViewBag.Pass = txtPass; 
            return View();
        }
    }
}