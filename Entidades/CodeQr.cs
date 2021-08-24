using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
   public class CodeQr
   {
      public int Id { get; set; }
      public string WorkCenter { get; set; }

      [Required(ErrorMessage = "Seleccione un equipo")]
      public string CodEquipo { get; set; }
      public string DescripEquipo { get; set; }
      public string CentroCostos { get; set; }

      [DataType(DataType.MultilineText)]
      public string Liga { get; set; }

      [Required(ErrorMessage = "Selecciones un Tipo")]
      public string Tipo { get; set; }
      public string DescripTipo { get; set; }
      public string WebServer { get; set; }
      public string Aplicacion { get; set; }
      public string userAlta { get; set; }
      public DateTime FecAlta { get; set; }
      public bool estatusCode { get; set; }
      public byte[] Qr { get; set; }
      public string Clasif {get; set;}
   }
}
