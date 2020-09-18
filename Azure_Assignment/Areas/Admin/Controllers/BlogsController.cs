using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    public class BlogsController : Controller
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        private ImageProvider imgProvider = new ImageProvider();
        private string ftpChild = "imgSales";

        // GET: Admin/Blogs
        public ActionResult Index()
        {
            var list = db.Blogs.Include(b => b.BlogCategories).Include(b => b.Users).ToList();
            foreach (var item in list)
            {
                item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
            }
            return View(list);
        }

        // GET: Admin/Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blogs blogs = db.Blogs.Find(id);
            blogs.Thumbnail = ftp.Get(blogs.Thumbnail, ftpChild);
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
        public ActionResult Create([Bind(Include = "BlogID,BlogName,Username,Content,BlogCategoryID,WritingDate,ImageFile")] Blogs blogs)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.SingleOrDefault(u => u.Username == blogs.Username);
                if (check != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(blogs.ImageFile.FileName);
                    string extension = Path.GetExtension(blogs.ImageFile.FileName);

                    if (imgProvider.Validate(blogs.ImageFile) != null)
                    {
                        ViewBag.Error = imgProvider.Validate(blogs.ImageFile);
                        return View(blogs);
                    }
                    blogs.Thumbnail = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    db.Blogs.Add(blogs);
                    if (db.SaveChanges() > 0)
                    {
                        ftp.Add(blogs.Thumbnail, ftpChild, blogs.ImageFile);
                        TempData["Notice_Create_Success"] = true;
                    }
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
            Session["OldImage"] = blogs.Thumbnail;
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
        public ActionResult Edit([Bind(Include = "BlogID,BlogName,Username,Content,BlogCategoryID,WritingDate,ImageFile")] Blogs blogs, String imageOldFile)
        {
            if (ModelState.IsValid)
            {
                if (blogs.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(blogs.ImageFile.FileName);
                    string extension = Path.GetExtension(blogs.ImageFile.FileName);

                    if (imgProvider.Validate(blogs.ImageFile) != null)
                    {
                        ViewBag.Error = imgProvider.Validate(blogs.ImageFile);
                        return View("Edit");
                    }
                    blogs.Thumbnail = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    ftp.Update(blogs.Thumbnail, ftpChild, blogs.ImageFile, imageOldFile);
                }
                else
                {
                    blogs.Thumbnail = imageOldFile;
                }
                db.Entry(blogs).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    Session.Remove("OldImage");
                    TempData["Notice_Save_Success"] = true;
                }
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
            blogs.Thumbnail = ftp.Get(blogs.Thumbnail, ftpChild);
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
                if (db.SaveChanges() > 0)
                {
                    ftp.Delete(blogs.Thumbnail, ftpChild);
                    TempData["Notice_Delete_Success"] = true;
                }    
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
