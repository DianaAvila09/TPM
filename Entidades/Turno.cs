using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Turno
    {
        public string CtroCtosSap {get; set;}
        public string Depto { get; set;}
        public int Planta { get; set;}
        public int IdTurno { get; set; }
        public int THrI { get; set;}
        public int TMinI { get; set;}
        public int THrF { get; set;}
        public int TMinF { get; set;}   
        public string TurnoI { get; set; }
        public string TurnoF { get; set; }
  
    }
}
