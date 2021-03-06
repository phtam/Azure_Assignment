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

    public partial class BlogCategories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BlogCategories()
        {
            this.Blogs = new HashSet<Blogs>();
        }

        [DisplayName("Category ID")]
        public int BlogCategoryID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Blog category name")]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "Blog category name must be between 5 to 50")]
        [DisplayName("Category Name")]
        public string BlogCategoryName { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Blogs> Blogs { get; set; }
    }
}
