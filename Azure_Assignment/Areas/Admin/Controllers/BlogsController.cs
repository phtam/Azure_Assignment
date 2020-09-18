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
    [Authorize]
    public class BlogsController : BaseController
    {
        private DataPalkia db = new DataPalkia();

        // GET: Admin/Blogs
        public ActionResult Index()
        {
            var blogs = db.Blogs.Include(b => b.BlogCategories).Include(b => b.Users);
            return View(blogs.ToList());
        }

        // GET: Admin/Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blogs blogs = db.Blogs.Find(id);
            BlogCategories blogCategories = db.BlogCategories.Find(blogs.BlogCategoryID);
            ViewBag.BlogCategory = blogCategories.BlogCategoryName;
            if (blogs == null)
            {
                return HttpNotFound();
            }
            return View(blogs);
        }

        // GET: Admin/Blogs/Create
        public ActionResult Create()
        {
            ViewBag.BlogCategoryID = new SelectList(db.BlogCategories, "BlogCategoryID", "BlogCategoryName");
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "BlogID,BlogName,Username,Content,BlogCategoryID,WritingDate")] Blogs blogs)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.SingleOrDefault(u => u.Username == blogs.Username);
                if (check != null)
                {
                    db.Blogs.Add(blogs);
                    db.SaveChanges();
                    TempData["Notice_Create_Success"] = true;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.NotifyUser = "User does not exist, please try again.";
                    ViewBag.BlogCategoryID = new SelectList(db.BlogCategories, "BlogCategoryID", "BlogCategoryName");
                    return View("Create");
                }
                
            }

            ViewBag.BlogCategoryID = new SelectList(db.BlogCategories, "BlogCategoryID", "BlogCategoryName", blogs.BlogCategoryID);
            return View(blogs);
        }

        // GET: Admin/Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blogs blogs = db.Blogs.Find(id);
            if (blogs == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogCategoryID = new SelectList(db.BlogCategories, "BlogCategoryID", "BlogCategoryName", blogs.BlogCategoryID);
            //ViewBag.Username = new SelectList(db.Users, "Username", "FirtName", blogs.Username);
            return View(blogs);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "BlogID,BlogName,Username,Content,BlogCategoryID,WritingDate")] Blogs blogs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogs).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
            }
            ViewBag.BlogCategoryID = new SelectList(db.BlogCategories, "BlogCategoryID", "BlogCategoryName", blogs.BlogCategoryID);
            return View(blogs);
        }

        // GET: Admin/Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blogs blogs = db.Blogs.Find(id);
            BlogCategories blogCategories = db.BlogCategories.Find(blogs.BlogCategoryID);
            ViewBag.BlogCategory = blogCategories.BlogCategoryName;
            if (blogs == null)
            {
                return HttpNotFound();
            }
            return View(blogs);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Blogs blogs = db.Blogs.Find(id);
                db.Blogs.Remove(blogs);
                db.SaveChanges();
                TempData["Notice_Delete_Success"] = true;
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }

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
