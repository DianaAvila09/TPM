using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Atk_TpmMantenimiento.Controllers
{
    public class CatGrupoActivController : Controller
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


        BL_TPM tpm = new BL_TPM();
        BL_GrupoActiv BlGpoActiv = new BL_GrupoActiv();
        BL_Usuarios blUsu = new BL_Usuarios();
        #endregion

        public ActionResult CatGrupoActiv()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            pDepto = "";
            TituCC = "";
            if (cCostos != "")
            {
                pDepto = config.Depto;
                pCostos = config.CtroCtosSap;
                TituCC = config.TituCC;
            }

            TituArea = config.TituArea;
            
            TituCA = config.TituCA;
            rutalog = config.RutaLog;           
            PlantaSat = config.Planta;

            List<GrupoActivEnc> lstGpoAct = new List<GrupoActivEnc>();
            lstGpoAct = BlGpoActiv.GetCatGpoActiv(cnxSqlMT, pDepto, cCostos);

            ViewBag.Message = "Catálogo de grupos de actividades para Checklist";
            ViewBag.Editar = false;
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(lstGpoAct);
        }

        [HttpGet]
        public ActionResult Nuevo(string mensaje, string ResultOperacion)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            GrupoActivEnc newGpo = new GrupoActivEnc();

            newGpo.CodGrupo = "";
            newGpo.DescripGpo = "";
            newGpo.EqParado = true;
            newGpo.Activo = true;
            newGpo.CodDepartamento = pDepto;
            newGpo.UserAlta = usuario;
            newGpo.UserModif = usuario;
            newGpo.FchModif = DateTime.Now;
            newGpo.FchAlta = DateTime.Now;

            ViewBag.Message = "Alta de grupo de actividades para checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.result = ResultOperacion;
            ViewBag.msg = mensaje;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion


            if (mensaje == null)
            {
                ViewBag.msg = "";
                ViewBag.result = "";
            }
            return View("Nuevo", newGpo);
        }

        [HttpPost]
        public ActionResult Nuevo(GrupoActivEnc newGpo)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            ViewBag.Message = "Alta de grupo de actividades para checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            newGpo.CentroCostos = cCostos;

            // Validamos la clave que no este repetida
            if (BlGpoActiv.ValidaClave(cnxSqlMT, newGpo, rutalog))
            {
                ModelState.AddModelError("CodGrupo", "Clave ya existe");
            }

            if (!ModelState.IsValid)
            {
                return View("Nuevo", newGpo);
            }

            newGpo.FchModif = DateTime.Now;
            int result = BlGpoActiv.Guardar(cnxSqlMT, newGpo);
            string msj = "";
            string valorResult = "";
            if (result == 1)
            {
                msj = "Registro Guardado";
                string usuario = Session["UserName"].ToString();
                valorResult = "B";
            }
            else
            {
                msj = "Error al Guardar";
                valorResult = "M";
            }
            return RedirectToAction("Nuevo", new { mensaje = msj, ResultOperacion = valorResult });
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            GrupoActivEnc editGpo = new GrupoActivEnc();
            editGpo = BlGpoActiv.DatosGpoActiv(cnxSqlMT, id);

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición de grupo actividades para checklist";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View("Editar", editGpo);
        }

        [HttpPost]
        public ActionResult Editar(GrupoActivEnc editGpo)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición de grupo actividades para checklist";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            if (!ModelState.IsValid)
            {
                return View("Editar", editGpo);
            }

            editGpo.FchModif = DateTime.Now;
            editGpo.UserModif = usuario;

            int result = BlGpoActiv.Update(cnxSqlMT, editGpo, rutalog);
            if (result == 1)
            {
                ViewBag.msg = "Registro Guardado";
            }
            else
            {
                ViewBag.msg = "Error al Guardar";
            }
            return RedirectToAction("CatGrupoActiv");

        }

        [HttpGet]
        public ActionResult VerActiv(int id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            AltaGpoAct editGpo = new AltaGpoAct();

            editGpo.GpoEncab = BlGpoActiv.DatosGpoActiv(cnxSqlMT, id);
            editGpo.LstGpoActDet = BlGpoActiv.DatosGpoDet(cnxSqlMT, id);
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Actividades del grupo para Checklist";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(editGpo);

        }


        [HttpPost]
        public ActionResult VerActiv()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            return RedirectToAction("CatGrupoActiv");

        }

        [HttpGet]
        public ActionResult EditarActivi(int id, string mensaje, string ResultOperacion)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Agregar actividades al grupo para checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.result = ResultOperacion;
            ViewBag.msg = mensaje;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            BL_CatActividades blCatActiv = new BL_CatActividades();
            AltaActivGrupo newAct = new AltaActivGrupo();

            GrupoActivEnc editGpo = new GrupoActivEnc();
            newAct.GpoEncab = BlGpoActiv.DatosGpoActiv(cnxSqlMT, id);

            BLSistemasEquipo blSist = new BLSistemasEquipo();
            newAct.lstSisManto = blSist.DatosCatSistManto(cnxSqlMT, pDepto).OrderBy(x => x.Sistema).ToList();
            newAct.lstActiv = BlGpoActiv.GetCatActivSist(cnxSqlMT, newAct.lstSisManto[0].CodSistema, pDepto, cCostos);
            if (mensaje == null)
            {
                ViewBag.msg = "";
                ViewBag.result = "";
            }
            return View(newAct);
        }

        [HttpPost]
        public ActionResult EditarActivi(AltaActivGrupo newAct)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Agreagar actividades al grupo para checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            if (!ModelState.IsValid)
            {
                BLSistemasEquipo blSist = new BLSistemasEquipo();
                newAct.lstSisManto = blSist.DatosCatSistManto(cnxSqlMT, pDepto).OrderBy(x => x.Sistema).ToList();
                BL_CatActividades blCatActiv = new BL_CatActividades();
                newAct.lstActiv = BlGpoActiv.GetCatActivSist(cnxSqlMT, newAct.ActNewGpo.CodSistema, pDepto, cCostos);
                return View("Nuevo", newAct);
            }

            int result = BlGpoActiv.GuardarNewAct(cnxSqlMT, newAct);
            string msj = "";
            string valorResult = "";
            if (result == 1)
            {
                msj = "Registro Guardado";
                valorResult = "B";
            }
            else
            {
                msj = "Error al Guardar";
                valorResult = "M";
            }
            return RedirectToAction("EditarActivi", new { id = newAct.GpoEncab.IdGrupoAct, mensaje = msj, ResultOperacion = valorResult });
        }

        [HttpPost]
        public JsonResult GetCatActivSist(string pCodSistema, string pCodDepartamento)
        {

            List<Actividad> lstActiv = new List<Actividad>();

            lstActiv = BlGpoActiv.GetCatActivSist(cnxSqlMT, pCodSistema, pCodDepartamento, cCostos);
            return Json(new SelectList(lstActiv, "IdActividad", "llave"));
        }


        public ActionResult Eliminar(int IdGpo, int IdAct)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            int result = BlGpoActiv.EliminarActiv(cnxSqlMT, IdGpo, IdAct, rutalog);



            return RedirectToAction("VerActiv", new { id = IdGpo });
        }
    }
}