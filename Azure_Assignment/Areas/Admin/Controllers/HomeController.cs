using Azure_Assignment.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        DataPalkia db = new DataPalkia();
        // GET: Admin/Home
        [Authorize(Roles = "0")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string pass)
        {
            var user = db.Users.SingleOrDefault(model => model.Username == username && model.Password == pass);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Username, false);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorLogin = "Username or password incorrect";
                return View();
            }
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}