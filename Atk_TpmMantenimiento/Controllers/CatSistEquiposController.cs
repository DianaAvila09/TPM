using BusinessLogic;
using Entidades;
using System.Collections.Generic;
using System.Web.Mvc;
using Atk_TpmMantenimiento.Properties;
using System;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CatSistEquiposController : Controller
    {
        #region variables de  conexion 
        DatosCnxSap cnxSap = new DatosCnxSap()
        {
            Host = Settings.Default.Host,
            SystemID = Settings.Default.SystemID,
            SystemNumber = Settings.Default.SystemNumber,
            Client = Settings.Default.Client,
            Language = Settings.Default.Language,
            PoolSize = Settings.Default.PoolSize,
            UserPRD = Settings.Default.UserPRD,
            PwdPRD = Settings.Default.PwdPRD
        };

        static string SrvSql = Settings.Default.SrvSql;
        static string BD = Settings.Default.BD;
        static string UserSql = Settings.Default.UserSql;
        static string PwdSql = Settings.Default.PwdSql;
        static string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

        static string TituArea = "";
        static string TituCC = "";
        //static string TituCA = "";
        static string rutalog = "";
        static string cCostos = "";
        static string Depto = "";

        BL_TPM tpm = new BL_TPM();
        BLSistemasEquipo BlSm = new BLSistemasEquipo();
        BLCatDeptos BlDepto = new BLCatDeptos();
        BL_Usuarios blUsu = new BL_Usuarios();

        #endregion

        // GET: CatSistEquipos
        public ActionResult CatSistEquipos()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            Depto = "";
            if (cCostos != "")
                Depto = config.Depto;

            List<SistemaManto> lstSm = new List<SistemaManto>();

            lstSm = BlSm.DatosCatSistManto(cnxSqlMT, Depto);
            ViewBag.Editar = false;
            TituArea = config.TituArea;
            TituCC = config.TituCC;

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;

            ViewBag.Message = "Catálogo de Sistemas de Mantenimiento";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(lstSm);
        }

        [HttpGet]
        public ActionResult Nuevo()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            AltaSistManto newSistManto = new AltaSistManto();
            newSistManto.SistManto = new SistemaManto() { CodSistema = "", Sistema = "", CodDepartamento = "", Estatus = false };
            newSistManto.lstDeptos = BlDepto.DatosCatalogo(cnxSqlMT, cCostos);

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            ViewBag.Message = "Alta de Sistema para Equipos";
            return PartialView("_Nuevo", newSistManto);

        }

        // POST: CatSistEquipos/Create
        [HttpPost]
        public ActionResult Nuevo(AltaSistManto smNew)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            if (!ModelState.IsValid)
            {
                smNew.lstDeptos = BlDepto.DatosCatalogo(cnxSqlMT, cCostos);

                ViewBag.Title = TituArea;
                ViewBag.TitleCC = TituCC;

                ViewBag.Message = "Alta de Sistema para Equipos";

                #region Combo Centro de Costos            
                ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
                #endregion

                //return View(smNew);
                System.Threading.Thread.Sleep(1000);
                return PartialView("_Nuevo", smNew);
            }

            BlSm.Guardar(cnxSqlMT, smNew.SistManto, rutalog);
            System.Threading.Thread.Sleep(1000);
            return Json(new { success = true });
            //return PartialView("_Nuevo", smNew);

        }


        // GET: CatSistEquipos/Edit
        public ActionResult Editar(int Id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            AltaSistManto newSistManto = new AltaSistManto();
            newSistManto.SistManto = BlSm.DatosSistMantto(cnxSqlMT, Id, rutalog);
            newSistManto.lstDeptos = BlDepto.DatosCatalogo(cnxSqlMT, cCostos);

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición de Sistema para Equipos";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return PartialView("_Editar", newSistManto);
        }

        // POST: CatSistEquipos/Edit
        [HttpPost]
        public ActionResult Editar(AltaSistManto editSistManto)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            int result = 0;
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            if (!ModelState.IsValid)
            {
                editSistManto.lstDeptos = BlDepto.DatosCatalogo(cnxSqlMT, cCostos);
                return PartialView("_Editar", editSistManto);
            }
            result = BlSm.Update(cnxSqlMT, editSistManto.SistManto, rutalog);
            return Json(new { success = true });

        }

    }
}
