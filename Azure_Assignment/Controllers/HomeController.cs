﻿using Azure_Assignment.EF;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Azure_Assignment.DAO;
using Azure_Assignment.Providers;

namespace Azure_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
         
        public ActionResult Index()
        {
            ProductDAO productDAO = new ProductDAO();
            CategoryDAO categoryDAO = new CategoryDAO();
            SaleDAO saleDAO = new SaleDAO();

            ViewBag.One_New_Category = categoryDAO.GetNewCategories().Take(1);
            ViewBag.Four_Categories = categoryDAO.Get().Take(4);
            ViewBag.Five_Category_For_Filter = categoryDAO.Get().Take(5);
            ViewBag.Eight_New_Product = productDAO.GetProduct().Take(8);
            ViewBag.Sale = saleDAO.Get().Take(3);
            ViewBag.Best_Seller = productDAO.GetBestSellerProduct().Take(3);
            ViewBag.Sale_Product = productDAO.GetSaleProduct().Take(3);

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