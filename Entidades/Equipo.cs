using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class Equipo
   {
      public string CodEquipo { get; set; }
      public string LengEqui { get; set; }
      public string TecObjAutGrp { get; set; }
      public string CategoryEqui { get; set; }
      public string TypeTechObj { get; set; }
      public string ManufAsset { get; set; }
      public string ManufModelNum { get; set; }
      public string ConsecutiveNum { get; set; }
      public string ObjectNumber { get; set; }
      public string MaintenancePlan { get; set; }
      public string MeasuringPoint { get; set; }
      public string DescripTechnical { get; set; }
      public string StatusInactive { get; set; }
      public string SystemStatus { get; set; }
      public string LengDescrip { get; set; }
      public string IndivStatusObject { get; set; }
      public string ObjectStatus { get; set; }
      public DateTime ValidDate { get; set; }
      public string NumNextEquipUsage { get; set; }
      public string MaintPlanningPlant { get; set; }
      public DateTime ValidFromDate { get; set; }
      public string Superordinate { get; set; }
      public string PlannerGroup { get; set; }
      public string PmObjType { get; set; }
      public string IdWorkCenter { get; set; }
      public string TechIdentNumber { get; set; }
      public string AccountAssignment { get; set; }
      public string CatalogProfile { get; set; }
      public string TechnicalInformation { get; set; }
      public string FunctionalLocation { get; set; }
      public string CrObjType { get; set; }
      public decimal ObjectIdPPWorkCenter { get; set; }
      public string ControllingArea { get; set; }
      public string CostCenter { get; set; }
      public string IdMainWorkCenter { get; set; }

      public string MainWorkCenter { get; set; }
      public string MainWCCategory { get; set; }
      public string MainWCLocation { get; set; }
      public string PersonResponsibleMWC { get; set; }
      public string StandardTextKeyMWC { get; set; }
      public string StandardValueKeyMWC { get; set; }
      public string KeyPerformanceEfficRateMWC { get; set; }
      public string WorkCenter { get; set; }
      public string CategoryWorkCenter { get; set; }
      public string LocationWorkCenter { get; set; }
      public string PersonResponsibleWC { get; set; }
      public string StandardTextKeyWC { get; set; }
      public string StandardValueKeyWC { get; set; }
      public string KeyPerformanceEfficRateWC { get; set; }
      public string ObjectTypesResource { get; set; }

      public int Id { get; set; }
   }
}
