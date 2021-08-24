using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class CheckListDet
   {  
         public CheckListEnc Encabezado { get; set; }
         public List<CheckListDt> lstActChk { get; set; }
         public List<GrupoActivEnc> lstGrupos { get; set; }
         public List<Actividad> lstCatAct { get; set; }
   }
}
