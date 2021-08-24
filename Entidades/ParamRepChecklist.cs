using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class ParamRepChecklist
   {
      public string CtroCostos { get; set; }
      public string Depto { get; set; }
      public DateTime FecInicial { get; set; }
      public DateTime FecFinal { get; set; }
      public List<EquipoPadre> lstEqPadres { get; set; }
      public List<CheckListEqEnc> lstChecklist { get; set; }
   }
}
