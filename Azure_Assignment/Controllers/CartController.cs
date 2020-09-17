using Azure_Assignment.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azure_Assignment.Common;
using Azure_Assignment.Models;
using System.Web.Script.Serialization;
using Azure_Assignment.EF;

namespace Azure_Assignment.Controllers
{
    public class CartController : Controller
    {
        DataPalkia db = new DataPalkia();
        public ActionResult Index()
        {
            var cart = Session[CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public ActionResult AddItem(int productID, int quantity)
        {
            var product = new ProductDAO().ViewDetail(productID);
            var cart = Session[CommonConstants.CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ProductID == productID))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ProductID == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[CommonConstants.CartSession] = list;
            }
            else
            {
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                Session[CommonConstants.CartSession] = list;
            }
            return RedirectToAction("Index");

        }

        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CommonConstants.CartSession];
            sessionCart.RemoveAll(x => x.Product.ProductID == id);
            Session[CommonConstants.CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult DeleteAll()
        {
            Session[CommonConstants.CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CommonConstants.CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ProductID == item.Product.ProductID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
                if (item.Quantity == 0)
                {
                    sessionCart.Remove(item);
                }
            }
            Session[CommonConstants.CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public ActionResult Checkout()
        {
            var cart = Session[CommonConstants.CartSession];
            
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }

            ViewBag.Payments = new PaymentDAO().GetAll();
            return View(list);
        }

        [HttpPost]
        public ActionResult Checkout(string username, string address, string note, int? payment)
        {
            if (address.Trim().Length == 0)
            {
                ViewBag.Error = "Please enter your address";
                return RedirectToAction("Checkout");
            }
            if (payment.ToString().Length == 0)
            {
                ViewBag.Error = "Please choice payment";
                return RedirectToAction("Checkout");
            }
            var order = new Orders();
            order.Username = username;
            order.ShippedAddress = address;
            order.Note = note;
            order.PaymentID = payment;
            order.CreationDate = DateTime.Now;
            order.Status = 0;

            var orderDAO = new OrderDAO();
            var orderDetailDAO = new OrderDetailDAO();
            try
            {
                var id = orderDAO.Insert(order);
                var cart = (List<CartItem>)Session[CommonConstants.CartSession];
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetails();
                    orderDetail.OrderID = (int)id;
                    orderDetail.ProductID = item.Product.ProductID;
                    orderDetail.UnitPrice = item.Product.UnitPrice;
                    orderDetail.Quantity = item.Quantity;

                    orderDetailDAO.Insert(orderDetail);
                }
            }
            catch (Exception)
            {
                return Redirect("cart");
            }
            Session[CommonConstants.CartSession] = null;
            return RedirectToAction("Index");
        }

    }
}