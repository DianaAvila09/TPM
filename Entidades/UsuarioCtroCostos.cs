using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class UsuarioCtroCostos
    {
        public int Id { get; set; }       
        public string CtroCostos { get; set; }
        public string WorkCenter { get; set; }
        public int Orden { get; set; }
    }
}
