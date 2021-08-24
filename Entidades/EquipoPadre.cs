using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class EquipoPadre
    {
        [Display(Name = "Work Center")]
        public string CodWorkCenter { get; set; }

        [Display(Name = "Equipo")]
        public string CodEquipo { get; set; }

        [Display(Name = "Tipo de Técnico")]
        public string TypeTechObj { get; set; }

        [Display(Name = "Objetc Id")]
        public string ObjectNumber { get; set; }

        [Display(Name = "Descripción")]
        public string DescripTechnical { get; set; }

        [Display(Name = "Planta mantto.")]
        public string MaintPlanningPlant { get; set; }

        [Display(Name = "Ubicación")]
        public string FunctionalLocation { get; set; }

        [Display(Name = "Centro de costo")]
        public string CostCenter { get; set; }

        [Display(Name = "Responsable")]
        public string MainWorkCenter { get; set; }

        [Display(Name = "Ubicacion padre")]
        public string Superordinate { get; set; }
        public string Cod_Descrip { get; set; }
        public string ValidFromDate { get; set; }
        public string ManufAsset { get; set; }
        public string ManufModelNum { get; set; }
        public string PlannerGroup { get; set; }
        public string IndivStatusObject { get; set; }
        public string StandardTextKeyWC { get; set; }
        public string FocusFactory { get; set; }
    }
}
