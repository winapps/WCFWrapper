using HttpMultipartParser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataAdaptor;
using System.Diagnostics;
using System.ServiceModel.Web;

namespace WellAuditService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AuditService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AuditService.svc or AuditService.svc.cs at the Solution Explorer and start debugging.
    public class AuditService : IAuditService
    {
        EHSDataProcessor processor = new EHSDataProcessor();

        public List<string> GetRegions()
        {
            EHSFaultException theFault = new EHSFaultException();

            theFault.Reason = "Reasons API is not implimented";

            throw new FaultException<EHSFaultException>(theFault);

        }
       
        public List<string> GetOperators()
        {
            List<string> operators = null;
            operators = processor.GetWellOperators();
            return operators;
        }

        public List<WellInfo> GetWells()
        {
            List<WellInfo> wellList = new List<WellInfo>();
            

            foreach (Well wl in processor.GetAllWells())
            {
                WellInfo wellInfo = new WellInfo();
                wellInfo.Country = wl.Country;
                wellInfo.Latitude = wl.Latitude;
                wellInfo.Longitude = wl.Longitude;
                wellInfo.State = wl.State;
                wellInfo.WellSerialID = wl.WellSerialID;
                wellInfo.WellNumber = wl.WellID;
                wellInfo.SectionTownshipRange = wl.SectionTownshipRange;
                wellInfo.WellDescription = wl.WellDescription;
                wellList.Add(wellInfo);
            }

            return wellList;
        }

        public WellInfo GetWellInfo(string wellName)
        {
            WellInfo wellInfo = new WellInfo();
            Well data =  processor.GetWellInformation(wellName);
            if (data != null)
            {
                wellInfo.Country = data.Country;
                wellInfo.Latitude = data.Latitude;
                wellInfo.Longitude = data.Longitude;
                wellInfo.State = data.State;
                wellInfo.WellSerialID = data.WellSerialID;
                wellInfo.WellNumber = data.WellID;
                wellInfo.SectionTownshipRange = data.SectionTownshipRange;
                wellInfo.WellDescription = data.WellDescription;
            }

            return wellInfo;
        }

        public List<string> GetEquipments()
        {
            List<string> operators = null;
            operators = processor.GetWellEquipments();
            return operators;
        }

        public Stream GetForm(string deviceType , string version)
        {
            string fileServer = ConfigurationManager.AppSettings["InstallationPath"];
            var directory = new DirectoryInfo(fileServer);
            string latestFile = null;
            Stream form = null;
            
            switch (deviceType)
            {
                case "android":
                    latestFile = directory.GetFiles("*android.*").OrderByDescending(f => f.LastWriteTime).First().FullName;
                    break;
                case "win8":
                    latestFile = directory.GetFiles("*win8.*").OrderByDescending(f => f.LastWriteTime).First().FullName;
                    break;
                case "ios":
                    latestFile = directory.GetFiles("*ios.*").OrderByDescending(f => f.LastWriteTime).First().FullName;
                    break;
            }

            if (CompareFileVersion(latestFile, version) < 0)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";               
                form = new FileStream(latestFile, FileMode.Open);                
            }
            else
            {
                EHSFaultException theFault = new EHSFaultException();
                theFault.Reason = "The current version is latest";
                throw new FaultException<EHSFaultException>(theFault);
            }
            
            return form;

        }

        public bool SyncAuditData(Stream fileContents)
        {

            bool result = false;
            EHSFaultException theFault = new EHSFaultException();
            FormData data = new FormData();
            try
            {

                data = ParseFormData(fileContents);
                if (data != null)
                {
                    foreach (WellEquipment eqp in data.WellEquipments)
                    {
                        byte[] buffer = StreamToByte(eqp.Image);
                        SaveImage(buffer, eqp.PhotoLink);
                    }
                    result = true;
                    //result = processor.UpdateAuditData();
                }
                else
                {
                    theFault.Reason = "Client data parsing failed";
                    throw new FaultException<EHSFaultException>(theFault);
                }


            }
            catch (Exception Ex)
            {
                

                theFault.Reason = Ex.Message;

                throw new FaultException<EHSFaultException>(theFault);
            }


            return result;
        }

        int CompareFileVersion(string latestFile, string clientVersion)
        {
            Version latestVersion = new Version(FileVersionInfo.GetVersionInfo(latestFile).FileVersion);            
            Version deviceVersion = new Version(clientVersion);
            return deviceVersion.CompareTo(latestVersion);

        }

        bool SaveImage(byte[] imageBuffer, String filename)
        {
            bool upload = false;
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(imageBuffer);
            bw.Close();
            upload = true;
            return upload;

        }

        byte[] StreamToByte(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();

            }

        }

        private FormData ParseFormData(Stream postData)
        {
            FormData data = new FormData();
            List<WellEquipment> equipments = new List<WellEquipment>();

            string filePath = ConfigurationManager.AppSettings["FileServerPath"];
            var parser = new MultipartFormDataParser(postData, Encoding.UTF8);

            data.SiteSettingID = "";
            data.OperatorID = "";
            data.WellID = "";
            data.AuditId = "";

            foreach (var image in parser.Files)
            {
                WellEquipment equipment = new WellEquipment();
                equipment.EquipmentID = "";
                equipment.Description = "Test equipment";
                equipment.PhotoLink = filePath + image.Name + DateTime.Now.ToString("s").Replace(':', '-') + ".jpg";
                equipment.Image = image.Data;
                equipments.Add(equipment);
            }
            data.WellEquipments = equipments;
            return data;
        }
    }

    public class FormData
    {
        string wellId;
        string operatorId;
        string siteSettingId;
        string auditId;

        List<WellEquipment> wellEquipments;

        public List<WellEquipment> WellEquipments
        {
            get { return wellEquipments; }
            set { wellEquipments = value; }
        }


        public string SiteSettingID
        {
            get { return siteSettingId; }
            set { siteSettingId = value; }
        }

        public string OperatorID
        {
            get { return operatorId; }
            set { operatorId = value; }
        }

        public string WellID
        {
            get { return wellId; }
            set { wellId = value; }
        }


        public string AuditId
        {
            get { return auditId; }
            set { auditId = value; }
        }

    }

    public class WellEquipment
    {
        string equipmentid;
        string description;
        Stream image;
        string photoLink;

        public string PhotoLink
        {
            get { return photoLink; }
            set { photoLink = value; }
        }


        public string EquipmentID
        {
            get { return equipmentid; }
            set { equipmentid = value; }
        }        

        public string Description
        {
            get { return description; }
            set { description = value; }
        }       

        public Stream Image
        {
            get { return image; }
            set { image = value; }
        } 

    }
}
