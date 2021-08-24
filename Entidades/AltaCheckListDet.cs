using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class AltaCheckListDet
   {
      public CheckListEnc Encabezado { get; set; }
      public CheckListDt ActChk { get; set; }
      public List<GrupoActivEnc> lstGrupos { get; set; }
      public List<Actividad> lstCatAct { get; set; }
   }
}
