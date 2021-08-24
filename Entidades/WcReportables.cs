using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class WcReportables
   {
      public int Id { get; set; }
      public string WcReportable { get; set; }
      public string WcHijo { get; set; }
      public bool StatusHijo { get; set; }
   }
}
