using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class AltaCodeQr
   {
      public CodeQr CodigoQr { get; set; }

      public List<EquipoPadre> lstEquPadres { get; set; }
      public List<TipoQr> lstTiposQr { get; set; }

   }
}
