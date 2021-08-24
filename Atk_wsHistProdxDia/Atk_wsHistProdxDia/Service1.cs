using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using SAP.Middleware.Connector;
using Atk_wsHistProdxDia.Properties;

namespace Atk_wsHistProdxDia
{
   public partial class Service1 : ServiceBase
   {
      #region Definicion de variables
      // Variable para el tiempo de ejecucion
      public Timer TmServicio = null;

      // Variables para SQL
      private static string cSrvSql = Settings.Default.SrvSql;
      private static string cBD = Settings.Default.BD;
      private static string cUserSql = Settings.Default.UserSql;
      private static string cPwdSql = Settings.Default.PwdSql;
      public string MensajeError = "";
      public int minEjecucion = 0;
      public string RutaLog = "";
      public static string pathe;
      public static string path;
      public static string cMsj = "";

      // Conceccion para SQL
      public static string CnxSql = "Data Source=" + cSrvSql + ";Initial Catalog=" + cBD + ";User ID=" + cUserSql + ";Password=" + cPwdSql;

      #endregion

      public Service1()
      {
         RutaLog = @Settings.Default.RutaLog;
         path = @RutaLog + "LogHtProdxDia.txt";
         pathe = @RutaLog + "LogErrorHtProdxDia.txt";

         minEjecucion = TiempodeEjecucion();

         InitializeComponent();
         TmServicio = new Timer();
         // tiempo en min para su ejecucion
         minEjecucion = TiempodeEjecucion();

         // Tomamos los minutos actuales de la hora que arranque el proyecto
         int minute = DateTime.Now.Minute;
         int multiplo = Convert.ToInt16(minute / minEjecucion);
         TmServicio.Interval = (((multiplo + 1) * minEjecucion) - minute) * (60 * 1000);
         TmServicio.Elapsed += new ElapsedEventHandler(tmServicio_Elapsed);
         TmServicio.Enabled = true;
      }

      /// <summary>
      /// Metodo que ejecuta el programa del serivicio cada x tiempo
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      public void tmServicio_Elapsed(object sender, ElapsedEventArgs e)
      {
         try
         {
            using (SqlConnection cnx = new SqlConnection(CnxSql))
            {
               //Abrimos la conexión
               cnx.Open();

               #region Llamamos al Metodo que lee la informacion

               try
               {
                  TextWriter tw = new StreamWriter(path, true);
                  tw.WriteLine("ws_HistProdxDia -- Lee_Notificado -- Iniciando --> fecha de : " + DateTime.Now.ToString());
                  tw.Close();

                  DateTime dFI = (DateTime.Now.Date.AddDays(-1));
                  DateTime dFF = DateTime.Now.Date;

                  Lee_Notificado(cnx, dFI, dFF);

                  TextWriter tw01 = new StreamWriter(path, true);
                  tw01.WriteLine("ws_HistProdxDia -- Lee_Notificado -- Terminado --> fecha de : " + DateTime.Now.ToString());
                  tw01.Close();
               }
               catch (SqlException ex)
               {
                  cMsj = "ws_HistProdxDia FALLA:  Lee_Notificado   " + ex.Message;
                  TextWriter twe = new StreamWriter(pathe, true);
                  twe.WriteLine(cMsj);
                  twe.Close();

                  EnviarCorreo email = new EnviarCorreo();
                  email.Enviar(cMsj);
               }

               #endregion
            }
         }

         catch (Exception ex)
         {
            cMsj = "ws_HistProdxDia - Falla:  general: " + ex.Message + " , " + ex.Source + " , " + ex.HResult + " , " + ex.ToString();
            TextWriter twe = new StreamWriter(pathe, true);
            twe.WriteLine(cMsj);
            twe.Close();

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
               TextWriter twsz = new StreamWriter(pathe, true);
               twsz.WriteLine("ws_HistProdxDia, Error en la ejecucion del Store procedure spu_UpdHtProd ==>  " + DateTime.Now.ToString());
               twsz.Close();
            }
         }

         catch (Exception ex)
         {

            cMsj = "ws_HistProdxDia FALLA: Lee_Notificado ERROR AL EJECUTAR RFC, " + ex.Message;
            TextWriter twe = new StreamWriter(pathe, true);
            twe.WriteLine(cMsj);
            twe.Close();
            EnviarCorreo email = new EnviarCorreo();
            email.Enviar(cMsj);
         }
         // Cierro conexion con SAP
         RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
      }



      protected override void OnStart(string[] args)
      {
         EnviarCorreo email = new EnviarCorreo();
         email.Enviar("ws_HistProdxDia, EMPEZO el servicio de Confirmacion de archivos");
      }

      protected override void OnStop()
      {
         EnviarCorreo email = new EnviarCorreo();
         email.Enviar("ws_HistProdxDia, EMPEZO el servicio de Confirmacion de archivos");
      }

      /// <summary>
      /// Obtiene la ruta de los archivos logs
      /// </summary>
      /// <returns></returns>
      private static string MiRutalog()
      {
         using (SqlConnection cnx = new SqlConnection(CnxSql))
         {
            #region Obtiene la ruta del archivo log
            try
            {
               string cmdSql = "Select LogHTProdxdia from Config";
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

      /// <summary>
      /// Obtiene el tiempo de ejecucion en minutos
      /// </summary>
      /// <returns></returns>
      private static int TiempodeEjecucion()
      {
         using (SqlConnection cnx = new SqlConnection(CnxSql))
         {
            #region Obtiene el tiempo de ejecucion del servicio
            try
            {
               string cmdSql = "Select Service_updHtProdxDia from Config";
               SqlCommand cmd = new SqlCommand(cmdSql, cnx);
               cnx.Open();
               int te = (int)cmd.ExecuteScalar();
               cnx.Close();
               return te;
            }
            catch (SqlException e)
            {
               return 0;
            }
            #endregion
         }
      }
   }
}
