using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Azure_Assignment.EF;
using Scrypt;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private DataPalkia db = new DataPalkia();

        // GET: Admin/Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,FirtName,LastName,Password,Gender,Birthday,Phone,Email,Address,Picture,Role,Status,ImageFile")] Users users)
        {
            if (ModelState.IsValid)
            {
                
                string fileName = Path.GetFileNameWithoutExtension(users.ImageFile.FileName);
                string extension = Path.GetExtension(users.ImageFile.FileName);
                if ((extension == ".png" || extension == ".jpg" || extension == ".jpeg") == false)
                {
                    ViewBag.Error = String.Format("The File, which extension is {0}, hasn't accepted. Please try again!", extension);
                    return View("Create");
                }

                long fileSize = ((users.ImageFile.ContentLength) / 1024);
                if (fileSize > 5120)
                {
                    ViewBag.Error = "The File, which size greater than 5MB, hasn't accepted. Please try again!";
                    return View("Create");
                }

                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                users.Picture = "~/public/uploadedFiles/userPictures/" + fileName;
                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/userPictures/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                users.ImageFile.SaveAs(fileName);

                ScryptEncoder encoder = new ScryptEncoder();
                users.Password = encoder.Encode(users.Password);

                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            Session["OldImage_User"] = users.Picture;
            return View(users);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,FirtName,LastName,Gender,Birthday,Phone,Email,Address,Picture,Role,Status,ImageFile")] Users users, String imageOldFile_User)
        {
            if (ModelState.IsValid)
            {
                
                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/userPictures/");
                if (users.ImageFile == null)
                {
                    users.Picture = imageOldFile_User;
                }
                else
                {
                    if (!imageOldFile_User.IsEmpty())
                    {
                        System.IO.File.Delete(Server.MapPath(imageOldFile_User));
                    }

                    string fileName = Path.GetFileNameWithoutExtension(users.ImageFile.FileName);

                    string extension = Path.GetExtension(users.ImageFile.FileName);
                    if ((extension == ".png" || extension == ".jpg" || extension == ".jpeg") == false)
                    {
                        ViewBag.Error = String.Format("The File, which extension is {0}, hasn't accepted. Please try again!", extension);
                        return View("Edit");
                    }

                    long fileSize = ((users.ImageFile.ContentLength) / 1024);
                    if (fileSize > 5120)
                    {
                        ViewBag.Error = "The File, which size greater than 5MB, hasn't accepted. Please try again!";
                        return View("Edit");
                    }

                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    users.Picture = "~/public/uploadedFiles/userPictures/" + fileName;


                    if (Directory.Exists(uploadFolderPath) == false)
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    fileName = Path.Combine(uploadFolderPath, fileName);

                    users.ImageFile.SaveAs(fileName);
                }

                //db.Users.Attach(users);
                
                db.Entry(users).State = EntityState.Modified;
                db.Entry(users).Property(x => x.Password).IsModified = false;

                db.SaveChanges();
                Session.Remove("OldImage_User");
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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
