using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class RolAcceso
   {
      public int rol { get; set; }
      public string ClaveRol { get; set; }
      public string Descripcion { get; set; }
      public bool EstatusRol { get; set; }
      public bool CatTpm { get; set; }
      public bool EditarTicket { get; set; }
      public bool CatChecklist { get; set; }
      public bool CapturaChecklist { get; set; }
   }
}
