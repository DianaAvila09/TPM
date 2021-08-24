using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class TblSap_IFLO
   {
      public string FunctionalLocation { get; set; }
      public string LanguageKey { get; set; }
      public string Description { get; set; }
      public string SuperiorFunctLoc { get; set; }
      public string ObjectTypeWorkCenter { get; set; }
      public string ObjectNumber { get; set; }
      public string MaintPlanningPlant { get; set; }
      public string ControllingArea { get; set; }
      public string CostCenter { get; set; }
      public string PlannerGroup { get; set; }
      public string ObjectIDWorkCenter { get; set; }
      public int id { get; set; }
      public DateTime fecUpdate { get; set; }



   }
}
