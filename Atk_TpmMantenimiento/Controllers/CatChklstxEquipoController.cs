using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CatChklstxEquipoController : Controller
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
        BL_ChklisxEquipo bl_ChkxEquipo = new BL_ChklisxEquipo();
        BL_Usuarios blUsu = new BL_Usuarios();
        #endregion

        public ActionResult CatChklstxEquipo()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            TituArea = config.TituArea;
            
            TituCA = config.TituCA;
            rutalog = config.RutaLog;

            TituCC = "";
            pDepto = "";
            if (cCostos != "")
            {
                TituCC = config.TituCC;
                pDepto = config.Depto;
                pCostos = config.CtroCtosSap;
            }

            PlantaSat = config.Planta;

            List<CheckListEqEnc> lstChkxEq = new List<CheckListEqEnc>();
            lstChkxEq = bl_ChkxEquipo.GetCatChkxEq(cnxSqlMT, PlantaSat, pDepto, pCostos, true);

            ViewBag.Message = "Catálogo de equipos con Checklist";
            ViewBag.Editar = false;
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion


            return View(lstChkxEq);
        }
        [HttpGet]
        public ActionResult Nuevo(string mensaje, string ResultOperacion)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            AltaChkxEq newChkxEq = new AltaChkxEq();


            BL_CatalogosSap blcatSap = new BL_CatalogosSap();
            BL_CatCheckList bl_CatCheck = new BL_CatCheckList();
            BL_CatTpm bl_CatTpm = new BL_CatTpm();

            newChkxEq.lstEquipos = blcatSap.GetDatosEquiposPadre(cnxSqlMT, pCostos, rutalog);
            newChkxEq.lstChecklst = bl_CatCheck.GetCatCheckList(cnxSqlMT, pDepto);
            newChkxEq.lstCiclos = bl_CatTpm.DatosCatCiclos(cnxSqlMT);

            newChkxEq.lstListAct = new List<CheckListDt>();
            newChkxEq.ChklsxEq = new CheckListEqEnc();
            newChkxEq.ChklsxEq.ChkEquipo = "";
            newChkxEq.ChklsxEq.CodChkList = "";
            newChkxEq.ChklsxEq.CodWorkCenter = "";
            newChkxEq.ChklsxEq.CodEquipo = "";
            newChkxEq.ChklsxEq.EqParado = true;
            newChkxEq.ChklsxEq.Activo = true;
            newChkxEq.ChklsxEq.CodDepartamento = pDepto;
            newChkxEq.ChklsxEq.UserAlta = usuario;
            newChkxEq.ChklsxEq.UserModif = usuario;
            newChkxEq.ChklsxEq.FchModif = DateTime.Now;
            newChkxEq.ChklsxEq.FchAlta = DateTime.Now;

            ViewBag.Message = "Asignación de un Checklist a un equipo";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.result = ResultOperacion;
            ViewBag.msg = mensaje;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            TempData["Error"] = null;

            return View("Nuevo", newChkxEq);
        }

        [HttpPost]
        public ActionResult Nuevo(AltaChkxEq newChkxEq, string accion)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Asignación de un Checklist a un equipo";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            BL_CatalogosSap blcatSap = new BL_CatalogosSap();

            CatEquipo datosEquipo = new CatEquipo();
            datosEquipo = blcatSap.GetDatosEquipo(cnxSqlMT, newChkxEq.ChklsxEq.CodEquipo);
            newChkxEq.ChklsxEq.CodWorkCenter = datosEquipo.WorkCenter;

            BL_CatCheckList bl_CatCheck = new BL_CatCheckList();
            var encb = bl_CatCheck.DatosEncChk(cnxSqlMT, newChkxEq.ChklsxEq.IdCheckList, pDepto);

            newChkxEq.ChklsxEq.CodChkList = encb.CodCheckList;
            newChkxEq.ChklsxEq.CodClasif = encb.CodClasif;
            newChkxEq.ChklsxEq.DescripChkList = encb.DescripCheckList;

            if (accion == "VerActiv")
            {
                BL_CatTpm bl_CatTpm = new BL_CatTpm();

                newChkxEq.lstEquipos = blcatSap.GetDatosEquiposPadre(cnxSqlMT, pCostos, rutalog);
                newChkxEq.lstChecklst = bl_CatCheck.GetCatCheckList(cnxSqlMT, pDepto);
                newChkxEq.lstListAct = bl_CatCheck.GetChecklstActiv(cnxSqlMT, newChkxEq.ChklsxEq.IdCheckList, pDepto);
                newChkxEq.lstCiclos = bl_CatTpm.DatosCatCiclos(cnxSqlMT);


            }

            if (accion == "Guardar")
            {

                newChkxEq.lstListAct = bl_CatCheck.GetChecklstActiv(cnxSqlMT, newChkxEq.ChklsxEq.IdCheckList, pDepto);
                // Validamos la clave que no este repetida
                if (bl_ChkxEquipo.ValidaChekxEq(cnxSqlMT, newChkxEq.ChklsxEq, rutalog))
                {
                    ModelState.AddModelError("ChklsxEq.CodEquipo", "Ya existe un checklist para este equipo con la misma frecuencia");

                    BL_CatTpm bl_CatTpm = new BL_CatTpm();
                    newChkxEq.lstEquipos = blcatSap.GetDatosEquiposPadre(cnxSqlMT, pCostos, rutalog);
                    newChkxEq.lstChecklst = bl_CatCheck.GetCatCheckList(cnxSqlMT, pDepto);
                    newChkxEq.lstCiclos = bl_CatTpm.DatosCatCiclos(cnxSqlMT);
                    return View("Nuevo", newChkxEq);
                }

                newChkxEq.ChklsxEq.FchModif = DateTime.Now;
                newChkxEq.ChklsxEq.Planta = PlantaSat;
                newChkxEq.ChklsxEq.CentroCostos = pCostos;
                newChkxEq.ChklsxEq.CodDepartamento = pDepto;
                int result = bl_ChkxEquipo.Guardar(cnxSqlMT, newChkxEq, rutalog);
                string msj = "";
                string valorResult = "";
                if (result == 1)
                {
                    msj = "Registro Guardado";
                    usuario = Session["UserName"].ToString();
                    valorResult = "B";
                }
                else
                {
                    msj = "Error al Guardar";
                    valorResult = "M";
                }
                return RedirectToAction("CatChklstxEquipo", new { mensaje = msj, ResultOperacion = valorResult });
            }
            return View("Nuevo", newChkxEq);
        }

        [HttpGet]
        public ActionResult Editar(int Id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Edición de Checklist asignado a un equipo";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            AltaChkxEq chkEncxEq = new AltaChkxEq();

            chkEncxEq.ChklsxEq = bl_ChkxEquipo.GetDatosChkxEq(cnxSqlMT, Id, pDepto);
            BL_CatCheckList bl_CatCheck = new BL_CatCheckList();
            chkEncxEq.lstListAct = bl_CatCheck.GetChecklstActiv(cnxSqlMT, chkEncxEq.ChklsxEq.IdCheckList, pDepto);

            BL_CatTpm bl_CatTpm = new BL_CatTpm();
            chkEncxEq.lstCiclos = bl_CatTpm.DatosCatCiclos(cnxSqlMT);

            return View(chkEncxEq);
        }


        [HttpPost]
        public ActionResult Editar(AltaChkxEq newChkxEq)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Edición de Checklist asignado a un equipo";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            newChkxEq.ChklsxEq.Planta = PlantaSat;
            newChkxEq.ChklsxEq.CentroCostos = pCostos;
            newChkxEq.ChklsxEq.CodDepartamento = pDepto;


            newChkxEq.ChklsxEq.FchModif = DateTime.Now;
            int result = bl_ChkxEquipo.UpdateEncb(cnxSqlMT, newChkxEq.ChklsxEq, rutalog);
            string msj = "";
            string valorResult = "";
            if (result == 1)
            {
                msj = "Registro Guardado";
                usuario = Session["UserName"].ToString();
                valorResult = "B";
            }
            else
            {
                msj = "Error al Guardar";
                valorResult = "M";
            }
            return RedirectToAction("CatChklstxEquipo", new { mensaje = msj, ResultOperacion = valorResult });

        }

        [HttpGet]
        public ActionResult VerActiv(int Id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Actividades de Checklist asignado a un equipo";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.SumPonder = 0;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            AltaChkxEq chkxEq = new AltaChkxEq();

            chkxEq.ChklsxEq = bl_ChkxEquipo.GetDatosChkxEq(cnxSqlMT, Id, pDepto);
            chkxEq.lstChckActEq = bl_ChkxEquipo.GetDatosChkxEqDet(cnxSqlMT, Id, pDepto);

            ViewBag.SumPonder = chkxEq.lstChckActEq.Sum(x => x.Ponderacion);

            return View(chkxEq);
        }

        [HttpGet]
        public ActionResult EditActividad(int idChkEqDet, int idCkEqEnc, int IdActiv)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            BL_CatTpm bl_catTpm = new BL_CatTpm();

            ViewBag.Message = "Edición de actividad de Checklist asignado a un equipo";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            AltaRangos editActivi = new AltaRangos();
            editActivi.datosAct = bl_ChkxEquipo.GetDatosEqAct(cnxSqlMT, idChkEqDet, idCkEqEnc, IdActiv);
            editActivi.lstUom = bl_catTpm.GetCatUom(cnxSqlMT);


            return PartialView("_EditActividad", editActivi);
        }

        [HttpPost]
        public ActionResult EditActividad(CheckListEqDt datosAct)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Edición de actividad de Checklist asignado a un equipo";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            int result = bl_ChkxEquipo.GuardarAct(cnxSqlMT, datosAct);
            return Json(new { success = true });
        }


        [HttpGet]
        public ActionResult BorrarActiv(int idChkEqDet, int idCkEqEnc, int IdActiv)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            int result = bl_ChkxEquipo.ElimActChk(cnxSqlMT, idChkEqDet, idCkEqEnc, IdActiv, rutalog);

            return RedirectToAction("VerActiv", new { id = idCkEqEnc });
        }

        [HttpGet]
        public ActionResult Activar(int idCkEqEnc)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Activar programación del Checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            CheckListEqEnc datosChk = bl_ChkxEquipo.GetDatosChkxEq(cnxSqlMT, idCkEqEnc, pDepto);

            datosChk.IniProgram = DateTime.Now;
            return PartialView("_Activar", datosChk);
        }

        [HttpPost]
        public ActionResult Activar(CheckListEqEnc chkEncb)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Activar programación del Checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            //CheckListEqEnc datosChk = bl_ChkxEquipo.GetDatosChkxEq(cnxSqlMT, chkEncb.IdChkEquipo, pDepto);
            chkEncb.UserActiva = usuario;

            int result = bl_ChkxEquipo.GuardarProgram(cnxSqlMT, chkEncb);
            return Json(new { success = true });


        }

        [HttpPost]
        public ActionResult Cancelar(int idCkEqEnc)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Eliminar programación del Checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            //CheckListEqEnc datosChk = bl_ChkxEquipo.GetDatosChkxEq(cnxSqlMT, chkEncb.IdChkEquipo, pDepto);
            

            int result = bl_ChkxEquipo.EliminarProgram(cnxSqlMT, idCkEqEnc, usuario);

            return RedirectToAction("VerActiv", new { id = idCkEqEnc });

        }


    }
}