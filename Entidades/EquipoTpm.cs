using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
   public class EquipoTpm
   {
      [Display( Name = "Work Center")]
      public string WorkCenter { get; set; }
      public string ObjectNumber { get; set; }

      [Display(Name = "Equipo")]
      public string CodEquipo { get; set; }

      [Display(Name = "Descripción")]
      public string DescripTechnical { get; set; }

      [Display(Name = "Status Actual")]
      public string IndivStatusObject { get; set; }

      [Display(Name = "Tipo de Técnico")]
      public string TypeTechObj { get; set; }

      [Display(Name = "Planeador")]
      public string PlannerGroup { get; set; }

      [Display(Name = "Centro de costo")]
      public string CostCenter { get; set; }

      [Display(Name = "Responsable")]
      public string MainWorkCenter { get; set; }

      [Display(Name = "Responsable")]
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
      [Display(Name = "Codigo del sistema")]
      public string CodSistema { get; set; }
      [Display(Name = "Sistema de Mantto.")]
      public string Sistema { get; set; }
      [Display(Name = "Código del ciclo")]
      public string CodCiclo { get; set; }
      [Display(Name = "Ciclo ")]
      public string Ciclo { get; set; }
      [Display(Name = "Frecuencia")]
      public int Frecuencia { get; set; }
      [Display(Name = "Fec. ultima Ejecuc.")]
      public DateTime FecUltEjecucion { get; set; }
      [Display(Name = "% de avance del ciclo")]
      public decimal PorcAvance { get; set; }
      [Display(Name = "avance del ciclo")]
      public decimal Avance { get; set; }
      [Display(Name = "Núm. de fallas")]
      public decimal numfallas { get; set; }
      [Display(Name = "PM Standar")]
      public decimal PmStandar { get; set; }
      [Display(Name = "PM Real")]
      public decimal PmReal { get; set; }
      [Display(Name = "Ultima Falla")]
      public string UltFalla { get; set; }
      [Display(Name = "Tipo de Falla")]
      public string TipoFalla { get; set; }
      [Display(Name = "Item")]
      public int Id { get; set; }


   }
}
