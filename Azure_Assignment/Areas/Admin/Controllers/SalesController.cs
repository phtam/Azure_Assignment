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

namespace Azure_Assignment.Areas.Admin.Controllers
{
    public class SalesController : Controller
    {
        private DataPalkia db = new DataPalkia();

        // GET: Admin/Sales
        public ActionResult Index()
        {
            return View(db.Sale.ToList());
        }

        // GET: Admin/Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sale.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: Admin/Sales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaleID,SaleName,Content,StartDate,EndDate,Picture,Code,Discount,ImageFile")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                if(sale.StartDate > sale.EndDate)
                {
                    ViewBag.NotiDate = "The start date must be before the end date.";
                    return View("Create");
                }
                string fileName = Path.GetFileNameWithoutExtension(sale.ImageFile.FileName);
                string extension = Path.GetExtension(sale.ImageFile.FileName);
                if ((extension == ".png" || extension == ".jpg" || extension == ".jpeg") == false)
                {
                    ViewBag.Error = String.Format("The File, which extension is {0}, hasn't accepted. Please try again!", extension);
                    return View("Create");
                }

                long fileSize = ((sale.ImageFile.ContentLength) / 1024);
                if (fileSize > 5120)
                {
                    ViewBag.Error = "The File, which size greater than 5MB, hasn't accepted. Please try again!";
                    return View("Create");
                }

                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                sale.Picture = "~/public/uploadedFiles/salePictures/" + fileName;
                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/salePictures/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                sale.ImageFile.SaveAs(fileName);

                db.Sale.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sale);
        }

        // GET: Admin/Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sale.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            Session["OldImage"] = sale.Picture;
            return View(sale);
        }

        // POST: Admin/Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaleID,SaleName,Content,StartDate,EndDate,Picture,Code,Discount,ImageFile")] Sale sale, String imageOldFile)
        {
            if (ModelState.IsValid)
            {
                if (sale.StartDate > sale.EndDate)
                {
                    ViewBag.NotiDate = "The start date must be before the end date.";
                    return View("Edit");
                }
                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/salePictures/");
                if (sale.ImageFile == null)
                {
                    sale.Picture = imageOldFile;
                }
                else
                {
                    if (!imageOldFile.IsEmpty())
                    {
                        System.IO.File.Delete(Server.MapPath(imageOldFile));
                    }

                    string fileName = Path.GetFileNameWithoutExtension(sale.ImageFile.FileName);

                    string extension = Path.GetExtension(sale.ImageFile.FileName);
                    if ((extension == ".png" || extension == ".jpg" || extension == ".jpeg") == false)
                    {
                        ViewBag.Error = String.Format("The File, which extension is {0}, hasn't accepted. Please try again!", extension);
                        return View("Edit");
                    }

                    long fileSize = ((sale.ImageFile.ContentLength) / 1024);
                    if (fileSize > 5120)
                    {
                        ViewBag.Error = "The File, which size greater than 5MB, hasn't accepted. Please try again!";
                        return View("Edit");
                    }

                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    sale.Picture = "~/public/uploadedFiles/salePictures/" + fileName;


                    if (Directory.Exists(uploadFolderPath) == false)
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    fileName = Path.Combine(uploadFolderPath, fileName);

                    sale.ImageFile.SaveAs(fileName);
                }
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                Session.Remove("OldImage");
                return RedirectToAction("Index");

            }
            return View(sale);
        } 

        // GET: Admin/Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sale.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Admin/Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sale.Find(id);
            if (!sale.Picture.IsEmpty())
            {
                System.IO.File.Delete(Server.MapPath(sale.Picture));
            }
            db.Sale.Remove(sale);
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
