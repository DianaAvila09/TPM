using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class WorkCenter
   {
      public string Planta { get; set; }
      public string TipoObjeto { get; set; }
      public decimal IdWorkCenter { get; set; }
      public string CodWorkCenter { get; set; }
      public string Categoria { get; set; }
      public string keyValorStandar { get; set; }
      public string Administrador { get; set; }
      public string FocusFactory { get; set; }
      public string keyValordeOperacion { get; set; }
      public DateTime InicioVigencia { get; set; }
      public DateTime FinVigencia { get; set; }
      public string Lenguaje { get; set; }
      public string Descripcion { get; set; }
      public DateTime FechaUpdate { get; set; }
      public int Id { get; set; }

   }
}
