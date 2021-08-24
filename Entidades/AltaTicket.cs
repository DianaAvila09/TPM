using System;
using System.Collections.Generic;

namespace Entidades
{
  public class AltaTicket
   {
      public Ticket Ticket { get; set; }
      public List<Falla> lstFallas { get; set; }
      public List<TipoFalla> lstTipoFalla { get; set; }
      public List<ClasificFalla> lstClasifFalla { get; set; }
      public List<SistemaManto> lstSistMantto { get; set; }
      public List<CatEquipo> lstExtrEqui { get; set; }
      public List<Ciclos> lstCatCiclos { get; set; }

   }
}
