using BusinessLogic;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Tpm_CheckListProgramados.Properties;

namespace Tpm_CheckListProgramados
{
    class Program
    {
        //Variables de configuracion para conectarse a SQL
        public static string SrvSql = Settings.Default.ServerSql;
        public static string BD = Settings.Default.BDSql;
        public static string UserSql = Settings.Default.UserSql;
        public static string PwdSql = Settings.Default.PwdSql;
        public static string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;
        public static string pathLog = @Settings.Default.RutaLog;
        public static string pDepto = Settings.Default.Depto;
        public static int pPlanta = Settings.Default.Planta;
        public static string pCtroCostosSap = Settings.Default.CtroCostosSap;

        static void Main(string[] args)
        {
            TextWriter tw21 = new StreamWriter(pathLog, true);
            tw21.WriteLine(" CheckListProgramados - EjecutaProceso - Generacion de checklist programados ==> Inicio ==>" + ",  " + DateTime.Now.ToString());
            tw21.Close();
           
            // Metemos el For

            EjecutaProceso();
        }

        private static void EjecutaProceso()
        {
            ToolsTpm.Tools tool = new ToolsTpm.Tools();
            List<DataRow> lstTemp = new List<DataRow>();
            BL_ChkProgram bl_prog = new BL_ChkProgram();
            try
            {
                TextWriter tw21 = new StreamWriter(pathLog, true);
                tw21.WriteLine(" CheckListProgramados - EjecutaProceso - Generacion de checklist programados ==> Inicio ==>" + ",  " + DateTime.Now.ToString());
                tw21.Close();

                List<CheckListEqEnc> lstCheck = new List<CheckListEqEnc>();
                bl_prog.GetChkProgAll(cnxSqlMT, pathLog);

                System.IO.TextWriter tw2 = new StreamWriter(pathLog, true);
                tw2.WriteLine("CheckListProgramados - EjecutaProceso - Generacion de checklist programados -Terminacion  ==>  " + DateTime.Now.ToString());
                tw2.WriteLine("------------------------------------------------------------------------");
                tw2.Close();

            }
            catch (Exception ex)
            {
                string cMsj = "CheckListProgramados - EjecutaProceso - Error en la Generacion de checklist programados ==>  " + ex.Message + ", fuente: " + ex.Source + ", fecha: " + DateTime.Now.ToString();
                TextWriter tw23 = new StreamWriter(pathLog, true);
                tw23.WriteLine(cMsj);
                tw23.Close();
                tool.GuardarError(cnxSqlMT, ex.Message, ex.StackTrace, "EjecutaProceso", "CheckListProgramados");
            }

        }
    }

}



