using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades
{
   public class CheckListDt
   {
      public int IdDetCheckList { get; set; }
      public int IdCheckList { get; set; }
      public string CodCheckList { get; set; }
      public int IdGrupoAct { get; set; }
      public string CodGpoActiv { get; set; }
      public string DescripGpo { get; set; }
      public string TipoActividad { get; set; }
      public int IdActividad { get; set; }
      public string CodActividad { get; set; }
      public string DescripAct { get; set; }
      public string TipoOperacion { get; set; }
      public bool EqParado { get; set; }
      public bool ActivoActiv { get; set; }
      public string CodSistema { get; set; }
      public string DescripSistema { get; set; }
      public int IdComponente { get; set; }
      public string DescripCompo { get; set; }

      [Column(TypeName = "decimal(10, 2)")]
      [Range(-999.99, 9999.99, ErrorMessage = "Valor fuera de rango")]
      public decimal RangoMin { get; set; }

      [StringLength(2, ErrorMessage = "Solo caracteres =, <,>, >=, <= ")]
      [RegularExpression("^(=|>|<|>=|<=)", ErrorMessage = "Caracter no válido")]
      public string OperadorMin { get; set; }


      [Column(TypeName = "decimal(10, 2)")]
      [Range(-999.99, 9999.99, ErrorMessage = "Valor fuera de rango")]
      public decimal RangoMax { get; set; }

      [StringLength(2, ErrorMessage = "Solo caracteres =, <,>, >=, <= ")]
      [RegularExpression("^(=|>|<|>=|<=)", ErrorMessage = "Caracter no válido")]
      public string OperadorMax { get; set; }

      [Column(TypeName = "decimal(6, 2)")]
      [Range(1, 100, ErrorMessage = "Valor de 1 - 100")]
      public decimal Ponderacion { get; set; }
      public int Item { get; set; }
      public int Orden { get; set; }
      public DateTime FchAlta {get; set;}
   }

}
