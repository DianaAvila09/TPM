using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
   public class CheckListEnc
   {
      public int IdCheckList { get; set; }

      [StringLength(20, ErrorMessage = "Máximo de 20 caracteres")]
      [Required(ErrorMessage = "Escriba una clave")]
      public string CodCheckList { get; set; }

      [StringLength(200, ErrorMessage = "Máximo de 200 caracteres")]
      [Required(ErrorMessage = "Escriba una descripción")]
      public string DescripCheckList { get; set; }

      [Required(ErrorMessage = "Seleccione un tipo")]
      public string CodClasif { get; set; }
      public bool EqParado { get; set; }
      public bool Activo { get; set; }
      public string CodDepartamento { get; set; }
      public string UserAlta { get; set; }

      [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
      public DateTime FchAlta { get; set; }
      public string UserModif { get; set; }

      [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
      public DateTime FchModif { get; set; }
      

   }
}
