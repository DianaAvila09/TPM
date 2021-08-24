using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.Linq;

namespace Atk_TpmMantenimiento.Controllers
{

    public class ReportesController : Controller
    {

        #region Variables para conexion a SQL
        static string SrvSql = Settings.Default.SrvSql;
        static string BD = Settings.Default.BD;
        static string UserSql = Settings.Default.UserSql;
        static string PwdSql = Settings.Default.PwdSql;
        static string cnxSqlMT = "Data Source=" + Settings.Default.SrvSql + ";Initial Catalog=" + Settings.Default.BD + ";User ID=" + Settings.Default.UserSql + ";Password=" + Settings.Default.PwdSql;
        #endregion


        #region variables 
        static DatosCnxSap cnxSap = new DatosCnxSap();
        static string cnxSqlRefec = "";
        static string depto = "";
        static string rutaLog = "";
        static string tituArea = "";
        static string tituCC = "";
        static string tituCA = "";
        static int pPlanta = 0;
        static string cCostosSap = "";
        static string pDepto = "";
        static string cCostos = "";
        #endregion
        BL_TPM tpm = new BL_TPM();
        BL_Usuarios blUsu = new BL_Usuarios();
        BL_Reportes bl_rep = new BL_Reportes();
        public static List<Ticket> lstTick = new List<Ticket>();


        [HttpGet]
        public ActionResult ReportesTicket()
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
            
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            pPlanta = config.Planta;
            rutaLog = config.RutaLog;


            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Reportes de Tickets";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            // definicion de parametros
            ParamRepTickets filtros = new ParamRepTickets();
            List<Ticket> lstTick = new List<Ticket>();
            Ticket tic = new Ticket();
            tic.IdTicket = 0;
            tic.Planta = pPlanta.ToString();
            filtros.LstTickets = new List<Ticket>();
            filtros.LstTickets.Add(tic);

            filtros.FecInicial = DateTime.Now.AddDays(-2);
            filtros.FecFinal = DateTime.Now.Date;
            filtros.Estatus = "TODO";
            filtros.Hallazgo = "AMBOS";
            filtros.CausoParo = "AMBOS";
            return View(filtros);
        }

        [HttpPost]
        public ActionResult ReportesTicket(ParamRepTickets filtros)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = "";
            if (cCostos != "")
            {
                depto = config.Depto;
                cCostosSap = config.CtroCtosSap;
            }

            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.Message = "Reportes de Tickets";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            filtros.LstTickets = bl_rep.GetRepTickets(filtros, cnxSqlMT, pPlanta, cCostos, depto, rutaLog);

            return View(filtros);
        }

        public ActionResult ToExcel(string pParo, string pHazllgo, string pDpto, DateTime pFi, DateTime pFf, string pStus)
        {
            ParamRepTickets pFiltros = new ParamRepTickets();
            pFiltros.CausoParo = pParo;
            pFiltros.Depto = pDpto;
            pFiltros.Estatus = pStus;
            pFiltros.FecInicial = pFi;
            pFiltros.FecFinal = pFf;
            pFiltros.Hallazgo = pHazllgo;

            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            DataTable dt = new DataTable();

            dt = bl_rep.GetRepTickdt(pFiltros, cnxSqlMT, pPlanta, cCostos, depto, rutaLog);
            dt.TableName = "tickets";
            EnviarXls(dt, pFiltros);

            return RedirectToAction("ReportesTicket");
        }

        private void EnviarXls(DataTable dt, ParamRepTickets prm)
        {
            string nomFile = "Ticket_" + DateTime.Now.Date + ".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {

                var ws = wb.Worksheets.Add(dt);
                ws.Row(1).InsertRowsAbove(1);
                ws.Row(1).InsertRowsAbove(1);
                // Hardcode title and contents locations
                IXLCell titleCell = ws.Cell(1, 1);
                IXLCell contentsCell = ws.Cell(3, 2);

                //Pretty-up the title
                titleCell.Value = "Reporte de tickets del " + prm.FecInicial.ToShortDateString() + " al " + prm.FecFinal.ToShortDateString();
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

        [HttpGet]
        public ActionResult ReporteCheckList()
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
            pPlanta = config.Planta;
            

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Consulta de Checklist de equipos";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            ParamRepChecklist param = new ParamRepChecklist();
            param.FecInicial = DateTime.Now;
            param.FecFinal = DateTime.Now;
            param.lstChecklist = new List<CheckListEqEnc>();
            return View(param);
        }

        [HttpPost]
        public ActionResult ReporteCheckList(ParamRepChecklist param)
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
            pPlanta = config.Planta;            
            pDepto = depto;

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Consulta de Checklist de equipos";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            param.lstChecklist = bl_rep.GetCapchklist(cnxSqlMT, param.FecInicial, param.FecFinal, pDepto);

            return View(param);
        }

        public ActionResult Checklist(int Id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

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
            pPlanta = config.Planta;
            pDepto = depto;

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Resultado de Checklist ";
            ViewBag.SumPonder = 0;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            CapturaChklst chkxEq = new CapturaChklst();

            chkxEq.ChklsxEq = bl_rep.GetDatosChkEncb(cnxSqlMT, Id, pDepto, pPlanta);
            chkxEq.lstChckActEq = bl_rep.GetDatosChkAct(cnxSqlMT, Id, pDepto);

            ViewBag.SumPonder = chkxEq.lstChckActEq.Sum(x => x.Ponderacion);

            return View(chkxEq);
        }


        [HttpGet]
        public ActionResult RepAtenciontickets()
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
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            pPlanta = config.Planta;
            rutaLog = config.RutaLog;
            

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Reportes de tiempos de respuesta de Tickets";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            // definicion de parametros
            ParamRepAtnTickets filtros = new ParamRepAtnTickets();
            List<RespuestaTick> lstTick = new List<RespuestaTick>();
            RespuestaTick rep = new RespuestaTick();
            rep.Anio = 0;
            rep.Mes = 0;

            filtros.LstRepAtnTick = new List<RespuestaTick>();
            filtros.LstRepAtnTick.Add(rep);

            filtros.FecI = DateTime.Now.AddDays(-60);
            filtros.FecF = DateTime.Now.Date;

            return View(filtros);
        }

        [HttpPost]
        public ActionResult RepAtenciontickets(ParamRepAtnTickets param)
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
            pPlanta = config.Planta;
            rutaLog = config.RutaLog;
            pDepto = depto;

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Reportes de tiempos de respuesta de Tickets";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            param.LstRepAtnTick = bl_rep.GetRepAtnTick(cnxSqlMT, param.FecI, param.FecF, pPlanta, pDepto).OrderByDescending(x => x.MinRepCierre).ToList();


            return View(param);
        }

    }
}