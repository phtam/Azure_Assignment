using Azure_Assignment.DAO;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Azure_Assignment.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();

        CategoryDAO categoryDAO = new CategoryDAO();
        SupplierDAO supplierDAO = new SupplierDAO();
        BlogCategoriesDAO blogCategoriesDAO = new BlogCategoriesDAO();
        BlogDAO blogDAO = new BlogDAO();
        BlogCommentDAO blogCommentDAO = new BlogCommentDAO();
        public ActionResult Index()
        {
            ViewBag.BlogCategoies = blogCategoriesDAO.getAllBlogCategories();
            ViewBag.Blog = blogDAO.getBlog().Take(3);
            //
            ViewBag.Layout_Menu = categoryDAO.Get().Take(2);
            ViewBag.Categories_List = categoryDAO.Get();
            ViewBag.Suppliers_List = supplierDAO.Get();
            ///
            return View();
        }

        public ActionResult BlogList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (db.BlogCategories.Find(id) == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogList = blogDAO.getBlogOfBlogCategories(id);
            ViewBag.BlogCategoies = blogCategoriesDAO.getAllBlogCategories();
            ViewBag.BlogCategoryName = db.BlogCategories.Find(id).BlogCategoryName;
            return View();
        }

        public ActionResult BlogDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.BlogDetail = blogDAO.getBlogdetail(id);
            ViewBag.BlogCategoies = blogCategoriesDAO.getAllBlogCategories();
            ViewBag.BlogAboutName = blogDAO.getBlogdetail(id);
            ViewBag.BlogAboutID = blogDAO.getBlogdetail(id);
            ViewBag.Blog = db.Blogs.Find(id);
            return View();
        }

        public ActionResult AddComment(BlogComments blogComments)
        {
            if (ModelState.IsValid)
            {
                var blogComment = new BlogCommentDAO().Insert(blogComments);
                if (blogComment == true)
                {
                    return RedirectToAction("BlogDetail", "Blog", new { id = blogComments.BlogID });
                }
            }
            return RedirectToAction("BlogDetail", "Blog", new { id = blogComments.BlogID });
        }
    }
}