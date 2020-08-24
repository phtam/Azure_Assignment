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
    public class BlogsController : Controller
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
            //ViewBag.Username = new SelectList(db.Users, "Username", "FirtName");
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
                blogs.WritingDate = DateTime.Now;
                db.Blogs.Add(blogs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogCategoryID = new SelectList(db.BlogCategories, "BlogCategoryID", "BlogCategoryName", blogs.BlogCategoryID);
            //ViewBag.Username = new SelectList(db.Users, "Username", "FirtName", blogs.Username);
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
            ViewBag.Username = new SelectList(db.Users, "Username", "FirtName", blogs.Username);
            return View(blogs);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogID,BlogName,Username,Content,BlogCategoryID,WritingDate")] Blogs blogs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogCategoryID = new SelectList(db.BlogCategories, "BlogCategoryID", "BlogCategoryName", blogs.BlogCategoryID);
            ViewBag.Username = new SelectList(db.Users, "Username", "FirtName", blogs.Username);
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
            Blogs blogs = db.Blogs.Find(id);
            db.Blogs.Remove(blogs);
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
