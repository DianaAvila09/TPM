using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpm_CalcHistorico.Properties;
using BusinessLogic;
using Entidades;
using System.Data;
using System.IO;
using RepositorySql;
using ToolsTpm;

namespace Tpm_CalcHistorico
{
   class Tpm_MantoCalcHistorico
   {
      #region Variables para conexion a SQL
      static string SrvSql = Settings.Default.SrvSql;
      static string BD = Settings.Default.BD;
      static string UserSql = Settings.Default.UserSql;
      static string PwdSql = Settings.Default.PwdSql;
      static string cnxSqlMT = "Data Source=" + Settings.Default.SrvSql + ";Initial Catalog=" + Settings.Default.BD + ";User ID=" + Settings.Default.UserSql + ";Password=" + Settings.Default.PwdSql;

      static string fileLog = @Settings.Default.RutaLog;
      static string fileBug = @Settings.Default.RutaLogError;

      static int pPlanta = Settings.Default.Planta;
      static string pDepto = Settings.Default.Departamento;
      static string pCtroCostos = Settings.Default.CtroCtos;

      #endregion

      static BL_TPM blTpm = new BL_TPM();

      static void Main(string[] args)
      {
         int result = 0;        
         List<EquipoTpmBasico> lstEqTpm = new List<EquipoTpmBasico>();

         TextWriter tw21 = new StreamWriter(fileLog, true);
         tw21.WriteLine("BLDatosSap-UpdateCatEquipos - EjecutaProceso -Inicio de la actualizacion de datos, ==>  " + ",  " + DateTime.Now.ToString());
         tw21.Close();

         // 1.- Obtenemos datos del TPM
         lstEqTpm = DatosTpmBasico(cnxSqlMT, pPlanta, pDepto, pCtroCostos);

        

         // 2.- Guardar Historico en BD.. este se utilizara para el calculo del KPI para visualizarlos en cualquier momento
         // Pensando en un scorecard ya se tendria la informacion lista para calcular      
         result = GuardarHistorico(cnxSqlMT, pPlanta, pDepto, pCtroCostos, lstEqTpm,  fileLog, fileBug);

         TextWriter tw2 = new StreamWriter(fileLog, true);
         tw2.WriteLine("BLDatosSap-UpdateCatEquipos - EjecutaProceso - Terminacion del proceso ==>  " + DateTime.Now.ToString());
         tw2.WriteLine("------------------------------------------------------------------------");
         tw2.Close();
      }

      private static int GuardarHistorico(string cnxSqlMT, int pPlanta, string pDepto, string pCtroCostos, List<EquipoTpmBasico> lstEqTpm, string fileLog, string fileBug)
      {
        
         int result = -1;
         try
         {
           

            SqlRepository repoSql = new SqlRepository();

            result = repoSql.GuardarTpmHisto(cnxSqlMT, pPlanta, pDepto, pCtroCostos, lstEqTpm,  fileLog, fileBug);

            
         }

         catch (Exception ex)
         {
            string cMsj = "BLDatosSap-UpdateCatEquipos - EjecutaProceso - Error en la actualizacion ==>  " + ex.Message + ", fuente: " + ex.Source + ", fecha: " + DateTime.Now.ToString();
            TextWriter tw23 = new StreamWriter(fileBug, true);
            tw23.WriteLine(cMsj);
            tw23.Close();
            Tools tool = new Tools();
            tool.GuardarError(cnxSqlMT, ex.Message, ex.StackTrace, "EjecutaProceso", "BLDatosSap-UpdateCatWC");
         }

         return result;
      }

      public static List<EquipoTpmBasico> DatosTpmBasico(string cnxSqlMT, int  pPlanta, string pDepto, string pCtroCostos)
      {

         // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
         DatosConfig config = new DatosConfig();
         config = blTpm.LeeConfig(cnxSqlMT, pCtroCostos);

         // negativo para que se reste a la fecha de hoy y de la fecha de  x meses hacia a tras
         config.MesesParaFallas = config.MesesParaFallas * (-1);

         // Barrar de la grafica        
         BarraAvance paramBarra = new BarraAvance()
         {
            Verde = config.TopGreenAcumMtto,
            Amarilla = config.TopYellowAcumMtto,
            Roja = config.TopRedAcumMtto
         };

         // Cadena de conexciona la BD del Historico de produccion
         string cnxSqlHT = "Data Source=" + config.SvrSqlTpm.Trim() + ";Initial Catalog=" + config.BdHtProd.Trim() + ";User ID=" + config.UserHtProd.Trim() + ";Password=" + config.PwdHtProd.Trim();
         string cnxSqlRefec = "Data Source=" + config.SrvSqlEmpl.Trim() + ";Initial Catalog=" + config.BdEmpl.Trim() + "; User ID=" + config.UserEmpl.Trim() + "; Password=" + config.PwdEmpl.Trim();
         string cnxSqlProd = "Data Source=" + config.SrvSqlProd.Trim() + ";Initial Catalog=" + config.BdProd.Trim() + "; User ID=" + config.UserProd.Trim() + "; Password=" + config.PwdProd.Trim();

       
         List<EquipoTpmBasico> lstEqTpm = new List<EquipoTpmBasico>();
         lstEqTpm = blTpm.DatosTpmBasico(cnxSqlMT, cnxSqlProd, config.Depto, config.Planta, config.CtroCtosSap, paramBarra,
            config.MesesParaFallas, config.RutaLog, config.HrsxDia, config.DiasxAno, config.DiasxMes, config.PrctjParaFallas, cnxSqlHT);

         return(lstEqTpm);

      }

   }
}
