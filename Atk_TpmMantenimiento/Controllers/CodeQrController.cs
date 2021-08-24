using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using System.Collections.Generic;
using System.Web.Mvc;
using Rotativa;
using System;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CodeQrController : Controller
    {
        #region variables de conexion
        static string SrvSql = Settings.Default.SrvSql;
        static string BD = Settings.Default.BD;
        static string UserSql = Settings.Default.UserSql;
        static string PwdSql = Settings.Default.PwdSql;
        static string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;
        static string cCentroCostos = null;
        static string cRutalog = "";
        static string cCostosSap = "";
        static string cWebServer = "";
        #endregion

        BL_TPM tpm = new BL_TPM();
        BLCatCodeQr blCodeQr = new BLCatCodeQr();
        BL_Usuarios blUsu = new BL_Usuarios();


        // GET: CodeQr
        //public ActionResult CatCodeQr()
        //{
        //   if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
        //   cCentroCostos = Session["costos"].ToString();

        //   // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
        //   DatosConfig config = new DatosConfig();
        //   config = tpm.LeeConfig(cnxSqlMT, cCentroCostos);
        //   cRutalog = config.RutaLog;
        //   cCostosSap = config.CtroCtosSap;

        //   List<CodeQr> lstCodeQr = new List<CodeQr>();

        //   ViewBag.Title = config.TituArea;
        //   ViewBag.TitleCC = config.TituCC;
        //   ViewBag.Message = "Administración de Code QR";
        //   return View();
        //}


        [HttpGet]
        public ActionResult Crear()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCentroCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCentroCostos);

            cRutalog = config.RutaLog;
            cCostosSap = config.CtroCtosSap;
            cWebServer = config.WebServer;

            AltaCodeQr cqr = new AltaCodeQr();
            CodeQr newCd = new CodeQr();
            newCd.Liga = "";

            cqr.lstEquPadres = blCodeQr.DatosEquipos(cnxSqlMT, cCentroCostos);
            cqr.lstTiposQr = blCodeQr.TiposQr(cnxSqlMT);
            cqr.CodigoQr = newCd;
            cqr.CodigoQr.WebServer = config.WebServer;

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Generación de códigos QR ";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCentroCostos);
            #endregion

            return View(cqr);
        }


        [HttpPost]
        public ActionResult Crear(AltaCodeQr datosQr)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCentroCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCentroCostos);
            cRutalog = config.RutaLog;
            cCostosSap = config.CtroCtosSap;

            datosQr.lstEquPadres = blCodeQr.DatosEquipos(cnxSqlMT, cCentroCostos);
            datosQr.lstTiposQr = blCodeQr.TiposQr(cnxSqlMT);
            datosQr.CodigoQr.WebServer = config.WebServer;
            datosQr.CodigoQr.CentroCostos = config.CtroCtosSap;

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Administración de Code QR";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCentroCostos);
            #endregion

            datosQr.CodigoQr = blCodeQr.GeneraCodeQr(cnxSqlMT, datosQr.CodigoQr, datosQr.lstTiposQr);

            return View(datosQr);
        }


        public ActionResult Imprimir(string id, string idTipo)
        {
            var archivo = "Cod|||||||||||||||||||||||||||||||||igo_" + id + ".pdf";
            var report = new Rotativa.ActionAsPdf("ImprimirQr", new { id, idTipo }) { FileName = archivo };
            return report;

        }
        public ActionResult ImprimirQr(string id, string idTipo, string ctroCtos)
        {


            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCentroCostos);
            cRutalog = config.RutaLog;
            cCostosSap = config.CtroCtosSap;

            List<TipoQr> lstTiposQr = blCodeQr.TiposQr(cnxSqlMT);

            CodeQr datosImp = new CodeQr();
            datosImp.CodEquipo = id;
            datosImp.Tipo = idTipo;
            datosImp.WebServer = config.WebServer;

            datosImp = blCodeQr.GeneraCodeQr(cnxSqlMT, datosImp, lstTiposQr);

            return View(datosImp);
        }

    }
}

