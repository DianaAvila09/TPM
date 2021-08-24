using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using Atk_TpmMantenimiento.Properties;
using Entidades;

namespace Atk_TpmMantenimiento.Controllers
{
    public class TpmController : Controller
    {
        #region Variables para conexion a SQL
        static string SrvSql = Settings.Default.SrvSql;
        static string BD = Settings.Default.BD;
        static string UserSql = Settings.Default.UserSql;
        static string PwdSql = Settings.Default.PwdSql;
        static string cnxSqlMT = "Data Source=" + Settings.Default.SrvSql + ";Initial Catalog=" + Settings.Default.BD + ";User ID=" + Settings.Default.UserSql + ";Password=" + Settings.Default.PwdSql;
        #endregion

        BL_TPM blTpm = new BL_TPM();
        BL_Usuarios blUsu = new BL_Usuarios();

        public ActionResult DatosTpm(string cCostos)
        {
            List<EquipoTpm> lstEqTpm = new List<EquipoTpm>();
            DatosConfig config = new DatosConfig();

            config = blTpm.LeeConfig(cnxSqlMT, cCostos);
            config.MesesParaFallas = config.MesesParaFallas * (-1);

            BarraAvance paramBarra = new BarraAvance()
            {
                Verde = config.TopGreenAcumMtto,
                Amarilla = config.TopYellowAcumMtto,
                Roja = config.TopRedAcumMtto
            };

            string cnxSqlHT = "Data Source=" + config.SvrSqlTpm + ";Initial Catalog=" + config.BdHtProd + ";User ID=" + config.UserHtProd + ";Password=" + config.PwdHtProd;


            lstEqTpm = blTpm.DatosTpm(cnxSqlMT, config.Depto, config.CtroCtosSap, config.RutaLog);
            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "PM: Mantenimiento de Equipos Productivos";

            ViewBag.Login = (Session["UserId"] == null || Convert.ToInt16(Session["UserId"].ToString()) == 0 ? false : true);
            return View(lstEqTpm);
        }




        /// <summary>
        /// Genera informacion del tpm con todos sus equipos asignados
        /// </summary>
        /// <returns></returns>
        public ActionResult DatosTpmBasico(string cCostos, string tipoTick = "")
        {
            cCostos = "";
            if (Session["costos"] != null)
            {
                if (Session["costos"].ToString() != "")
                    cCostos = Session["costos"].ToString();
            }
            

            //if (cCostos == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            if (cCostos == null)
                cCostos = "";
            Session["costos"] = cCostos;
            if (Session["UserId"] == null)
            {
                Session["UserId"] = "";
                Session["UserName"] = "";
                Session["CatTpm"] = false;
                Session["EditarTicket"] = false;
                Session["CatChecklist"] = false;
                Session["IdUsuario"] = "0";
            }
            ViewBag.Result = false;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = blTpm.LeeConfig(cnxSqlMT, cCostos);

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

            // Obtenemos datos del TPM
            List<EquipoTpmBasico> lstEqTpm = new List<EquipoTpmBasico>();


            lstEqTpm = blTpm.DatosTpmBasico(cnxSqlMT, cnxSqlProd, config.Depto, config.Planta, cCostos, paramBarra,
                config.MesesParaFallas, config.RutaLog, config.HrsxDia, config.DiasxAno, config.DiasxMes, config.PrctjParaFallas, cnxSqlHT);



            ViewBag.rojos = lstEqTpm.Count(x => x.TipoFalla == "R").ToString();
            ViewBag.amarillo = lstEqTpm.Count(x => x.TipoFalla == "A").ToString();
            ViewBag.naranja = lstEqTpm.Count(x => x.TipoFalla == "M").ToString();
            ViewBag.azul = lstEqTpm.Count(x => x.TipoFalla == "Z").ToString();
            ViewBag.verde = lstEqTpm.Count(x => x.TipoFalla == "G").ToString();
            ViewBag.azulmar = lstEqTpm.Count(x => x.TipoFalla == "P").ToString();
            ViewBag.sintick = lstEqTpm.Count(x => x.TipoFalla == "X").ToString();


            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "PM: Mantenimiento de Equipos Productivos";
            string valor = Session["UserId"] == null ? "" : Session["UserId"].ToString();

            List<EquipoTpmBasico> lstEqTpmF = (from lstEq in lstEqTpm orderby lstEq.Orden ascending, lstEq.NumFallas descending select lstEq).ToList();

            if (cCostos == "")
                lstEqTpmF = lstEqTpmF.Take(200).ToList();

            return View(lstEqTpmF);
        }
    }

}