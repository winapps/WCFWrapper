using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAdaptor
{
    public class EHSDataProcessor 
    {
        public List<string> GetWellOperators()
        {
            List<string> operators = null;

            using (EHSAuditEntities auditEntity = new EHSAuditEntities())
            {
                var operatorList = from oprt in auditEntity.Operators select oprt.OperatorName;
                operators = operatorList.ToList();
            }
            return operators;
        }

        public List<string> GetWellEquipments()
        {
            List<string> equipments = null;

            using (EHSAuditEntities auditEntity = new EHSAuditEntities())
            {
                var equipmentList = from oprt in auditEntity.EquipmentLists select oprt.EquipmentName;
                equipments = equipmentList.ToList();
            }
            return equipments;
        }

        public List<string> GetWellNames()
        {
            List<string> wellNames = null;

            using (EHSAuditEntities auditEntity = new EHSAuditEntities())
            {
                var wellList = from wellname in auditEntity.WellNames select wellname.WellName1;
                wellNames = wellList.ToList();
            }
            return wellNames;
        }

        public List<Well> GetAllWells()
        {
            List<Well> wellNames = null;

            using(EHSAuditEntities auditEntity = new EHSAuditEntities())
            {
                var wellList = from wells in auditEntity.Wells select wells;
                wellNames= wellList.ToList();                
            }
            return wellNames;
        }

        public Well GetWellInformation(string wellName)
        {
            Well wellInfo = new Well();
            using (EHSAuditEntities auditEntity = new EHSAuditEntities())
            {
                var wellList = from wells in auditEntity.Wells join wellnames in auditEntity.WellNames on wells.WellNameID equals wellnames.WellNameID where wellnames.WellName1 == wellName select wells;
                wellInfo = wellList.FirstOrDefault();
            }
            return wellInfo;
        }

        public bool UpdateAuditData()
        {
            using (EHSAuditEntities auditEntity = new EHSAuditEntities())
            {
            }
            return true;
        }

    }
}
