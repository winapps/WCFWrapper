using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WellAuditService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAuditService" in both code and config file together.
    [ServiceContract]
    public interface IAuditService
    {

        [OperationContract]
        [WebGet(UriTemplate = "/GetForm/{deviceType}/{version}")]
        [FaultContract(typeof(EHSFaultException))]
        Stream GetForm(string deviceType, string version);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/Regions")]
        [FaultContract(typeof(EHSFaultException))]
        List<string> GetRegions();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/Wells")]
        [FaultContract(typeof(EHSFaultException))]
        List<WellInfo> GetWells();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/GetWellInfo/{wellName}",
           BodyStyle = WebMessageBodyStyle.Wrapped)]
        [FaultContract(typeof(EHSFaultException))]    
        WellInfo GetWellInfo(string wellName);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/Operators",
           BodyStyle = WebMessageBodyStyle.Wrapped)]
        [FaultContract(typeof(EHSFaultException))] 
         List<string> GetOperators();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/Equipments",
           BodyStyle = WebMessageBodyStyle.Wrapped)]        
        [FaultContract(typeof(EHSFaultException))] 
        List<string> GetEquipments();

        [OperationContract]
        [WebInvoke(Method = "POST",
           UriTemplate = "/SyncAudit")]
        [FaultContract(typeof(EHSFaultException))]
        bool SyncAuditData(Stream fileContents);
    }

    [DataContract]
    public class WellInfo
    {
        string wellName;
        int wellNumber;
        string sectionTownshipRange;
        string wellSerialID;
        string latitude;
        string longitude;
        string state;
        string country;
        string wellDescription;

        
        [DataMember]
        public string WellName
        {
            get { return wellName; }
            set { wellName = value; }
        }
        [DataMember]
        public int WellNumber
        {
            get { return wellNumber; }
            set { wellNumber = value; }
        }     

        [DataMember]
        public string WellSerialID
        {
            get { return wellSerialID; }
            set { wellSerialID = value; }
        }
        [DataMember]
        public string SectionTownshipRange
        {
            get { return sectionTownshipRange; }
            set { sectionTownshipRange = value; }
        }
        [DataMember]
        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        [DataMember]
        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        [DataMember]
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        [DataMember]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        [DataMember]
        public string WellDescription
        {
            get { return wellDescription; }
            set { wellDescription = value; }
        }
    }

    [DataContract]
    public class EHSFaultException
    {

        private string _reason;

        [DataMember]

        public string Reason
        {

            get { return _reason; }

            set { _reason = value; }

        }

    }


}
