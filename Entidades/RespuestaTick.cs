using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class RespuestaTick
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string CodWorkCenter { get; set; }
        public string CodEquipo { get; set; }
        public string CodSubEquipo { get; set; }
        public int IdTicket { get; set; }
        public string TipoTicket { get; set; }
        public string DescripTipo { get; set; }
        public string CodClasif { get; set; }
        public string DescripCodClasif { get; set; }

        public DateTime FchReporte { get; set; }
        public DateTime FchAtendido { get; set; }
        public DateTime FchEntregaReparacion { get; set; }
        public DateTime FchClose { get; set; }
        public int MinRepAtn { get; set; }
        public int MinAtnRepa { get; set; }
        public int MinAtnCierre { get; set; }
        public int MinRepCierre { get; set; }
    }
}
