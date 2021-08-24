using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class SensorActividad
    {
        public int Id { get; set; }
        public  string Sensor { get; set; }
        public string Celda { get; set; }
        public string Causa { get; set; }
        public string Accion { get; set; }
        public string UsrRegistro { get; set; }
        public DateTime FchRegistro { get; set; }
        public string Estatus { get; set; }
    }
}
