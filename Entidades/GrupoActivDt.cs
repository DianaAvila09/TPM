using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class GrupoActivDt
   {
      public int IdDetGrupo { get; set; }
      public int IdGrupoAct { get; set; }
      public string CodGrupo { get; set; }
      public int IdActividad { get; set; }
      public string CodActividad { get; set; }
      public int Item { get; set; }
      public int Orden { get; set; }
      public string CategoriaAct { get; set; }
      public string DescripAct { get; set; }
      public string TipoOperacion { get; set; }
      public bool EqParado { get; set; }
      public bool ActivoActiv { get; set; }
      public string CodSistema { get; set; }
      public string DescripSist { get; set; }
      public DateTime FchModif { get; set; }

   }
}
