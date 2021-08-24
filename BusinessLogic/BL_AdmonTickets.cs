using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using RepositorySql;
using ToolsTpm;


namespace BusinessLogic
{
   public class BL_AdmonTickets
   {
      public List<Ticket> GetListaTickxEqu(string cnxSqlMt, string codEquipo, string codWorkCenter, bool statusTicket, string depto, string rutalog, string cCostos)
      {
         List<Ticket> lstTickets = new List<Ticket>();

         BL_CatTpm catlg = new BL_CatTpm();
         List<Falla> lstCatFallas = catlg.GetCatFallas(cnxSqlMt, depto);
         List<CatStatusTicket> lstStatusTickets = catlg.DatosCatStatusTicket(cnxSqlMt);

         BLSistemasEquipo datosSistEqui = new BLSistemasEquipo();
         List<SistemaManto> lstSistManto = datosSistEqui.DatosCatSistManto(cnxSqlMt, depto);


         SqlRepository repoSql = new SqlRepository();

         List<CatEquipo> lstEquipos = new List<CatEquipo>();
         lstEquipos = repoSql.GetCatEquiposPad(cnxSqlMt, cCostos, rutalog);


         lstTickets = repoSql.ListaTickxEqu(cnxSqlMt, rutalog, codEquipo, codWorkCenter, statusTicket);

         foreach (var dr in lstTickets)
         {
            int index = lstCatFallas.FindIndex(x => x.CodFalla == dr.CodFalla);
            //if (index >= 0)   dr.CodFallaDescrip = "["+dr.CodFalla.Trim()+"] - "+lstCatFallas[index].Descrip;
            if (index >= 0) dr.CodFallaDescrip = lstCatFallas[index].Descrip;
            else dr.CodFallaDescrip = "";

            index = lstSistManto.FindIndex(x => x.CodSistema == dr.CodSistema);
            //if (index >= 0) dr.CodSistemaDescrip = "[" + dr.CodSistema.Trim() + "] - " + lstSistManto[index].Sistema;
            if (index >= 0) dr.CodSistemaDescrip = lstSistManto[index].Sistema;
            else dr.CodSistemaDescrip = "";

            index = lstStatusTickets.FindIndex(x => x.CodStatus == dr.CodStatus);
            //if (index >= 0) dr.CodStatusDescrip = "[" + dr.CodStatus.Trim() + "] - " + lstStatusTickets[index].Descrip;
            if (index >= 0) dr.CodStatusDescrip = lstStatusTickets[index].Descrip;
            else dr.CodStatusDescrip = "";

            index = lstEquipos.FindIndex(x => x.CodEquipo == dr.CodSubEquipo.Trim());
            //if (index >= 0) dr.CodStatusDescrip = "[" + dr.CodStatus.Trim() + "] - " + lstStatusTickets[index].Descrip;
            if (index >= 0) dr.SubEquipo = lstEquipos[index].DescripTechnical;
            else dr.SubEquipo = "";

            dr.UsuarioReporto = "[" + dr.UsuarioReporto + "] " + GetNombreEmpleado(cnxSqlMt, dr.UsuarioReporto, rutalog);

         }
         return lstTickets.OrderByDescending(x => x.IdTicket).ToList();
      }

      public Ticket GetTicket(string cnxSqlMt, int numTicket, string rutalog)
      {
         Ticket tick = new Ticket();
         SqlRepository repoSql = new SqlRepository();
         tick = repoSql.GetDatosTicket(cnxSqlMt, numTicket, rutalog);
         return tick;
      }

      public int ValidarEmpleado(string cnxSqlMt, string cnxSqlRefec, string numControl, string rutalog)
      {
         int result = 1;
         SqlRepository repoSql = new SqlRepository();
         string nombre = string.Empty;

         // Revicion de datos
         nombre = GetNombreEmpleado(cnxSqlRefec, numControl.Trim(), rutalog);

         if (string.IsNullOrEmpty(nombre))
            return (-99);
         else
         {
            return (result);
         }
      }

      public int GuardarTicket(string cnxSqlMt, Ticket DatosTick, string rutalog, bool todo, DatosCorreo datosEmail, string usuario)
      {
         int result = 0;
         Tools tool = new Tools();
         SqlRepository repoSql = new SqlRepository();

         // obtener la descripcion del equipo
         DatosTick.CodEquipoDescrip = repoSql.GetDescripEquipo(cnxSqlMt, DatosTick.CodSubEquipo);

         if (string.IsNullOrEmpty(DatosTick.CodClasif)) { DatosTick.CodClasif = ""; }
         if (string.IsNullOrEmpty(DatosTick.CodFalla)) { DatosTick.CodFalla = ""; }
         if (string.IsNullOrEmpty(DatosTick.CodSistema)) { DatosTick.CodSistema = ""; }

         if (string.IsNullOrEmpty(DatosTick.CausoParo)) { DatosTick.CausoParo = ""; }
         if (DatosTick.TiempoReparacion == 0)
         {
            DatosTick.UnidadTiempoRep = "";
         }
         if (string.IsNullOrEmpty(DatosTick.Diagnostico)) { DatosTick.Diagnostico = ""; }
         if (string.IsNullOrEmpty(DatosTick.AccionPlan)) { DatosTick.AccionPlan = ""; }
         if (string.IsNullOrEmpty(DatosTick.WorkerAsignado)) { DatosTick.WorkerAsignado = ""; }
         if (string.IsNullOrEmpty(DatosTick.WOM)) { DatosTick.WOM = ""; }
         if (string.IsNullOrEmpty(DatosTick.Notas)) { DatosTick.Notas = ""; }
         if (string.IsNullOrEmpty(DatosTick.Sensor)) { DatosTick.Sensor = ""; }

         // Cambio de Estatus de NVO a ATN Atendido
         if (DatosTick.CodStatus == "NVO" &&
             (!string.IsNullOrEmpty(DatosTick.CodClasif) || !string.IsNullOrEmpty(DatosTick.Diagnostico) || !string.IsNullOrEmpty(DatosTick.CausoParo)))
         {
            DatosTick.CodStatus = "ATN";
            DatosTick.UsrAtendio = usuario;
            DatosTick.FchAtendido = DateTime.Now;
         }

         // Cambio de Estatus a AAR  Asignado a un Responsable
         if (!string.IsNullOrEmpty(DatosTick.AccionPlan) || !string.IsNullOrEmpty(DatosTick.WorkerAsignado))
         {
            DatosTick.CodStatus = "AAR";
            DatosTick.UsrAsigno = usuario;
            DatosTick.FchAsignacion = DateTime.Now;
         }

         // Cambio de Estatus a WAS  WOM asignada
         if (!string.IsNullOrEmpty(DatosTick.WOM))
         {
            DatosTick.CodStatus = "WAS";
            DatosTick.UsrAsignoWom = usuario;
            DatosTick.FchWom = DateTime.Now;
         }


         // Cambio de Estatus a WAS  WOM asignada
         if (DatosTick.FecEntgregaReparacion.ToString("dd/MM/yyyy") != "01/01/0001")
         {
            DatosTick.CodStatus = "LPP";
            DatosTick.UsrEntrReparacion = usuario;
         }

         // Fecha de Cierre
         if (DatosTick.FchClose.ToString("dd/MM/yyyy") != "01/01/0001")
         {
            DatosTick.CodStatus = "CER";
            DatosTick.UsrCerroTicket = usuario;
         }

         result = repoSql.GuardarTicket(cnxSqlMt, DatosTick, rutalog, todo);

         // si es un ticket de mantto. preventivo y se cierra.. reinicamos el ciclo del plan
         if (DatosTick.CodStatus == "CER" && DatosTick.TipoTicket == "M")
         {
            int ResulCierre = repoSql.ActualizarCiclo(cnxSqlMt, DatosTick.CodEquipo, DatosTick.CodSistema, DatosTick.FchClose, rutalog);
         }

         // Si es nuevo enviamos correo
         if (!todo)
         {

            string cl = string.Empty;
            if (DatosTick.TipoTicket == "R") { cl = "background-color: #ff9999"; }
            if (DatosTick.TipoTicket == "A") { cl = "background-color: #ffff99"; }
            if (DatosTick.TipoTicket == "M") { cl = "background-color: #ffad33"; }
            if (DatosTick.TipoTicket == "L") { cl = "background-color: #ffffff"; }
            if (DatosTick.TipoTicket == "Z") { cl = "background-color: #b3e0ff"; }
            if (DatosTick.TipoTicket == "G") { cl = "background-color: #00b33c"; }

            string clha = string.Empty;
            if (DatosTick.HallazgoSeguridad == "SI") { clha = "background-color: #ff9999"; }

            // Enviamos correo para avisar del ticket

            string TipFalla = GetTipoTickDescrip(cnxSqlMt, DatosTick.TipoTicket);

            datosEmail.Subject = "TPM-Mantenimiento, Work Center: " + DatosTick.CodWorkCenter.Trim() + "  Equipo:" + DatosTick.CodSubEquipo.Trim() + " - " + DatosTick.CodEquipoDescrip + "  Falla: " + TipFalla;

            datosEmail.Mensaje = "<h3 style='border-bottom: 2px solid black;" + cl + "'>Tipo de falla:  <strong>" + TipFalla + "</strong> </h3>" +
                                 "Considerada como hallazgo de seguridad:  <span style='width:10px; " + clha + "'> <strong> " + DatosTick.HallazgoSeguridad + " </strong></span><br><br>" +
                                 "<Div style='background-color: #e6ffe6; '>" +
                                   "Work Center: <strong>" + DatosTick.CodWorkCenter + "</strong><br>" +
                                  "Equipo: <strong> [" + DatosTick.CodEquipo + "]  " + DatosTick.CodEquipoDescrip + "</strong><br>" +
                                  "Descripcion de Falla: <p><strong>" + DatosTick.FallaReportada + "</strong><p><br>" +
                                  "Reporto: <strong>[" + DatosTick.UsuarioReporto + "] " + DatosTick.NombreReporto + "</strong><br>" +
                                  "Fecha de reporte: <strong>" + DatosTick.FchReporte.ToString() + "</strong><br><br>" +
                                 "<p style='background-color: #b3e0ff;'> &copy; @" + DateTime.Now.Year + " - <strong> Magna, Cosma Mx </strong>  Tecnologías de la Información </p>" + "<Div>";

            tool.EnviarCorreo(datosEmail);
         }

         return result;

      }

      public string GetTipoTickDescrip(string cnxSqlMt, string tipoTick)
      {
         string DescrpTipoTick = "";
         SqlRepository repoSql = new SqlRepository();

         DescrpTipoTick = repoSql.LeeTipoTicketDesc(cnxSqlMt, tipoTick);
         return DescrpTipoTick;
      }

      public List<CatEquipo> GetSubEquipos(string cnxSqlMt, string pWc, string pCodEquipo, string rutalog)
      {
         List<CatEquipo> lstEE = new List<CatEquipo>();

         SqlRepository repoSql = new SqlRepository();
         lstEE = repoSql.GetCatEstrucEquWC(cnxSqlMt, pWc, pCodEquipo);
         return lstEE;
      }

      public string GetNombreEmpleado(string cnxSqlMt, string pNumControl, string rutalog)
      {
         string nombre = "";

         SqlRepository repoSql = new SqlRepository();
         nombre = repoSql.GetNombreEmpl(cnxSqlMt, pNumControl);
         return nombre;
      }

      public List<string> EmailsTpm(string cnxSqlMt, string rutalog, string CentroCostos)
      {
         List<string> lstEmail = new List<string>();
         string ctroCostos = int.Parse(CentroCostos).ToString();
         SqlRepository repoSql = new SqlRepository();
         lstEmail = repoSql.GetEmailTpm(cnxSqlMt, rutalog, ctroCostos);
         return (lstEmail);
      }
   }
}
