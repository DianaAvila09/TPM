
using SAP.Middleware.Connector;
using Atk_wsHistProdxDia.Properties;


namespace Atk_wsHistProdxDia
{
    class CnxToSap : IDestinationConfiguration
    {
        // Datos de conexion a SAP por medio de RFC
        private string cHost = Settings.Default.Host;
        private string cSystemID = Settings.Default.SystemID;

        private string cSystemNumber = Settings.Default.SystemNumber;
        private string cClient = Settings.Default.Client;
        private string cLanguage = Settings.Default.Language;
        private string cPoolSize = Settings.Default.PoolSize;

        public string cUserPRD = Settings.Default.UserPRD;
        public string cPwdPRD = Settings.Default.PwdPRD;

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
            if ("Htp_001".Equals(destinationName))
            {
                parms.Add(RfcConfigParameters.AppServerHost, cHost);
                parms.Add(RfcConfigParameters.SystemNumber, cSystemNumber);
                parms.Add(RfcConfigParameters.SystemID, cSystemID);
                parms.Add(RfcConfigParameters.User, cUserPRD);
                parms.Add(RfcConfigParameters.Password, cPwdPRD);
                parms.Add(RfcConfigParameters.Client, cClient);
                parms.Add(RfcConfigParameters.Language, cLanguage);
                parms.Add(RfcConfigParameters.PoolSize, cPoolSize);
            }
            return parms;
        }
    }
}