using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ParamKpiPkyk
    {
        public int Planta { get; set; }
        public string CtroCostos { get; set; }
        public string Depto { get; set; }
        public string TipoTick { get; set; }

        [Required(ErrorMessage = "Escriba un año válido")]
        public int AnioIni { get; set; }

        [Required(ErrorMessage = "Escriba un año válido")]
        public int AnioFin { get; set; }

        [Required(ErrorMessage = "Selecciones un mes")]
        public int MesIni { get; set; }

        [Required(ErrorMessage = "Selecciones un mes")]
        public int MesFin { get; set; }

        public int MesAtras { get; set; }
        public List<Mes> lstMesIni { get; set; }
        public List<Mes> lstMesFin { get; set; }
        public List<KpiPkyk> lstGrafpkyk { get; set; }
        public List<KpiPkyk> lstTop5pkyk { get; set; }
        public List<KpiPkykMes> lstpkykxMes { get; set; }

        public int Top5Anio { get; set; }
        public int Top5Mes { get; set; }
        public string Pareto { get; set; }

        public SensorBitacora bitacora { get; set; }
    }
}
