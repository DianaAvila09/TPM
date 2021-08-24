using Entidades;
using System.Web.Mvc;
using BusinessLogic;
using System.Collections.Generic;
using Atk_TpmMantenimiento.Properties;
using System;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CatDeptosController : Controller
    {

        #region variables de  conexion 

        static string SrvSql = Settings.Default.SrvSql;
        static string BD = Settings.Default.BD;
        static string UserSql = Settings.Default.UserSql;
        static string PwdSql = Settings.Default.PwdSql;
        static string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

        static string TituArea = Settings.Default.TituArea;
        static string TituCC = Settings.Default.TituCC;
        static string TituCA = Settings.Default.TituCA;
        static string rutalog = @Settings.Default.RutaLog;
        static string cCostos = Settings.Default.CentroCostos;
        static string Depto = Settings.Default.Depto;

        BLCatDeptos blDpt = new BLCatDeptos();
        BL_Usuarios blUsu = new BL_Usuarios();

        #endregion


        // GET: CatDeptos
        public ActionResult CatDeptos()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            List<Departamento> lstDpt = new List<Departamento>();

            lstDpt = blDpt.DatosCatalogo(cnxSqlMT, cCostos);

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Catálogo de Departamentos por planta";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(lstDpt);
        }



        public ActionResult Editar()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            List<Departamento> lstDatos = new List<Departamento>();

            lstDatos = blDpt.DatosCatalogo(cnxSqlMT, cCostos);
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Catálogo de Departamentos";
            ViewBag.Editar = true;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View("CatDeptos", lstDatos);
        }

        [HttpPost]
        public ActionResult Guardar(List<Departamento> lstDatos)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            int result = blDpt.GuardarCatalogo(cnxSqlMT, lstDatos);
            List<Departamento> lstDpt = blDpt.DatosCatalogo(cnxSqlMT, cCostos);
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Catálogo de Departamentos";
            ViewBag.Editar = false;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View("CatDeptos", lstDpt);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            BLCatDeptos blCd = new BLCatDeptos();
            List<PlantaSatelite> lstPlantas = blCd.LeePlantasSat(cnxSqlMT);
            ViewBag.Plantas = new SelectList(lstPlantas, "ClavePlanta", "planta");
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View();
        }

        [HttpPost]
        public ActionResult Create(Departamento dato, FormCollection datos)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            string plant = datos["Plantas"];
            dato.PlantaSatelite = plant;
            int result = blDpt.Guardar(cnxSqlMT, dato);
            List<Departamento> lstDpt = blDpt.DatosCatalogo(cnxSqlMT, cCostos);
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Catálogo de Departamentos";
            ViewBag.Editar = false;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View("CatDeptos", lstDpt);

        }


        [HttpPost]
        public ActionResult CreateCheck(Departamento Depto, FormCollection datos)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            string plant = datos["Plantas"];
            Depto.PlantaSatelite = plant;

            if (string.IsNullOrEmpty(Depto.PlantaSatelite))
            {
                ModelState.AddModelError("PlantaSatelite", "Valor Requerido");
            }

            if (string.IsNullOrEmpty(Depto.CodDepartamento))
            {
                ModelState.AddModelError("CodDepartamento", "Valor Requerido");
            }
            else
            {
                if (Depto.CodDepartamento.Length > 4)
                {
                    ModelState.AddModelError("CodDepartamento", "Valor menor a 4 caracteres");
                }
            }


            if (string.IsNullOrEmpty(Depto.Descrip))
            {
                ModelState.AddModelError("Descrip", "Escriba una breve descripción");
            }


            if (string.IsNullOrEmpty(Depto.CentroCostos))
            {
                ModelState.AddModelError("CentroCostos", "Escriba un valor");
            }


            if (ModelState.IsValid)
            {
                BLCatDeptos blCd = new BLCatDeptos();
                List<PlantaSatelite> lstPlantas = blCd.LeePlantasSat(cnxSqlMT);
                ViewBag.Plantas = new SelectList(lstPlantas, "ClavePlanta", "planta");
                return View("Create");
            }
            else
            {
                BLCatDeptos blCd = new BLCatDeptos();
                List<PlantaSatelite> lstPlantas = blCd.LeePlantasSat(cnxSqlMT);
                ViewBag.Plantas = new SelectList(lstPlantas, "ClavePlanta", "planta");

                #region Combo Centro de Costos            
                ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
                #endregion

                return View("Create");
            }
        }
    }
}