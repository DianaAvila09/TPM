using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class SistemaManto
   {

      public string Id { get; set; }

      [Required(ErrorMessage = "Valor Obligatorio.")]
      [MaxLength(10, ErrorMessage = "Máximo 10 caracteres.")]
      [Display(Prompt = "Escriba código", Name = "Código: ")]
      public string CodSistema { get; set; }

      [Required(ErrorMessage = "Valor Obligatorio.")]
      [MaxLength(25, ErrorMessage = "Máximo 25 caracteres.")]
      [Display(Prompt = "Escriba la Descripción", Name = "Descripción: ")]
      public string Sistema { get; set; }

      [Required(ErrorMessage = "Valor Obligatorio.")]
      [Display(Name = "Departamento: ")]
      public string CodDepartamento { get; set; }
      [Required(ErrorMessage = "Valor Obligatorio.")]
      [Display(Name = "Estatus: ")]
      public bool Estatus { get; set; }

      public string keySistemas {get; set;}

   }

}
