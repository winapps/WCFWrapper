//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAdaptor
{
    using System;
    using System.Collections.Generic;
    
    public partial class Finding
    {
        public int FindingID { get; set; }
        public int AuditID { get; set; }
        public Nullable<int> FindingTypeCode { get; set; }
        public Nullable<int> FieldDesc { get; set; }
    
        public virtual Audit Audit { get; set; }
        public virtual Finding Finding1 { get; set; }
        public virtual Finding Finding2 { get; set; }
    }
}
