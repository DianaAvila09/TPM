using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class AltaActividad
   {
      public Actividad Activi { get; set; }
      public List<Componente> lstCompo { get; set; }
      public List<SistemaManto> lstSisManto { get; set; }
   }
}
