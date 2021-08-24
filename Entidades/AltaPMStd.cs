using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class AltaPMStd
   {
      public PmStandar PmStd { get; set; }
      public List<EquipoPadre> lstEquipos { get; set; }
      public List<SistemaManto> lstSistMantto { get; set; }
      public List<Ciclos> lstCiclos { get; set; }
   }
}
