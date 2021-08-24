using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using ClosedXML.Excel;
using System.IO;
using System.Globalization;

namespace Atk_TpmMantenimiento.Controllers
{
    public class KpiTpmController : Controller
    {

        #region Variables para conexion a SQL
        static string SrvSql = Settings.Default.SrvSql;
        static string BD = Settings.Default.BD;
        static string UserSql = Settings.Default.UserSql;
        static string PwdSql = Settings.Default.PwdSql;
        static string cnxSqlMT = "Data Source=" + Settings.Default.SrvSql + ";Initial Catalog=" + Settings.Default.BD + ";User ID=" + Settings.Default.UserSql + ";Password=" + Settings.Default.PwdSql;
        static string cnxSqlRefec = "";
        #endregion

        #region variables 
        static string depto = "";
        static string rutaLog = "";
        static string tituArea = "";
        static string tituCC = "";
        static string tituCA = "";
        static int planta = 0;
        static string cCostos = "";
        static string cCostosSap = "";
        static string pDepto = "";
        static decimal metaCumpl = 0;
        static decimal metaEfic = 0;
        #endregion
        BL_TPM tpm = new BL_TPM();
        BL_KpiTpm bl_kpi = new BL_KpiTpm();
        BL_Reportes bl_rep = new BL_Reportes();
        BL_Usuarios blUsu = new BL_Usuarios();


        [HttpGet]
        public ActionResult KpiCumpli()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = "";
            if (cCostos != "")
                depto = config.Depto;

            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;

            cCostosSap = cCostos;

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "KPI: CUMPLIMIENTO, área: " + tituArea;
            ViewBag.Periodo = "";
            ViewBag.Top5Mes = "";

            ParamKpiTpm param = new ParamKpiTpm();

            param.AnioIni = DateTime.Now.Year;
            param.lstMesIni = bl_kpi.GetMeses(cnxSqlMT);
            param.AnioFin = DateTime.Now.Year;
            param.lstMesFin = bl_kpi.GetMeses(cnxSqlMT);
            param.lstGrafCumpl = null;
            ViewBag.datosx = null;
            ViewBag.datosCumpl = null;
            ViewBag.datosMeta = null;
            ViewBag.datosTrend = null;
            ViewBag.t5WC = null;
            ViewBag.t5Cerrados = null;
            ViewBag.Top5Mes = "";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(param);
        }

        [HttpPost]
        public ActionResult KpiCumpli(ParamKpiTpm param)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = "";
            if (cCostos != "")
            {
                depto = config.Depto;
                cCostosSap = config.CtroCtosSap;
                
            }
            rutaLog = config.RutaLog;
            tituArea = config.TituArea;;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            param.Depto = depto;

            param.Planta = config.Planta;
            param.MetaCumpl = config.MetaKbmCmpl;
            param.CtroCostos = cCostos;
           
            param.top5Anio = 0;
            param.top5Mes = 0;
            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            ViewBag.Message = "KPI: CUMPLIMIENTO, área: " + tituArea;
            ViewBag.Periodo = param.AnioIni.ToString() + '-' + param.MesIni.ToString().Trim().PadLeft(2, '0') + "  al  " + param.AnioFin.ToString() + '-' + param.MesFin.ToString().Trim().PadLeft(2, '0');
            ViewBag.Top5Mes = "";

            param.lstMesIni = bl_kpi.GetMeses(cnxSqlMT);
            param.lstMesFin = bl_kpi.GetMeses(cnxSqlMT);

            param.lstGrafCumpl = bl_kpi.GetGrafCumpl(cnxSqlMT, param);

            if (param.top5Anio != 0)
            {
                ViewBag.Top5Mes = param.top5Anio.ToString() + "-" + param.lstMesFin.Where(x => x.NumMes == param.top5Mes).Select(y => y.DescripEsp).FirstOrDefault();
                param.lstTop5Cumpl = bl_kpi.GetTop5Cumpli(cnxSqlMT, param);
            }

            //Preparamos datos para la grafica
            ViewBag.dx = param.lstGrafCumpl.Select(x => x.Periodo).ToArray();
            ViewBag.dc = param.lstGrafCumpl.Select(i => i.Cumplimiento).ToArray();
            ViewBag.dcm = param.lstGrafCumpl.Select(i => i.MetaCumpl).ToArray();
            ViewBag.dct = param.lstGrafCumpl.Select(i => i.Trend).ToArray();

            if (param.lstTop5Cumpl != null)
            {
                ViewBag.t5Wc = param.lstTop5Cumpl.Select(x => x.CodWorkCenter).ToArray();
                ViewBag.t5Cl = param.lstTop5Cumpl.Select(x => x.PorcAvance).ToArray();
            }
            return View(param);
        }

        [HttpGet]
        public ActionResult KpiEfic()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = "";
            if (cCostos != "")
            {
                depto = config.Depto;
                cCostosSap = config.CtroCtosSap;

            }

            
            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "KPI: EFICIENCIA , área: " + tituArea;
            ViewBag.Periodo = "";
            ViewBag.Top5Mes = "";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            ParamKpiTpm param = new ParamKpiTpm();

            param.AnioIni = DateTime.Now.Year;
            param.lstMesIni = bl_kpi.GetMeses(cnxSqlMT);
            param.AnioFin = DateTime.Now.Year;
            param.lstMesFin = bl_kpi.GetMeses(cnxSqlMT);
            param.lstGrafEfic = null;
            param.top5Anio = 0;
            param.top5Mes = 0;
            ViewBag.datosx = null;
            ViewBag.datosEfic = null;
            ViewBag.datosMeta = null;
            ViewBag.datosTrend = null;
            ViewBag.t5WC = null;
            ViewBag.t5Fallas = null;
            ViewBag.Top5Mes = "";

            return View(param);
        }

        [HttpPost]
        public ActionResult KpiEfic(ParamKpiTpm param)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = "";
            if (cCostos != "")
            {
                depto = config.Depto;
                cCostosSap = config.CtroCtosSap;
            }

            
            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            
            param.Planta = config.Planta;
            param.MetaEfic = config.MetaKbmEfic;
            param.CtroCostos = cCostos;
            param.Depto = depto;
            param.top5Anio = 0;
            param.top5Mes = 0;
            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "KPI: EFICIENCIA , área: " + tituArea;
            ViewBag.Periodo = param.AnioIni.ToString() + '-' + param.MesIni.ToString().Trim().PadLeft(2, '0') + "  al  " + param.AnioFin.ToString() + '-' + param.MesFin.ToString().Trim().PadLeft(2, '0');
            ViewBag.Top5Mes = "";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            param.lstMesIni = bl_kpi.GetMeses(cnxSqlMT);
            param.lstMesFin = bl_kpi.GetMeses(cnxSqlMT);

            param.lstGrafEfic = bl_kpi.GetGrafEfic(cnxSqlMT, param);

            if (param.top5Anio != 0)
            {
                ViewBag.Top5Mes = param.top5Anio.ToString() + "-" + param.lstMesFin.Where(x => x.NumMes == param.top5Mes).Select(y => y.DescripEsp).FirstOrDefault();
                param.lstTop5Efic = bl_kpi.GetTop5Efic(cnxSqlMT, param);
            }

            //Preparamos datos para la grafica
            ViewBag.dx = param.lstGrafEfic.Select(x => x.Periodo).ToArray();
            ViewBag.de = param.lstGrafEfic.Select(i => i.Eficiencia).ToArray();
            ViewBag.dem = param.lstGrafEfic.Select(i => i.MetaEfic).ToArray();
            ViewBag.det = param.lstGrafEfic.Select(i => i.Trend).ToArray();

            if (param.lstTop5Efic != null)
            {
                ViewBag.t5x = param.lstTop5Efic.Select(x => x.WorkCenter).ToArray();
                ViewBag.t5falla = param.lstTop5Efic.Select(x => x.NumFallas).ToArray();
            }

            return View(param);
        }


        [HttpGet]
        public ActionResult KpiTickets()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = "";
            if (cCostos != "")
            {
                depto = config.Depto;
                cCostosSap = config.CtroCtosSap;
            }

            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Análsis de tickets, area: " + tituArea;
            ViewBag.Periodo = "";
            ViewBag.Top5Mes = "";

            ParamKpiTpm param = new ParamKpiTpm();

            param.AnioIni = DateTime.Now.Year;
            param.lstMesIni = bl_kpi.GetMeses(cnxSqlMT);
            param.AnioFin = DateTime.Now.Year;
            param.lstMesFin = bl_kpi.GetMeses(cnxSqlMT);
            param.lstKpiTick = null;
            ViewBag.datosx = null;
            ViewBag.datosTick = null;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(param);
        }

        [HttpPost]
        public ActionResult KpiTickets(ParamKpiTpm param)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = "";
            if (cCostos != "")
            {
                depto = config.Depto;
                cCostosSap = config.CtroCtosSap;
            }


            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            
            param.Planta = config.Planta;
            param.CtroCostos = cCostos;
            param.Depto = depto;
            param.top5Anio = 0;
            param.top5Mes = 0;
            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Análsis de tickets, area: " + tituArea;
            ViewBag.Periodo = "Análisis de tickets, peridodo: " + param.AnioIni.ToString() + '-' + param.MesIni.ToString().Trim().PadLeft(2, '0') + "  al  " + param.AnioFin.ToString() + '-' + param.MesFin.ToString().Trim().PadLeft(2, '0');
            ViewBag.Top5Mes = "";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            param.lstMesIni = bl_kpi.GetMeses(cnxSqlMT);
            param.lstMesFin = bl_kpi.GetMeses(cnxSqlMT);

            param.lstKpiTick = bl_kpi.GetAnalisisTickets(cnxSqlMT, param);

            if (param.top5Anio != 0)
            {
                ViewBag.Top5Mes = param.top5Anio.ToString() + "-" + param.lstMesFin.Where(x => x.NumMes == param.top5Mes).Select(y => y.DescripEsp).FirstOrDefault();
                param.lstTop5WcTick = bl_kpi.GetTop5WcTivk(cnxSqlMT, param.Planta, param.Depto, param.CtroCostos, param.top5Anio, param.top5Mes);
            }

            //Preparamos datos para la grafica
            ViewBag.Meses = param.lstKpiTick.Select(x => x.Periodo).ToArray();
            ViewBag.Abiertos = param.lstKpiTick.Select(i => i.Abiertos).ToArray();
            ViewBag.Cerrados = param.lstKpiTick.Select(i => i.Cerrados).ToArray();
            ViewBag.Tendencia = param.lstKpiTick.Select(i => i.Trend).ToArray();
            ViewBag.Vacio = param.lstKpiTick.Select(i => i.Vacio).ToArray();

            if (param.lstTop5WcTick != null && param.lstTop5WcTick.Count > 0)
            {
                ViewBag.t5WC = param.lstTop5WcTick.Select(x => x.CodWorkCenter).ToArray();
                ViewBag.t5Abiertos = param.lstTop5WcTick.Select(x => x.Abiertos).ToArray();
                ViewBag.t5Cerrados = param.lstTop5WcTick.Select(x => x.Cerrados).ToArray();
                ViewBag.t5Vacio = param.lstTop5WcTick.Select(i => i.Vacio).ToArray();
            }

            return View(param);
        }

        [HttpGet]
        public ActionResult KpiTm()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = "";
            if (cCostos != "")
            {
                depto = config.Depto;
                cCostosSap = config.CtroCtosSap;
            }

            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "KPI: Tiempo Promedio para Reparar (MTTR) y Tiempo Promedio Entre Fallas (MTBF), área:  " + tituArea;
            ViewBag.Periodo = "";
            ViewBag.Top5Mes = "";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            ParamKpiTpm param = new ParamKpiTpm();

            //********** Metas por area ********//

            param.MetaMtrrAuto = config.MetaMtrrAutoMin;
            param.MetaMtbfAuto = config.MetaMtbfAutoMin;
            param.MetaMtrrMnt = config.MetaMtrrMntMin;
            param.MetaMtbfMnt = config.MetaMtbfMntMin;
            param.MetaMtrrTkl = config.MetaMtrrTklMin;
            param.MetaMtbfTkl = config.MetaMtbfTklMin;

            param.AnioIni = DateTime.Now.Year;
            param.lstMesIni = bl_kpi.GetMeses(cnxSqlMT);
            param.AnioFin = DateTime.Now.Year;
            param.lstMesFin = bl_kpi.GetMeses(cnxSqlMT);
            param.lstEqPadres = bl_kpi.GetEqyWc(cnxSqlMT, cCostos);
            param.lstFocus = bl_kpi.GetFocus(param.lstEqPadres);
            param.lstKpiMt = new List<KpiTm>();
            param.tipoRep = 1;

            param.lstKpiTick = null;
            ViewBag.datosx = null;
            ViewBag.datosTick = null;
            return View(param);
        }

        [HttpPost]
        public ActionResult KpiTm(ParamKpiTpm param)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = "";
            if (cCostos != "")
            {
                depto = config.Depto;
                cCostosSap = config.CtroCtosSap;
            }


            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;            
            param.Planta = config.Planta;
            param.CtroCostos = cCostos;
            param.Depto = depto;

            //********** Metas por area ********//
            param.MetaMtrrAuto = config.MetaMtrrAutoMin;
            param.MetaMtbfAuto = config.MetaMtbfAutoMin;
            param.MetaMtrrMnt = config.MetaMtrrMntMin;
            param.MetaMtbfMnt = config.MetaMtbfMntMin;
            param.MetaMtrrTkl = config.MetaMtrrTklMin;
            param.MetaMtbfTkl = config.MetaMtbfTklMin;

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion


            ViewBag.Message = "KPI: Tiempo Promedio para Reparar (MTTR) y Tiempo Promedio Entre Fallas (MTBF), área de:   " + tituArea;
            //param.tipoRep = 1;
            switch (param.tipoRep)
            {
                case 1:
                    ViewBag.Periodo = " Area de: " + tituArea + ", Peridodo: " + param.AnioIni.ToString() + '-' + param.MesIni.ToString().Trim().PadLeft(2, '0') + "  al  " + param.AnioFin.ToString() + '-' + param.MesFin.ToString().Trim().PadLeft(2, '0');
                    break;
                case 2:
                    ViewBag.Periodo = " Work Center : " + param.WorkCenter + ", Peridodo: " + param.AnioIni.ToString() + '-' + param.MesIni.ToString().Trim().PadLeft(2, '0') + "  al  " + param.AnioFin.ToString() + '-' + param.MesFin.ToString().Trim().PadLeft(2, '0');
                    break;
                case 3:
                    ViewBag.Periodo = " Focus Factory :  " + param.FocusFactory + ", Peridodo: " + param.AnioIni.ToString() + '-' + param.MesIni.ToString().Trim().PadLeft(2, '0') + "  al  " + param.AnioFin.ToString() + '-' + param.MesFin.ToString().Trim().PadLeft(2, '0');
                    break;
            }
            // ViewBag.Message = "KPI: Tiempo Promedio para Reparar (MTTR) y Tiempo Promedio Entre Fallas (MTBF), área de:   " + tituArea;
            // ViewBag.Periodo = " Area de: " + tituArea +", Peridodo: " + param.AnioIni.ToString() + '-' + param.MesIni.ToString().Trim().PadLeft(2, '0') + "  al  " + param.AnioFin.ToString() + '-' + param.MesFin.ToString().Trim().PadLeft(2, '0');

            param.lstMesIni = bl_kpi.GetMeses(cnxSqlMT);
            param.lstMesFin = bl_kpi.GetMeses(cnxSqlMT);
            param.lstEqPadres = bl_kpi.GetEqyWc(cnxSqlMT, cCostos);
            param.lstFocus = bl_kpi.GetFocus(param.lstEqPadres);

            param.lstKpiMt = bl_kpi.GetKpiMt(cnxSqlMT, param);
            Session["DatosCalculo"] = param.lstKpiMt;

            ////Preparamos datos para la grafica
            ViewBag.Meses = param.lstKpiMt.Select(x => x.Periodo).ToArray();
            ViewBag.MttrAuto = param.lstKpiMt.Select(i => i.MttrAuto).ToArray();
            ViewBag.MetaMttrAuto = param.lstKpiMt.Select(i => i.MetaMtrrAuto).ToArray();
            ViewBag.MtbfAuto = param.lstKpiMt.Select(i => i.MtbfAuto).ToArray();
            ViewBag.MetaMtbfAuto = param.lstKpiMt.Select(i => i.MetaMtbfAuto).ToArray();

            ViewBag.MttrMnt = param.lstKpiMt.Select(i => i.MttrManto).ToArray();
            ViewBag.MetaMttrMnt = param.lstKpiMt.Select(i => i.MetaMtrrMnt).ToArray();
            ViewBag.MtbfMnt = param.lstKpiMt.Select(i => i.MtbfManto).ToArray();
            ViewBag.MetaMtbfMnt = param.lstKpiMt.Select(i => i.MetaMtbfMnt).ToArray();

            ViewBag.MttrTkl = param.lstKpiMt.Select(i => i.MttrTroquel).ToArray();
            ViewBag.MetaMttrTkl = param.lstKpiMt.Select(i => i.MetaMtrrTkl).ToArray();
            ViewBag.MtbfTkl = param.lstKpiMt.Select(i => i.MtbfTroquel).ToArray();
            ViewBag.MetaMtbfTkl = param.lstKpiMt.Select(i => i.MetaMtbfTkl).ToArray();

            return View(param);
        }

        [HttpGet]
        public ActionResult KpiPkyk()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = config.Depto;
            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            cCostosSap = config.CtroCtosSap;

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "KPI: Análisis de Sensores/Pokayokes, área: " + tituArea;
            ViewBag.Bita = "Bitácora de Análisis de Sensores/Pokayokes, área: " + tituArea;
            ViewBag.Periodo = "";
            ViewBag.PeriodoHt = "";
            ViewBag.Top5Mes = "";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            ParamKpiPkyk param = new ParamKpiPkyk();

            param.AnioIni = DateTime.Now.AddMonths(-5).Year;
            param.AnioFin = DateTime.Now.Year;
            param.MesIni = DateTime.Now.AddMonths(-5).Month;
            param.MesFin = DateTime.Now.Month;
            param.lstMesIni = bl_kpi.GetMeses(cnxSqlMT);
            param.lstMesFin = bl_kpi.GetMeses(cnxSqlMT);
            param.Pareto = "ACTU";

            // Bitacora
            param.bitacora = new SensorBitacora();
            param.bitacora.FchInicio = DateTime.Now.AddDays(-5);
            param.bitacora.FchFin = DateTime.Now;
            param.bitacora.WorkCenter = "";
            param.bitacora.Usuario = "";
            param.bitacora.Estatus = "";
            param.bitacora.Sensor = "";
            param.bitacora.lstBitacora = new List<SensorActividad>();
            //            param.bitacora.lstBitacora.Add(new SensorActividad() { });
            param.bitacora.lstEstatus = bl_kpi.GetCatSts(cnxSqlMT);
            param.bitacora.lstWc = bl_kpi.GetCatWc(cnxSqlMT, "0007", cCostos);

            ViewBag.datosx = null;
            ViewBag.datosEfic = null;
            ViewBag.datosMeta = null;
            ViewBag.datosTrend = null;
            ViewBag.t5WC = null;
            ViewBag.t5Fallas = null;
            ViewBag.Top5Mes = "";

            return View(param);
        }

        [HttpPost]
        public ActionResult KpiPkyk(ParamKpiPkyk param)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = config.Depto;
            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            cCostosSap = config.CtroCtosSap;

            param.Planta = config.Planta;

            param.CtroCostos = cCostos;
            param.Depto = config.Depto;
            param.MesAtras = config.PkykMesesAtrasHT;
            param.TipoTick = "P";
            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "KPI: Análisis de Sensores/Pokayokes, área: " + tituArea;
            ViewBag.PeriodoHt = "Análisis de Sensores/Pokayokes, histórico hasta el " + param.AnioFin.ToString() + '-' + param.MesFin.ToString().Trim().PadLeft(2, '0');
            ViewBag.Periodo = "Análisis de Sensores, Periodo" + param.AnioIni.ToString() + '-' + param.MesIni.ToString().Trim().PadLeft(2, '0') +
                " al " + param.AnioFin.ToString() + '-' + param.MesFin.ToString().Trim().PadLeft(2, '0');
            ViewBag.Top5Mes = "";


            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            // Bitacora
            param.bitacora = new SensorBitacora();
            param.bitacora.FchInicio = DateTime.Now.AddDays(-5);
            param.bitacora.FchFin = DateTime.Now;
            param.bitacora.WorkCenter = "";
            param.bitacora.Usuario = "";
            param.bitacora.Estatus = "";
            param.bitacora.Sensor = "";
            param.bitacora.lstBitacora = new List<SensorActividad>();
            //            param.bitacora.lstBitacora.Add(new SensorActividad() { });
            param.bitacora.lstEstatus = bl_kpi.GetCatSts(cnxSqlMT);
            param.bitacora.lstWc = bl_kpi.GetCatWc(cnxSqlMT, "0007", cCostos);

            param.lstMesIni = bl_kpi.GetMeses(cnxSqlMT);
            param.lstMesFin = bl_kpi.GetMeses(cnxSqlMT);

            param.lstGrafpkyk = bl_kpi.GetGrafPkykHt(cnxSqlMT, param);
            param.lstpkykxMes = bl_kpi.GetGrafPkykMes(cnxSqlMT, param);

            if (param.Top5Anio != 0)
            {
                if (param.Pareto == "ACTU")
                {
                    ViewBag.Top5Mes = param.Top5Anio.ToString() + "-" + param.lstMesFin.Where(x => x.NumMes == param.Top5Mes).Select(y => y.DescripEsp).FirstOrDefault();
                }
                else
                {
                    ViewBag.Top5Mes = param.Top5Anio.ToString() + "-" +
                        param.lstMesFin.Where(x => x.NumMes == param.Top5Mes).Select(y => y.DescripEsp).FirstOrDefault() +
                        " ultimos 30 dias";
                }
                param.lstTop5pkyk = bl_kpi.GetTop5PkykMes(cnxSqlMT, param);
            }

            // datos de la grafica de meses

            ViewBag.dxMes = param.lstpkykxMes.Select(x => x.Periodo).ToArray();
            ViewBag.dMesEventos = param.lstpkykxMes.Select(x => x.Eventos).ToArray();
            ViewBag.dMesTenden = param.lstpkykxMes.Select(x => x.Tendencia).ToArray();

            //Preparamos datos para la grafica
            ViewBag.dx = param.lstGrafpkyk.Select(x => x.Sensor + "/" + x.CodWorkCenter).ToArray();
            ViewBag.dHt = param.lstGrafpkyk.Select(x => x.EventosHist).ToArray();
            ViewBag.dMs = param.lstGrafpkyk.Select(x => x.EventosMes).ToArray();


            if (param.lstTop5pkyk != null)
            {
                ViewBag.t5x = param.lstTop5pkyk.Select(x => x.Sensor + "/" + x.CodWorkCenter).ToArray();
                ViewBag.t5tick = param.lstTop5pkyk.Select(x => x.EventosHist).ToArray();
            }

            return View(param);
        }

        [HttpGet]
        public ActionResult KpiCheckList()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            cnxSqlRefec = "Data Source=" + config.SrvSqlEmpl.Trim() + ";Initial Catalog=" + config.BdEmpl.Trim() + "; User ID=" + config.UserEmpl.Trim() + "; Password=" + config.PwdEmpl.Trim();

            depto = "";
            if (cCostos != "")
            {
                depto = config.Depto;
                cCostosSap = config.CtroCtosSap;
            }


            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            rutaLog = config.RutaLog;
            

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "KPI's de Checklist";
            ViewBag.Periodo = "";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            ParamKpiChklst param = new ParamKpiChklst();
            param.AnioIni = DateTime.Now.Year;
            param.AnioFin = DateTime.Now.Year;

            param.lstMesIni = bl_kpi.GetMesesNew();
            param.lstMesFin = bl_kpi.GetMesesNew();
            param.AnioIni = DateTime.Now.Year;
            param.AnioFin = DateTime.Now.Year;
            param.lstEqPadres = bl_kpi.GetEquiPadres(cnxSqlMT, cCostos);
            param.CodEquipo = "Todos";
            param.lstGraf = null;
            return View(param);
        }

        [HttpPost]
        public ActionResult KpiCheckList(ParamKpiChklst param)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            cnxSqlRefec = "Data Source=" + config.SrvSqlEmpl.Trim() + ";Initial Catalog=" + config.BdEmpl.Trim() + "; User ID=" + config.UserEmpl.Trim() + "; Password=" + config.PwdEmpl.Trim();

            depto = "";
            if (cCostos != "")
            {
                depto = config.Depto;
                cCostosSap = config.CtroCtosSap;
            }


            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            rutaLog = config.RutaLog;
            

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "KPI's de Checklist";
            ViewBag.Periodo = param.AnioIni + "-" + param.MesIni + " al " + param.AnioFin + "-" + param.MesFin.ToString().PadLeft(2, '0');
            param.CtroCostos = cCostos;
            param.Depto = depto;
            param.lstMesIni = bl_kpi.GetMesesNew();
            param.lstMesFin = bl_kpi.GetMesesNew();
            param.lstGraf = new List<GraficakpiChk>();
            param.lstEqPadres = bl_kpi.GetEquiPadres(cnxSqlMT, cCostos);
            param.lstChecklist = new List<CheckListEqEnc>();

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            // Datos calulados para la grafica
            param.lstGraf = bl_kpi.GetInfoChk(cnxSqlMT, param);
            //Preparamos datos para la grafica
            string[] vTipo = { "Periodo", "Puntuales", "Retardados", "Pendientes" };
            ViewBag.datosx = vTipo;
            ViewBag.datosy = param.lstGraf.Select(i => new { i.Periodo, Puntuales = i.OnTime, Retardados = i.Delay, Pendientes = i.NoExecuted }).ToArray();
            return View(param);

        }
        public ActionResult ToExcel(string pPeri)
        {

            List<KpiTm> datos = (List<KpiTm>)Session["DatosCalculo"];
            DataTable dt = new DataTable();

            dt.Locale = CultureInfo.CurrentCulture;
            var columns = new[]
                {
            new DataColumn("Periodo", typeof(string)),
            new DataColumn("Anio", typeof(int)),
            new DataColumn("Mes", typeof(int)),
            new DataColumn("MinAutomat", typeof(decimal)),
            new DataColumn("MinMantto", typeof(decimal)),
            new DataColumn("MinProgramado", typeof(decimal)),
            new DataColumn("MinTroqueles", typeof(decimal)),
            new DataColumn("MinParoProd", typeof(decimal)),
            new DataColumn("MinLogistica", typeof(decimal)),
            new DataColumn("MinError", typeof(decimal)),
            new DataColumn("MinCorriendo", typeof(decimal)),
            new DataColumn("MinCalidad", typeof(decimal)),
            new DataColumn("TotalMinutos", typeof(decimal)),
            new DataColumn("EventosAutoma", typeof(int)),
            new DataColumn("EventosMantto", typeof(int)),
            new DataColumn("EventosTroqueles", typeof(int)),
            new DataColumn("MttrAuto", typeof(decimal)),
            new DataColumn("MtbfAuto", typeof(decimal)),
            new DataColumn("MttrManto", typeof(decimal)),
            new DataColumn("MtbfManto", typeof(decimal)),
            new DataColumn("MttrTroquel", typeof(decimal)),
            new DataColumn("MtbfTroquel", typeof(decimal)),
            new DataColumn("MetaMtrrAuto", typeof(decimal)),
            new DataColumn("MetaMtbfAuto", typeof(decimal)),
            new DataColumn("MetaMtrrMnt", typeof(decimal)),
            new DataColumn("MetaMtbfMnt", typeof(decimal)),
            new DataColumn("MetaMtrrTkl", typeof(decimal)),
            new DataColumn("MetaMtbfTkl", typeof(decimal))
        };

            dt.Columns.AddRange(columns);
            foreach (var x in datos)
            {
                var row = dt.NewRow();

                row["Periodo"] = x.Periodo;
                row["Anio"] = x.Anio;
                row["Mes"] = x.Mes;
                row["MinAutomat"] = x.MinAutomat;
                row["MinMantto"] = x.MinMantto;
                row["MinProgramado"] = x.MinProgramado;
                row["MinTroqueles"] = x.MinTroqueles;
                row["MinParoProd"] = x.MinParoProd;
                row["MinLogistica"] = x.MinLogistica;
                row["MinError"] = x.MinError;
                row["MinCorriendo"] = x.MinCorriendo;
                row["MinCalidad"] = x.MinCalidad;
                row["TotalMinutos"] = x.TotalMinutos;
                row["EventosAutoma"] = x.EventosAutoma;
                row["EventosMantto"] = x.EventosMantto;
                row["EventosTroqueles"] = x.EventosTroqueles;
                row["MttrAuto"] = x.MttrAuto;
                row["MtbfAuto"] = x.MtbfAuto;
                row["MttrManto"] = x.MttrManto;
                row["MtbfManto"] = x.MtbfManto;
                row["MttrTroquel"] = x.MttrTroquel;
                row["MtbfTroquel"] = x.MtbfTroquel;
                row["MetaMtrrAuto"] = x.MetaMtrrAuto;
                row["MetaMtbfAuto"] = x.MetaMtbfAuto;
                row["MetaMtrrMnt"] = x.MetaMtrrMnt;
                row["MetaMtbfMnt"] = x.MetaMtbfMnt;
                row["MetaMtrrTkl"] = x.MetaMtrrTkl;
                row["MetaMtbfTkl"] = x.MetaMtbfTkl;

                dt.Rows.Add(row);
            }

            dt.TableName = "MT";
            EnviarXls(dt, pPeri);

            return RedirectToAction("KpiTm");
        }

        private void EnviarXls(DataTable dt, string titulo)
        {
            string nomFile = "Mttr_Mtfb_" + DateTime.Now.Date + ".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {

                var ws = wb.Worksheets.Add(dt);
                ws.Row(1).InsertRowsAbove(1);
                ws.Row(1).InsertRowsAbove(1);
                // Hardcode title and contents locations
                IXLCell titleCell = ws.Cell(1, 1);
                IXLCell contentsCell = ws.Cell(3, 2);

                //Pretty-up the title
                titleCell.Value = "MTTR y MTFB del periodo " + titulo;
                titleCell.Style.Font.Bold = true;

                titleCell.Style.Font.FontColor = XLColor.White;
                titleCell.Style.Font.FontSize = 14;
                titleCell.Style.Fill.BackgroundColor = XLColor.Brown;

                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                // Merge cells for title
                ws.Range(titleCell, ws.Cell(2, dt.Columns.Count + 1)).Merge();

                // Insert table contents, and adjust for content width
                // contentsCell.InsertTable(dt);

                ws.Columns().AdjustToContents();
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                wb.ShowGridLines = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + nomFile);

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

    }
}