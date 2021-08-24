using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class KpiTicket
   {
      public int Anio { get; set; }
      public int Mes { get; set; }
      public string Periodo {get; set;}
      public string CodWorkCenter { get; set; }
      public string CodEquipo { get; set; }
      public int Total { get; set; }
      public int Abiertos { get; set; }
      public int Nuevos { get; set; }
      public int EnProceso { get; set; }
      public int Cerrados { get; set; }
      public int Criticos { get; set; }
      public int Alertas { get; set; }
      public int Mejoras { get; set; }
      public int Calidad { get; set; }
      public int Pky { get; set; }
      public int Mantto { get; set; }
      public decimal Trend { get; set; }
      public decimal Vacio { get; set; }
   }
}
