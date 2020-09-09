using Azure_Assignment.EF;
using Azure_Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Azure_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private DataPalkia db = new DataPalkia();

        public ActionResult Index()
        {
            ViewBag.One_New_Category = new List<Categories>(db.Categories.OrderByDescending(c => c.CategoryID).ToList().Take(1));
            ViewBag.Four_Categories = new List<Categories>(db.Categories.ToList().Take(4));
            ViewBag.Five_Category_For_Filter = new List<Categories>(db.Categories.ToList().Take(5));
            ViewBag.Eight_New_Product = getProduct().Take(8);
            ViewBag.Sale = new List<Sale>(db.Sale.OrderByDescending(s => s.SaleID).ToList()).Take(3);
            

            return View();
        }

        private List<ProductViewModel> getProduct()
        {
            var product = from pro in db.Products
                          from img in pro.ProductImage
                          select new ProductViewModel()
                          {
                              ProductID = pro.ProductID,
                              ProductName = pro.ProductName,
                              UnitPrice = pro.UnitPrice,
                              OldUnitPrice = pro.OldUnitPrice,
                              ImgFileName = img.ImgFileName
                          };
            
            return product.ToList();
        }
        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UploadTooBig()
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
        }
    }
}