using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class KpiPkykMes
    {
        public string Periodo { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Eventos { get; set; }
        public decimal Tendencia { get; set; }

    }
}
