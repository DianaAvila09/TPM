using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class KpiPkyk
    {
        public string Periodo { get; set; }
       public string CodWorkCenter { get; set; }
        public string CodEquipo { get; set; }
        public string Sensor { get; set; }
        public decimal EventosHist { get; set; }
        public decimal EventosMes { get; set; }
      
    }
}
