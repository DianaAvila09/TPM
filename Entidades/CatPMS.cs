using System;

namespace Entidades
{
   public class CatPMS
   {
      public int Id { get; set; }
      public string WorkCenter { get; set; }
      public string CodEquipo { get; set; }
      public string DescripEquipo { get; set; }
      public string CodSistemas { get; set; }
      public string DescripSistema { get; set; }
      public string CodCiclo { get; set; }
      public string DescripCiclo { get; set; }
      public decimal Ppm { get; set; }
      public DateTime FecAlta { get; set; }
      public DateTime FecModif { get; set; }
      public string UsuarioAlta { get; set; }
      public bool Estatus { get; set; }
      public string CentroCostos { get; set; }
      public decimal PMStandar { get; set; }
      public string CodDepartamento { get; set; }
   }
}
