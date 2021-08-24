using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
   public class PlanMantto
   {
      public int Id { get; set; }

    
      [Required(ErrorMessage = "Dato Obligatorio, seleccione un equipo")]
      public string  CodEquipo { get; set;}

      public string  Equipo { get; set; }

    
      [Required(ErrorMessage = "Dato Obligatorio, seleccione un sistema")]
      public string  CodSistema { get; set;}
      public string  Sistema { get; set; }

      [Required(ErrorMessage = "Dato Obligatorio, selecione un ciclo")]
      public string   CodCiclo { get; set;}
      public string   Ciclo { get; set; }

      [Required(ErrorMessage = "Dato Obligatorio, capture una valor")]
      [Range(1, int.MaxValue)]
      public int Frecuencia { get; set;}

      [Required(ErrorMessage = "Dato Obligatorio, seleccione una fecha valida")]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
      public DateTime FechaAlta { get; set;}
      public string   UsuarioAlta { get; set;}

      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
      public DateTime FechaCancelacion { get; set;}
      public string   UsuarioCancelo { get; set;}
      public bool     Estatus { get; set;}

      [Required(ErrorMessage = "Dato Obligatorio, seleccione una fecha valida")]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
      public DateTime FecUltEjecucion { get; set; }

      public string CodWorkCenter{ get; set; }
   }
}
