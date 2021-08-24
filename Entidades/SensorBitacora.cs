using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class SensorBitacora
    {
        public DateTime FchInicio { get; set; }
        public DateTime FchFin { get; set; }
        public string WorkCenter { get; set; }
        public string Sensor { get; set; }
        public string Usuario { get; set; }
        public string Estatus { get; set; }
        public List<WorkCter> lstWc { get; set; }
        public List<StsBitSensor> lstEstatus { get; set; }
        public List<SensorActividad> lstBitacora {get;set;}

    }
}
