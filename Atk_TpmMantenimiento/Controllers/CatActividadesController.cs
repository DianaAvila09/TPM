using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CatActividadesController : Controller
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
        static string CCostos = "";
        static string pCostos = "";

        static string pDepto = "";
        static int PlantaSat = 0;


        BL_TPM tpm = new BL_TPM();
        BL_CatActividades BlActiv = new BL_CatActividades();
        BL_Usuarios blUsu = new BL_Usuarios();
        #endregion

        public ActionResult CatActividades()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, CCostos);

            pDepto = "";
            TituCC = "";
            if (CCostos != "")
            {
                pDepto = config.Depto;
                pCostos = config.CtroCtosSap;
                TituCC = config.TituCC;
            }

            TituArea = config.TituArea;
            
            TituCA = config.TituCA;
            rutalog = config.RutaLog;
            
            
            PlantaSat = config.Planta;

            List<Actividad> lstAct = new List<Actividad>();
            lstAct = BlActiv.GetCatActividades(cnxSqlMT, pDepto, CCostos);

            ViewBag.Message = "Catálogo de actividades para checklsit del área de " + TituArea;
            ViewBag.Editar = false;
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion

            return View(lstAct);
        }

        [HttpGet]
        public ActionResult Nuevo()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();


            string usuario = Session["UserName"].ToString();
            AltaActividad newAct = new AltaActividad();

            BLSistemasEquipo blSist = new BLSistemasEquipo();
            BL_CatCompon blCompo = new BL_CatCompon();
            newAct.lstSisManto = blSist.DatosCatSistManto(cnxSqlMT, pDepto).OrderBy(x => x.Sistema).ToList();
            newAct.lstCompo = blCompo.GetCatCompo(cnxSqlMT, pDepto, CCostos);

            newAct.Activi = new Actividad();
            newAct.Activi.CodActividad = "";
            newAct.Activi.DescripcionAct = "";
            newAct.Activi.TipoOperacion = "V";
            newAct.Activi.EqParado = true;
            newAct.Activi.Activo = true;
            newAct.Activi.UserAlta = usuario;
            newAct.Activi.UserModif = usuario;
            newAct.Activi.FchModif = DateTime.Now;
            newAct.Activi.FchAlta = DateTime.Now;
            newAct.Activi.CodDepartamento = pDepto;
            newAct.Activi.IdComponente = 0;
            newAct.Activi.CodSistema = "";
            ViewBag.Message = "Alta de un actividad para checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion

            return View("Nuevo", newAct);
        }

        [HttpPost]
        public JsonResult GetCatCompoSistema(string pCodSistema, string pCodDepartamento)
        {
            List<Componente> lstCompo = new List<Componente>();
            BL_CatCompon blCompo = new BL_CatCompon();

           

            pDepto = "";            
            if (CCostos != "")
            {
                DatosConfig config = new DatosConfig();
                config = tpm.LeeConfig(cnxSqlMT, CCostos);
                pDepto = config.Depto;
               
            }

            lstCompo = blCompo.GetCatCompoxSist(cnxSqlMT, pCodSistema, pDepto, CCostos);
            return Json(new SelectList(lstCompo, "IdComponente", "DescripCompo"));
        }

        [HttpPost]
        public ActionResult Nuevo(AltaActividad newAct)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();

            ViewBag.Message = "Alta de un actividad para checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            // Validamos la clave que no este repetida
            if (BlActiv.ValidaClave(cnxSqlMT, newAct.Activi.CodActividad, rutalog))
            {
                ModelState.AddModelError("Activi.CodActividad", "Clave ya existe");
            }

            if (!ModelState.IsValid)
            {
                BLSistemasEquipo blSist = new BLSistemasEquipo();
                BL_CatCompon blCompo = new BL_CatCompon();
                newAct.lstSisManto = blSist.DatosCatSistManto(cnxSqlMT, pDepto).OrderBy(x => x.Sistema).ToList();
                newAct.lstCompo = blCompo.GetCatCompo(cnxSqlMT, pDepto, CCostos);
                return View("Nuevo", newAct);
            }

            newAct.Activi.FchModif = DateTime.Now;
            int result = BlActiv.Guardar(cnxSqlMT, newAct.Activi);
            if (result == 1)
            {
                ViewBag.msg = "Registro Guardado";
                string usuario = Session["UserName"].ToString();
            }
            else
            {
                ViewBag.msg = "Error al Guardar";
            }


            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion


            return RedirectToAction("Nuevo");

        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            Actividad editAct = new Actividad();
            editAct = BlActiv.DatosActiv(cnxSqlMT, id);

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición de una actividad para checklist";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion

            return View("Editar", editAct);
        }

        [HttpPost]
        public ActionResult Editar(Actividad editAct)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();


            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición de una Actividad para checklist";

            if (!ModelState.IsValid)
            {

                return View("Editar", editAct);
            }

            editAct.FchModif = DateTime.Now;
            editAct.UserModif = usuario;

            int result = BlActiv.Update(cnxSqlMT, editAct, rutalog);
            if (result == 1)
            {
                ViewBag.msg = "Registro Guardado";
            }
            else
            {
                ViewBag.msg = "Error al Guardar";
            }

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion

            return RedirectToAction("CatActividades");

        }


    }
}