using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entidades
{
    public class ParamKpiTpm
    {
        public int Planta { get; set; }
        public string CtroCostos { get; set; }
        public string Depto { get; set; }

        [Required(ErrorMessage = "Escriba un año válido")]
        public int AnioIni { get; set; }

        [Required(ErrorMessage = "Escriba un año válido")]
        public int AnioFin { get; set; }

        [Required(ErrorMessage = "Selecciones un mes")]
        public int MesIni { get; set; }

        [Required(ErrorMessage = "Selecciones un mes")]
        public int MesFin { get; set; }
        public int tipoRep { get; set; }
        public string WorkCenter { get; set; }
        public string FocusFactory { get; set; }

        public List<Mes> lstMesIni { get; set; }
        public List<Mes> lstMesFin { get; set; }

        public List<EquipoPadre> lstEqPadres { get; set; }
        public List<KpiTpmCumpl> lstGrafCumpl { get; set; }

        public List<EquipoTpmBasico> lstTop5Cumpl { get; set; }
        public List<KpiTpmEfic> lstGrafEfic { get; set; }
        public List<KpiTop5> lstTop5Efic { get; set; }
        public decimal MetaCumpl { get; set; }
        public decimal MetaEfic { get; set; }

        public List<KpiTicket> lstKpiTick { get; set; }
        public List<Top5WCTick> lstTop5WcTick { get; set; }
        public int top5Anio { get; set; }
        public int top5Mes { get; set; }
        public List<FocusFactory> lstFocus { get; set; }
        public List<KpiTm> lstKpiMt { get; set; }
        public decimal MetaMtrrAuto { get; set; }
        public decimal MetaMtbfAuto { get; set; }
        public decimal MetaMtrrMnt { get; set; }
        public decimal MetaMtbfMnt { get; set; }
        public decimal MetaMtrrTkl { get; set; }
        public decimal MetaMtbfTkl { get; set; }

    }
}
