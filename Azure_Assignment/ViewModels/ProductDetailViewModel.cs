using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.ViewModels
{
    public class ProductDetailViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> OldUnitPrice { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ImgFileName { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public Nullable<int> UnitsInStock { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string SaleName { get; set; }
        public Nullable<int> SaleID { get; set; }
        public Nullable<bool> Discontinued { get; set; }
        public int ImgID { get; internal set; }
    }
}