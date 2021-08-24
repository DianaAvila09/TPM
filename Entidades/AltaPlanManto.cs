using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class AltaPlanManto
   {
      public List<CatEquipo> lstEquipos { get; set; }
      public List<SistemaManto> lstSisManto { get; set; }
      public List<Ciclos> lstCiclos { get; set; }
      public PlanMantto planMt { get; set; }
   }
}
