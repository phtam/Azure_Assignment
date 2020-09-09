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
    public class ProductsController : Controller
    {
        private DataPalkia db = new DataPalkia();

        // GET: Admin/Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Categories).Include(p => p.Sale).Include(p => p.Suppliers);
            return View(products.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.SaleID = new SelectList(db.Sale, "SaleID", "SaleName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,UnitPrice,OldUnitPrice,ShortDescription,Description,Specification,UnitsInStock,UnitsOnOrder,SupplierID,CategoryID,SaleID,Discontinued")] Products products)
        {
            if (ModelState.IsValid)
            {
                products.UnitsInStock = 0;
                products.UnitsOnOrder = 0;
                db.Products.Add(products);
                db.SaveChanges();
                TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SaleID = new SelectList(db.Sale, "SaleID", "SaleName", products.SaleID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SaleID = new SelectList(db.Sale, "SaleID", "SaleName", products.SaleID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,UnitPrice,OldUnitPrice,ShortDescription,Description,Specification,UnitsInStock,UnitsOnOrder,SupplierID,CategoryID,SaleID,Discontinued")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.Entry(products).Property(x => x.UnitsInStock).IsModified = false;
                db.Entry(products).Property(x => x.UnitsOnOrder).IsModified = false;
                db.SaveChanges();
                TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SaleID = new SelectList(db.Sale, "SaleID", "SaleName", products.SaleID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Products products = db.Products.Find(id);
                db.Products.Remove(products);
                db.SaveChanges();
                TempData["Notice_Delete_Success"] = true;
            }
            catch (System.Data.SqlClient.SqlException )
            {
                TempData["Notice_Delete_Fail"] = true;
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }
            return RedirectToAction("Index");

        }

        // GET: Admin/Products/Edit/5
        public ActionResult ProductImage(int? id)
        {
            if (id == null || db.Products.Find(id) == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var productImage = db.ProductImage.Include(p => p.Products).Where(m => m.ProductID == id);
            string[] proinfo = new string[] { id.ToString() , db.Products.Find(id).ProductName.ToString() };
            ViewBag.ProInfo = proinfo;

            return View(productImage.ToList());
        }

        [HttpPost]
        public ActionResult AddImage(int ProductID, HttpPostedFileBase ImageFile)
        {
            
            ProductImage proImage = new EF.ProductImage();
            proImage.ProductID = ProductID;

            string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
            string extension = Path.GetExtension(ImageFile.FileName);
            if ((extension == ".png" || extension == ".jpg" || extension == ".jpeg") == false)
            {
                TempData["Error"] = String.Format("The File, which extension is {0}, hasn't accepted. Please try again!", extension);
                return RedirectToAction("ProductImage", "Products", new { @id = proImage.ProductID });
            }

            long fileSize = ((ImageFile.ContentLength) / 1024);
            if (fileSize > 5120)
            {
                TempData["Error"] = "The File, which size greater than 5MB, hasn't accepted. Please try again!";
                return RedirectToAction("ProductImage", "Products", new { @id = proImage.ProductID });
            }

            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            proImage.ImgFileName = "~/public/uploadedFiles/productPictures/" + fileName;
            string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/productPictures/");

            if (Directory.Exists(uploadFolderPath) == false)
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            fileName = Path.Combine(uploadFolderPath, fileName);

            ImageFile.SaveAs(fileName);

            db.ProductImage.Add(proImage);
            db.SaveChanges();
            TempData["Notice_Save_Success"] = true;
            return RedirectToAction("ProductImage", "Products", new { @id = proImage.ProductID });
        }

        [HttpPost, ActionName("DeleteImage")]
        public ActionResult DeleteImage(int proImage, int proID)
        {
            ProductImage productImage = db.ProductImage.Find(proImage);
            if (!productImage.ImgFileName.IsEmpty())
            {
                System.IO.File.Delete(Server.MapPath(productImage.ImgFileName));
            }

            db.ProductImage.Remove(productImage);
            db.SaveChanges();
            TempData["Notice_Delete_Success"] = true; 
            return RedirectToAction("ProductImage", "Products", new { @id = proID });
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
