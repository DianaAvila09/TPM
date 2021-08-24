using SAP.Middleware.Connector;

namespace GetDatosSap
{
   public class CnxToSap : IDestinationConfiguration
   {
      // Datos de conexion a SAP por medio de RFC
      public string Host { get; set; }
      public string SystemID { get; set; }
      public string SystemNumber { get; set; }
      public string Client { get; set; }
      public string Language { get; set; }
      public string PoolSize { get; set; }
      public string UserPRD { get; set; }
      public string PwdPRD { get; set; }


      public bool ChangeEventsSupported()
      {
         return true;
      }

      public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

      /// <summary>
      /// Definicion del Parametro para la conexion
      /// </summary>
      /// <param name="destinationName"></param>
      /// <returns></returns>
      public RfcConfigParameters GetParameters(string destinationName)
      {
         RfcConfigParameters parms = new RfcConfigParameters();
         if ("Tpm".Equals(destinationName))
         {
            parms.Add(RfcConfigParameters.AppServerHost, Host);
            parms.Add(RfcConfigParameters.SystemNumber, SystemNumber);
            parms.Add(RfcConfigParameters.SystemID, SystemID);
            parms.Add(RfcConfigParameters.User, UserPRD);
            parms.Add(RfcConfigParameters.Password, PwdPRD);
            parms.Add(RfcConfigParameters.Client, Client);
            parms.Add(RfcConfigParameters.Language, Language);
            parms.Add(RfcConfigParameters.PoolSize, PoolSize);
            //parms.Add(RfcConfigParameters.MaxPoolSize, "10");
            //parms.Add(RfcConfigParameters.IdleTimeout, "600");
         }
         return parms;
      }

   }
}

