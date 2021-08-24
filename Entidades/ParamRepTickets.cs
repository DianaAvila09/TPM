using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class ParamRepTickets
   {
      public string Depto { get; set; }
      public DateTime FecInicial { get; set; }
      public DateTime FecFinal { get; set; }      
      public string Estatus { get; set; }
      public string Hallazgo { get; set; }
      public string CausoParo { get; set; }
      public List<Ticket> LstTickets { get; set; }

   }
}
