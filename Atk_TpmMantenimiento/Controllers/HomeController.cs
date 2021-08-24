using BusinessLogic;
using Entidades;
using System.Web.Mvc;
using Atk_TpmMantenimiento.Properties;
using System;

namespace Atk_TpmMantenimiento.Controllers
{
    public class HomeController : Controller
    {

        static string ctroCostos = "";
        static string cnxSqlMT = "Data Source=" + Settings.Default.SrvSql + ";Initial Catalog=" + Settings.Default.BD + ";User ID=" + Settings.Default.UserSql + ";Password=" + Settings.Default.PwdSql;
        BL_Usuarios blUsu = new BL_Usuarios();
        public ActionResult Index()
        {
            ctroCostos = Session["costos"].ToString();
            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            BusinessLogic.BL_TPM tpm = new BL_TPM();
            config = tpm.LeeConfig(cnxSqlMT, ctroCostos);

            ctroCostos = config.CtroCtosSap;
            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), ctroCostos);
            #endregion

            return View();
        }

        public ActionResult Contact()
        {
            ctroCostos = Session["costos"].ToString();
            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            BusinessLogic.BL_TPM tpm = new BL_TPM();
            config = tpm.LeeConfig(cnxSqlMT, ctroCostos);

            ctroCostos = config.CtroCtosSap;
            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), ctroCostos);
            #endregion

            return View();
        }

        public ActionResult About()
        {
            ctroCostos = Session["costos"].ToString();
            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            BusinessLogic.BL_TPM tpm = new BL_TPM();
            config = tpm.LeeConfig(cnxSqlMT, ctroCostos);

            ctroCostos = config.CtroCtosSap;
            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), ctroCostos);
            #endregion

            return View();
        }
    }
}