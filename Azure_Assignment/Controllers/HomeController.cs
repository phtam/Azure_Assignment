using Azure_Assignment.EF;
using Azure_Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Azure_Assignment.DAO;

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

        


        //public ActionResult demo()
        //{
        //    String fileName = "tivi_led_samsung.jpg";
        //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://156.67.222.163:21/NhomHoangTam/" + fileName);
        //    request.Method = WebRequestMethods.Ftp.DownloadFile;
        //    request.Credentials = new NetworkCredential("u657022003.ftpuser", "123456789-Aa");

        //    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        //    Stream responseStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(responseStream);


        //    TempData["Image"] = reader.ReadToEnd().ToArray();
        //    TempData["status"] = response.StatusDescription;

        //    reader.Close();
        //    response.Close();
        //    return View();

        //}





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