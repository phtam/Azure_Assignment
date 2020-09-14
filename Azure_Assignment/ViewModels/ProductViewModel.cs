using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.ViewModels
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> OldUnitPrice { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ImgFileName { get; set; }
        public int ImgID { get; set; }
    }
}