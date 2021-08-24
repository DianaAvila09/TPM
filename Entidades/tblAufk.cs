using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   // Tabla utilizada para obtener datos de las WOM's de sap
   public class tblAufk
   {
      public string Ordernum {get; set;}
      public string OrderType{ get; set;}
      public string OrderCategory { get; set;}
      public string Creadapor { get; set;}
      public string FchaCreacion { get; set;}
      public string Descripcion { get; set;}
      public string Planta { get; set;}
      public string CentroCtos { get; set;}
      public string FchaTeco { get; set;}
      public string GLAccount { get; set;}
      public string ObjectNumber { get; set;}
      public string ObjectClass { get; set;}
      public string TimeCreate { get; set;}
      public string MainWC { get; set;}
   }
}
