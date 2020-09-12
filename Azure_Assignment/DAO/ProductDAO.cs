using Azure_Assignment.EF;
using Azure_Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.DAO
{
    public class ProductDAO
    {
        DataPalkia db = new DataPalkia();

        public List<ProductViewModel> getProduct()
        {
            var product = from pro in db.Products
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                          from img in pro.ProductImage
                          where pro.UnitsInStock > 0
                          select new ProductViewModel()
                          {
                              ProductID = pro.ProductID,
                              ProductName = pro.ProductName,
                              UnitPrice = pro.UnitPrice,
                              OldUnitPrice = pro.OldUnitPrice,
                              CategoryID = cate.CategoryID,
                              CategoryName = cate.CategoryName,
                              ImgFileName = img.ImgFileName
                          };

            return product.ToList();
        }

        public List<ProductViewModel> getSaleProduct()
        {
            var product = from pro in db.Products
                          join sale in db.Sale on pro.SaleID equals sale.SaleID
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                          from img in pro.ProductImage
                          where pro.UnitsInStock > 0 && sale.SaleID > 1
                          orderby sale.SaleID descending
                          select new ProductViewModel()
                          {
                              ProductID = pro.ProductID,
                              ProductName = pro.ProductName,
                              UnitPrice = pro.UnitPrice,
                              OldUnitPrice = pro.OldUnitPrice,
                              SaleID = sale.SaleID,
                              SaleName = sale.SaleName,
                              CategoryID = cate.CategoryID,
                              CategoryName = cate.CategoryName,
                              ImgFileName = img.ImgFileName
                          };

            return product.ToList();
        }

        public List<ProductViewModel> getBestSellerProduct()
        {
            var product = from pro in db.Products
                          join sale in db.Sale on pro.SaleID equals sale.SaleID
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                          from img in pro.ProductImage
                          where pro.UnitsInStock > 0
                          orderby pro.UnitsOnOrder descending
                          select new ProductViewModel()
                          {
                              ProductID = pro.ProductID,
                              ProductName = pro.ProductName,
                              UnitPrice = pro.UnitPrice,
                              OldUnitPrice = pro.OldUnitPrice,
                              SaleID = sale.SaleID,
                              SaleName = sale.SaleName,
                              CategoryID = cate.CategoryID,
                              CategoryName = cate.CategoryName,
                              ImgFileName = img.ImgFileName
                          };

            return product.ToList();
        }

        public List<ProductViewModel> getCheapAccessories()
        {
            var product = from pro in db.Products
                          join sale in db.Sale on pro.SaleID equals sale.SaleID
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                          from img in pro.ProductImage
                          where pro.UnitsInStock > 0
                          orderby pro.UnitsOnOrder descending
                          select new ProductViewModel()
                          {
                              ProductID = pro.ProductID,
                              ProductName = pro.ProductName,
                              UnitPrice = pro.UnitPrice,
                              OldUnitPrice = pro.OldUnitPrice,
                              SaleID = sale.SaleID,
                              SaleName = sale.SaleName,
                              CategoryID = cate.CategoryID,
                              CategoryName = cate.CategoryName,
                              ImgFileName = img.ImgFileName
                          };

            return product.ToList();
        }
    }
}