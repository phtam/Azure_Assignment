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

    public partial class Suppliers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Suppliers()
        {
            this.Products = new HashSet<Products>();
        }

        [DisplayName("Supplier ID")]
        public int SupplierID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter supplier name")]
        [DisplayName("Supplier name")]
        public string CompanyName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter contact name")]
        [DisplayName("Contact name")]
        public string ContactName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter address")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessage = "Address must be between 5 to 100")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter phone")]
        [StringLength(maximumLength: 15, MinimumLength = 5, ErrorMessage = "Phone must be between 5 to 15")]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Incorrect Phone Number Format")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter email")]
        [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessage = "Email must be between 5 to 30")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Incorrect Email Format")]
        public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products> Products { get; set; }
    }
}