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
    
    public partial class ObservationPhoto
    {
        public int ObservationPhotoId { get; set; }
        public Nullable<int> ObservationID { get; set; }
        public string PhotoLink { get; set; }
        public string PhotoLabel { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual Observation Observation { get; set; }
        public virtual Observation Observation1 { get; set; }
    }
}
