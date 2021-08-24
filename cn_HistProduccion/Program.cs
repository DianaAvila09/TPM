using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cn_HistProduccion.Properties;
using SAP.Middleware.Connector;

namespace cn_HistProduccion
{
    internal class Program
    {
        // Variables para SQL
        private static string cSrvSql = Settings.Default.SrvSql;
        private static string cBD = Settings.Default.BD;
        private static string cUserSql = Settings.Default.UserSql;
        private static string cPwdSql = Settings.Default.PwdSql;

        public string MensajeError = "";

        public static string RutaLog = "";
        public static string pathe;
        public static string path;
        public static string cMsj = "";

        public static string CnxSql = "Data Source=" + cSrvSql + ";Initial Catalog=" + cBD + ";User ID=" + cUserSql + ";Password=" + cPwdSql;


        private static void Main(string[] args)
        {
            DateTime dFI = Convert.ToDateTime("01/01/2019");
            DateTime dFF = Convert.ToDateTime("03/26/2019");

            RutaLog = MiRutalog();
            path = @RutaLog + "logHTProd.txt";
            pathe = @RutaLog + "logErrorHTProd.txt";
            try
            {
                using (SqlConnection cnx = new SqlConnection(CnxSql))
                {
                    //Abrimos la conexión
                    cnx.Open();
                    Lee_Notificado(cnx, dFI, dFF);



                }
            }
            catch (SqlException ex)
            {
                cMsj = "cn_HistProduccion - Falla para conectarse a SQL: " + ex.Message + " , " + ex.Source + " , " + ex.HResult + " , " + ex.ToString();
                EnviarCorreo email = new EnviarCorreo();
                email.Enviar(cMsj);
            }
        }

        public static void Lee_Notificado(SqlConnection cnx, DateTime dFI, DateTime dFF)
        {

            CnxToSap cfgSap = null;
            try
            {
                RfcDestination rfcDest = RfcDestinationManager.TryGetDestination("Htp_001");

                if (rfcDest == null)
                {
                    // Conceccion con SAP

                    cfgSap = new CnxToSap();
                    RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
                }

                rfcDest = RfcDestinationManager.GetDestination("Htp_001");
                // Creamos la clase del RFC
                RfcSap htProduc = new RfcSap();

                // Invocamos el RFC para obtener informacion
                htProduc.LeeMatNotificado(rfcDest, cnx, dFI, dFF);

                SqlCommand cmdUpdate = new SqlCommand("spu_UpdHTProd", cnx);
                cmdUpdate.CommandType = CommandType.StoredProcedure;

                if (cmdUpdate.ExecuteNonQuery() > 0)
                {
                    TextWriter twsz = new StreamWriter(Program.pathe, true);
                    twsz.WriteLine("Error en la ejecucion del Store procedure spu_UpdHtProd ==>  " + DateTime.Now.ToString());
                    twsz.Close();
                }
            }

            catch (Exception ex)
            {

                cMsj = "cn_HistProduccion FALLA: Lee_Notificado ERROR AL EJECUTAR RFC, " + ex.Message;
                TextWriter twe = new StreamWriter(pathe, true);
                twe.WriteLine(cMsj);
                twe.Close();
                EnviarCorreo email = new EnviarCorreo();
                email.Enviar(cMsj);
            }
            // Cierro conexion con SAP
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
        }


        private static string MiRutalog()
        {
            using (SqlConnection cnx = new SqlConnection(CnxSql))
            {
                #region Obtiene la ruta del archivo log
                try
                {
                    string cmdSql = "Select RutaHTlog from Config";
                    SqlCommand cmd = new SqlCommand(cmdSql, cnx);
                    cnx.Open();
                    string ruta = (string)cmd.ExecuteScalar();
                    cnx.Close();
                    return ruta;
                }
                catch (SqlException e)
                {
                    return String.Empty;
                }
                #endregion
            }
        }
    }
}