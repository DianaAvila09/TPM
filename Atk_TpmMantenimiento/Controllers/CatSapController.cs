using BusinessLogic;
using Entidades;
using System.Web.Mvc;
using Atk_TpmMantenimiento.Properties;
using System.Web.Routing;
using System;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CatSapController : Controller
    {

        #region variables de conexion
        static string SrvSql = Settings.Default.SrvSql;
        static string BD = Settings.Default.BD;
        static string UserSql = Settings.Default.UserSql;
        static string PwdSql = Settings.Default.PwdSql;
        static string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;
        static string cCostos = null;
        static string rutalog = "";
        #endregion

        BL_CatalogosSap Bl_catSap = new BL_CatalogosSap();
        BL_TPM tpm = new BL_TPM();
        BL_Usuarios blUsu = new BL_Usuarios();

        public ActionResult CatEquipos()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            rutalog = config.RutaLog;

            ViewBag.Title = config.TituArea;

            ViewBag.TitleCC = " [ Todos ] ";
            if (cCostos != "")
                ViewBag.TitleCC = config.TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(Bl_catSap.GetDatosEquiposSap(cnxSqlMT, cCostos, rutalog));
        }

        public ActionResult CatWorkCenter()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = " [ Todos ] ";
            if (cCostos != "")
                ViewBag.TitleCC = config.TituCC;
            ViewBag.TitleCA = config.TituCA;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(Bl_catSap.GetDatosWC(cnxSqlMT));
        }

        public ActionResult CatFunctionalLocation()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = " [ Todos ] ";
            if (cCostos != "")
                ViewBag.TitleCC = config.TituCC;
            ViewBag.TitleCA = config.TituCA;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(Bl_catSap.GetDatosFL(cnxSqlMT, cCostos));
        }

        public ActionResult EstructuraEquipos()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = " [ Todos ] ";
            if (cCostos != "")
                ViewBag.TitleCC = config.TituCC;
            ViewBag.TitleCA = config.TituCA;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(Bl_catSap.GetDatosEEqui(cnxSqlMT, cCostos, rutalog));
        }

        [HttpGet]
        public ActionResult UpdateCatSap()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = " [ Todos ] ";
            if (cCostos != "")
                ViewBag.TitleCC = config.TituCC;
            ViewBag.TitleCA = config.TituCA;
            ViewBag.Message = "Actualización del catálogo de Equipos ";
            ViewBag.Equipos = false;
            ViewBag.Estructuras = false;
            ViewBag.Ubicaciones = false;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            UpdCatSap catSap = new UpdCatSap();
            catSap.Equipos = catSap.WorkCenter = catSap.Estructuras = catSap.Ubicaciones = catSap.Procedimientos = false;
            catSap.ResultEquipos = catSap.ResultWc = catSap.ResultEstruc = catSap.ResultUbica = catSap.ResultProced = -1;
            catSap.MsgEquipos = catSap.MsgWc = catSap.MsgEstruc = catSap.MsgUbica = catSap.MsgProced = "";
            return PartialView("_UpdateCatSap", catSap);
        }

        [HttpPost]
        public ActionResult UpdateCatSap(UpdCatSap catSap)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            BLDatosSap blCatSap = new BLDatosSap();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = " [ Todos ] ";
            if (cCostos != "")
                ViewBag.TitleCC = config.TituCC;
            ViewBag.TitleCA = config.TituCA;
            ViewBag.Message = "Actualización del catálogo de Equipos ";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            catSap.ResultEquipos = catSap.ResultWc = catSap.ResultEstruc = catSap.ResultUbica = catSap.ResultProced = -1;
            catSap.MsgEquipos = catSap.MsgWc = catSap.MsgEstruc = catSap.MsgUbica = catSap.MsgProced = "";

            DatosCnxSap cnxSap = new DatosCnxSap()
            {
                Host = config.HostIPSap,
                SystemID = config.SystemIDSap,
                SystemNumber = config.SystemNumberSap,
                Client = config.ClientSap,
                Language = config.LanguageSap,
                PoolSize = config.PoolSizeSap,
                UserPRD = config.UserSap,
                PwdPRD = config.PwdSap
            };
            string rutaLog = config.RutaLog;

            if (ModelState.IsValid)
            {

                //Ejecutar procesos seleccionados
                if (catSap.Equipos)
                {
                    catSap.ResultEquipos = blCatSap.UpdateCatEquipos(cnxSqlMT, cnxSap, rutaLog);
                    if (catSap.ResultEquipos == 0) catSap.MsgEquipos = "Actualización CORRECTA";
                    else catSap.MsgEquipos = "!ERROR! en la  Actualización";
                }


                if (catSap.WorkCenter)
                {
                    catSap.ResultWc = blCatSap.UpdateCatWC(cnxSqlMT, cnxSap, rutaLog);
                    if (catSap.ResultWc == 0) catSap.MsgWc = "Actualización CORRECTA";
                    else catSap.MsgWc = "!ERROR! en la  Actualización";
                }

                if (catSap.Estructuras)
                {
                    catSap.ResultEstruc = blCatSap.UpdateCatEstructuras(cnxSqlMT, cnxSap, rutaLog);
                    if (catSap.ResultEstruc == 0) catSap.MsgEstruc = "Actualización CORRECTA";
                    else catSap.MsgEstruc = "!ERROR! en la  Actualización";
                }

                if (catSap.Ubicaciones)
                {
                    catSap.ResultUbica = blCatSap.UpdateCatFunctLocat(cnxSqlMT, cnxSap, rutaLog);
                    if (catSap.ResultUbica == 0) catSap.MsgUbica = "Actualización CORRECTA";
                    else catSap.MsgUbica = "!ERROR! en la  Actualización";
                }

                if (catSap.Procedimientos)
                {
                    //catSap.ResultProced = blCatSap.UpdateCatProced(cnxSqlMT, cnxSap, rutaLog);
                    //if (catSap.ResultProced == 0) catSap.MsgProced = "Actualización CORRECTA";
                    //else catSap.MsgProced = "!ERROR! en la  Actualización";
                }

            }

            return PartialView("_UpdateCatSap", catSap);
        }
    }
}