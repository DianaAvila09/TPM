using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atk_TpmMantenimiento.Controllers
{
    public class TicketsController : Controller
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
        static int planta = 0;
        static string cCostos = "";
        

        DatosCorreo datosEmail = new DatosCorreo();
        BL_AdmonTickets bl_tickets = new BL_AdmonTickets();
        BL_TPM tpm = new BL_TPM();
        BL_Usuarios blUsu = new BL_Usuarios();

        #endregion

        public ActionResult ListaTickets(string CodEq, bool statusTick, bool showError, string pCtroCostos)
        {
            if (Session["costos"] == null && pCtroCostos == "")
            {
                return RedirectToAction("FinSesion", "Usuarios");
            }

            if (pCtroCostos != null)
            {
                Session["costos"] = pCtroCostos;
                cCostos = pCtroCostos;
            }
            else
               if (Session["costos"] != null)
            {
                cCostos = Session["costos"].ToString();
            }
            else
            {
                return RedirectToAction("FinSesion", "Usuarios");
            }

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            cnxSqlRefec = "Data Source=" + config.SrvSqlEmpl.Trim() + ";Initial Catalog=" + config.BdEmpl.Trim() + "; User ID=" + config.UserEmpl.Trim() + "; Password=" + config.PwdEmpl.Trim();
            depto = config.Depto;
            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            rutaLog = config.RutaLog;
            cCostos = config.CtroCtosSap;

            ListaTickets DatosTickets = new ListaTickets();

            List<Ticket> lstTick = new List<Ticket>();

            CatEquipo Equi = new CatEquipo();
            BL_CatalogosSap blCatSap = new BL_CatalogosSap();

            Equi = blCatSap.GetDatosEquipo(cnxSqlMT, CodEq);

            ViewBag.DatosEqui = Equi;
            ViewBag.statusTicket = statusTick;
            ViewBag.CentroCostos = cCostos;

            DatosTickets.lstTickets = bl_tickets.GetListaTickxEqu(cnxSqlMT, Equi.CodEquipo, Equi.WorkCenter, ViewBag.statusTicket, depto, rutaLog, cCostos);
            DatosTickets.filtro = (statusTick ? "NO" : "SI");

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Tickets";
            ViewBag.ErrorLogin = showError;
            ViewBag.ErrorTitu = "NO EXISTE USUARIO AUTENTIFICADO";
            ViewBag.ErrorMsj = " Necesita estar autentificado para poder hacer modificaciones a los tickets.." +
                         "Si no cuenta con un usuario, por favor solicitelo al administrador del TPM de Mantenimiento";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(DatosTickets);
        }

        [HttpGet]
        public ActionResult Crear(string codWc, string codEqui, string equipo, string tipoTick, string pCtroCostos)
        {
            if (Session["costos"] == null && pCtroCostos == "")
            {
                return RedirectToAction("FinSesion", "Usuarios");
            }

            if (pCtroCostos != null)
            {
                Session["costos"] = pCtroCostos;
                cCostos = pCtroCostos;
            }
            else
            if (Session["costos"] != null)
            {
                cCostos = Session["costos"].ToString();
            }
            else
            {
                return RedirectToAction("FinSesion", "Usuarios");
            }

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            cnxSqlRefec = "Data Source=" + config.SrvSqlEmpl.Trim() + ";Initial Catalog=" + config.BdEmpl.Trim() + "; User ID=" + config.UserEmpl.Trim() + "; Password=" + config.PwdEmpl.Trim();
            depto = config.Depto;
            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            planta = config.Planta;
            rutaLog = config.RutaLog;
            //cCostos = config.CtroCtosSap;



            AltaTicket tickNew = new AltaTicket();
            BL_CatTpm catlg = new BL_CatTpm();
            BLSistemasEquipo datosSistEqui = new BLSistemasEquipo();

            List<CatEquipo> lstEstrEq = new List<CatEquipo>();

            tickNew.lstSistMantto = datosSistEqui.DatosCatSistManto(cnxSqlMT, depto).OrderBy(x => x.Sistema).ToList();
            tickNew.lstExtrEqui = bl_tickets.GetSubEquipos(cnxSqlMT, codWc, codEqui, rutaLog);

            tickNew.Ticket = new Ticket();

            tickNew.Ticket.CodWorkCenter = codWc;
            tickNew.Ticket.CodEquipo = codEqui;
            tickNew.Ticket.CodEquipoDescrip = equipo;
            tickNew.Ticket.FchReporte = DateTime.Now;
            tickNew.Ticket.TipoTicket = tipoTick;
            tickNew.Ticket.CodDepartamento = depto;
            tickNew.Ticket.UsuarioReporto = "";
            tickNew.Ticket.HallazgoSeguridad = "NO";
            tickNew.Ticket.CodSistema = tickNew.lstSistMantto[0].CodSistema;
            tickNew.Ticket.CodFalla = "FGRAL";
            tickNew.Ticket.Sensor = "";

            tickNew.lstFallas = catlg.DatosCatFallas(cnxSqlMT, tickNew.Ticket.CodSistema, tickNew.Ticket.CodDepartamento);

            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.Message = "Reporte de Falla: *** " + bl_tickets.GetTipoTickDescrip(cnxSqlMT, tipoTick) + " ***";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion


            ViewBag.ErrorLogin = false;
            ViewBag.CC = Session["costos"].ToString();
            return View(tickNew);
        }

        [HttpPost]
        public ActionResult Guardar(AltaTicket tickNew)
        {
            string usuario = "";
            string cCostos = "";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion


            //if (Session["costos"] == null)
            //{
            //   return RedirectToAction("FinSesion", "Usuarios");
            //}
            cCostos = Session["costos"].ToString();

            if (Session["UserName"] == null)
            {
                usuario = "CODEQR";
            }

            datosEmail.To = new List<string>();
            datosEmail.Subject = "";
            datosEmail.Mensaje = "";

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            cnxSqlRefec = "Data Source=" + config.SrvSqlEmpl.Trim() + ";Initial Catalog=" + config.BdEmpl.Trim() + "; User ID=" + config.UserEmpl.Trim() + "; Password=" + config.PwdEmpl.Trim();

            // Como es un ticket nuevo le vamos asignar datos especificos ok
            tickNew.Ticket.IdPlanta = planta;
            tickNew.Ticket.CodStatus = "NVO";

            tickNew.Ticket.FecStatus = DateTime.Now;
            tickNew.Ticket.CodTipoFalla = "Z02";
            tickNew.Ticket.CodDepartamento = depto;
            tickNew.Ticket.CentroCostos = cCostos;

            if (!ModelState.IsValid)
            {
                BL_CatTpm catlg = new BL_CatTpm();
                BLSistemasEquipo datosSistEqui = new BLSistemasEquipo();

                List<EstruturaEquipo> lstEstrEq = new List<EstruturaEquipo>();
                tickNew.lstSistMantto = datosSistEqui.DatosCatSistManto(cnxSqlMT, depto).OrderBy(x => x.Sistema).ToList();
                tickNew.lstExtrEqui = bl_tickets.GetSubEquipos(cnxSqlMT, tickNew.Ticket.CodWorkCenter, tickNew.Ticket.CodEquipo, rutaLog);
                tickNew.lstFallas = catlg.DatosCatFallas(cnxSqlMT, tickNew.Ticket.CodSistema, tickNew.Ticket.CodDepartamento);

                ViewBag.Title = tituArea;
                ViewBag.TitleCC = tituCC;
                ViewBag.Message = "Reporte de Falla: *** " + bl_tickets.GetTipoTickDescrip(cnxSqlMT, tickNew.Ticket.TipoTicket) + " ***";
                return View("Crear", tickNew);

            }


            int valida = bl_tickets.ValidarEmpleado(cnxSqlMT, cnxSqlRefec, tickNew.Ticket.UsuarioReporto, rutaLog);

            if (valida != 1)
            {
                ModelState.AddModelError("Ticket.UsuarioReporto", "Clave de empleado inválida");
            }

            ViewBag.ErrorLogin = false;

            if (valida != 1)
            {
                BL_CatTpm catlg = new BL_CatTpm();
                BLSistemasEquipo datosSistEqui = new BLSistemasEquipo();

                List<EstruturaEquipo> lstEstrEq = new List<EstruturaEquipo>();
                tickNew.lstSistMantto = datosSistEqui.DatosCatSistManto(cnxSqlMT, depto).OrderBy(x => x.Sistema).ToList();
                tickNew.lstExtrEqui = bl_tickets.GetSubEquipos(cnxSqlMT, tickNew.Ticket.CodWorkCenter, tickNew.Ticket.CodEquipo, rutaLog);
                tickNew.lstFallas = catlg.DatosCatFallas(cnxSqlMT, tickNew.Ticket.CodSistema, tickNew.Ticket.CodDepartamento);

                ViewBag.Title = tituArea;
                ViewBag.TitleCC = tituCC;
                ViewBag.Message = "Reporte de Falla: *** " + bl_tickets.GetTipoTickDescrip(cnxSqlMT, tickNew.Ticket.TipoTicket) + " ***";
                return View("Crear", tickNew);
            }
            else
            {
                datosEmail.To = bl_tickets.EmailsTpm(cnxSqlMT, rutaLog, cCostos);
                datosEmail.Subject = "";
                datosEmail.Mensaje = "";

                int result = bl_tickets.GuardarTicket(cnxSqlMT, tickNew.Ticket, rutaLog, false, datosEmail, usuario);

                return RedirectToAction("ListaTickets", "Tickets", routeValues: new { CodEq = tickNew.Ticket.CodEquipo, statusTick = false, showError = false });
            }

        }

        public JsonResult GetEmpleado(string pClave)
        {
            string nombre = bl_tickets.GetNombreEmpleado(cnxSqlMT, pClave, rutaLog);

            return Json(nombre, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Consulta(int Id, string Eq)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            Ticket tick = bl_tickets.GetTicket(cnxSqlMT, Id, rutaLog);
            if (tick.CodEquipo == null)
            {
                return RedirectToAction("ListaTickets", "Tickets", routeValues: new { CodEq = Eq, statusTick = false, showError = true });
            }
            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.Message = "Tipo de Falla Reportada: *** " + bl_tickets.GetTipoTickDescrip(cnxSqlMT, tick.TipoTicket) + " ***";
            ViewBag.Equipo = tick.CodEquipo;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(tick);
        }

        [HttpGet]
        public ActionResult Editar(int Id, string Eq)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();
            ViewBag.Login = true;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion


            if (Session["UserId"] == null || Session["UserId"].ToString() == "")
            {
                return RedirectToAction("ListaTickets", "Tickets", routeValues: new { CodEq = Eq, statusTick = false, showError = true });
            }

            AltaTicket tickNew = new AltaTicket();
            BL_CatTpm catlg = new BL_CatTpm();

            BLSistemasEquipo datosSistEqui = new BLSistemasEquipo();

            List<EstruturaEquipo> lstEstrEq = new List<EstruturaEquipo>();

            Ticket tick = bl_tickets.GetTicket(cnxSqlMT, Id, rutaLog);
            if (tick.CodEquipo == null)
            {

                return RedirectToAction("ListaTickets", "Tickets", routeValues: new { CodEq = tick.CodEquipo, statusTick = false, showError = true });
            }



            tickNew.lstClasifFalla = catlg.DatosCatClasifFallas(cnxSqlMT);
            tickNew.lstFallas = catlg.DatosCatFallas(cnxSqlMT, tick.CodSistema, tick.CodDepartamento);
            tickNew.lstCatCiclos = catlg.DatosCatCiclos(cnxSqlMT);
            tickNew.lstSistMantto = datosSistEqui.DatosCatSistManto(cnxSqlMT, depto).OrderBy(x => x.Sistema).ToList();
            tickNew.lstExtrEqui = bl_tickets.GetSubEquipos(cnxSqlMT, tick.CodWorkCenter, tick.CodEquipo, rutaLog);

            tickNew.Ticket = new Ticket();
            tickNew.Ticket = tick;

            if (string.IsNullOrEmpty(tickNew.Ticket.UsrAtendio))
            {
                tickNew.Ticket.UsrAtendio = usuario;
            }

            tickNew.Ticket.FecStatus = DateTime.Now;


            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.Message = "Reporte de Falla: *** " + bl_tickets.GetTipoTickDescrip(cnxSqlMT, tick.TipoTicket) + " ***";

            return View(tickNew);
        }

        [HttpPost]
        public ActionResult Editar(AltaTicket tickEdit)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            string usuario = Session["UserName"].ToString();
            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            tickEdit.Ticket.CentroCostos = cCostos;
            datosEmail.To = null;
            datosEmail.Subject = "";
            datosEmail.Mensaje = "";
            if (!ModelState.IsValid)
            {

            }

            if (tickEdit.Ticket.CodStatus == "CER" || tickEdit.Ticket.CodStatus == "CAN")
            {
                DatosError error = new DatosError();

                error.Cabecera = "Falla en la Actualizacion";
                if (tickEdit.Ticket.CodStatus == "CER")
                {
                    error.mensaje1 = "Ticket CERRADO, ya no es posible modificarlo";
                }
                if (tickEdit.Ticket.CodStatus == "CAN")
                {
                    error.mensaje1 = "Ticket CANCELADO, Imposible modificarlo";
                }

                return PartialView("_EditarError", error);
            }

            else
            {

                int result = bl_tickets.GuardarTicket(cnxSqlMT, tickEdit.Ticket, rutaLog, true, datosEmail, usuario);
            }

            return RedirectToAction("ListaTickets", "Tickets", routeValues: new { CodEq = tickEdit.Ticket.CodEquipo, statusTick = false, showError = false });
        }
        public ActionResult Cancelar(int idTicket)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            return View();
        }

        [HttpPost]
        public JsonResult GetFallas(string pCodSistema, string pCodDepartamento)
        {

            List<Falla> lstFallas = new List<Falla>();
            BL_CatTpm catlg = new BL_CatTpm();

            lstFallas = catlg.DatosCatFallas(cnxSqlMT, pCodSistema, pCodDepartamento);
            //return Json(lstFallas);
            return Json(new SelectList(lstFallas, "CodFalla", "Descrip"));
        }


        [HttpPost]
        public ActionResult filtrarTickets(FormCollection formu)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            string filtro = formu["filtro"].ToString();
            string equipo = formu["datEquipo.CodEquipo"].ToString();

            bool statusTick = (filtro == "NO" ? true : false);

            System.Threading.Thread.Sleep(1000);
            return RedirectToAction("ListaTickets", "Tickets", routeValues: new { CodEq = equipo, statusTick, showError = false });
        }

        public ActionResult AbrirPdf()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            string file = @"~\pdf\Manual de Operacion - Tpm Prensas - Creacion de tickets.pdf";
            string contentType = System.Net.Mime.MediaTypeNames.Application.Pdf;
            System.Threading.Thread.Sleep(1000);

            return new FilePathResult(file, contentType);

        }

        [HttpGet]
        public ActionResult CrearTT(string codWc, string codEqui, string equipo, string tipoTick, string pCtroCostos)
        {

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion


            if (Session["costos"] == null && pCtroCostos == "") { return RedirectToAction("FinSesion", "Usuarios"); }

            if (pCtroCostos != null) cCostos = pCtroCostos;
            else
            if (Session["costos"] != null) cCostos = Session["costos"].ToString();
            else { return RedirectToAction("FinSesion", "Usuarios"); }

            AltaTicket tickNew = new AltaTicket();
            BL_CatTpm catlg = new BL_CatTpm();
            BLSistemasEquipo datosSistEqui = new BLSistemasEquipo();

            List<CatEquipo> lstEstrEq = new List<CatEquipo>();

            tickNew.lstSistMantto = datosSistEqui.DatosCatSistManto(cnxSqlMT, depto).OrderBy(x => x.Sistema).ToList();
            tickNew.lstExtrEqui = bl_tickets.GetSubEquipos(cnxSqlMT, codWc, codEqui, rutaLog);

            tickNew.Ticket = new Ticket();

            tickNew.Ticket.CodWorkCenter = codWc;
            tickNew.Ticket.CodEquipo = codEqui;
            tickNew.Ticket.CodEquipoDescrip = equipo;
            tickNew.Ticket.FchReporte = DateTime.Now;
            tickNew.Ticket.TipoTicket = tipoTick;
            tickNew.Ticket.CodDepartamento = depto;
            tickNew.Ticket.UsuarioReporto = "";
            tickNew.Ticket.HallazgoSeguridad = "NO";
            tickNew.Ticket.CodSistema = tickNew.lstSistMantto[0].CodSistema;
            tickNew.Ticket.CodFalla = "FGRAL";

            tickNew.lstFallas = catlg.DatosCatFallas(cnxSqlMT, tickNew.Ticket.CodSistema, tickNew.Ticket.CodDepartamento);

            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.Message = "Reporte de Falla: *** " + bl_tickets.GetTipoTickDescrip(cnxSqlMT, tipoTick) + " ***";


            ViewBag.ErrorLogin = false;
            return View(tickNew);
        }


    }
}

