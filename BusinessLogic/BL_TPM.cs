using Entidades;
using RepositorySql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace BusinessLogic
{
    public class BL_TPM
    {
        BLCatPlanesManto bl_PM = new BLCatPlanesManto();

        BL_CatalogosSap bl_sap = new BL_CatalogosSap();

        public List<EquipoTpm> DatosTpm(string cnxSql, string depto, string centroCtos, string rutaLog)
        {
            List<EquipoTpm> lstETpm = new List<EquipoTpm>();
            List<PlanMantto> lstPM = new List<PlanMantto>();
            List<CatEquipo> lstEqui = new List<CatEquipo>();
            List<WorkCenter> lstWc = new List<WorkCenter>();
            List<WorkCenter> lstMWc = new List<WorkCenter>();
            List<Ciclos> lstCiclos = new List<Ciclos>();
            List<Ticket> lstTickets = new List<Ticket>();

            SqlRepository repoSql = new SqlRepository();

            lstPM = repoSql.LeeCatPlanMantto(cnxSql, centroCtos);
            lstEqui = repoSql.GetCatEquiposPad(cnxSql, centroCtos, rutaLog);
            lstWc = repoSql.GetCatWorkCter(cnxSql, "0007");
            lstMWc = repoSql.GetCatWorkCter(cnxSql, "0005");
            lstCiclos = repoSql.LeeCatCiclos(cnxSql);
            // valor temporal 10
            lstTickets = repoSql.GetUltimaFalla(cnxSql, rutaLog, depto, 10);
            List<SistemaManto> lstSistemas = repoSql.LeeCatSistManto(cnxSql, depto);

            lstETpm = (from p in lstPM
                       join e in lstEqui on p.CodEquipo.Trim() equals e.CodEquipo.Trim() into unionPM
                       join c in lstCiclos on p.CodCiclo equals c.CodCiclo
                       join s in lstSistemas on p.CodSistema equals s.CodSistema
                       from e in unionPM.DefaultIfEmpty()
                       select new EquipoTpm()
                       {
                           Id = 0,
                           CodEquipo = p.CodEquipo,
                           CodSistema = p.CodSistema,
                           Sistema = s.Sistema,
                           CodCiclo = p.CodCiclo,
                           Ciclo = c.Descripcion,
                           Frecuencia = p.Frecuencia,
                           FecUltEjecucion = p.FecUltEjecucion,

                           WorkCenter = e.WorkCenter,
                           ObjectNumber = e.ObjectNumber,
                           DescripTechnical = e.DescripTechnical,
                           IndivStatusObject = e.IndivStatusObject,
                           TypeTechObj = e.TecObjAutGrp,
                           PlannerGroup = e.PlannerGroup,
                           CostCenter = e.CostCenter,
                           MainWorkCenter = e.MainWorkCenter,
                           FunctionalLocation = e.FunctionalLocation,
                           ManufModelNum = e.ManufModelNum,
                           ManufAsset = e.ManufAsset,
                           ValidFromDate = e.ValidFromDate,
                           Superordinate = e.Superordinate,
                           StandardTextKeyWC = e.StandardTextKeyWC,

                           PorcAvance = 0,
                           Avance = 0,
                           numfallas = 0,
                           PmStandar = 0,
                           PmReal = 0,
                           UltFalla = "",
                           TipoFalla = ""
                       }).ToList();

            foreach (var dr in lstETpm)
            {
                int index = lstMWc.FindIndex(x => x.CodWorkCenter == dr.MainWorkCenter);
                if (index >= 0)
                {
                    dr.NombreMwc = lstMWc[index].Descripcion;
                }

            }

            return (lstETpm.OrderByDescending(x => x.PorcAvance).ThenBy(y => y.CodEquipo).ToList());
        }

        public List<EquipoTpmBasico> DatosTpmBasico(string cnxSqlMT, string cnxSqlProd, string depto, int planta, string centroCtos, BarraAvance pBarra, int pMeses, string rutaLog, decimal pHrsxDia, decimal pDiasxAnio, decimal pDiasxMes, decimal pPorcFallas, string cnxSqlHT)
        {
            #region Variables
            List<EquipoTpm> lstEqTpm = new List<EquipoTpm>();
            List<EquipoTpmBasico> lstEqTpmBas = new List<EquipoTpmBasico>();
            List<Ciclos> lstCiclos = new List<Ciclos>();
            List<PmStandar> lstPmS = new List<PmStandar>();
            List<PlanMantto> lstCatPm = new List<PlanMantto>();
            List<Ticket> lstTickUltFalla = new List<Ticket>();
            List<CantFallasEquipo> lstNumFallasxEq = new List<CantFallasEquipo>();
            List<CantFallasEquipo> lstTickHallazgos = new List<CantFallasEquipo>();
            List<ProducNotificada> lstProdNotifMat = new List<ProducNotificada>();
            List<ProducNotificada> lstProdNotifSumWC = new List<ProducNotificada>();

            List<WcReportables> lstWcr = new List<WcReportables>();
            List<ProducNotificada> lstPnm = new List<ProducNotificada>();

            SqlRepository repoSql = new SqlRepository();
            int index = 0;

            #endregion

            #region Generamos los equipos padres del tpm

            // 1.- catalogo de ciclos
            lstCiclos = repoSql.LeeCatCiclos(cnxSqlMT);

            // 2.- Generamos los equipos padres
            List<EquipoPadre> lstEquiPadre = new List<EquipoPadre>();
            lstEquiPadre = repoSql.GetCatEquiposPadres(cnxSqlMT, centroCtos);

            lstEqTpmBas = (from p in lstEquiPadre
                           select new EquipoTpmBasico()
                           {
                               Id = 0,
                               CodWorkCenter = p.CodWorkCenter,
                               CodEquipo = p.CodEquipo,
                               DescripTechnical = p.DescripTechnical,
                               TypeTechObj = p.TypeTechObj,
                               CostCenter = p.CostCenter,
                               MainWorkCenter = p.MainWorkCenter,
                               FunctionalLocation = p.FunctionalLocation,
                               ValidFromDate = p.ValidFromDate,
                               ManufAsset = p.ManufAsset,
                               ManufModelNum = p.ManufModelNum,
                               IndivStatusObject = p.IndivStatusObject,
                               PlannerGroup = p.PlannerGroup,
                               Superordinate = p.Superordinate,
                               StandardTextKeyWC = p.StandardTextKeyWC,
                               Codciclo = "",
                               Ciclo = "",
                               Frecuencia = 0,
                               PorcAvance = 0,
                               Avance = 0,
                               NumFallas = 0,
                               PmReal = 0,
                               PmStandar = 0,
                               PzasProducidas = repoSql.GetProducMatxWc(cnxSqlHT, p.CodWorkCenter),
                               PzasScrap = 0,
                               UltFalla = "",
                               TipoFalla = "",
                               Barra = new BarraAvance() { Verde = 0, Amarilla = 0, Roja = 0, Blanca = 0 }
                           }).OrderBy(x => x.CodEquipo).ToList();

            #endregion



            #region Calculamos la cantidad de fallas x equipo cualquier tipo menos las de mantenimiento

            lstNumFallasxEq = repoSql.GetFallasxEquipo(cnxSqlMT, pMeses, rutaLog);
            index = 0;
            foreach (var dr in lstNumFallasxEq)
            {

                index = lstEqTpmBas.FindIndex(x => x.CodEquipo.Trim() == dr.CodEquipo.Trim());
                if (index >= 0)
                {
                    lstEqTpmBas[index].NumFallas = dr.NumFallas;
                }
            }
            #endregion

            #region  Buscamos la ultima falla reportada
            //
            lstTickUltFalla = repoSql.GetUltimaFalla(cnxSqlMT, rutaLog, depto, planta);

            foreach (var dr in lstTickUltFalla)
            {
                index = lstEqTpmBas.FindIndex(x => x.CodEquipo.Trim() == dr.CodEquipo.Trim());
                if (index >= 0)
                {
                    lstEqTpmBas[index].UltFalla = dr.IdTicket.ToString();
                    lstEqTpmBas[index].TipoFalla = dr.TipoTicket;
                    lstEqTpmBas[index].Sensor = dr.Sensor;
                }
            }

            foreach (var dr in lstEqTpmBas)
            {
                if (string.IsNullOrEmpty(dr.TipoFalla))
                {
                    dr.TipoFalla = "X";
                }
            }
            #endregion

            #region  Buscamos los tickets de hallazgos de segurida abiertos
            //
            lstTickHallazgos = repoSql.GetFallasSeguridad(cnxSqlMT, rutaLog);

            foreach (var dr in lstTickHallazgos)
            {
                index = lstEqTpmBas.FindIndex(x => x.CodEquipo == dr.CodEquipo);
                if (index >= 0)
                {
                    lstEqTpmBas[index].HallazgoSeguridad = dr.NumFallas;
                }
            }
            #endregion

            #region Calculamos el PM Standar basado en el standar PPM del equipo

            // calculo autorizado por IB
            // 1.- Obtenemos el PM standar general PPM
            lstPmS = repoSql.LeePMStandar(cnxSqlMT, centroCtos);

            // 2.- Obtenemos el Plan de Mantto del general
            lstCatPm = repoSql.LeeCatPM(cnxSqlMT, centroCtos);

            // 3.- Calculamos el PM de acuerdo PPM y la frecuencia del plan de mantto.
            foreach (var dr in lstCatPm)
            {

                index = lstPmS.FindIndex(x => x.CodEquipo.Trim() == dr.CodEquipo.Trim());
                if (index >= 0)
                {
                    var pmxHr = lstPmS[index].Ppm * 60;
                    var pmxDia = pmxHr * pHrsxDia;
                    var pmxMes = pmxDia * pDiasxMes;
                    var pmxAnio = pmxHr * (pHrsxDia * pDiasxAnio);


                    int indext = lstEqTpmBas.FindIndex(x => x.CodEquipo.Trim() == dr.CodEquipo.Trim());
                    if (indext >= 0)
                    {
                        lstEqTpmBas[indext].Frecuencia = dr.Frecuencia;
                        lstEqTpmBas[indext].Codciclo = dr.CodCiclo;
                        lstEqTpmBas[indext].FecUltManto = dr.FecUltEjecucion;

                        int indexc = lstCiclos.FindIndex(x => x.CodCiclo.Trim() == dr.CodCiclo.Trim());
                        if (indexc >= 0)
                        {
                            lstEqTpmBas[indext].Ciclo = lstCiclos[indexc].Descripcion;
                        }

                        switch (dr.CodCiclo)
                        {
                            case "A":
                                lstEqTpmBas[indext].PmStandar = (pmxAnio * dr.Frecuencia);
                                break;
                            case "M":
                                lstEqTpmBas[indext].PmStandar = (pmxMes * dr.Frecuencia);
                                break;

                            case "S":
                                lstEqTpmBas[indext].PmStandar = (pmxDia * 5 * dr.Frecuencia);

                                break;
                            case "D":
                                lstEqTpmBas[indext].PmStandar = (pmxDia * dr.Frecuencia);
                                break;
                            case "P":
                                lstEqTpmBas[indext].PmStandar = dr.Frecuencia;
                                break;
                            default:
                                break;
                        }


                        // Calculo anterior
                        // lstEqTpmBas[indext].PmReal = lstEqTpmBas[indext].PmStandar - (lstEqTpmBas[indext].NumFallas * (pPorcFallas / 100) * lstEqTpmBas[indext].PmStandar);

                        // Calculo aplicando el tope de falla al 30%

                        decimal porcjFalla = pPorcFallas / 100;
                        decimal topeFalla = (decimal)(.7) / porcjFalla;

                        if (lstEqTpmBas[indext].NumFallas > topeFalla)
                        {
                            lstEqTpmBas[indext].PmReal = lstEqTpmBas[indext].PmStandar - (lstEqTpmBas[indext].PmStandar * porcjFalla * topeFalla);
                        }
                        else
                        {                            
                            lstEqTpmBas[indext].PmReal = lstEqTpmBas[indext].PmStandar - (lstEqTpmBas[indext].PmStandar * porcjFalla * lstEqTpmBas[indext].NumFallas);
                        }

                    }
                }
            }
            #endregion


            #region Comentarios
            // Obtener produccion del periodo del ultimo mantenimiento del work center hasta la fecha
            // Cada planta tiene su particularidad

            //*****************************************************************************************
            // Concepto de Familia : Son los materiales que se producen al mismo tiempo, A y/o B, 
            // se considerara solo produccion del material con cantidad mayor. ej A= 1500 B= 1350, se tomara como produccion  a A
            // estas familas se obtendran de la notif. Automatica conocidos  como CoProductos.
            //*****************************************************************************************

            // Puebla-Prensas: Esta se basara en la piezas reportables por cada work Center, considernado el crieterio de familias. 

            // Puebla-Soldadura: Esta se basara en la piezas reportables por cada work Center, considernado el crieterio de familias. 
            //                   ademas se tienen celdas reportables y varias work Center considerados como hijos, por lo que la produccion de esta celda
            //          de le asignara a los hijos considerando la fecha de su ult. manto. preventivo.
            // !! si llevaramos el calculo por equipo este caso deberia de aplicar para todos !!

            // Morelos-Soldadura: Esta estara basada en los ciclos que se reportan en los sistemas de produccion
            // Cuautitlan-Soldadura: Esta se basara en la piezas reportables por cada work Center

            // Dedibo a que no todo es notificacion automatica , vamos a tomar la produccion del historico que se guarda diario
            // y debemos de calcular todo primero ya considernado la produccion de las celdas hijas 
            // y despues aplicar el criterio de familias 
            // sustituir dependiendo el criterio de cada planta excepto Morelos 13205.

            #endregion

            string WcPadre = "";

            // Obtenemos el catalogo de Wc 
            lstWcr = getWcReportables(cnxSqlMT, centroCtos).OrderBy(y => y.WcReportable).ToList();


            // Generamos las produccion por material del WorkCenter
            int w = 0;
            foreach (var item in lstEqTpmBas)
            {
                if (w == 0)
                {

                    if (item.CodWorkCenter == "ATK-Q22")
                    {
                        WcPadre = "";
                    }
                    WcPadre = "";
                    // Para los casos en que el work center (hijo) depende de la produccion de otro Work center(Padre)
                    // buscamos si es un Wc hijo, para calcular la produccion del Wc padre pero considerando la fecha del ultimo mantenimiento del hijo

                    // buscamos si es hijo
                    index = lstWcr.FindIndex(x => x.WcHijo.Trim() == item.CodWorkCenter.Trim());
                    if (index > 0)
                    {
                        // Obtenemos el Wc Padre
                        WcPadre = lstWcr[index].WcReportable;
                    }

                    // si es hijo le calculamos su produccion con la del padre
                    if (!string.IsNullOrEmpty(WcPadre))
                    {
                        if (item.FecUltManto.ToShortDateString() != "01/01/0001")
                        {
                            // Si tiene fecha del ult. manto. 
                            lstPnm = repoSql.GetProducMatxWc(cnxSqlHT, WcPadre, item.FecUltManto);
                            // le cambiamos el Work center por el hijo
                            foreach (var t in lstPnm)
                            { t.WorkCenter = item.CodWorkCenter; }
                            lstProdNotifMat.AddRange(lstPnm);
                        }
                        else
                        {
                            // sino tiene fec ult. manto le calculamos 30 dias atras
                            lstPnm = repoSql.GetProducMatxWc(cnxSqlHT, WcPadre, DateTime.Now.AddDays(-30));
                            // le cambiamos el Work center por el hijo
                            foreach (var t in lstPnm)
                            { t.WorkCenter = item.CodWorkCenter; }
                            lstProdNotifMat.AddRange(lstPnm);
                        }
                    }
                    else
                    {
                        // sino es hijo todo normal
                        if (item.FecUltManto.ToShortDateString() != "01/01/0001")
                        {
                            // si tiene fec. ult. manto.
                            lstProdNotifMat.AddRange(repoSql.GetProducMatxWc(cnxSqlHT, item.CodWorkCenter, item.FecUltManto));
                        }
                        else
                        {
                            // sino tiene fec ult. manto le calculamos 30 dias atras
                            lstProdNotifMat.AddRange(repoSql.GetProducMatxWc(cnxSqlHT, item.CodWorkCenter, DateTime.Now.AddDays(-30)));
                        }
                    }
                }
                w++;
            }



            //TextWriter tw21 = new StreamWriter(@"c:\paso\lstProdNotifMat.txt", true);
            //foreach (var x in lstProdNotifMat)
            //{
            //   tw21.WriteLine("{0},{1},{2}", x.WorkCenter, x.Material, x.PiezasProducidas);
            //}
            //tw21.Close();



            List<FamiliaProductos> lstProdFam = new List<FamiliaProductos>();

            switch (centroCtos)
            {
                case "0061131022":
                    {
                        // generamos el catalogo de materiales de la notif. autom. de las piezas que se producen en el mismo ciclo
                        // y asigamos la produc. del material que mas produjo 
                        lstProdFam = GetProduccionxFamilia(cnxSqlMT, cnxSqlProd, cnxSqlHT, lstEqTpmBas, lstProdNotifMat, lstWcr);

                        // Aqui debemos cambiar la produccion de las familias: 
                        // vamos a tomar el material con mayor produccion y dejar en cero el material que tuvo menos produccion 
                        // para no duplicar

                        lstProdNotifSumWC = GetSumProducXWc(lstProdFam, lstProdNotifMat);                       
                    }
                    break;
                case "0061131020":
                    {
                        // generamos el catalogo de materiales de la notif. autom. de las piezas que se producen en el mismo ciclo
                        // y asigamos la produc. del material que mas produjo 
                        lstProdFam = GetProduccionxFamilia(cnxSqlMT, cnxSqlProd, cnxSqlHT, lstEqTpmBas, lstProdNotifMat, lstWcr);

                        // Aqui debemos cambiar la produccion de las familias: 
                        // vamos a tomar el material con mayor produccion y dejar en cero el material que tuvo menos produccion 
                        // para no duplicar

                        lstProdNotifSumWC = GetSumProducXWc(lstProdFam, lstProdNotifMat);
                    }
                    break;
                case "0061131004":
                    {
                        // generamos el catalogo de materiales de la notif. autom. de las piezas que se producen en el mismo ciclo
                        // y asigamos la produc. del material que mas produjo 
                        lstProdFam = GetProduccionxFamilia(cnxSqlMT, cnxSqlProd, cnxSqlHT, lstEqTpmBas, lstProdNotifMat, lstWcr);

                        // Aqui debemos cambiar la produccion de las familias: 
                        // vamos a tomar el material con mayor produccion y dejar en cero el material que tuvo menos produccion 
                        // para no duplicar

                        lstProdNotifSumWC = GetSumProducXWc(lstProdFam, lstProdNotifMat);
                    }
                    break;
                case "0061131005":
                    {
                        // generamos el catalogo de materiales de la notif. autom. de las piezas que se producen en el mismo ciclo
                        // y asigamos la produc. del material que mas produjo 
                        lstProdFam = GetProduccionxFamilia(cnxSqlMT, cnxSqlProd, cnxSqlHT, lstEqTpmBas, lstProdNotifMat, lstWcr);

                        // Aqui debemos cambiar la produccion de las familias: 
                        // vamos a tomar el material con mayor produccion y dejar en cero el material que tuvo menos produccion 
                        // para no duplicar

                        lstProdNotifSumWC = GetSumProducXWc(lstProdFam, lstProdNotifMat);
                    }
                    break;                
                case "0000013205":
                    {
                        // Para le caso de la planta de Morelos nos van a generar una tabla con los ciclos por dia de cada equipo

                        lstProdNotifSumWC = GetSumCiclosWc(cnxSqlMT, cnxSqlProd, lstEqTpmBas);

                    }
                    break;
                default:
                    {
                        foreach (var item in lstEqTpmBas)
                        {
                            if (item.FecUltManto.ToShortDateString() != "01/01/0001")
                            {
                                lstProdNotifMat.AddRange(repoSql.GetProducMatxWc(cnxSqlHT, item.CodWorkCenter, item.FecUltManto));
                            }
                            else
                            {
                                lstProdNotifMat.AddRange(repoSql.GetProducMatxWc(cnxSqlHT, item.CodWorkCenter, DateTime.Now.AddDays(-30)));
                            }
                        }

                        // Sumario de piezas producidas
                        lstProdNotifSumWC = lstProdNotifMat
                                             .GroupBy(x => new { x.WorkCenter })
                                             .Select(g => new ProducNotificada
                                             {
                                                 WorkCenter = g.Key.WorkCenter,
                                                 Uom = "",
                                                 Material = "",
                                                 PiezasProducidas = g.Sum(x => x.PiezasProducidas)
                                             }).ToList();
                    }
                    break;
            }

            //********************************************************************
            // Asignamos las piezas al Work Center
            #region
            //foreach (var dr in lstProdNotifSumWC)
            //{

            //    index = lstEqTpmBas.FindIndex(x => x.CodWorkCenter.Trim() == dr.WorkCenter.Trim());
            //    if (index >= 0)
            //    {
            //        lstEqTpmBas[index].PzasProducidas = dr.PiezasProducidas;
            //    }
            //}
            #endregion

            #region Calculamos el % de Avance del periodo de plan de Mannto.

            for (int i = 0; i < lstEqTpmBas.Count; i++)
            {
                if (lstEqTpmBas[i].PmReal > 0)
                {
                    lstEqTpmBas[i].PorcAvance = Convert.ToInt32((lstEqTpmBas[i].PzasProducidas / lstEqTpmBas[i].PmReal) * 100);
                    lstEqTpmBas[i].Avance = Convert.ToInt32((lstEqTpmBas[i].PzasProducidas / lstEqTpmBas[i].PmReal) * 100);
                }
            }
            #endregion

            #region Generamos la grafica del avance de mantto.

            int verde = pBarra.Verde;
            int amarilla = pBarra.Verde + pBarra.Amarilla;
            int roja = pBarra.Verde + pBarra.Amarilla + pBarra.Roja;

            for (int i = 0; i < lstEqTpmBas.Count; i++)
            {
                lstEqTpmBas[i].Barra.Verde = 0;
                lstEqTpmBas[i].Barra.Amarilla = 0;
                lstEqTpmBas[i].Barra.Roja = 0;
                lstEqTpmBas[i].Barra.Blanca = 0;

                switch (lstEqTpmBas[i].PorcAvance)
                {
                    case int x when x <= verde:

                        lstEqTpmBas[i].Barra.Verde = lstEqTpmBas[i].PorcAvance;
                        lstEqTpmBas[i].Orden = 3;
                        break;

                    case int x when x <= amarilla:

                        lstEqTpmBas[i].Barra.Verde = verde;
                        lstEqTpmBas[i].Barra.Amarilla = lstEqTpmBas[i].PorcAvance - verde;
                        lstEqTpmBas[i].Orden = 2;
                        break;
                    case int x when x > amarilla:

                        lstEqTpmBas[i].Barra.Verde = pBarra.Verde;
                        lstEqTpmBas[i].Barra.Amarilla = pBarra.Amarilla;

                        if (lstEqTpmBas[i].PorcAvance > roja)
                        {
                            lstEqTpmBas[i].Barra.Roja = pBarra.Roja;
                        }
                        else
                        {
                            lstEqTpmBas[i].Barra.Roja = lstEqTpmBas[i].PorcAvance - amarilla;
                        }
                        lstEqTpmBas[i].Orden = 1;
                        break;
                }

            }
            #endregion

            return (lstEqTpmBas);
        }

        public List<ProducNotificada> GetSumCiclosWc(string cnxSqlMT, string cnxSqlProd, List<EquipoTpmBasico> lstTpmBas)
        {
            SqlRepository repoSql = new SqlRepository();
            List<ProducNotificada> lstWcCliclos = new List<ProducNotificada>();
            ProducNotificada ciclos = new ProducNotificada();

            foreach (var t in lstTpmBas)
            {
                ciclos = repoSql.GetWcCiclos(cnxSqlMT, cnxSqlProd, t);
                if (ciclos != null)
                    lstWcCliclos.Add(ciclos);
            }
            return (lstWcCliclos);
        }

        public List<ProducNotificada> GetSumProducXWc(List<FamiliaProductos> lstProdFam, List<ProducNotificada> lstProdNotifMat)
        {
            int index = 0;
            List<ProducNotificada> lstProdNotifSum = new List<ProducNotificada>();

            //********************************************************************
            //**   Aqui debemos cambiar la produccion de las familias por la que se genero por Work Center  
            //**   y tomar el de mayor produccion y dejar en cero el material que tuvo menos produccion 
            #region cambiar la produccion de las familias

            foreach (var d in lstProdFam)
            {
                // Busco el primer Material, si viene vacio salto el ciclo
                if (string.IsNullOrEmpty(d.Material1)) { continue; }

                index = lstProdNotifMat.FindIndex(x => x.Material == d.Material1);
                if (index > 0)
                {
                    // si lo encuentro le asigno las piezas
                    lstProdNotifMat[index].PiezasProducidas = d.PzasMayor;

                    // y debo de poner en 0 el otro material para no duplicar piezas
                    if (!string.IsNullOrEmpty(d.Material2))
                    {
                        index = lstProdNotifMat.FindIndex(x => x.Material == d.Material2);
                        if (index > 0)
                        { lstProdNotifMat[index].PiezasProducidas = 0; }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(d.Material2)) { continue; }

                    // Si no esta el Primer Material Busco el Segundo
                    index = lstProdNotifMat.FindIndex(x => x.Material == d.Material2);
                    if (index > 0)
                    {
                        // si lo encuentro le asigno las piezas
                        lstProdNotifMat[index].PiezasProducidas = d.PzasMayor;

                        // y debo de poner en 0 el otro material para no duplicar piezas
                        index = lstProdNotifMat.FindIndex(x => x.Material == d.Material1);
                        if (index > 0)
                        { lstProdNotifMat[index].PiezasProducidas = 0; }
                    }

                }

            }

            //TextWriter tw21 = new StreamWriter(@"c:\paso\lstProdNotifMat.txt", true);
            //foreach (var x in lstProdNotifMat)
            //{
            //   tw21.WriteLine("{0},{1},{2}", x.WorkCenter, x.Material, x.PiezasProducidas);
            //}
            //tw21.Close();

            #endregion

            // 4.- Generamos un sumario de piezas prodc. por Work Center
            lstProdNotifSum = lstProdNotifMat.GroupBy(x => new { x.WorkCenter })
                                             .Select(g => new ProducNotificada
                                             {
                                                 WorkCenter = g.Key.WorkCenter,
                                                 Uom = "",
                                                 Material = "",
                                                 PiezasProducidas = g.Sum(x => x.PiezasProducidas)
                                             }).ToList();



            return (lstProdNotifSum);
        }

        public List<FamiliaProductos> GetProduccionxFamilia(string cnxSqlMT, string cnxSqlProd, string cnxSqlHT,
                                                             List<EquipoTpmBasico> lstEqTpmBas, List<ProducNotificada> lstProdNotifMat,
                                                             List<WcReportables> lstWcrep)
        {
            int index = 0;
            List<ProducNotificada> lstProdNotifSum = new List<ProducNotificada>();
            SqlRepository repoSql = new SqlRepository();

            // vamos a a sacar de la BD produccion autom. las piezas que se producen igual al mismo tiempo
            List<FamiliaProductos> lstFamilias = new List<FamiliaProductos>();
            List<FamiliaProductos> lstFamily = new List<FamiliaProductos>();

            // Seleccionamos solo los work Center del Centro de costos que se va a generar
            List<string> lstWc = lstEqTpmBas.GroupBy(x => x.CodWorkCenter).Select(g => g.Key).OrderBy(g => g).ToList();

            //  Obtenemos la lista de coproductos y generamos una lista con los que se producen igual y cuantas pzas se producen
            lstFamilias = getCoproductos(cnxSqlMT, cnxSqlProd, lstWc);

            // si una familia tiene un WC padre.. 
            // entonces deberiamos de asignar a los hijos los mismos materiales para hacer los mismo de asignar el mayor de la familia

            index = 0;
            FamiliaProductos f = new FamiliaProductos();
            List<FamiliaProductos> temFam = new List<FamiliaProductos>();

            foreach (var t in lstFamilias)
            {
                index = 0;

                index = lstWcrep.FindIndex(x => x.WcReportable == t.WorkCenter);
                if (index > 0)
                {
                    var lsthijos = lstWcrep.Where(g => g.WcReportable == t.WorkCenter).ToList();
                    foreach (var j in lsthijos)
                    {
                        // busacar todos los hijos y agregarlos
                        FamiliaProductos n = new FamiliaProductos();
                        n.Material1 = t.Material1;
                        n.Material2 = t.Material2;
                        n.PiezasMat1 = t.PiezasMat1;
                        n.PiezasMat2 = t.PiezasMat2;
                        n.WorkCenter = j.WcHijo;
                        temFam.Add(n);
                    }
                }
            }
            lstFamilias.AddRange(temFam);

            // teniedo las familias vamos a tener que recorrer todas la producion para seber si es parte de una familia y asignar el mayor
            // se halla producido

            // Asignamos la piezas producidas del material que produjo mas
            foreach (var t in lstFamilias)
            {
                index = lstProdNotifMat.FindIndex(x => x.Material == t.Material1 && x.WorkCenter == t.WorkCenter);
                if (index >= 0)
                {
                    t.PzasProdMat1 = lstProdNotifMat[index].PiezasProducidas;
                }
                if (!string.IsNullOrEmpty(t.Material2))
                {
                    index = lstProdNotifMat.FindIndex(x => x.Material == t.Material2 && x.WorkCenter == t.WorkCenter);
                    if (index >= 0)
                    {
                        t.PzasProdMat2 = lstProdNotifMat[index].PiezasProducidas;
                    }
                }

                if (t.PzasProdMat1 > t.PzasProdMat2)
                    t.PzasMayor = t.PzasProdMat1;
                else
                    t.PzasMayor = t.PzasProdMat2;
            }

            lstFamily = lstFamilias.Where(x => x.PzasMayor > 0).OrderBy(c => c.WorkCenter).ToList();

            return (lstFamily);
        }

        public List<FamiliaProductos> getCoproductos(string cnxSqlMT, string cnxSqlProd, List<string> lstWc)
        {
            SqlRepository repoSql = new SqlRepository();
            List<FamiliaProductos> lstFamilias = new List<FamiliaProductos>();
            List<FamiliaProductos> lstFam = new List<FamiliaProductos>();

            // vamos a a sacar de la produccion automatica las piezas que se producen igual

            // a) Obtenemos la lista de coproductos y generamos una lista con los que se producen igual y cuantas pzas se prducen
            List<Coproducto> lstCoprod = new List<Coproducto>();
            lstCoprod = repoSql.GetCoproduc(cnxSqlMT, cnxSqlProd);

            var lstCo = lstCoprod.GroupBy(x => x.Co_Productos.ToString()).ToList();

            int c = 1;

            // Armamos los coproductos
            foreach (var d in lstCo)
            {
                string x = d.Key.ToString();
                if (!string.IsNullOrEmpty(x))
                {
                    FamiliaProductos dt = new FamiliaProductos();
                    dt.Id = c;
                    dt.Cprdts = x;
                    string[] cad = x.Split(',');

                    dt.coprod1 = cad[0].Trim();
                    if (cad.Count() == 2)
                    {
                        dt.coprod2 = cad[1].Trim();
                    }
                    lstFamilias.Add(dt);
                    c++;
                }
            }

            // b) Asignamos los materiales de los coproductos a la lista

            foreach (var d in lstFamilias)
            {
                if (!string.IsNullOrEmpty(d.coprod1))
                {
                    int index = lstCoprod.FindIndex(x => x.Id == (Convert.ToInt16(d.coprod1)));
                    if (index >= 0)
                    {
                        d.Material1 = lstCoprod[index].Material;
                        d.PiezasMat1 = lstCoprod[index].Twb;
                        d.WorkCenter = lstCoprod[index].WorkCenter;
                    }

                    if (!string.IsNullOrEmpty(d.coprod2))
                    {
                        index = lstCoprod.FindIndex(x => x.Id == (Convert.ToInt16(d.coprod2)));
                        if (index >= 0)
                        {
                            d.Material2 = lstCoprod[index].Material;
                            d.PiezasMat2 = lstCoprod[index].Twb;

                        }

                    }
                    d.PzasxFamilia = d.PiezasMat1 + d.PiezasMat2;
                }
            }

            // c) Aqui seleccionamos solo lo que pertenecen a los work center que son del centro de costos

            lstFam = (from r in lstFamilias
                      where lstWc.Contains(r.WorkCenter)
                      select r).ToList();

            return (lstFam);
        }

        public List<WcReportables> getWcReportables(string cnxSqlMT, string pCtroCto)
        {
            SqlRepository repoSql = new SqlRepository();
            List<WcReportables> lstWcr = new List<WcReportables>();

            lstWcr = repoSql.GetWcReport(cnxSqlMT, pCtroCto);

            return (lstWcr);
        }

        public DatosConfig LeeConfig(string cnxSql, string pCtroCostos)
        {
            SqlRepository repoSql = new SqlRepository();

            DatosConfig configTpm = new DatosConfig();

            configTpm = repoSql.GetConfigTpm(cnxSql, pCtroCostos);
            return (configTpm);
        }

    }
}