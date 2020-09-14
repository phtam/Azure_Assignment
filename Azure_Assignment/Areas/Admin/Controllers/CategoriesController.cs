﻿using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        private string ftpChild = "imgCategories";
        private ImageProvider imgProvider = new ImageProvider();

        public ActionResult Index()
        {
            var list = db.Categories.ToList();
            foreach (var item in list)
            {
                item.Picture = ftp.Get(item.Picture, ftpChild);
            }

            return View(list);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = db.Categories.Find(id);
            categories.Picture = ftp.Get(categories.Picture, ftpChild);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,Description,Picture,ImageFile")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(categories.ImageFile.FileName);
                string extension = Path.GetExtension(categories.ImageFile.FileName);

                if(imgProvider.Validate(categories.ImageFile) != null)
                {
                    ViewBag.Error = imgProvider.Validate(categories.ImageFile);
                    return View("Create");
                }
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                categories.Picture = fileName;
                db.Categories.Add(categories);
                
                if (db.SaveChanges() > 0)
                {
                    ftp.Add(fileName, ftpChild, categories.ImageFile);
                    TempData["Notice_Create_Success"] = true;
                }
                ModelState.Clear();
                return RedirectToAction("Index");
                
            }

            return View(categories);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = db.Categories.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            Session["OldImage"] = categories.Picture;
            return View(categories);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,Description,Picture,ImageFile")] Categories categories, String imageOldFile)
        {
            if (ModelState.IsValid)
            {
                if (categories.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(categories.ImageFile.FileName);
                    string extension = Path.GetExtension(categories.ImageFile.FileName);

                    if (imgProvider.Validate(categories.ImageFile) != null)
                    {
                        ViewBag.Error = imgProvider.Validate(categories.ImageFile);
                        return View("Edit");
                    }
                    categories.Picture = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    ftp.Update(categories.Picture, ftpChild, categories.ImageFile, imageOldFile);
                }
                else
                {
                    categories.Picture = imageOldFile;
                }

                db.Entry(categories).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    Session.Remove("OldImage");
                    TempData["Notice_Save_Success"] = true;
                }
                return RedirectToAction("Index");
                
            }
            return View(categories);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = db.Categories.Find(id);
            categories.Picture = ftp.Get(categories.Picture, ftpChild);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Categories categories = db.Categories.Find(id);
                db.Categories.Remove(categories);
                
                if (db.SaveChanges() > 0)
                {
                    ftp.Delete(categories.Picture, ftpChild);
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
