using Azure_Assignment.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Azure_Assignment.DAO;
using System.Data;

namespace Azure_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private DataPalkia db = new DataPalkia();
         
        public ActionResult Index()
        {
            ProductDAO productDAO = new ProductDAO();

            ViewBag.One_New_Category = new List<Categories>(db.Categories.OrderByDescending(c => c.CategoryID).ToList().Take(1));
            ViewBag.Four_Categories = new List<Categories>(db.Categories.ToList().Take(4));
            ViewBag.Five_Category_For_Filter = new List<Categories>(db.Categories.ToList().Take(5));
            ViewBag.Eight_New_Product = productDAO.getProduct().Take(8);
            ViewBag.Sale = new List<Sale>(db.Sale.OrderByDescending(s => s.SaleID).ToList()).Take(3);
            ViewBag.Best_Seller = productDAO.getBestSellerProduct().Take(3);
            ViewBag.Sale_Product = productDAO.getSaleProduct().Take(3);

            return View();
        }


        public ActionResult DetailProduct()
        {
            return View();
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