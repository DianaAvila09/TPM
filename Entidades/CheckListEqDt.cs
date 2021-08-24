using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class CheckListEqDt
   {
      public int IdDtCheckList { get; set; }
      public int IdChkEquipo { get; set; }
      public string CodWorkCenter { get; set; }
      public string CodEquipo { get; set; }
      public int IdCheckList { get; set; }
      public string CodChkList { get; set; }
      public string CodGpoActiv { get; set; }
      public int IdActividad { get; set; }
      public string CodActividad { get; set; }
      public string DescripcionAct { get; set; }
      public string CodSistema { get; set; }
      public string DescripSistema { get; set; }
      public int IdComponente { get; set; }
      public string DescripCompo { get; set; }
      public int Orden { get; set; }
      public int Item { get; set; }
      public string TipoActividad { get; set; }
      public string TipoOperacion { get; set; }
      public bool EqParado { get; set; }

      [Column(TypeName = "decimal(10, 2)")]
      [Range(-999.99, 9999.99, ErrorMessage = "Valor fuera de rango")]
      public decimal RangoMin { get; set; }
      public string OperadorMin { get; set; }

      [Column(TypeName = "decimal(10, 2)")]
      [Range(-999.99, 9999.99, ErrorMessage = "Valor fuera de rango")]
      public decimal RangoMax { get; set; }
      public string OperadorMax { get; set; }

      public decimal Ponderacion { get; set; }
      public bool Activo { get; set; }

      public bool? ResultVisual { get; set;  }

      [Column(TypeName = "decimal(10, 2)")]
      [Range(-999.99, 9999.99, ErrorMessage = "Valor fuera de rango")]
      public decimal? ResultMedible { get; set; }
      public string Criterio { get; set; }
      public string CodUom { get; set; }
      public string DescripUom { get; set; }
      public bool? ResultActiv { get; set; }
   }
}

