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
    
    public partial class Observation
    {
        public Observation()
        {
            this.ObservationPhotoes = new HashSet<ObservationPhoto>();
            this.ObservationPhotoes1 = new HashSet<ObservationPhoto>();
        }
    
        public int ObservationID { get; set; }
        public Nullable<int> AuditSiteWellEquipmentID { get; set; }
        public string ObservationDesc { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual ICollection<ObservationPhoto> ObservationPhotoes { get; set; }
        public virtual ICollection<ObservationPhoto> ObservationPhotoes1 { get; set; }
    }
}
