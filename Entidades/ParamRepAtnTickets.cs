using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ParamRepAtnTickets
    {
        public string Planta { get; set; }
        public string Depto { get; set; }
        public string CentroCostos { get; set; }
        public DateTime FecI { get; set; }
        public DateTime FecF { get; set; }
        public string StatusTicket {get; set;}
        public List<RespuestaTick> LstRepAtnTick { get; set; }

    }
}
