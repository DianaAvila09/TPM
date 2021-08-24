using System;
using System.ComponentModel.DataAnnotations;


namespace Entidades
{
   public class Falla
   {
      public int IdFalla { get; set; }
      [MaxLength(10, ErrorMessage = "Máximo 10 caracteres.")]
      [Display(Prompt = "Escriba código", Name = "Código: ")]
      [Required(ErrorMessage = "Dato Obligatorio, escriba una código para la falla")]
      public string CodFalla { get; set; }

      [Required(ErrorMessage = "Dato Obligatorio, escriba una descripción de la falla")]
      public string Descrip { get; set; }

      [Required(ErrorMessage = "Dato Obligatorio,Selecciones un sistema ")]
      public string CodSistema { get; set; }
      public string Sistema { get; set; }
      public bool StsSistema { get; set; }
      public string CodDepartamento { get; set; }
      public string departamento { get; set; }
      public string Tipo { get; set; }
      public bool StatusFalla { get; set; }

      [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
      public DateTime FecAlta { get; set; }

      [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
      public DateTime FecActualizacion { get; set; }
      public string UsuarioAlta { get; set; }
   }
}
