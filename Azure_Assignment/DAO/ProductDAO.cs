using Azure_Assignment.EF;
using Azure_Assignment.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Azure_Assignment.Providers;

namespace Azure_Assignment.DAO
{
    public class ProductDAO
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        private string ftpChild = "imgThumbnailProducts";

        public List<ProductViewModel> GetProduct()
        {
            var list = (from pro in db.Products
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                          where pro.UnitsInStock > 0
                          select new ProductViewModel()
                          {
                              ProductID = pro.ProductID,
                              ProductName = pro.ProductName,
                              UnitPrice = pro.UnitPrice,
                              OldUnitPrice = pro.OldUnitPrice,
                              CategoryID = cate.CategoryID,
                              CategoryName = cate.CategoryName,
                              Thumbnail = pro.Thumbnail
                          }).ToList();

            foreach (var item in list)
            {
                item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
            }

            return list;
        }

        public List<ProductViewModel> GetSaleProduct()
        {
            var list = (from pro in db.Products
                          join sale in db.Sale on pro.SaleID equals sale.SaleID
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
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
                              Thumbnail = pro.Thumbnail
                          }).ToList();

            foreach (var item in list)
            {
                item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public List<ProductViewModel> GetBestSellerProduct()
        {
            var list = (from pro in db.Products
                          join sale in db.Sale on pro.SaleID equals sale.SaleID
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
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
                              Thumbnail = pro.Thumbnail
                          }).ToList();
            foreach (var item in list)
            {
                item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public List<ProductViewModel> GetHighlightProducts()
        {
            var list = (from pro in db.Products
                          join sale in db.Sale on pro.SaleID equals sale.SaleID
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
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
                              Thumbnail = pro.Thumbnail
                          }).ToList();
            foreach (var item in list)
            {
                item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
            }
            return list;
        }
    }
}