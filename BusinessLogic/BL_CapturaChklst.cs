using Entidades;
using RepositorySql;
using System;
using System.Collections.Generic;
using System.Data;


namespace BusinessLogic
{
    public class BL_CapturaChklst
    {
        public List<CheckListEqDt> GetDatosChkxEqDet(string cnxSql, int Id, string pDepto)
        {
            SqlRepository repoSql = new SqlRepository();

            List<CheckListEqDt> lstChkEqDet = repoSql.GetChkxEqDet(cnxSql, Id, pDepto);

            return (lstChkEqDet);
        }

        public List<CheckListEqEnc> GetCatChkxEq(string cnxSql, int pPlanta, string pDepto, string pCostos, bool soloActivos = false)
        {
            SqlRepository repoSql = new SqlRepository();
            List<CheckListEqEnc> lstEqConChk = repoSql.GetCatChklstxEq(cnxSql, pPlanta, pDepto, pCostos, soloActivos);
            return (lstEqConChk);
        }

        public CheckListEqEnc GetDatosChkxEq(string cnxSql, int Id, string pDepto)
        {
            SqlRepository repoSql = new SqlRepository();

            CheckListEqEnc lstEqConChk = repoSql.GetChklstxEq(cnxSql, Id, pDepto);
            return (lstEqConChk);
        }
        public CheckListEqEnc GetDatosChkxEqProg(string cnxSql, int Id, string pDepto, int pPlanta)
        {
            SqlRepository repoSql = new SqlRepository();

            CheckListEqEnc lstEqConChk = repoSql.GetChklstxEq(cnxSql, Id, pDepto);
            return (lstEqConChk);
        }

        public CapturaChklst Guardar(string cnxSql, CapturaChklst newchklst, string rutalog, int pPlanta, string pDepto, string pUsuario, string cCostos, string pUserReporto)
        {
            int result = 0;
            SqlRepository repoSql = new SqlRepository();

            newchklst.ChklsxEq.Observaciones = newchklst.ChklsxEq.Observaciones == null ? "" : newchklst.ChklsxEq.Observaciones;

            result = repoSql.GuardarCapCklstEncb(cnxSql, newchklst.ChklsxEq, pPlanta);

            // Obtenemos el Id con el que se guardo para guardar el detalle
            int tempId = GetNewCapChklst(cnxSql, newchklst.ChklsxEq, rutalog);

            newchklst.ChklsxEq.IdChkEquipo = tempId;

            foreach (var i in newchklst.lstChckActEq)
            {
                if (i.TipoOperacion == "V") // Visual
                {
                    i.RangoMin = 0;
                    i.RangoMax = 0;
                    i.OperadorMin = "";
                    i.OperadorMax = "";
                    i.ResultMedible = 0;
                    i.CodUom = "";

                    i.ResultActiv = (bool)i.ResultVisual;
                }
                if (i.TipoOperacion == "M") // Medible
                {
                    i.OperadorMin = i.OperadorMin == null ? "" : i.OperadorMin;
                    i.OperadorMax = i.OperadorMax == null ? "" : i.OperadorMax;
                    if (string.IsNullOrEmpty(i.CodUom)) { i.CodUom = ""; }
                    i.ResultVisual = false;

                    // Con un solo operador
                    if (!string.IsNullOrEmpty(i.OperadorMin) && string.IsNullOrEmpty(i.OperadorMax))
                    {
                        switch (i.OperadorMin)
                        {
                            case ">":
                                if (i.ResultMedible > i.RangoMin) { i.ResultActiv = true; } else { i.ResultActiv = false; }

                                break;

                            case ">=":
                                if (i.ResultMedible >= i.RangoMin) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                                break;

                            case "<":
                                if (i.ResultMedible < i.RangoMin) { i.ResultActiv = true; } else { i.ResultActiv = false; }

                                break;

                            case "<=":
                                if (i.ResultMedible <= i.RangoMin) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                                break;
                        }
                    }

                    // Con 2 operadores
                    if (!string.IsNullOrEmpty(i.OperadorMin) && !string.IsNullOrEmpty(i.OperadorMax))
                    {
                        if (i.OperadorMin == ">" && i.OperadorMax == "<")
                        {
                            if (i.ResultMedible > i.RangoMin && i.ResultMedible < i.RangoMax) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                        }

                        if (i.OperadorMin == ">=" && i.OperadorMax == "<=")
                        {
                            if (i.ResultMedible >= i.RangoMin && i.ResultMedible <= i.RangoMax) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                        }

                        if (i.OperadorMin == ">=" && i.OperadorMax == "<")
                        {
                            if (i.ResultMedible >= i.RangoMin && i.ResultMedible < i.RangoMax) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                        }
                        if (i.OperadorMin == ">" && i.OperadorMax == "<=")
                        {
                            if (i.ResultMedible > i.RangoMin && i.ResultMedible <= i.RangoMax) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                        }
                    }

                    i.Criterio = "";

                    if (!string.IsNullOrEmpty(i.OperadorMin))
                    {
                        i.Criterio = i.OperadorMin.Trim() + " " + i.RangoMin.ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(i.OperadorMax))
                    {
                        i.Criterio = i.Criterio + " y " + i.OperadorMax.Trim() + " " + i.RangoMax.ToString().Trim();
                    }

                }
            }

            newchklst.ResultSave = repoSql.GuardarNewCapAct(cnxSql, newchklst, rutalog);

            if (newchklst.ResultSave == 1)
            {

                DatosCorreo datosEmail = new DatosCorreo();
                BL_AdmonTickets bl_tickets = new BL_AdmonTickets();

                // vamos a generar los reportes de fallas de acuerdo a los resultados del checklist
                foreach (var i in newchklst.lstChckActEq)
                {
                    // si el resultado fue falso, es porquen no cumplio con los parametros o visualmente no es correcto
                    if (!i.ResultActiv.Value)
                    {

                        Ticket newTicket = new Ticket();

                        newTicket.CodWorkCenter = i.CodWorkCenter;
                        newTicket.CodEquipo = i.CodEquipo;
                        newTicket.CodSubEquipo = i.CodEquipo;
                        newTicket.CodEquipoDescrip = "";
                        newTicket.FchReporte = DateTime.Now;
                        newTicket.FecStatus = DateTime.Now;
                        newTicket.TipoTicket = "A";
                        newTicket.IdPlanta = pPlanta;
                        newTicket.CodDepartamento = pDepto;

                        // ********************* AQUI
                        newTicket.UsuarioReporto = pUserReporto;

                        newTicket.HallazgoSeguridad = "NO";
                        newTicket.CodSistema = i.CodSistema;
                        newTicket.CodFalla = "FGRAL";
                        newTicket.Sensor = "";
                        newTicket.CausoParo = "NO";
                        newTicket.CodStatus = "NVO";
                        newTicket.Notas = "Generado en automático por resultado del Chechlist";
                        if (i.TipoOperacion == "M")
                        {
                            newTicket.FallaReportada = "CheckList: " + newchklst.ChklsxEq.CodClasif + " =>  [" + i.CodActividad + "], Componente: " + i.DescripCompo + ", Sistema: " + i.DescripSistema
                               + ", Actividad: " + i.DescripcionAct + ", Parametros: " + i.Criterio + ",  Resultado: " + i.ResultMedible;
                        }
                        else
                        {
                            newTicket.FallaReportada = "CheckList: " + newchklst.ChklsxEq.CodClasif + " => [" + i.CodActividad + "], Componente: " + i.DescripCompo + ", Sistema: " + i.DescripSistema
                               + ", Actividad: " + i.DescripcionAct + ",  Resultado: NO OK";
                        }

                        datosEmail.To = bl_tickets.EmailsTpm(cnxSql, rutalog, cCostos);
                        //datosEmail.To = new List<string>();
                        //datosEmail.To.Add( "victor.rodriguez1 @magna.com");
                        datosEmail.Subject = "";
                        datosEmail.Mensaje = "";
                        newTicket.CentroCostos = cCostos;
                        result = bl_tickets.GuardarTicket(cnxSql, newTicket, rutalog, false, datosEmail, pUsuario);

                    }
                }
            }

            return (newchklst);
        }

        public int GetNewCapChklst(string cnxSql, CheckListEqEnc chkEqui, string rutalog)
        {
            int newId = 0;
            List<DataRow> lstDatos = null;
            SqlRepository repoSql = new SqlRepository();
            lstDatos = repoSql.Valid_ChkNewCap(cnxSql, chkEqui, rutalog);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];
                newId = (int)dr["IdChkEquipo"];
            }
            else
            {
                newId = 0;
            }
            return (newId);

        }

        public List<CheckListEqEnc> GetChkxEqProg(string cnxSql, string pDepto, int pPlanta, string CodEquipo = "", string CtroCtos = "")
        {
            SqlRepository repoSql = new SqlRepository();
            List<CheckListEqEnc> lstEqConChk = repoSql.GetChklstxEqProg(cnxSql, pDepto, pPlanta, CodEquipo, CtroCtos);
            return (lstEqConChk);
        }

        public CheckListEqEnc GetDatosChkEncb(string cnxSql, int Id, string pDepto, int pPlanta)
        {
            SqlRepository repoSql = new SqlRepository();
            CheckListEqEnc lstEqConChk = repoSql.GetDatosChklstEncb(cnxSql, Id, pDepto, pPlanta);
            return (lstEqConChk);
        }

        public List<CheckListEqDt> GetDatosChkActProg(string cnxSql, int Id, string pDepto)
        {
            SqlRepository repoSql = new SqlRepository();
            List<CheckListEqDt> lstChkAct = new List<CheckListEqDt>();
            lstChkAct = repoSql.GetChecklistAct(cnxSql, Id, pDepto);
            return (lstChkAct);
        }

        public CapturaChklst Update(string cnxSql, CapturaChklst newchklst, string rutalog, string cCostos, string pUsuario, string pClave)
        {
            int result = 0;
            SqlRepository repoSql = new SqlRepository();

            newchklst.ChklsxEq.Observaciones = newchklst.ChklsxEq.Observaciones == null ? "" : newchklst.ChklsxEq.Observaciones;

            result = repoSql.UpdateCapCklstEncbProg(cnxSql, newchklst.ChklsxEq);

            if (result == 1)
            {
                foreach (var i in newchklst.lstChckActEq)
                {
                    if (i.TipoOperacion == "V") // Visual
                    {
                        i.RangoMin = 0;
                        i.RangoMax = 0;
                        i.OperadorMin = "";
                        i.OperadorMax = "";
                        i.ResultMedible = 0;

                        i.ResultActiv = (bool)i.ResultVisual;
                    }
                    if (i.TipoOperacion == "M") // Medible
                    {
                        i.OperadorMin = i.OperadorMin == null ? "" : i.OperadorMin;
                        i.OperadorMax = i.OperadorMax == null ? "" : i.OperadorMax;
                        i.ResultVisual = false;
                        
                        // Con un solo operador
                        if (!string.IsNullOrEmpty(i.OperadorMin) && string.IsNullOrEmpty(i.OperadorMax))
                        {
                            switch (i.OperadorMin)
                            {
                                case ">":
                                    if (i.ResultMedible > i.RangoMin) { i.ResultActiv = true; } else { i.ResultActiv = false; }

                                    break;

                                case ">=":
                                    if (i.ResultMedible >= i.RangoMin) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                                    break;

                                case "<":
                                    if (i.ResultMedible < i.RangoMin) { i.ResultActiv = true; } else { i.ResultActiv = false; }

                                    break;

                                case "<=":
                                    if (i.ResultMedible <= i.RangoMin) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                                    break;
                            }
                        }

                        // Con 2 operadores
                        if (!string.IsNullOrEmpty(i.OperadorMin) && !string.IsNullOrEmpty(i.OperadorMax))
                        {
                            if (i.OperadorMin == ">" && i.OperadorMax == "<")
                            {
                                if (i.ResultMedible > i.RangoMin && i.ResultMedible < i.RangoMax) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                            }

                            if (i.OperadorMin == ">=" && i.OperadorMax == "<=")
                            {
                                if (i.ResultMedible >= i.RangoMin && i.ResultMedible <= i.RangoMax) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                            }

                            if (i.OperadorMin == ">=" && i.OperadorMax == "<")
                            {
                                if (i.ResultMedible >= i.RangoMin && i.ResultMedible < i.RangoMax) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                            }
                            if (i.OperadorMin == ">" && i.OperadorMax == "<=")
                            {
                                if (i.ResultMedible > i.RangoMin && i.ResultMedible <= i.RangoMax) { i.ResultActiv = true; } else { i.ResultActiv = false; }
                            }
                        }

                        i.Criterio = "";

                        if (!string.IsNullOrEmpty(i.OperadorMin))
                        {
                            i.Criterio = i.OperadorMin.Trim() + " " + i.RangoMin.ToString().Trim();
                        }
                        if (!string.IsNullOrEmpty(i.OperadorMax))
                        {
                            i.Criterio = i.Criterio + " y " + i.OperadorMax.Trim() + " " + i.RangoMax.ToString().Trim();
                        }

                    }
                }


                newchklst.ResultSave = repoSql.UpdateCapActProg(cnxSql, newchklst, rutalog);

                if (newchklst.ResultSave == 1)
                {

                    DatosCorreo datosEmail = new DatosCorreo();
                    BL_AdmonTickets bl_tickets = new BL_AdmonTickets();

                    // vamos a generar los reportes de fallas de acuerdo a los resultados del checklist
                    foreach (var i in newchklst.lstChckActEq)
                    {
                        // si el resultado fue falso, es porquen no cumplio con los parametros o visualmente no es correcto
                        if (!i.ResultActiv.Value)
                        {

                            Ticket newTicket = new Ticket();

                            newTicket.CodWorkCenter = i.CodWorkCenter;
                            newTicket.CodEquipo = i.CodEquipo;
                            newTicket.CodSubEquipo = i.CodEquipo;
                            newTicket.CodEquipoDescrip = "";
                            newTicket.FchReporte = DateTime.Now;
                            newTicket.FecStatus = DateTime.Now;
                            newTicket.TipoTicket = "A";
                            newTicket.IdPlanta = newchklst.ChklsxEq.Planta;
                            newTicket.CodDepartamento = newchklst.ChklsxEq.CodDepartamento;
                            newTicket.UsuarioReporto = pClave;
                            newTicket.HallazgoSeguridad = "NO";
                            newTicket.CodSistema = i.CodSistema;
                            newTicket.CodFalla = "FGRAL";
                            newTicket.Sensor = "";
                            newTicket.CausoParo = "NO";
                            newTicket.CodStatus = "NVO";
                            newTicket.Notas = "Generado en automático por resultado del Chechlist";
                            if (i.TipoOperacion == "M")
                            {
                                newTicket.FallaReportada = "CheckList: " + newchklst.ChklsxEq.CodClasif + " =>  [" + i.CodActividad + "], Componente: " + i.DescripCompo + ", Sistema: " + i.DescripSistema
                                   + ", Actividad: " + i.DescripcionAct + ", Parametros: " + i.Criterio + ",  Resultado: " + i.ResultMedible;
                            }
                            else
                            {
                                newTicket.FallaReportada = "CheckList: " + newchklst.ChklsxEq.CodClasif + " => [" + i.CodActividad + "], Componente: " + i.DescripCompo + ", Sistema: " + i.DescripSistema
                                   + ", Actividad: " + i.DescripcionAct + ",  Resultado: NO OK";
                            }

                            datosEmail.To = bl_tickets.EmailsTpm(cnxSql, rutalog, cCostos);
                            //datosEmail.To = new List<string>();
                            //datosEmail.To.Add( "victor.rodriguez1 @magna.com");
                            datosEmail.Subject = "";
                            datosEmail.Mensaje = "";
                            newTicket.CentroCostos = cCostos;
                            result = bl_tickets.GuardarTicket(cnxSql, newTicket, rutalog, false, datosEmail, pUsuario);

                        }
                    }
                }
            }
            return (newchklst);
        }

    }
}
