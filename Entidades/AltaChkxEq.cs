using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entidades
{
   public class AltaChkxEq
   {
      public CheckListEqEnc ChklsxEq { get; set; }
      public List<CheckListDt> lstListAct { get; set; }
      public List<CheckListEnc> lstChecklst { get; set; }
      public  List<CatEquipo> lstEquipos { get; set; }
      public List<Ciclos> lstCiclos { get; set; }
      public List<CheckListEqDt> lstChckActEq { get; set; }

   }
}
