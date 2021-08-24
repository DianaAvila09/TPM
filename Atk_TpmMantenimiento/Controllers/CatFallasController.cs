using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atk_TpmMantenimiento.Properties;
using Entidades;
using BusinessLogic;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CatFallasController : Controller
    {

        #region Variables

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

        static string pDepto = "";
        static int PlantaSat = 0;

        BL_CatFallas BlCatFallas = new BL_CatFallas();
        BL_TPM tpm = new BL_TPM();
        BLSistemasEquipo datosSistEqui = new BLSistemasEquipo();
        BL_Usuarios blUsu = new BL_Usuarios();
        #endregion

        // GET: CatFallas
        public ActionResult CatFallas()
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

            pDepto = "";
            if (cCostos != "")
            {
                pDepto = config.Depto;
                pCostos = config.CtroCtosSap;
                PlantaSat = config.Planta;
            }

            List<Falla> lstDatos = BlCatFallas.DatosCatFallas(cnxSqlMT, pDepto);

            ViewBag.Message = "Catálogo de Fallas del área de " + TituArea;
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

            AltaFallas fallaNew = new AltaFallas();
            fallaNew.lstSisManto = datosSistEqui.DatosCatSistManto(cnxSqlMT, pDepto);

            fallaNew.fallaMt = new Falla();
            fallaNew.fallaMt.FecAlta = DateTime.Now;
            fallaNew.fallaMt.UsuarioAlta = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            fallaNew.fallaMt.FecActualizacion = DateTime.Now;
            fallaNew.fallaMt.CodDepartamento = pDepto;

            ViewBag.Message = "Alta de un Falla de Mantenimiento";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return PartialView("_Nuevo", fallaNew);
        }

        [HttpPost]
        public ActionResult Nuevo(AltaFallas fallaNew)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            ViewBag.Message = "Alta de un Falla de Mantenimiento";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion


            if (!ModelState.IsValid)
            {
                return PartialView("_Nuevo", fallaNew);
            }

            fallaNew.fallaMt.FecActualizacion = DateTime.Now;
            int result = BlCatFallas.Guardar(cnxSqlMT, fallaNew.fallaMt, pDepto);            
            if (result == 1)
            {
                ViewBag.msg = "Registro Guardado";
                string usuario = Session["UserName"].ToString();
            }
            else
            {
                ViewBag.msg = "Error al Guardar";
            }


            return Json(new { success = true });
        }



        [HttpGet]
        public ActionResult Editar(int id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            AltaFallas fallanew = new AltaFallas();

            fallanew.lstSisManto = datosSistEqui.DatosCatSistManto(cnxSqlMT, pDepto);
            fallanew.fallaMt = BlCatFallas.DatosFalla(cnxSqlMT, id);
            fallanew.fallaMt.FecActualizacion = DateTime.Now;
            fallanew.fallaMt.UsuarioAlta = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición de una falla Mantenimiento";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion


            return PartialView("_Editar", fallanew);
        }

        [HttpPost]
        public ActionResult Editar(AltaFallas fallaNew)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            fallaNew.fallaMt.FecActualizacion = DateTime.Now;
            fallaNew.fallaMt.UsuarioAlta = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición de una falla Mantenimiento";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion


            if (!ModelState.IsValid)
            {
                fallaNew.lstSisManto = datosSistEqui.DatosCatSistManto(cnxSqlMT, pDepto);
                return PartialView("_Editar", fallaNew);
            }

            int result = BlCatFallas.Update(cnxSqlMT, fallaNew.fallaMt, rutalog);
            if (result == 1)
            {
                ViewBag.msg = "Registro Guardado";
            }
            else
            {
                ViewBag.msg = "Error al Guardar";
            }

            return Json(new { success = true });

        }


    }
}