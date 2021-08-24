namespace Entidades
{
   public class KpiTpmCumpl
   {
      public string Periodo { get; set; }
      public int anio { get; set; }
      public int Mes { get; set; }      
      public int NumEquipos { get; set; }
      public int NoRealizados { get; set; }
      public decimal MetaCumpl { get; set; }
      public decimal Cumplimiento { get; set; }
      public decimal Trend { get; set; }
   }
}
