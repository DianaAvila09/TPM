using System.ComponentModel.DataAnnotations;

namespace Entidades
{
   public class Departamento
   {
      public int Id { get; set; }
      public int IdPlanta { get; set; }
      public string PlantaSatelite { get; set; }
      public string CodDepartamento { get; set; }
      public string Descrip { get; set; }
      public string CentroCostos { get; set; }
      public bool Estatus { get; set; }
      public string CentroCostosSap { get; set; }
      public string KeyDescrip { get; set; }
   }
}
   