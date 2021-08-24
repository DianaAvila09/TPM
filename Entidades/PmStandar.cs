using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Entidades
{
   public class PmStandar
   {
      public int Id { get; set; }

      [Required(ErrorMessage = "Dato Obligatorio, seleccione un valor")]
      public string WorkCenter { get; set; }

      [Required(ErrorMessage = "Dato Obligatorio, seleccione un valor")]
      public string CodEquipo { get; set; }

      [Required(ErrorMessage = "Dato Obligatorio, seleccione un valor")]
      public string CodSistemas { get; set; }
      [Required(ErrorMessage = "Dato Obligatorio, seleccione un equipo")]
      public string CodCiclo { get; set; }

      [Required(ErrorMessage = "Dato Obligatorio, seleccione un equipo")]


      [Range(typeof(decimal), "0.001", "1000")]

      public decimal Ppm { get; set; }
      public DateTime FecAlta { get; set; }
      public DateTime FecModif { get; set; }
      public string UsuarioAlta { get; set; }

      public bool Estatus { get; set; }
      public string CentroCostos { get; set; }
      public decimal PMStandar { get; set; }
      public int Frecuencia { get; set; }
   }
}
