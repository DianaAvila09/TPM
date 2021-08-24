﻿using System;
using System.Globalization;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using atk_wsCatEquipos.Properties;
using ToolsTpm;
using Entidades;
using BusinessLogic;
using System.Collections.Generic;
using RepositorySql;
using System.Data;

namespace atk_wsCatEquipos
{
   public partial class UpdCatEquipos : ServiceBase
   {
      //Variables de configuracion para conectarse a SQL
      public static string SrvSql = Settings.Default.ServerSql;
      public static string BD = Settings.Default.BDSql;
      public static string UserSql = Settings.Default.UserSql;
      public static string PwdSql = Settings.Default.PwdSql;
      public static string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

      // Programamos la hora de inicio del servicio sera a las 8:00 am porque a las 7:00 am se ejecuta el Job en SAP
      public static int minEjecucion = Settings.Default.MinEjec;
      public static string pathLog = @Settings.Default.RutaLog;

      Timer tmServicio = null;
      Tools tool = new Tools();
      DatosCorreo correo = new DatosCorreo();
      public UpdCatEquipos()
      {
         InitializeComponent();
 
         tmServicio = new Timer();

         // Tomamos los minutos actuales de la hora que arranque del servicio
         int minute = DateTime.Now.Minute;
         int multiplo = Convert.ToInt16(minute / minEjecucion);
         EjecutaProceso();
         tmServicio.Interval = (((multiplo + 1) * minEjecucion) - minute) * (60 * 1000);
         tmServicio.Elapsed += new ElapsedEventHandler(tmServicio_Elapsed);
         tmServicio.Enabled = true;
      }

      public void tmServicio_Elapsed(object sender, ElapsedEventArgs e)
      {
         Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
         EjecutaProceso();
      }

      private void EjecutaProceso()
      {
         List<Equipo> lstEquipos = new List<Equipo>();
         List<DataRow> lstTemp = new List<DataRow>();
         try
         {
            TextWriter tw21 = new StreamWriter(pathLog, true);
            tw21.WriteLine(" atk_wsCatEquipos - EjecutaProceso -Inicio de la actualizacion de datos, intervalo ==>  " + ",  " + DateTime.Now.ToString());
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

            // Obtiene los equipos de sap
           lstEquipos = datosSap.DatosCatEquipos(cnxSap, cnxSqlMT);

            SqlRepository repoSql = new SqlRepository();

           int resul = repoSql.GuardarCatEquiposSql(cnxSqlMT, lstEquipos, "CatEquipos", pathLog);

            tmServicio.Interval = 1000 * 60 * minEjecucion;
            tmServicio.Start();

            TextWriter tw2 = new StreamWriter(pathLog, true);
            tw2.WriteLine("atk_wsCatEquipos - EjecutaProceso - Terminacion del proceso ==>  " + DateTime.Now.ToString());
            tw2.WriteLine("------------------------------------------------------------------------");
            tw2.Close();

         }
         catch (Exception ex)
         {
            string cMsj = "atk_wsCatEquipos - EjecutaProceso - Error en la actualizacion ==>  " + ex.Message + ", fuente: " + ex.Source + ", fecha: " + DateTime.Now.ToString();
            TextWriter tw23 = new StreamWriter(pathLog, true);
            tw23.WriteLine(cMsj);
            tw23.Close();
            tool.GuardarError(cnxSqlMT, ex.Message, ex.StackTrace, "EjecutaProceso", "UpdCatEquipos");
         }

      }
      protected override void OnStart(string[] args)
      {
         string cMsj = " atk_wsCatEquipos - Inicio del servicio ==>  " + DateTime.Now.ToString();
         TextWriter tw = new StreamWriter(pathLog, true);
         tw.WriteLine("------------------------------------------------------------------------");
         tw.WriteLine(cMsj);
         tw.Close();
         correo.Mensaje = cMsj;
         correo.To = new List<string> { Settings.Default.To }; ;
         correo.Subject = Settings.Default.Subject + " Inicio del servicio ";
         tool.EnviarCorreo(correo);

      }

      protected override void OnStop()
      {
         string cMsj = "atk_wsCatEquipos - Paro de servicio ==>  " + DateTime.Now.ToString();
         TextWriter tw = new StreamWriter(pathLog, true);
         tw.WriteLine("------------------------------------------------------------------------");
         tw.WriteLine(cMsj);
         tw.Close();
         correo.Mensaje = cMsj;
         correo.To = new List<string> { Settings.Default.To }; ;
         correo.Subject = Settings.Default.Subject + " Inicio del servicio ";
         tool.EnviarCorreo(correo);

      }
   }
}
