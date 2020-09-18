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

    public partial class Blogs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Blogs()
        {
            this.BlogComments = new HashSet<BlogComments>();
        }

        [DisplayName("Blog ID")]
        public int BlogID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter blog name")]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "Blog name must be between 5 to 50")]
        [DisplayName("Blog name")]
        public string BlogName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter username")]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "Username must be between 5 to 50")]
        [DisplayName("Username")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter content")]
        [DisplayName("Content")]
        public string Content { get; set; }
        public Nullable<int> BlogCategoryID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please choose writing date")]
        [DataType(DataType.DateTime, ErrorMessage = "Writing date has been invalid")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}")]
        [DisplayName("Writing date")]
        public Nullable<System.DateTime> WritingDate { get; set; }
        public string Thumbnail { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
        public virtual BlogCategories BlogCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogComments> BlogComments { get; set; }
        public virtual Users Users { get; set; }
    }
}
