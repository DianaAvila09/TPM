using Entidades;
using RepositorySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusinessLogic
{
    public class BL_KpiTpm
    {
        public List<Mes> GetMeses(string cnxSql)
        {
            SqlRepository repoSql = new SqlRepository();

            List<Mes> lstTemp = repoSql.GetCatMeses(cnxSql);

            return (lstTemp);
        }


        public List<Meses> GetMesesNew()
        {
            List<Meses> temp = new List<Meses>();
            temp.Add(new Meses { Mes = 1, DescripMes = "Enero" });
            temp.Add(new Meses { Mes = 2, DescripMes = "Febrero" });
            temp.Add(new Meses { Mes = 3, DescripMes = "Marzo" });
            temp.Add(new Meses { Mes = 4, DescripMes = "Abril" });
            temp.Add(new Meses { Mes = 5, DescripMes = "Mayo" });
            temp.Add(new Meses { Mes = 6, DescripMes = "Junio" });
            temp.Add(new Meses { Mes = 7, DescripMes = "Julio" });
            temp.Add(new Meses { Mes = 8, DescripMes = "Agosto" });
            temp.Add(new Meses { Mes = 9, DescripMes = "Septiembre" });
            temp.Add(new Meses { Mes = 10, DescripMes = "Octubre" });
            temp.Add(new Meses { Mes = 11, DescripMes = "Noviembre" });
            temp.Add(new Meses { Mes = 12, DescripMes = "Diciembre" });

            return (temp);

        }
        public List<EquipoPadre> GetEquiPadres(string cnxSqlMT, string pCtroCtos)
        {
            SqlRepository repoSql = new SqlRepository();
            List<EquipoPadre> temp = repoSql.GetCatEquiposPadres(cnxSqlMT, pCtroCtos);
            temp.Add(new EquipoPadre { CodEquipo = "Todos", Cod_Descrip = " Todos" });
            return (temp);
        }


        public List<EquipoPadre> GetEqyWc(string cnxSqlMT, string pCtroCtos)
        {
            SqlRepository repoSql = new SqlRepository();
            List<EquipoPadre> temp = repoSql.GetCatEqyWc(cnxSqlMT, pCtroCtos);
            temp.Add(new EquipoPadre { CodWorkCenter = "Todos", Cod_Descrip = " Todos", CodEquipo = "Todos" });
            return (temp.OrderBy(x => x.Cod_Descrip).ToList());
        }

        public List<KpiTpmCumpl> GetGrafCumpl(string cnxSqlMT, ParamKpiTpm param)
        {
            //  Calcular el Tpm de cumplimiento por mes, ya tenemos el historico diario, vamos a considerar
            //     solo el del fin de mes

            SqlRepository repo = new SqlRepository();
            List<KpiTpmCumpl> cumpl = new List<KpiTpmCumpl>();

            // 1.- Llenar los meses y el ult. dia de cada mes, para obtener los calculos historicos

            List<Mes> lstMeses = new List<Mes>();
            List<Periodos> lstPeriodos = new List<Periodos>();

            lstMeses = repo.GetCatMeses(cnxSqlMT);

            lstPeriodos = repo.GenPeriodos(lstMeses, param.AnioIni, param.MesIni, param.AnioFin, param.MesFin);

            List<EquipoTpmBasico> lstDatos = new List<EquipoTpmBasico>();
            foreach (var j in lstPeriodos)
            {
                KpiTpmCumpl c = new KpiTpmCumpl();

                c.anio = j.Anio;
                c.Mes = j.Mes;
                c.Periodo = j.Periodo;
                c.MetaCumpl = param.MetaCumpl;
                c.NoRealizados = 0;
                c.NumEquipos = 0;
                c.Cumplimiento = 0;

                // 2.- Obtener datos de los Equipos con PM Standar Mayor a cero
                // osea que ya tienen un plan de mantenimiento

                lstDatos = repo.GetEqCalculo(cnxSqlMT, param.Planta, param.Depto, param.CtroCostos, j.Anio, j.Mes);
                if (lstDatos.Count > 0)
                {
                    c.NumEquipos = lstDatos.Count(x => x.PmStandar > 0);

                    // 3.- Obtener datos de los equipo con Avance mayor al 99 %
                    //  Esto indica que no se les ha hecho un mantto.
                    int vLimite = 99;
                    c.NoRealizados = lstDatos.Count(x => x.PmStandar > 0 && x.Avance > vLimite);

                    // 4.- Calculo del % de cumplimiento : 100 - (No Realizados / total de equipos)
                    c.Cumplimiento = 100 - ((c.NoRealizados / c.NumEquipos) * 100);

                    // guardamos el ultimo mes que tenga valores para seleccionar el top 5 de Work Center
                    if (c.NumEquipos > 0)
                    {
                        param.top5Mes = c.Mes;
                        param.top5Anio = c.anio;
                    }

                }

                cumpl.Add(c);
            }

            // 4.- calculamos las linea de tendencia para los valores

            // Como el Echarts no tienen para calcular al tendecia de un valor
            // se busco la forma de calcular la regresion lineal
            // se debe de mandar como parametros el arreglo de valores en cadena...
            // para que haga un correcto calculo
            // esto de valido en Excel y es identico

            // Formula 
            // y= slope * Item + Intercep
            string[] valores = cumpl.Where(x => x.Cumplimiento > 0).Select(y => y.Cumplimiento.ToString()).ToArray();
            Tendencia trend = new Tendencia();

            if (valores.Count() > 0)
            {
                // Calculamos el Slope y el intercep para calcular la linea de tendencia lineal
                var r = trend.CalculateLinearRegression(valores);
                for (int i = 0; i < cumpl.Count; i++)
                {
                    cumpl[i].Trend = Math.Round(r.Slope * i + r.Intercept, 2);
                }
            }
            return (cumpl);
        }

        public List<KpiTpmEfic> GetGrafEfic(string cnxSqlMT, ParamKpiTpm param)
        {
            //  Calcular el Tpm de cumplimiento por mes, ya tenemos el historico diario, vamos a considerar
            //     solo el del fin de mes

            SqlRepository repo = new SqlRepository();
            List<KpiTpmEfic> lstEf = new List<KpiTpmEfic>();

            List<Mes> lstMeses = new List<Mes>();
            List<Periodos> lstPeriodos = new List<Periodos>();

            lstMeses = repo.GetCatMeses(cnxSqlMT);

            lstPeriodos = repo.GenPeriodos(lstMeses, param.AnioIni, param.MesIni, param.AnioFin, param.MesFin);

            List<EquipoTpmBasico> lstDatos = new List<EquipoTpmBasico>();

            foreach (var j in lstPeriodos)
            {
                KpiTpmEfic e = new KpiTpmEfic();

                e.Anio = j.Anio;
                e.Mes = j.Mes;
                e.Periodo = j.Periodo;
                e.MetaEfic = param.MetaEfic;
                e.PMStandar = 0;
                e.PmReal = 0;
                e.Eficiencia = 0;

                // 2.- Obtener datos de los Equipos con PM Standar y PM Real
                lstDatos = repo.GetEqCalculo(cnxSqlMT, param.Planta, param.Depto, param.CtroCostos, j.Anio, j.Mes);
                if (lstDatos.Count > 0)
                {
                    //Obtenemos la suma del PM Standar y PM Real
                    e.PMStandar = lstDatos.Sum(x => x.PmStandar);
                    e.PmReal = lstDatos.Sum(x => x.PmReal);

                    // 4.- Calculo del % de Eficiencia Sum(Pm Real) / sum(Pm Real)
                    e.Eficiencia = Math.Round((e.PmReal / e.PMStandar) * 100, 2);
                }

                // guardamos el ultimo mes que tenga valores para seleccionar el top 5 de Work Center
                if (lstDatos.Count > 0)
                {
                    param.top5Mes = e.Mes;
                    param.top5Anio = e.Anio;
                }

                lstEf.Add(e);
            }

            // 4.- calculamos las linea de tendencia para los valores

            // Como el Echarts no tienen para calcular al tendecia de un valor
            // se busco la forma de calcular la regresion lineal
            // se debe de mandar como parametros el arreglo de valores en cadena...
            // para que haga un correcto calculo
            // esto de valido en Excel y es identico

            // Formula 
            // y= slope * Item + Intercep
            string[] valores = lstEf.Where(x => x.Eficiencia > 0).Select(y => y.Eficiencia.ToString("###.##")).ToArray();
            Tendencia trend = new Tendencia();
            if (valores.Count() > 0)
            {
                // Calculamos el Slope y el intercep para calcular la linea de tendencia lineal
                var r = trend.CalculateLinearRegression(valores);
                for (int i = 0; i < lstEf.Count; i++)
                {
                    lstEf[i].Trend = Math.Round(r.Slope * i + r.Intercept, 2);
                }
            }
            return (lstEf);
        }

        public List<KpiTop5> GetTop5Efic(string cnxSqlMT, ParamKpiTpm param)
        {
            SqlRepository repo = new SqlRepository();
            List<EquipoTpmBasico> lstDatos = new List<EquipoTpmBasico>();
            List<Mes> lstMeses = new List<Mes>();
            lstMeses = repo.GetCatMeses(cnxSqlMT);

            lstDatos = repo.GetEqCalculo(cnxSqlMT, param.Planta, param.Depto, param.CtroCostos, param.top5Anio, param.top5Mes);

            var top5 = lstDatos.Where(y => y.PmStandar > 0).Select(j => new { j.CodWorkCenter, j.NumFallas }).OrderByDescending(y => y.NumFallas).Take(5);

            List<KpiTop5> lstTop5 = new List<KpiTop5>();
            foreach (var t in top5)
            {
                KpiTop5 nt = new KpiTop5();
                nt.WorkCenter = t.CodWorkCenter;
                nt.NumFallas = t.NumFallas;
                lstTop5.Add(nt);
            }

            return (lstTop5.OrderByDescending(y => y.NumFallas).ToList());
        }

        public List<EquipoTpmBasico> GetTop5Cumpli(string cnxSqlMT, ParamKpiTpm param)
        {
            SqlRepository repo = new SqlRepository();
            List<EquipoTpmBasico> lstDatos = new List<EquipoTpmBasico>();
            List<Mes> lstMeses = new List<Mes>();
            lstMeses = repo.GetCatMeses(cnxSqlMT);

            DateTime fec = DateTime.Now;
            var x = lstMeses.Where(y => y.NumMes == fec.Month).First();
            lstDatos = repo.GetEqCalculo(cnxSqlMT, param.Planta, param.Depto, param.CtroCostos, param.top5Anio, param.top5Mes);

            var top5 = lstDatos.Where(y => y.PmStandar > 0).Select(j => new { j.CodWorkCenter, j.PorcAvance }).OrderByDescending(y => y.PorcAvance).Take(5);

            List<EquipoTpmBasico> lstTop5 = new List<EquipoTpmBasico>();
            foreach (var t in top5)
            {
                EquipoTpmBasico nt = new EquipoTpmBasico();
                nt.CodWorkCenter = t.CodWorkCenter;
                nt.PorcAvance = t.PorcAvance;
                lstTop5.Add(nt);
            }

            return (lstTop5.OrderByDescending(y => y.PorcAvance).ToList());
        }

        public List<KpiTicket> GetAnalisisTickets(string cnxSqlMT, ParamKpiTpm param)
        {

            SqlRepository repo = new SqlRepository();
            List<KpiTicket> lstResul = new List<KpiTicket>();
            List<KpiTicket> lstTemp = new List<KpiTicket>();

            List<DataRow> lstDr = new List<DataRow>();

            // 1.- Llenar los meses y el ult. dia de cada mes, para obtener los calculos historicos
            List<Mes> lstMeses = new List<Mes>();
            List<Periodos> lstPeriodos = new List<Periodos>();

            lstMeses = repo.GetCatMeses(cnxSqlMT);

            lstPeriodos = repo.GenPeriodos(lstMeses, param.AnioIni, param.MesIni, param.AnioFin, param.MesFin);

            DateTime fecIni = Convert.ToDateTime(lstPeriodos[0].Anio.ToString() + "-" + lstPeriodos[0].IniMes);
            DateTime fecFin = Convert.ToDateTime(lstPeriodos[lstPeriodos.Count - 1].Anio.ToString() + "-" + lstPeriodos[lstPeriodos.Count - 1].FinMes);

            // generamos el resumen de tickets del periodo solicitado
            lstDr = repo.GenResumenTick(cnxSqlMT, param.Planta, param.Depto, param.CtroCostos, fecIni, fecFin);

            foreach (var t in lstDr)
            {
                KpiTicket e = new KpiTicket();
                e.Anio = (int)t["Anio"];
                e.Mes = (int)t["Mes"];
                e.Periodo = e.Anio.ToString().Trim() + "-" + e.Mes.ToString().Trim().PadLeft(2, '0');
                e.Total = (int)t["TotTickets"];
                e.Cerrados = (int)t["Cerrados"];
                e.Abiertos = (int)t["Abiertos"];
                e.Nuevos = (int)t["Nuevos"];
                e.EnProceso = (int)t["EnProceso"];
                e.Alertas = (int)t["Alertas"];
                e.Criticos = (int)t["Criticos"];
                e.Pky = (int)t["Pky"];
                e.Calidad = (int)t["Calidad"];
                e.Mejoras = (int)t["Mejoras"];
                e.Mantto = (int)t["Manto"];
                e.Vacio = 0;
                lstTemp.Add(e);
            }

            int index = 0;
            foreach (var j in lstPeriodos)
            {
                KpiTicket e = new KpiTicket();

                e.Anio = j.Anio;
                e.Mes = j.Mes;
                e.Periodo = j.Periodo;
                e.Trend = 0;



                index = lstTemp.FindIndex(x => x.Periodo == j.Periodo);
                if (index >= 0)
                {
                    e.Total = lstTemp[index].Total;
                    e.Cerrados = lstTemp[index].Cerrados;
                    e.Abiertos = lstTemp[index].Abiertos;
                    e.Nuevos = lstTemp[index].Nuevos;
                    e.EnProceso = lstTemp[index].EnProceso;
                    e.Alertas = lstTemp[index].Alertas;
                    e.Criticos = lstTemp[index].Criticos;
                    e.Pky = lstTemp[index].Pky;
                    e.Calidad = lstTemp[index].Calidad;
                    e.Mejoras = lstTemp[index].Mejoras;
                    e.Mantto = lstTemp[index].Mantto;

                    // guardamos el ultimo mes que tenga valores para seleccionar el top 5 de Work Center
                    if (e.Total > 0)
                    {
                        param.top5Mes = e.Mes;
                        param.top5Anio = e.Anio;
                    }
                }

                lstResul.Add(e);
            }


            // 4.- calculamos las linea de tendencia para los valores

            // Como el Echarts no tienen para calcular al tendecia de un valor
            // se busco la forma de calcular la regresion lineal
            // se debe de mandar como parametros el arreglo de valores en cadena...
            // para que haga un correcto calculo
            // esto de valido en Excel y es identico

            // Formula 
            // y= slope * Item + Intercep
            string[] valores = lstResul.Where(x => x.Total > 0).Select(y => y.Total.ToString("#####")).ToArray();
            Tendencia trend = new Tendencia();
            if (valores.Count() > 0)
            {
                // Calculamos el Slope y el intercep para calcular la linea de tendencia lineal
                var r = trend.CalculateLinearRegression(valores);

                for (int i = 0; i < lstResul.Count; i++)
                {
                    lstResul[i].Trend = Math.Round(r.Slope * i + r.Intercept, 2);
                }
            }
            return (lstResul);
        }

        public List<Top5WCTick> GetTop5WcTivk(string cnxSqlMT, int planta, string depto, string cCostos, int Anio, int Mes)
        {
            List<DataRow> lstDr = new List<DataRow>();
            List<Top5WCTick> lstTemp = new List<Top5WCTick>();
            SqlRepository repo = new SqlRepository();
            // generamos el resumen de tickets del periodo solicitado
            lstDr = repo.GenTop5WcTick(cnxSqlMT, planta, depto, cCostos, Anio, Mes);

            if (lstDr.Count > 0)
            {
                foreach (var t in lstDr)
                {
                    Top5WCTick e = new Top5WCTick();
                    e.Anio = (int)t["Anio"];
                    e.Mes = (int)t["Mes"];
                    e.CodWorkCenter = t["CodWorkCenter"].ToString();
                    e.TotalFallas = (int)t["TotTickets"];
                    e.Cerrados = (int)t["Cerrados"];
                    e.Abiertos = (int)t["Abiertos"];
                    e.Vacio = 0;
                    lstTemp.Add(e);
                }
            }
            return (lstTemp);

        }

        public List<FocusFactory> GetFocus(List<EquipoPadre> lstEqPadres)
        {
            List<FocusFactory> lstFocus = new List<FocusFactory>();

            lstFocus = (from p in lstEqPadres
                        where p.FocusFactory != null
                        group p by p.FocusFactory into newGroup
                        orderby newGroup.Key
                        select new FocusFactory { Focus = newGroup.Key.Trim() }).ToList();

            lstFocus.Add(new FocusFactory { Focus = "Todos" });


            return (lstFocus.OrderBy(x => x.Focus).ToList());

        }

        public List<KpiTm> GetKpiMt(string cnxSqlMT, ParamKpiTpm param)
        {
            List<KpiTm> lstTm = new List<KpiTm>();
            List<DataRow> lstDatosMin = new List<DataRow>();
            List<DataRow> lstDatosEven = new List<DataRow>();
            //  Calcular el kpi por mes
            SqlRepository repo = new SqlRepository();

            List<Mes> lstMeses = new List<Mes>();
            List<Periodos> lstPeriodos = new List<Periodos>();

            lstMeses = repo.GetCatMeses(cnxSqlMT);

            lstPeriodos = repo.GenPeriodos(lstMeses, param.AnioIni, param.MesIni, param.AnioFin, param.MesFin);


            foreach (var j in lstPeriodos)
            {
                KpiTm e = new KpiTm();

                e.Anio = j.Anio;
                e.Mes = j.Mes;
                e.Periodo = j.Periodo;
                e.MetaMtrrAuto = param.MetaMtrrAuto;
                e.MetaMtbfAuto = param.MetaMtbfAuto;
                e.MetaMtrrMnt = param.MetaMtrrMnt;
                e.MetaMtbfMnt = param.MetaMtbfMnt;
                e.MetaMtrrTkl = param.MetaMtrrTkl;
                e.MetaMtbfTkl = param.MetaMtbfTkl;

                switch (param.tipoRep)
                {
                    case 1:
                        lstDatosMin = repo.GenKpiTmMin(cnxSqlMT, param.tipoRep, param.Planta, param.Depto, param.CtroCostos, j.Anio, j.Mes);
                        break;
                    case 2:
                        lstDatosMin = repo.GenKpiTmMin(cnxSqlMT, param.tipoRep, param.Planta, param.Depto, param.CtroCostos, j.Anio, j.Mes, param.WorkCenter);
                        break;
                }

                if (lstDatosMin.Count > 0)
                {
                    DataRow t = lstDatosMin[0];

                    e.MinAutomat = (decimal)t["Automatizacion"];
                    e.MinMantto = (decimal)t["Mantto"];
                    e.MinProgramado = (decimal)t["Programado"];
                    e.MinTroqueles = (decimal)t["Troqueles"];
                    e.MinParoProd = (decimal)t["ParoProd"];
                    e.MinLogistica = (decimal)t["Logistica"];
                    e.MinError = (decimal)t["Error"];
                    e.MinCorriendo = (decimal)t["Corriendo"];
                    e.MinCalidad = (decimal)t["Calidad"];
                    e.TotalMinutos = (decimal)t["TotalMinutos"];
                }

                e.EventosAutoma = 0;
                e.EventosMantto = 0;
                e.EventosTroqueles = 0;

                switch (param.tipoRep)
                {
                    case 1:
                        lstDatosEven = repo.GenKpiTmEventos(cnxSqlMT, param.tipoRep, param.Planta, param.Depto, param.CtroCostos, j.Anio, j.Mes);
                        break;
                    case 2:
                        lstDatosEven = repo.GenKpiTmEventos(cnxSqlMT, param.tipoRep, param.Planta, param.Depto, param.CtroCostos, j.Anio, j.Mes, param.WorkCenter);
                        break;
                }

                if (lstDatosEven.Count > 0)
                {
                    foreach (var t in lstDatosEven)
                    {

                        switch ((int)t["status"])
                        {
                            case 6:
                            case 10:
                                e.EventosAutoma = e.EventosAutoma + (int)t["eventos"];
                                break;
                            case 7:
                            case 11:
                                e.EventosMantto = e.EventosMantto + (int)t["eventos"];
                                break;
                            case 5:
                            case 9:
                                e.EventosTroqueles = e.EventosTroqueles + (int)t["eventos"];
                                break;
                        }
                    }
                }

                e.MttrAuto = e.EventosAutoma > 0 ? Math.Round(e.MinAutomat / e.EventosAutoma, 2) : 0;
                e.MtbfAuto = e.EventosAutoma > 0 ? Math.Round((e.TotalMinutos - e.MinProgramado - e.MinAutomat) / e.EventosAutoma, 2) : 0;

                if ((e.Anio == 2020 && e.Mes == 4) || (e.Anio == 2020 && e.Mes == 5))
                {
                    e.MttrManto = 0;
                    e.MtbfManto = 0;
                }
                else
                {
                    e.MttrManto = e.EventosMantto > 0 ? Math.Round(e.MinMantto / e.EventosMantto, 2) : 0;
                    e.MtbfManto = e.EventosMantto > 0 ? Math.Round((e.TotalMinutos - e.MinProgramado - e.MinMantto) / e.EventosMantto, 2) : 0;
                }
                e.MttrTroquel = e.EventosTroqueles > 0 ? Math.Round(e.MinTroqueles / e.EventosTroqueles, 2) : 0;
                e.MtbfTroquel = e.EventosTroqueles > 0 ? Math.Round((e.TotalMinutos - e.MinProgramado - e.MinTroqueles) / e.EventosTroqueles, 2) : 0;

                lstTm.Add(e);
            }

            // 4.- calculamos las linea de tendencia para los valores

            // Como el Echarts no tienen para calcular al tendecia de un valor
            // se busco la forma de calcular la regresion lineal
            // se debe de mandar como parametros el arreglo de valores en cadena...
            // para que haga un correcto calculo
            // esto de valido en Excel y es identico

            // Formula 
            // y= slope * Item + Intercep

            //string[] valores = lstEf.Where(x => x.Eficiencia > 0).Select(y => y.Eficiencia.ToString("###.##")).ToArray();
            //Tendencia trend = new Tendencia();
            //if (valores.Count() > 0)
            //{
            //    // Calculamos el Slope y el intercep para calcular la linea de tendencia lineal
            //    var r = trend.CalculateLinearRegression(valores);
            //    for (int i = 0; i < lstEf.Count; i++)
            //    {
            //        lstEf[i].Trend = Math.Round(r.Slope * i + r.Intercept, 2);
            //    }
            //}

            return (lstTm);
        }

        public List<KpiPkyk> GetGrafPkykHt(string cnxSqlMT, ParamKpiPkyk param)
        {
            List<KpiPkyk> lstResul = new List<KpiPkyk>();
            List<KpiPkyk> lstResulMes = new List<KpiPkyk>();

            SqlRepository repo = new SqlRepository();
            List<DataRow> lstDr = new List<DataRow>();

            // generamos el resumen de tickets del periodo solicitado
            lstDr = repo.GetHtpkyk(cnxSqlMT, param.Planta, param.Depto, param.AnioIni, param.MesIni, param.TipoTick);


            foreach (var t in lstDr)
            {
                KpiPkyk e = new KpiPkyk();
                e.CodWorkCenter = t["CodWorkCenter"].ToString();
                e.CodEquipo = t["CodEquipo"].ToString();
                e.Sensor = t["Sensor"].ToString();
                e.EventosHist = (int)t["Eventos"];
                lstResul.Add(e);
            }

            // generamos el resumen de tickets del periodo solicitado
            lstDr = repo.GetpkykMes(cnxSqlMT, param.Planta, param.Depto, param.TipoTick, param.MesAtras);

            int index = 0;
            foreach (var t in lstDr)
            {
                KpiPkyk e = new KpiPkyk();
                e.CodWorkCenter = t["CodWorkCenter"].ToString();
                e.CodEquipo = t["CodEquipo"].ToString();
                e.Sensor = t["Sensor"].ToString();
                e.EventosMes = (int)t["Eventos"];

                index = lstResul.FindIndex(x => x.Sensor == e.Sensor && x.CodEquipo == e.CodEquipo && e.CodWorkCenter == x.CodWorkCenter);
                if (index >= 0)
                {
                    lstResul[index].EventosMes = e.EventosMes;
                }
            }

            return (lstResul.OrderByDescending(x => x.EventosHist).ToList());
        }

       

        public List<KpiPkykMes> GetGrafPkykMes(string cnxSqlMT, ParamKpiPkyk param)
        {

            List<KpiPkykMes> lstResulMes = new List<KpiPkykMes>();

            SqlRepository repo = new SqlRepository();
            List<DataRow> lstDatos = new List<DataRow>();

            List<Mes> lstMeses = new List<Mes>();
            List<Periodos> lstPeriodos = new List<Periodos>();

            lstMeses = repo.GetCatMeses(cnxSqlMT);
            lstPeriodos = repo.GenPeriodos(lstMeses, param.AnioIni, param.MesIni, param.AnioFin, param.MesFin);
            int t = 1;
            foreach (var j in lstPeriodos)
            {
                KpiPkykMes e = new KpiPkykMes();

                e.Periodo = j.Periodo;
                e.Anio = j.Anio;
                e.Mes = j.Mes;

                e.Eventos = 0;
                // Obtener los eventos del mes considerando 2 mese atras, es decir si calculamos febrero, 
                // tendriamos que tomar como referencia de enero a febrero
                // asi lo pidio Ingo B.
                // se cambia a un mes
                if (t != lstPeriodos.Count)
                {
                    lstDatos = repo.GetEventosxMes(cnxSqlMT, param.Planta, param.Depto, j.Anio, j.Mes, param.MesAtras * (-1), param.TipoTick, lstMeses, false);
                }
                else
                {
                    // Si es el ultmo mes, vamos a considerar 30 dias a tras como periodo y ya
                    if (t == lstPeriodos.Count)
                    {
                        lstDatos = repo.GetEventosxMes(cnxSqlMT, param.Planta, param.Depto, j.Anio, j.Mes, param.MesAtras * (-1), param.TipoTick, lstMeses, true);
                    }
                }
                if (lstDatos.Count > 0)
                {
                    DataRow dr = lstDatos[0];
                    e.Eventos = (int)dr["eventos"];
                }

                // guardamos el ultimo mes que tenga valores para seleccionar el top 5 de Work Center
                if (lstDatos.Count > 0)
                {
                    param.Top5Mes = e.Mes;
                    param.Top5Anio = e.Anio;
                }

                lstResulMes.Add(e);
                t++;
            }

            // 4.- calculamos las linea de tendencia para los valores

            // Como el Echarts no tienen para calcular al tendecia de un valor
            // se busco la forma de calcular la regresion lineal
            // se debe de mandar como parametros el arreglo de valores en cadena...
            // para que haga un correcto calculo
            // esto de valido en Excel y es identico

            // Formula 
            // y= slope * Item + Intercep
            string[] valores = lstResulMes.Where(x => x.Eventos > 0).Select(y => y.Eventos.ToString("###.##")).ToArray();
            Tendencia trend = new Tendencia();

            if (valores.Count() > 0)
            {
                // Calculamos el Slope y el intercep para calcular la linea de tendencia lineal
                var r = trend.CalculateLinearRegression(valores);
                for (int i = 0; i < lstResulMes.Count; i++)
                {
                    lstResulMes[i].Tendencia = Math.Round(r.Slope * i + r.Intercept, 2);
                }
            }

            return (lstResulMes);
        }

        public List<KpiPkyk> GetTop5PkykMes(string cnxSqlMT, ParamKpiPkyk param)
        {
            List<KpiPkyk> lstResul = new List<KpiPkyk>();
            SqlRepository repo = new SqlRepository();
            List<DataRow> lstDr = new List<DataRow>();
            List<Mes> lstMeses = new List<Mes>();

            lstMeses = repo.GetCatMeses(cnxSqlMT);
            // generamos el resumen de tickets del periodo solicitado

            lstDr = repo.GetPkykTop5(cnxSqlMT, param.Planta, param.Depto, param.Top5Anio, param.Top5Mes, param.MesAtras * (-1), param.TipoTick, lstMeses, param.Pareto);


            if (lstDr.Count > 0)
            {
                foreach (var t in lstDr)
                {
                    KpiPkyk e = new KpiPkyk();
                    e.CodWorkCenter = t["CodWorkCenter"].ToString();
                    e.CodEquipo = t["CodEquipo"].ToString();
                    e.Sensor = t["Sensor"].ToString();
                    e.EventosHist = (int)t["Eventos"];
                    lstResul.Add(e);
                }
            }


            return (lstResul.OrderBy(x => x.EventosHist).ToList());

        }

        public List<GraficakpiChk> GetInfoChk(string cnxSqlMT, ParamKpiChklst p)
        {

            // 1 - Obtenemos los datos de los checklist programados
            SqlRepository repoSql = new SqlRepository();
            List<DataRow> lstDatos = new List<DataRow>();


            string periIni = p.AnioIni.ToString().Trim() + p.MesIni.ToString().Trim().PadLeft(2, '0');
            string periFin = p.AnioFin.ToString().Trim() + p.MesFin.ToString().Trim().PadLeft(2, '0');

            if (p.CodEquipo == "Todos")
            {
                lstDatos = repoSql.GetInfoChlst(cnxSqlMT, periIni, periFin, p.CtroCostos, p.Depto, null);
            }
            else
            {
                lstDatos = repoSql.GetInfoChlst(cnxSqlMT, periIni, periFin, p.CtroCostos, p.Depto, p.CodEquipo);
            }

            foreach (DataRow i in lstDatos)
            {
                CheckListEqEnc e = new CheckListEqEnc();

                e.Periodo = i["Periodo"].ToString();
                e.Planta = (int)i["Planta"];
                e.CentroCostos = i["CentroCostos"].ToString();
                e.CodDepartamento = i["CodDepartamento"].ToString();
                e.IdChkEquipo = (int)i["IdChkEquipo"];
                e.CodWorkCenter = i["CodWorkCenter"].ToString();
                e.CodEquipo = i["CodEquipo"].ToString();
                e.IdCheckList = (int)i["IdCheckList"];
                e.CodChkList = i["CodChkList"].ToString();
                e.DescripChkList = i["DescripChkList"].ToString();
                e.CodClasif = i["CodClasif"].ToString();
                e.IdFrecuencia = i["IdFrecuencia"].ToString();
                e.Frecuencia = (int)i["Frecuencia"];
                e.Observaciones = i["Observaciones"].ToString();
                if (i["FecProgramada"].ToString() != "") { e.FecProgramada = Convert.ToDateTime(i["FecProgramada"]); }
                else { e.FecProgramada = null; }
                if (i["FchEjecucion"].ToString() != "") { e.FchEjecucion = Convert.ToDateTime(i["FchEjecucion"]); }
                else { e.FchEjecucion = null; }

                p.lstChecklist.Add(e);
            }

            // Creamos los item de los periodos
            int x = p.AnioIni;
            int m = p.MesIni;


            p.lstGraf = GenPeriodos(p.AnioIni, p.MesIni, p.AnioFin, p.MesFin);
            // 3.- Asignar los datos por mes anio

            int index = 0;

            foreach (var d in p.lstGraf)
            {
                index = p.lstChecklist.FindIndex(y => y.Periodo.Trim() == d.Periodo.Trim());
                if (index != -1)
                {
                    var lista = p.lstChecklist.Where(s => s.Periodo == d.Periodo);

                    //Nota Importante:
                    //Para que no marque error en la consulta de linq por si algun dato viene com Null
                    //se le pone un signo "?", esto evita el error ya que controla los valores null
                    //aunque es importante ponerlo asi tambien en el modelo

                    d.QtyProg = lista.Count(s => s.Periodo == d.Periodo);
                    d.QtyOnTime = lista.Count(s => s.Periodo == d.Periodo && s.FchEjecucion?.Date != null && s.FchEjecucion?.Date == s.FecProgramada?.Date);
                    d.QtyNoExec = lista.Count(s => s.Periodo == d.Periodo && s.FchEjecucion?.Date == null);
                    d.QtyDelay = lista.Count(s => s.Periodo == d.Periodo && s.FchEjecucion?.Date != null && s.FchEjecucion?.Date > s?.FecProgramada?.Date);


                    d.OnTime = d.QtyOnTime > 0 ? Math.Round((d.QtyOnTime / d.QtyProg) * 100, 2) : 0;
                    d.Delay = d.QtyDelay > 0 ? Math.Round((d.QtyDelay / d.QtyProg) * 100, 2) : 0;
                    d.NoExecuted = Math.Round((100 - (d.OnTime + d.Delay)), 2);
                    //d.NoExecuted = d.QtyNoExec > 0 ? d.QtyNoExec / d.QtyProg : 0;

                }
            }
            return (p.lstGraf);
        }
        private static List<GraficakpiChk> GenPeriodos(int aIni, int mIni, int aFin, int mFin)
        {
            List<GraficakpiChk> lstTemp = new List<GraficakpiChk>();
            int x = aIni;
            int m = mIni;
            while (x <= aFin)
            {
                if (x == aFin)
                {
                    while (m <= mFin)
                    {
                        lstTemp.Add(new GraficakpiChk { Periodo = x.ToString().Trim() + "-" + m.ToString().Trim().PadLeft(2, '0') });
                        m = m + 1;
                    }
                }
                else
                {
                    while (m <= 12)
                    {
                        lstTemp.Add(new GraficakpiChk { Periodo = x.ToString().Trim() + "-" + m.ToString().Trim().PadLeft(2, '0') });
                        m = m + 1;
                    }
                }
                x = x + 1;
                m = 1;
            }
            return (lstTemp);
        }

        public List<WorkCter> GetCatWc(string cnxSql, string pCategoria, string pCC)
        {
            SqlRepository repoSql = new SqlRepository();

            List<WorkCter> lstTemp = repoSql.GetCatWC(cnxSql, pCategoria, pCC );

            return (lstTemp);
        }

        public List<StsBitSensor> GetCatSts(string cnxSqlMT)
        {
            SqlRepository repoSql = new SqlRepository();

            List<StsBitSensor> lstTemp = repoSql.GetCatStsBitacSensores(cnxSqlMT);

            return (lstTemp);
        }
    }
}
