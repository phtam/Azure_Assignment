using Azure_Assignment.DAO;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Azure_Assignment.Controllers
{
    public class ShopController : Controller
    {
        
        ProductDAO productDAO = new ProductDAO();
        CategoryDAO categoryDAO = new CategoryDAO();
        SaleDAO saleDAO = new SaleDAO();
        SupplierDAO supplierDAO = new SupplierDAO();

        public ActionResult Index(int? id, int? page, int? min, int? max)
        {
            return MainView(id,page,min,max);
        }

        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            ViewBag.Categories_List = categoryDAO.Get();
            ViewBag.Suppliers_List = supplierDAO.Get();
            return PartialView();
        }

        public ActionResult MainView(int? id, int? page, int? min, int? max)
        {
            ViewBag.CurrentCategory = id;
            if (page == null) page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            if (min != null || max != null)
            {
                var model = new ProductDAO().GetProductsByCategory_Price(id, min, max).ToPagedList(pageNumber, pageSize);
                return PartialView(model);
            }
            else
            {
                var model = new ProductDAO().GetProductsByCategory(id).ToPagedList(pageNumber, pageSize);
                return PartialView(model);
            }
        }
    }
}