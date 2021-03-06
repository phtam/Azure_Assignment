//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Azure_Assignment.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class Categories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categories()
        {
            this.Products = new HashSet<Products>();
        }

        [DisplayName("Category ID")]
        public int CategoryID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter category name")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Category name must be between 3 to 50")]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter description of category")]
        [StringLength(maximumLength: 250, MinimumLength = 5, ErrorMessage = "Category name must be between 5 to 250")]
        [DisplayName("Description")]
        public string Description { get; set; }
        public string Picture { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products> Products { get; set; }
    }
}
