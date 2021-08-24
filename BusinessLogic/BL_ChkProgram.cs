using Entidades;
using RepositorySql;
using System;
using System.Collections.Generic;
using System.Data;
using ToolsTpm;

namespace BusinessLogic
{
    public class BL_ChkProgram
    {
        public void GetChkProg(string cnxSqlMT, int pPlanta, string pDepto, string pCtroCtos, string pathLog)
        {

            //Proceso
            //
            // 1.- Obtener los checklist activados y programados  
            // 2.- Insertar un registro del encabezado y el detalle de cada uno en las tablas de captura de checklist
            // 3-  Enviar un correo por cada uno de los checklist

            // 1.- Obtener los checklist activados y programados 
            List<CheckListEqEnc> lstCheck = new List<CheckListEqEnc>();

            SqlRepository repo = new SqlRepository();

            lstCheck = repo.GetCheckProg(cnxSqlMT, pPlanta, pDepto, pCtroCtos);

            // Vamos a revisar la fecha de programacion .. es la que lleva cuando se debe hacer el checklist
            // si viene vacio la programamos y lanzamos el aviso
            foreach (var r in lstCheck)
            {
                // su primera programacion
                if (r.FecProgramada == null)
                {
                    if (r.IdFrecuencia == "D")
                    {
                        //empezamos aplicar el calculo de fecha de programacion fecha de hoy + frecuencia 
                        r.FecProgramada = DateTime.Now.AddDays(r.Frecuencia);
                        if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                        {
                            r.FecProgramada = r.FecProgramada.Value.AddDays(1);
                        }

                        // Guardamos su fec. de program.
                        GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                        GeneraAviso(cnxSqlMT, r, pathLog, pDepto, pCtroCtos);
                    }
                    if (r.IdFrecuencia == "TUR")
                    {
                        // Obtener turnos definidos para el centro de costos
                        List<Turno> lstTurnos = new List<Turno>();

                        lstTurnos = GetTurnos(cnxSqlMT, pPlanta, pDepto, pCtroCtos);

                        //empezamos aplicar el calculo de fecha de programacion fecha de hoy + frecuencia 
                        r.FecProgramada = DateTime.Now.AddDays(1);
                        if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                        {
                            r.FecProgramada = r.FecProgramada.Value.AddDays(1);
                        }

                        foreach (var t in lstTurnos)
                        {
                            r.FecProgramada = Convert.ToDateTime(r.FecProgramada.Value.ToShortDateString() +' '+ t.THrI +':'+ t.TMinI);
                            // Guardamos su fec. de program).
                            GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                            GeneraAviso(cnxSqlMT, r, pathLog, pDepto, pCtroCtos);
                        }
                    }
                }
                else
                {
                    // Si es el dia de la programacion se calcula al nueva programacion
                    if (r.FecProgramada.Value.Date == DateTime.Now.Date)
                    {
                        if (r.IdFrecuencia == "D")
                        {
                            r.FecProgramada = Convert.ToDateTime(r.FecProgramada).AddDays(r.Frecuencia);
                            if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                            {
                                r.FecProgramada = r.FecProgramada.Value.AddDays(1);
                            }

                            // Guardamos su fec. de program.
                            GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                            GeneraAviso(cnxSqlMT, r, pathLog, pDepto, pCtroCtos);
                        }
                        if (r.IdFrecuencia == "TUR")
                        {
                            // Obtener turnos definidos para el centro de costos
                            List<Turno> lstTurnos = new List<Turno>();

                            lstTurnos = GetTurnos(cnxSqlMT, pPlanta, pDepto, pCtroCtos);

                            //empezamos aplicar el calculo de fecha de programacion fecha de hoy + frecuencia 
                            r.FecProgramada = Convert.ToDateTime(r.FecProgramada).AddDays(r.Frecuencia); ;
                            if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                            {
                                r.FecProgramada = r.FecProgramada.Value.AddDays(1);
                            }

                            foreach (var t in lstTurnos)
                            {
                                r.FecProgramada = Convert.ToDateTime(r.FecProgramada.Value.ToShortDateString() + ' ' + t.THrI + ':' + t.TMinI);
                                // Guardamos su fec. de program).
                                GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                                GeneraAviso(cnxSqlMT, r, pathLog, pDepto, pCtroCtos);
                            }
                        }
                    }
                }

                // Si por alguna razon no se actualizo y se atrazo la fecha de programacion y es menor a la fecha de hoy
                // se generar como si fuera la primera
                if (r.FecProgramada.Value.Date <= DateTime.Now.Date)
                {
                    if (r.IdFrecuencia == "D")
                    {
                        r.FecProgramada = DateTime.Now.AddDays(1);
                        if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                        {
                            r.FecProgramada = r.FecProgramada.Value.AddDays(1);
                        }
                    }
                    // Guardamos su fec. de program.
                    GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                    GeneraAviso(cnxSqlMT, r, pathLog, pDepto, pCtroCtos);

                }

            }


            return;
        }

        public void GetChkProgAll(string cnxSqlMT, string pathLog)
        {

            //Proceso
            //
            // 1.- Obtener los checklist activados y programados  
            // 2.- Insertar un registro del encabezado y el detalle de cada uno en las tablas de captura de checklist
            // 3-  Enviar un correo por cada uno de los checklist

            // 1.- Obtener los checklist activados y programados 
            List<CheckListEqEnc> lstCheck = new List<CheckListEqEnc>();

            SqlRepository repo = new SqlRepository();

            lstCheck = repo.GetCheckProgAll(cnxSqlMT);

            // Vamos a revisar la fecha de programacion .. es la que lleva cuando se debe hacer el checklist
            // si viene vacio la programamos y lanzamos el aviso
            foreach (var r in lstCheck)
            {
                // su primera programacion
                if (r.FecProgramada == null)
                {
                    if (r.IdFrecuencia == "D")
                    {
                        //empezamos aplicar el calculo de fecha de programacion fecha de hoy + frecuencia 
                        r.FecProgramada = DateTime.Now.AddDays(r.Frecuencia);
                        if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                        {
                            r.FecProgramada = r.FecProgramada.Value.AddDays(1);
                        }

                        // Guardamos su fec. de program.
                        GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                        GeneraAviso(cnxSqlMT, r, pathLog, r.CodDepartamento, r.CentroCostos);
                    }else if (r.IdFrecuencia == "HR")
                    {
                        //empezamos aplicar el calculo de fecha de programacion fecha de hoy + frecuencia 
                        r.FecProgramada = DateTime.Now.AddHours(r.Frecuencia);
                        if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                        {
                            r.FecProgramada = r.FecProgramada.Value.AddHours(r.Frecuencia);
                        }

                        // Guardamos su fec. de program.
                        GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                        GeneraAviso(cnxSqlMT, r, pathLog, r.CodDepartamento, r.CentroCostos);
                    }
                    else if (r.IdFrecuencia == "MN")
                    {
                        //empezamos aplicar el calculo de fecha de programacion fecha de hoy + frecuencia 
                        r.FecProgramada = DateTime.Now.AddMinutes(r.Frecuencia);
                        if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                        {
                            r.FecProgramada = r.FecProgramada.Value.AddMinutes(r.Frecuencia);
                        }

                        // Guardamos su fec. de program.
                        GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                        GeneraAviso(cnxSqlMT, r, pathLog, r.CodDepartamento, r.CentroCostos);
                    }

                    else if (r.IdFrecuencia == "TUR")
                    {
                        // Obtener turnos definidos para el centro de costos
                        List<Turno> lstTurnos = new List<Turno>();

                        lstTurnos = GetTurnos(cnxSqlMT, r.Planta, r.CodDepartamento, r.CentroCostos);

                        //empezamos aplicar el calculo de fecha de programacion fecha de hoy + frecuencia 
                        r.FecProgramada = DateTime.Now.AddDays(1);
                        if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                        {
                            r.FecProgramada = r.FecProgramada.Value.AddDays(1);
                        }

                        foreach (var t in lstTurnos)
                        {
                            r.FecProgramada = Convert.ToDateTime(r.FecProgramada.Value.ToShortDateString() + ' ' + t.THrI + ':' + t.TMinI);
                            // Guardamos su fec. de program).
                            GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                            GeneraAviso(cnxSqlMT, r, pathLog, r.CodDepartamento, r.CentroCostos);
                        }
                    }
                }
                else
                {
                    // Si es el dia de la programacion se calcula al nueva programacion
                    if (r.FecProgramada.Value.Date == DateTime.Now.Date)
                    {
                        if (r.IdFrecuencia == "D")
                        {
                            r.FecProgramada = Convert.ToDateTime(r.FecProgramada).AddDays(r.Frecuencia);
                            if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                            {
                                r.FecProgramada = r.FecProgramada.Value.AddDays(1);
                            }

                            // Guardamos su fec. de program.
                            GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                            GeneraAviso(cnxSqlMT, r, pathLog, r.CodDepartamento, r.CentroCostos);
                        }else
                        if (r.IdFrecuencia == "HR")
                        {
                            r.FecProgramada = Convert.ToDateTime(r.FecProgramada).AddHours(r.Frecuencia);
                            if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                            {
                                r.FecProgramada = r.FecProgramada.Value.AddHours(1);
                            }

                            // Guardamos su fec. de program.
                            GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                            GeneraAviso(cnxSqlMT, r, pathLog, r.CodDepartamento, r.CentroCostos);
                        }else
                        if (r.IdFrecuencia == "MN")
                        {
                            r.FecProgramada = Convert.ToDateTime(r.FecProgramada).AddMinutes(r.Frecuencia);
                            if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                            {
                                r.FecProgramada = r.FecProgramada.Value.AddMinutes(1);
                            }

                            // Guardamos su fec. de program.
                            GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                            GeneraAviso(cnxSqlMT, r, pathLog, r.CodDepartamento, r.CentroCostos);
                        }else
                        if (r.IdFrecuencia == "TUR")
                        {
                            // Obtener turnos definidos para el centro de costos
                            List<Turno> lstTurnos = new List<Turno>();

                            lstTurnos = GetTurnos(cnxSqlMT, r.Planta, r.CodDepartamento, r.CentroCostos);

                            //empezamos aplicar el calculo de fecha de programacion fecha de hoy + frecuencia 
                            r.FecProgramada = Convert.ToDateTime(r.FecProgramada).AddDays(r.Frecuencia); ;
                            if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                            {
                                r.FecProgramada = r.FecProgramada.Value.AddDays(1);
                            }

                            foreach (var t in lstTurnos)
                            {
                                r.FecProgramada = Convert.ToDateTime(r.FecProgramada.Value.ToShortDateString() + ' ' + t.THrI + ':' + t.TMinI);
                                // Guardamos su fec. de program).
                                GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                                GeneraAviso(cnxSqlMT, r, pathLog, r.CodDepartamento, r.CentroCostos);
                            }
                        }
                    }
                }

                // Si por alguna razon no se actualizo y se atrazo la fecha de programacion y es menor a la fecha de hoy
                // se generar como si fuera la primera
                if (r.FecProgramada.Value.Date <= DateTime.Now.Date)
                {
                    if (r.IdFrecuencia == "D")
                    {
                        r.FecProgramada = DateTime.Now.AddDays(1);
                        if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                        {
                            r.FecProgramada = r.FecProgramada.Value.AddDays(1);
                        }
                    }else if (r.IdFrecuencia == "HR")
                    {
                        r.FecProgramada = DateTime.Now.AddHours(1);
                        if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                        {
                            r.FecProgramada = r.FecProgramada.Value.AddHours(r.Frecuencia);
                        }
                    }else
                    if (r.IdFrecuencia == "MN")
                    {
                        r.FecProgramada = DateTime.Now.AddMinutes(1);
                        if ((int)r.FecProgramada.Value.DayOfWeek == 0) // Domingo
                        {
                            r.FecProgramada = r.FecProgramada.Value.AddMinutes(r.Frecuencia);
                        }
                    }
                    // Guardamos su fec. de program.
                    GuarfaFechaProgram(cnxSqlMT, r, pathLog);
                    GeneraAviso(cnxSqlMT, r, pathLog, r.CodDepartamento, r.CentroCostos);

                }

            }


            return;
        }

        private List<Turno> GetTurnos(string cnxSqlMT, int pPlanta, string pDepto, string pCtroCtos)
        {
            List<Turno> lstTurnos = new List<Turno>();

            SqlRepository repo = new SqlRepository();
            List<DataRow> lstDatos = null;

            lstDatos = repo.GetCatTurnos(cnxSqlMT, pPlanta, pDepto, pCtroCtos);

            //Generamos los turnos

            if (lstDatos.Count !=0)
            {
                Turno tur = new Turno();
                DataRow dr = lstDatos[0];

                // Debido a que el tpm-Mantenimiento no controla turnos
                // estos son parametrizables en caso de que cambien
                // Generamos un registro por cada Turno

                tur.CtroCtosSap = dr["CtroCtosSap"].ToString();
                tur.Depto = dr["Depto"].ToString();
                tur.Planta = (int)dr["Planta"];

                tur.IdTurno = 1;
                tur.THrI = (int)dr["Turno1HrIni"];
                tur.TMinI = (int)dr["Turno1MinIni"];
                tur.THrF = (int)dr["Turno1HrFin"];
                tur.TMinF = (int)dr["Turno1MinFin"];
                tur.TurnoI = tur.THrI.ToString().PadLeft(2,'0') + ":" + tur.TMinI.ToString().PadLeft(2, '0');
                tur.TurnoF = tur.THrF.ToString().PadLeft(2, '0') + ":" + tur.TMinF.ToString().PadLeft(2, '0');

                lstTurnos.Add(tur);

                tur = new Turno();
                tur.IdTurno = 2;
                tur.CtroCtosSap = dr["CtroCtosSap"].ToString();
                tur.Depto = dr["Depto"].ToString();
                tur.Planta = (int)dr["Planta"];
                tur.THrI = (int)dr["Turno2HrIni"];
                tur.TMinI = (int)dr["Turno2MinIni"];
                tur.THrF = (int)dr["Turno2HrFin"];
                tur.TMinF = (int)dr["Turno2MinFin"];
                tur.TurnoI = tur.THrI.ToString().PadLeft(2, '0') + ":" + tur.TMinI.ToString().PadLeft(2, '0');
                tur.TurnoF = tur.THrF.ToString().PadLeft(2, '0') + ":" + tur.TMinF.ToString().PadLeft(2, '0');

                lstTurnos.Add(tur);


                tur = new Turno();
                tur.CtroCtosSap = dr["CtroCtosSap"].ToString();
                tur.Depto = dr["Depto"].ToString();
                tur.Planta = (int)dr["Planta"];
                tur.THrI = (int)dr["Turno3HrIni"];
                tur.TMinI = (int)dr["Turno3MinIni"];
                tur.THrF = (int)dr["Turno3HrFin"];
                tur.TMinF = (int)dr["Turno3MinFin"];
                tur.IdTurno = 3;
                tur.TurnoI = tur.THrI.ToString().PadLeft(2, '0') + ":" + tur.TMinI.ToString().PadLeft(2, '0');
                tur.TurnoF = tur.THrF.ToString().PadLeft(2, '0') + ":" + tur.TMinF.ToString().PadLeft(2, '0');

                lstTurnos.Add(tur);
            }

            return (lstTurnos);
        }

        private int GuarfaFechaProgram(string cnxSqlMT, CheckListEqEnc re, string pathLog)
        {
            int result = 0;
            SqlRepository repo = new SqlRepository();

            result = repo.updateFecProg(cnxSqlMT, re, pathLog);

            return (result);
        }

        public void GeneraAviso(string cnxSqlMT, CheckListEqEnc chk, string rutalog, string pDepto, string pCtroCtos)
        {
            int idNew = 0;
            int result = 0;
            SqlRepository repo = new SqlRepository();
            BL_CapturaChklst bl_CapchkxEquipo = new BL_CapturaChklst();
            Tools tool = new Tools();

            //chk.IniProgram = DateTime.Now;

            // 2.- Insertar un registro del  encabezado y el detalle de cada uno en las tablas de captura de checklist
            idNew = repo.GuardarchkEncProg(cnxSqlMT, chk);

            // Obtenemos el detalle y lo guardamos
            List<CheckListEqDt> lstChckActEq = new List<CheckListEqDt>();
            lstChckActEq = bl_CapchkxEquipo.GetDatosChkxEqDet(cnxSqlMT, chk.IdChkEquipo, pDepto);
            foreach (var i in lstChckActEq)
            {
                i.IdChkEquipo = idNew;
                i.CodWorkCenter = chk.CodWorkCenter;
                i.CodEquipo = chk.CodEquipo;
                i.IdCheckList = chk.IdCheckList;
                i.CodChkList = chk.CodChkList;
                i.ResultActiv = false;
                i.ResultMedible = null;
                i.ResultVisual = null;
                if (i.TipoOperacion == "V") // Visual
                {
                    i.RangoMin = 0;
                    i.RangoMax = 0;
                    i.OperadorMin = "";
                    i.OperadorMax = "";
                }
                if (i.TipoOperacion == "M") // Medible
                {
                    i.OperadorMin = i.OperadorMin == null ? "" : i.OperadorMin;
                    i.OperadorMax = i.OperadorMax == null ? "" : i.OperadorMax;
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

                result = repo.GuardarchkActProg(cnxSqlMT, i, rutalog);
            }

            if (result == 1)
            {
                DatosCorreo datosEmail = new DatosCorreo();
                // 3-  Enviar un correo con informacion del checklist que debe realizarse

                // Asignar responsable de check list
                datosEmail.To = new List<string>();
                datosEmail.To.Add("victor.rodriguez1@magna.com");
                datosEmail.To.Add("victor.rodriguez1@magna.com");
                datosEmail.Subject = "";
                datosEmail.Mensaje = "";

                //datosEmail.To = bl_tickets.EmailsTpm(cnxSqlMT, rutaLog, vCostos);

                string textoEnb = "TPM-Mantenimiento ";
                textoEnb = textoEnb + ", Work Center: " + chk.CodWorkCenter;
                textoEnb = textoEnb + ", CHECKLIST: " + chk.CodClasif + ", " + chk.DescripChkList;

                datosEmail.Subject = textoEnb;

                textoEnb = "CHECKLIST: " + chk.CodClasif + ", " + chk.DescripChkList + "<br>";
                textoEnb = textoEnb + "Work Center: " + chk.CodWorkCenter;

                datosEmail.Mensaje =
                                     "<Div>" +
                                     "<h3 style='border-bottom: 2px solid black;'> Recordatorio:  <strong></h3>" +
                                     "<h4> Clasificación del Checklist: " + chk.CodClasif + " </h4>" +
                                     "<h4> Descripción: " + chk.DescripChkList.ToUpper() + " </h4>" +
                                     "Work Center: <strong>" + chk.CodWorkCenter + "</strong><br>" +
                                     "Frecuencia: <strong> [" + chk.IdFrecuencia + "]  " + chk.DesripFrencu + "</strong><br>" +
                                     "Fecha de programada: <strong> " + chk.FecProgramada.Value.ToShortDateString() + "</strong><br>" +
                                     "<br><br>" +
                                      "<h3> ID del Checklist: <strong> " + idNew.ToString() + "</strong></h3> <br>" +

                                    "<p style='background-color: #b3e0ff;'> &copy; @" + DateTime.Now.Year + " - <strong> Magna, Cosma Mx </strong>,  Tecnologías de la Información </p>"
                                + "<Div>";

                tool.EnviarCorreo(datosEmail);
            }
        }
    }
}
