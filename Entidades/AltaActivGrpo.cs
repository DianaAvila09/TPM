using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class AltaActivGrupo
   {
      public GrupoActivDt ActNewGpo { get; set; }
      public GrupoActivEnc GpoEncab { get; set; }
      public List<Actividad> lstActiv { get; set; }
      public List<SistemaManto> lstSisManto { get; set; }
   }
}
