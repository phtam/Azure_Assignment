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
    
    public partial class ReplyBlogComments
    {
        public int ReplyID { get; set; }
        public string ReplyName { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> ReplyDate { get; set; }
        public Nullable<int> BlogCommentID { get; set; }
    
        public virtual BlogComments BlogComments { get; set; }
    }
}
