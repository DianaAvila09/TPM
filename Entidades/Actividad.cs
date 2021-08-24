using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
   public class Actividad
   {
      public int IdActividad { get; set; }

      [StringLength(20, ErrorMessage = "Máximo de 20 caracteres")]
      [Required(ErrorMessage = "Escriba una clave")]
      public string CodActividad { get; set; }

      [StringLength(250, ErrorMessage = "Máximo de 250 caracteres")]
      [Required(ErrorMessage = "Escriba una descripción")]
      public string DescripcionAct { get; set; }
      public string CodSistema { get; set; }
      public string DescripSistema { get; set; }

      public int IdComponente { get; set; }
      public string DescripCompo { get; set; }

      public string CodDepartamento { get; set; }
      public string DescripDepto { get; set; }

      [Required(ErrorMessage = "Seleccione un tipo")]
      public string TipoOperacion { get; set; }

      public bool EqParado { get; set; }
      public bool Activo { get; set; }
      public string UserAlta { get; set; }

      [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
      public DateTime FchAlta { get; set; }
      public string UserModif { get; set; }

      [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
      public DateTime FchModif { get; set; }
      public string Llave { get; set; }
   }
}
