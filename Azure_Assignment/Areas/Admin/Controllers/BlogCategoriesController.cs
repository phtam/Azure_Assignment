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
    [Authorize(Roles = "0,1")]
    public class BlogCategoriesController : BaseController
    {
        private DataPalkia db = new DataPalkia();

        // GET: Admin/BlogCategories
        public ActionResult Index()
        {
            return View(db.BlogCategories.ToList());
        }

        // GET: Admin/BlogCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategories blogCategories = db.BlogCategories.Find(id);
            if (blogCategories == null)
            {
                return HttpNotFound();
            }
            return View(blogCategories);
        }

        // GET: Admin/BlogCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/BlogCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogCategoryID,BlogCategoryName")] BlogCategories blogCategories)
        {
            if (ModelState.IsValid)
            {
                db.BlogCategories.Add(blogCategories);
                db.SaveChanges();
                TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");
            }

            return View(blogCategories);
        }

        // GET: Admin/BlogCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategories blogCategories = db.BlogCategories.Find(id);
            if (blogCategories == null)
            {
                return HttpNotFound();
            }
            return View(blogCategories);
        }

        // POST: Admin/BlogCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogCategoryID,BlogCategoryName")] BlogCategories blogCategories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogCategories).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
            }
            return View(blogCategories);
        }

        // GET: Admin/BlogCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategories blogCategories = db.BlogCategories.Find(id);
            if (blogCategories == null)
            {
                return HttpNotFound();
            }
            return View(blogCategories);
        }

        // POST: Admin/BlogCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                BlogCategories blogCategories = db.BlogCategories.Find(id);
                db.BlogCategories.Remove(blogCategories);
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
