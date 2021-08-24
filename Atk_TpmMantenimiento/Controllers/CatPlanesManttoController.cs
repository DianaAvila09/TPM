using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidades;
using BusinessLogic;
using Atk_TpmMantenimiento.Properties;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CatPlanesManttoController : Controller
    {
        #region Variables
        //DatosCnxSap cnxSap = new DatosCnxSap()
        //{
        //   Host = Settings.Default.Host,
        //   SystemID = Settings.Default.SystemID,
        //   SystemNumber = Settings.Default.SystemNumber,
        //   Client = Settings.Default.Client,
        //   Language = Settings.Default.Language,
        //   PoolSize = Settings.Default.PoolSize,
        //   UserPRD = Settings.Default.UserPRD,
        //   PwdPRD = Settings.Default.PwdPRD
        //};

        static string SrvSql = Settings.Default.SrvSql;
        static string BD = Settings.Default.BD;
        static string UserSql = Settings.Default.UserSql;
        static string PwdSql = Settings.Default.PwdSql;
        static string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

        static string TituArea = "";
        static string TituCC = "";
        static string TituCA = "";
        static string rutalog = "";
        static string cCostos = "";
        static string pCostos = "";

        static string Depto = "";

        BL_TPM tpm = new BL_TPM();
        BLCatPlanesManto blPm = new BLCatPlanesManto();
        BLDatosSap datosSap = new BLDatosSap();
        BLSistemasEquipo datosSistEqui = new BLSistemasEquipo();
        BL_CatalogosSap blcatSap = new BL_CatalogosSap();
        BL_Usuarios blUsu = new BL_Usuarios();

        #endregion
        public ActionResult CatPlanesMantto()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            TituArea = config.TituArea;
            TituCC = config.TituCC;
            TituCA = config.TituCA;
            rutalog = config.RutaLog;

            Depto = "";
            if (cCostos != "")
                Depto = config.Depto;
            //pCostos = config.CtroCtosSap;

            List<PlanMantto> lstDatos = blPm.DatosCatalogo(cnxSqlMT, Depto, cCostos, rutalog);

            ViewBag.Message = "Catálogo de Planes de mantenimiento";
            ViewBag.Editar = false;
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(lstDatos);
        }

        [HttpGet]
        public ActionResult Nuevo()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            AltaPlanManto plan = new AltaPlanManto();
            List<CatEquipo> ldtE = new List<CatEquipo>();
            if (cCostos != "")
            {
                plan.lstEquipos = blcatSap.GetDatosEquiposPadre(cnxSqlMT, cCostos, rutalog).OrderBy(x => x.DescripTechnical).ToList();
            }
            else {
                plan.lstEquipos = ldtE;
            }
            plan.lstSisManto = datosSistEqui.DatosCatSistManto(cnxSqlMT, Depto).OrderBy(x => x.Sistema).ToList();
            plan.lstCiclos = blPm.DatosCiclos(cnxSqlMT).OrderBy(x => x.CodCiclo).ToList();
            plan.planMt = new PlanMantto();
            plan.planMt.FechaAlta = DateTime.Now;
            plan.planMt.UsuarioAlta = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            plan.planMt.FecUltEjecucion = DateTime.Now;

            ViewBag.Message = "Alta de un Plan de Mantenimiento";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return PartialView("_Nuevo", plan);
        }

        [HttpPost]
        public ActionResult Nuevo(AltaPlanManto plan)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Editar = false;

            if (!ModelState.IsValid)
            {
                plan.lstEquipos = blcatSap.GetDatosEquiposPadre(cnxSqlMT, cCostos, rutalog);
                plan.lstSisManto = datosSistEqui.DatosCatSistManto(cnxSqlMT, Depto).OrderBy(x => x.Sistema).ToList();
                plan.lstCiclos = blPm.DatosCiclos(cnxSqlMT).OrderBy(x => x.CodCiclo).ToList(); ;
                plan.planMt.FechaAlta = DateTime.Now;
                plan.planMt.UsuarioAlta = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                plan.planMt.FecUltEjecucion = DateTime.Now;

                ViewBag.Message = "Alta de Plan de Mantenimiento";
                ViewBag.Title = TituArea;
                ViewBag.TitleCC = TituCC;

                #region Combo Centro de Costos            
                ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
                #endregion

                return PartialView("_Nuevo", plan);

            }

            int result = blPm.Guardar(cnxSqlMT, plan.planMt, cCostos);

            return Json(new { success = true });
            //return RedirectToAction("CatPlanesMantto", "CatPlanesMantto");

        }


        [HttpGet]
        public ActionResult Editar(int Id)
        {

            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            AltaPlanManto plan = new AltaPlanManto();
            BLCatPlanesManto blPlanMto = new BLCatPlanesManto();

            plan.lstEquipos = blcatSap.GetDatosEquiposPadre(cnxSqlMT, cCostos, rutalog).OrderBy(x => x.DescripTechnical).ToList();
            plan.lstSisManto = datosSistEqui.DatosCatSistManto(cnxSqlMT, Depto).OrderBy(x => x.Sistema).ToList();
            plan.lstCiclos = blPm.DatosCiclos(cnxSqlMT).OrderBy(x => x.CodCiclo).ToList();

            plan.planMt = blPlanMto.DatosPlanManto(cnxSqlMT, plan.lstEquipos, plan.lstSisManto, plan.lstCiclos, Id);
            plan.planMt.FechaAlta = DateTime.Now;
            plan.planMt.UsuarioAlta = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición del Plan de Mantenimiento";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return PartialView("_Editar", plan);
        }

        [HttpPost]
        public ActionResult Editar(AltaPlanManto planEdit)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición del Plan de Mantenimiento";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            if (!ModelState.IsValid)
            {
                planEdit.lstEquipos = blcatSap.GetDatosEquiposPadre(cnxSqlMT, cCostos, rutalog).OrderBy(x => x.DescripTechnical).ToList();
                planEdit.lstSisManto = datosSistEqui.DatosCatSistManto(cnxSqlMT, Depto).OrderBy(x => x.Sistema).ToList();
                planEdit.lstCiclos = blPm.DatosCiclos(cnxSqlMT).OrderBy(x => x.CodCiclo).ToList();

                return PartialView("_Editar", planEdit);
            }

            blPm.Update(cnxSqlMT, planEdit.planMt, rutalog);

            return Json(new { success = true });
            //return RedirectToAction("CatPlanesMantto", "CatPlanesMantto");

        }
    }
}