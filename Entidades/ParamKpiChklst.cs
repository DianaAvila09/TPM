using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
   public class ParamKpiChklst
   {
      public string CtroCostos { get; set; }
      public string Depto { get; set; }

      [Required(ErrorMessage = "Escriba un año válido")]
      public int AnioIni { get; set; }

      [Required(ErrorMessage = "Escriba un año válido")]
      public int AnioFin { get; set; }

      [Required]
      public int MesIni { get; set; }

      [Required]
      public int MesFin { get; set; }

      [Required(ErrorMessage = "Seleccione un equipo")]
      public string CodEquipo { get; set; }     
      public List<Meses> lstMesIni { get; set; }
      public List<Meses> lstMesFin { get; set; }
      public List<EquipoPadre> lstEqPadres { get; set; }
      public List<CheckListEqEnc> lstChecklist { get; set; }
      public List<GraficakpiChk> lstGraf { get; set; }
   }
}
