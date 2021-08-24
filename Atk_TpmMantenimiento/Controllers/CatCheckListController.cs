using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CatCheckListController : Controller
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
        BL_CatCheckList Bl_CheckList = new BL_CatCheckList();
        BL_Usuarios blUsu = new BL_Usuarios();
        #endregion
        // GET: CatCheckList
        public ActionResult CatCheckList()
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

            List<CheckListEnc> lstChk = new List<CheckListEnc>();
            lstChk = Bl_CheckList.GetCatCheckList(cnxSqlMT, pDepto);

            ViewBag.Message = "Catálogo de Checklist del área de " + TituArea;
            ViewBag.Editar = false;
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Error = TempData["Error"];

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion

            return View(lstChk);
        }

        [HttpGet]
        public ActionResult Nuevo(string mensaje, string ResultOperacion)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            AltaCheckListEnc newChk = new AltaCheckListEnc();
            newChk.EncCheckList = new CheckListEnc();
            newChk.EncCheckList.CodCheckList = "";
            newChk.EncCheckList.DescripCheckList = "";
            newChk.EncCheckList.CodClasif = "";
            newChk.EncCheckList.EqParado = true;
            newChk.EncCheckList.Activo = true;
            newChk.EncCheckList.CodDepartamento = pDepto;
            newChk.EncCheckList.UserAlta = usuario;
            newChk.EncCheckList.UserModif = usuario;
            newChk.EncCheckList.FchModif = DateTime.Now;
            newChk.EncCheckList.FchAlta = DateTime.Now;

            newChk.lstClasif = Bl_CheckList.GetClasifCheckList(cnxSqlMT);

            ViewBag.Message = "Alta de Checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.result = ResultOperacion;
            ViewBag.msg = mensaje;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion

            TempData["Error"] = null;

            return View("Nuevo", newChk);
        }

        [HttpPost]
        public ActionResult Nuevo(AltaCheckListEnc newChk)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();

            ViewBag.Message = "Alta de Checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion

            TempData["Error"] = null;

            // Validamos la clave que no este repetida
            if (Bl_CheckList.ValidaClave(cnxSqlMT, newChk.EncCheckList.CodCheckList, pDepto, rutalog))
            {
                ModelState.AddModelError("CodCheckList", "Clave ya existe");
            }

            if (!ModelState.IsValid)
            {
                newChk.lstClasif = Bl_CheckList.GetClasifCheckList(cnxSqlMT);
                return View("Nuevo", newChk);
            }
            newChk.EncCheckList.FchAlta = DateTime.Now;
            newChk.EncCheckList.FchModif = DateTime.Now;
            int result = Bl_CheckList.Guardar(cnxSqlMT, newChk.EncCheckList);

            return RedirectToAction("CatCheckList");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            AltaCheckListEnc editEncChk = new AltaCheckListEnc();
            editEncChk.EncCheckList = Bl_CheckList.DatosEncChk(cnxSqlMT, id, pDepto);
            editEncChk.lstClasif = Bl_CheckList.GetClasifCheckList(cnxSqlMT);

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición de Checklist";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion

            TempData["Error"] = null;

            return View("Editar", editEncChk);
        }

        [HttpPost]
        public ActionResult Editar(AltaCheckListEnc editEncChk)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición de Checklist";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion

            if (!ModelState.IsValid)
            {
                editEncChk.lstClasif = Bl_CheckList.GetClasifCheckList(cnxSqlMT);
                return View("Editar", editEncChk);
            }

            editEncChk.EncCheckList.FchModif = DateTime.Now;
            editEncChk.EncCheckList.UserModif = usuario;

            int result = Bl_CheckList.Update(cnxSqlMT, editEncChk.EncCheckList, rutalog);

            if (result != 1)
                TempData["Error"] = "Error al guardar el registro: " + editEncChk.EncCheckList.DescripCheckList.ToString();
            return RedirectToAction("CatCheckList");

        }

        [HttpGet]
        public ActionResult VerActiv(int id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            CheckListDet ChkDt = new CheckListDet();
            BL_CatActividades BlCatAct = new BL_CatActividades();
            BL_GrupoActiv BlGpoAct = new BL_GrupoActiv();

            ChkDt.lstGrupos = BlGpoAct.GetCatGpoActiv(cnxSqlMT, pDepto, CCostos, true);
            ChkDt.lstCatAct = BlCatAct.GetCatActividades(cnxSqlMT, pDepto, CCostos);

            ChkDt.lstActChk = Bl_CheckList.GetChecklstActiv(cnxSqlMT, id, pDepto);

            ChkDt.Encabezado = Bl_CheckList.DatosEncChk(cnxSqlMT, id, pDepto);
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Actividades del Checklist";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion


            return View(ChkDt);
        }

        [HttpGet]
        public ActionResult EditarActivi(int id, string mensaje, string ResultOperacion)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Agregar actividades al Checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.result = ResultOperacion;
            ViewBag.msg = mensaje;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion

            AltaCheckListDet newChkDt = new AltaCheckListDet();
            BL_CatActividades BlCatAct = new BL_CatActividades();
            BL_GrupoActiv BlGpoAct = new BL_GrupoActiv();

            newChkDt.Encabezado = Bl_CheckList.DatosEncChk(cnxSqlMT, id, pDepto);
            newChkDt.lstGrupos = BlGpoAct.GetCatGpoActiv(cnxSqlMT, pDepto, CCostos, true);
            newChkDt.lstCatAct = BlCatAct.GetCatActividades(cnxSqlMT, pDepto, CCostos, true).OrderBy(x => x.DescripcionAct).ToList();
            newChkDt.ActChk = new CheckListDt();
            newChkDt.ActChk.TipoActividad = "A";
            if (mensaje == null)
            {
                ViewBag.msg = "";
                ViewBag.result = "";
            }
            //return PartialView("_AgregarAct", newChkDt);
            return View(newChkDt);
        }

        [HttpPost]
        public ActionResult EditarActivi(AltaCheckListDet newChkDt)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Message = "Agregar actividades al Checklist";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Error = "";
            string msj = "";
            string valorResult = "";

            if (!ModelState.IsValid)
            {
                if (newChkDt.ActChk.TipoActividad == "A" && newChkDt.ActChk.IdActividad == 0)
                {
                    ViewBag.Error = "Seleccione una Actividad";
                    valorResult = "B";
                    ModelState.AddModelError("ActChk.IdActividad", "Seleccione una Actividad");
                    BL_CatActividades BlCatAct = new BL_CatActividades();
                    BL_GrupoActiv BlGpoAct = new BL_GrupoActiv();
                    newChkDt.lstGrupos = BlGpoAct.GetCatGpoActiv(cnxSqlMT, pDepto, CCostos, true);
                    newChkDt.lstCatAct = BlCatAct.GetCatActividades(cnxSqlMT, pDepto, CCostos, true);
                    return View(newChkDt);

                }
                if (newChkDt.ActChk.TipoActividad == "G" && newChkDt.ActChk.IdGrupoAct == 0)
                {
                    ViewBag.Error = "Seleccione un Grupo";
                    valorResult = "B";
                    ModelState.AddModelError("ActChk.IdGrupoAct ", "Seleccione un grupo");

                    BL_CatActividades BlCatAct = new BL_CatActividades();
                    BL_GrupoActiv BlGpoAct = new BL_GrupoActiv();
                    newChkDt.lstGrupos = BlGpoAct.GetCatGpoActiv(cnxSqlMT, pDepto, CCostos, true);
                    newChkDt.lstCatAct = BlCatAct.GetCatActividades(cnxSqlMT, pDepto, CCostos, true);
                    return View(newChkDt);
                }


            }

            int result = Bl_CheckList.GuardarActivi(cnxSqlMT, newChkDt);


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

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion

            return RedirectToAction("EditarActivi", new { id = newChkDt.ActChk.IdCheckList, mensaje = msj, ResultOperacion = valorResult });
        }

        public ActionResult Eliminar(int idDetChk, int idChecklst)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            CCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            int result = Bl_CheckList.ElimActChk(cnxSqlMT, idDetChk, idChecklst, rutalog);


            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), CCostos);
            #endregion


            return RedirectToAction("VerActiv", new { id = idChecklst });
        }

    }
}