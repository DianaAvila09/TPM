using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CatPmStdController : Controller
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
        static string pCtroCtos = "";
        static string pDepto = "";


        BL_CatPmStd BlCatPms = new BL_CatPmStd();
        BL_TPM tpm = new BL_TPM();
        BLSistemasEquipo BlSistEqui = new BLSistemasEquipo();
        BL_CatalogosSap BlCatSap = new BL_CatalogosSap();
        BL_CatTpm BlCatTpm = new BL_CatTpm();
        BL_Usuarios blUsu = new BL_Usuarios();

        #endregion

        public ActionResult CatPMStd()
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
                pCtroCtos = config.CtroCtosSap;
            }

            List<CatPMS> lstDatos = BlCatPms.DatosCatPMStd(cnxSqlMT, cCostos, pDepto);

            ViewBag.Message = "Catálogo de PM Standar, Centro de Costos: " + TituCC + "   Area: " + TituArea;
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

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);
            pDepto = config.Depto;
            pCtroCtos = config.CtroCtosSap;

            AltaPMStd pmStdNew = new AltaPMStd();

            pmStdNew.lstSistMantto = BlSistEqui.DatosCatSistManto(cnxSqlMT, pDepto);
            pmStdNew.lstEquipos = BlCatPms.GetEquipoPadres(cnxSqlMT, pCtroCtos);
            pmStdNew.lstCiclos = BlCatTpm.DatosCatCiclos(cnxSqlMT);
            pmStdNew.PmStd = new PmStandar();
            pmStdNew.PmStd.FecAlta = DateTime.Now;
            pmStdNew.PmStd.FecModif = DateTime.Now;
            pmStdNew.PmStd.UsuarioAlta = Session["UserId"].ToString();
            pmStdNew.PmStd.CodCiclo = "P";
            pmStdNew.PmStd.CodSistemas = pmStdNew.lstSistMantto[0].CodSistema;
            pmStdNew.PmStd.CentroCostos = pCtroCtos;

            ViewBag.Message = "Alta de PM Standar para un equipo";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return PartialView("_Nuevo", pmStdNew);
        }

        [HttpPost]
        public ActionResult Nuevo(AltaPMStd pmStdNew)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            ViewBag.Message = "Alta de PM Standar para un equipo";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            if (!ModelState.IsValid)
            {
                pmStdNew.lstSistMantto = BlSistEqui.DatosCatSistManto(cnxSqlMT, pDepto);
                pmStdNew.lstEquipos = BlCatPms.GetEquipoPadres(cnxSqlMT, pCtroCtos);
                pmStdNew.lstCiclos = BlCatTpm.DatosCatCiclos(cnxSqlMT);

                pmStdNew.PmStd.FecAlta = DateTime.Now;
                pmStdNew.PmStd.FecModif = DateTime.Now;
                pmStdNew.PmStd.UsuarioAlta = Session["UserId"].ToString();

                return PartialView("_Nuevo", pmStdNew);
            }

            if (BlCatPms.ValidarPmsEquipo(cnxSqlMT, pmStdNew.PmStd.CodEquipo, pmStdNew.PmStd.CentroCostos))
            {
                pmStdNew.lstSistMantto = BlSistEqui.DatosCatSistManto(cnxSqlMT, pDepto);
                pmStdNew.lstEquipos = BlCatPms.GetEquipoPadres(cnxSqlMT, pCtroCtos);
                pmStdNew.lstCiclos = BlCatTpm.DatosCatCiclos(cnxSqlMT);

                pmStdNew.PmStd.FecAlta = DateTime.Now;
                pmStdNew.PmStd.FecModif = DateTime.Now;
                pmStdNew.PmStd.UsuarioAlta = Session["UserId"].ToString();

                ModelState.AddModelError("PmStd.CodEquipo", "Ya Existe en el PM para este equipo");
                return PartialView("_Nuevo", pmStdNew);
            }

            pmStdNew.PmStd.FecModif = DateTime.Now;

            int result = BlCatPms.Guardar(cnxSqlMT, pmStdNew.PmStd);

            return Json(new { success = true });
        }


        [HttpGet]
        public ActionResult Editar(int Id)
        {

            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);
            pDepto = config.Depto;
            pCtroCtos = config.CtroCtosSap;

            AltaPMStd pmStdNew = new AltaPMStd();

            pmStdNew.lstSistMantto = BlSistEqui.DatosCatSistManto(cnxSqlMT, pDepto);
            pmStdNew.lstEquipos = BlCatPms.GetEquipoPadres(cnxSqlMT, pCtroCtos);
            pmStdNew.lstCiclos = BlCatTpm.DatosCatCiclos(cnxSqlMT);

            pmStdNew.PmStd = BlCatPms.DatosPms(cnxSqlMT, Id);

            ViewBag.Message = "Edición del PM Standar";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return PartialView("_Editar", pmStdNew);
        }

        [HttpPost]
        public ActionResult Editar(AltaPMStd pmStdNew)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            ViewBag.Message = "Alta de PM Standar para un equipo";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion


            if (!ModelState.IsValid)
            {
                pmStdNew.lstSistMantto = BlSistEqui.DatosCatSistManto(cnxSqlMT, pDepto);
                pmStdNew.lstEquipos = BlCatPms.GetEquipoPadres(cnxSqlMT, pCtroCtos);
                pmStdNew.lstCiclos = BlCatTpm.DatosCatCiclos(cnxSqlMT);

                pmStdNew.PmStd.FecModif = DateTime.Now;
                pmStdNew.PmStd.UsuarioAlta = Session["UserId"].ToString();

                return PartialView("_Editar", pmStdNew);
            }

            pmStdNew.PmStd.FecModif = DateTime.Now;

            int result = BlCatPms.Update(cnxSqlMT, pmStdNew.PmStd);

            return Json(new { success = true });
        }


        [HttpPost]
        public JsonResult GetWorkCenter(string pEquipo)
        {
            BL_CatTpm catlg = new BL_CatTpm();

            string vWorkCenter = catlg.DatosWcEquipo(cnxSqlMT, pEquipo);

            return Json(vWorkCenter);
        }



    }
}