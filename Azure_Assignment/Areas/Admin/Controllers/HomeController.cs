using Azure_Assignment.EF;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    [Authorize(Roles = "0,1")]
    public class HomeController : BaseController
    {
        private DataPalkia db = new DataPalkia();
        [Authorize]
        public ActionResult Index()
        {
            var productInStock = 0;
            var productOnOrder = 0;
            var total = 0;
            var import = 0;
            var export = 0;
            
            foreach(var item in db.Products.ToList())

            {
                productInStock += item.UnitsInStock.GetValueOrDefault(0);
                productOnOrder += item.UnitsOnOrder.GetValueOrDefault(0);
            }
            var _baseInfo = new int[] { productInStock, (db.Feedbacks.Count() + db.BlogComments.Count()), db.Orders.Count(), db.Users.Count() };

            ViewBag.BaseInfo = _baseInfo;

            foreach (var item in db.OrderDetails.ToList())
            {
                total += (item.Quantity.GetValueOrDefault(0) * item.UnitPrice.GetValueOrDefault(0));
            }
            foreach (var item in db.Importation.ToList())
            {
                import += (item.Quantity.GetValueOrDefault(0) * item.UnitPrice.GetValueOrDefault(0));
            }
            foreach (var item in db.Exportation.ToList())
            {
                export += (item.Quantity.GetValueOrDefault(0) * item.UnitPrice.GetValueOrDefault(0));
            }
            var _total = new int[] { productOnOrder, total, import, export };
            ViewBag.Total = _total;

            var products = (from pro in db.Products
                            join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                            orderby cate.CategoryID descending
                            select pro).Take(7);
            ViewBag.Categories = db.Categories.OrderByDescending(i=>i.CategoryID).ToList().Take(5);
            // chart.js
            //var labelChart = db.Categories.Select(x => x.CategoryName).Distinct();

            //List<int> data = new List<int>();
            //foreach (var item in db.Products.Where(x => x.CategoryID == )) ;
            //{

            //}
            //data = ;
            //TempData["lbCategories"] = 


            return View();
        }

        
    }
}