using Atk_wsCatWCenter.Properties;
using BusinessLogic;
using Entidades;
using RepositorySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using ToolsTpm;
using Timer = System.Timers.Timer;

namespace Atk_wsCatWCenter
{
   public partial class UpdCatWCenter : ServiceBase
   {

      //Variables de configuracion para conectarse a SQL
      public static string SrvSql = Settings.Default.ServerSql;
      public static string BD = Settings.Default.BDSql;
      public static string UserSql = Settings.Default.UserSql;
      public static string PwdSql = Settings.Default.PwdSql;
      public static string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

      // Programamos la hora de inicio del servicio sera a las 8:00 am porque a las 7:00 am se ejecuta el Job en SAP
      public static int minEjecucion = Settings.Default.MinEjec;
      public static string pathLog = Settings.Default.RutaLog;

      Timer tmServicio = null;
      Tools tool = new Tools();
      DatosCorreo correo = new DatosCorreo();


      public UpdCatWCenter()
      {
         InitializeComponent();

         pathLog = @pathLog + "LogCatWC.txt";

         tmServicio = new Timer();

         // Tomamos los minutos actuales de la hora que arranque del servicio
         int minute = DateTime.Now.Minute;
         int multiplo = Convert.ToInt16(minute / minEjecucion);
         EjecutaProceso();
         tmServicio.Interval = (((multiplo + 1) * minEjecucion) - minute) * (60 * 1000);
         tmServicio.Elapsed += new ElapsedEventHandler(tmServicio_Elapsed);
         tmServicio.Enabled = true;
      }

      private void tmServicio_Elapsed(object sender, ElapsedEventArgs e)
      {

         Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
         EjecutaProceso();
      }

      private void EjecutaProceso()
      {
         List<WorkCenter> lstWc = new List<WorkCenter>();
         List<DataRow> lstTemp = new List<DataRow>();
         try
         {
            TextWriter tw21 = new StreamWriter(pathLog, true);
            tw21.WriteLine(" Atk_wsCatWCenter - EjecutaProceso -Inicio de la actualizacion de datos, ==>  " + ",  " + DateTime.Now.ToString());
            tw21.Close();

            BLDatosSap datosSap = new BLDatosSap();

            DatosCnxSap cnxSap = new DatosCnxSap()
            {
               Host = Settings.Default.SapHost,
               SystemID = Settings.Default.SapSystemID,
               SystemNumber = Settings.Default.SApSystemNumber,
               Client = Settings.Default.SapClient,
               Language = Settings.Default.SapLanguage,
               PoolSize = Settings.Default.SapPoolSize,
               UserPRD = Settings.Default.SapUserPRD,
               PwdPRD = Settings.Default.SapPwdPRD

            };

            //// Obtiene los datos de sap
            lstWc = datosSap.DatosWC(cnxSap, cnxSqlMT);

            SqlRepository repoSql = new SqlRepository();

            int resul = repoSql.GuardarCatWCSql(cnxSqlMT, lstWc, "CatWorkCenter", pathLog);

            tmServicio.Interval = 1000 * 60 * minEjecucion;
            tmServicio.Start();


            TextWriter tw2 = new StreamWriter(pathLog, true);
            tw2.WriteLine("Atk_wsCatWCenter - EjecutaProceso - Terminacion del proceso ==>  " + DateTime.Now.ToString());
            tw2.WriteLine("------------------------------------------------------------------------");
            tw2.Close();

         }
         catch (Exception ex)
         {
            string cMsj = "Atk_wsCatWCenter - EjecutaProceso - Error en la actualizacion ==>  " + ex.Message + ", fuente: " + ex.Source + ", fecha: " + DateTime.Now.ToString();
            TextWriter tw23 = new StreamWriter(pathLog, true);
            tw23.WriteLine(cMsj);
            tw23.Close();
            tool.GuardarError(cnxSqlMT, ex.Message, ex.StackTrace, "EjecutaProceso", "UpdCatWCenter");
         }


      }
      protected override void OnStart(string[] args)
      {
         string cMsj = " Atk_wsCatWCenter - INICIO del servicio ==>  " + DateTime.Now.ToString();
         TextWriter tw = new StreamWriter(pathLog, true);
         tw.WriteLine("------------------------------------------------------------------------");
         tw.WriteLine(cMsj);
         tw.Close();
         correo.Mensaje = cMsj;
         correo.To = new List<string> { Settings.Default.To };
         correo.Subject = Settings.Default.Subject + " INICIO del servicio ";
         tool.EnviarCorreo(correo);

      }

      protected override void OnStop()
      {
         string cMsj = "Atk_wsCatWCenter - PARO de servicio ==>  " + DateTime.Now.ToString();
         TextWriter tw = new StreamWriter(pathLog, true);
         tw.WriteLine("------------------------------------------------------------------------");
         tw.WriteLine(cMsj);
         tw.Close();
         correo.Mensaje = cMsj;
         correo.To = new List<string> { Settings.Default.To};
         correo.Subject = Settings.Default.Subject + " PARO del servicio ";
         tool.EnviarCorreo(correo);

      }
   }
}
