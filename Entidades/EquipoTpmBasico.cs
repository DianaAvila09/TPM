using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class EquipoTpmBasico
    {
        public int Id { get; set; }

        [Display(Name = "Work Center")]
        public string CodWorkCenter { get; set; }

        [Display(Name = "Equipo")]
        public string CodEquipo { get; set; }

        [Display(Name = "Descripción")]
        public string DescripTechnical { get; set; }

        [Display(Name = "Status Actual")]
        public string IndivStatusObject { get; set; }

        [Display(Name = "Clasif. Técnica")]
        public string TypeTechObj { get; set; }

        [Display(Name = "Planeador")]
        public string PlannerGroup { get; set; }

        [Display(Name = "Centro de costo")]
        public string CostCenter { get; set; }

        [Display(Name = "Responsable")]
        public string MainWorkCenter { get; set; }

        [Display(Name = "Nombre del Responsable")]
        public string NombreMwc { get; set; }

        [Display(Name = "Ubicación")]
        public string FunctionalLocation { get; set; }
        public string ManufAsset { get; set; }
        public string ManufModelNum { get; set; }

        [Display(Name = "Valido desde")]
        public string ValidFromDate { get; set; }

        [Display(Name = "Ubicacion padre")]
        public string Superordinate { get; set; }
        public string StandardTextKeyWC { get; set; }

        [Display(Name = "% Avance")]
        public int PorcAvance { get; set; }

        public BarraAvance Barra { get; set; }

        [Display(Name = "avance del ciclo")]
        public decimal Avance { get; set; }
        [Display(Name = "Núm. de fallas")]
        public decimal NumFallas { get; set; }

        [Display(Name = "PM Standar")]
        public decimal PmStandar { get; set; }
        [Display(Name = "PM Real")]
        public decimal PmReal { get; set; }
        [Display(Name = "Ultima Falla")]
        public string UltFalla { get; set; }
        [Display(Name = "Tipo de Falla")]
        public string TipoFalla { get; set; }
        [Display(Name = "Item")]

        public string Codciclo { get; set; }
        public string Ciclo { get; set; }
        public int Frecuencia { get; set; }
        public decimal PzasProducidas { get; set; }
        public decimal PzasScrap { get; set; }
        public DateTime FecUltManto { get; set; }
        public int HallazgoSeguridad { get; set; }
        public string Sensor { get; set; }
        public int Planta { get; set; }
        public string CodDepartamento { get; set; }
        public string CodCentroCostos { get; set; }
        public DateTime FecCalculo { get; set; }
        public decimal Eficiencia { get; set; }
        public int Orden { get; set; }
    }
}
