using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Azure_Assignment.EF;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    public class BlogCommentsController : Controller
    {
        private DataPalkia db = new DataPalkia();

        // GET: Admin/BlogComments
        public ActionResult Index()
        {
            var blogComments = db.BlogComments.Include(b => b.Blogs);
            return View(blogComments.ToList());
        }

        // GET: Admin/BlogComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogComments blogComments = db.BlogComments.Find(id);
            Blogs blogs = db.Blogs.Find(blogComments.BlogID);
            ViewBag.Blog = blogs.BlogName;
            if (blogComments == null)
            {
                return HttpNotFound();
            }
            return View(blogComments);
        }

        // GET: Admin/BlogComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogComments blogComments = db.BlogComments.Find(id);
            Blogs blogs = db.Blogs.Find(blogComments.BlogID);
            ViewBag.Blog = blogs.BlogName;
            if (blogComments == null)
            {
                return HttpNotFound();
            }
            return View(blogComments);
        }

        // POST: Admin/BlogComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogComments blogComments = db.BlogComments.Find(id);
            db.BlogComments.Remove(blogComments);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
