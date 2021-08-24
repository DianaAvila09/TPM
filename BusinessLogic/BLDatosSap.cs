using Entidades;
using RepositorySap;
using RepositorySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using ToolsTpm;

namespace BusinessLogic
{
    public class BLDatosSap
    {
        public int UpdateCatWC(string cnxSqlMT, DatosCnxSap cnxSap, string pathLog)
        {
            List<WorkCenter> lstWc = new List<WorkCenter>();
            List<DataRow> lstTemp = new List<DataRow>();
            int result = -1;
            try
            {
                TextWriter tw21 = new StreamWriter(pathLog, true);
                tw21.WriteLine("BLDatosSap-UpdateCatWC - EjecutaProceso -Inicio de la actualizacion de datos, ==>  " + ",  " + DateTime.Now.ToString());
                tw21.Close();


                //// Obtiene los datos de sap
                lstWc = DatosWC(cnxSap, cnxSqlMT);

                SqlRepository repoSql = new SqlRepository();

                result = repoSql.GuardarCatWCSql(cnxSqlMT, lstWc, "CatWorkCenter", pathLog);

                TextWriter tw2 = new StreamWriter(pathLog, true);
                tw2.WriteLine("BLDatosSap-UpdateCatWC - EjecutaProceso - Terminacion del proceso ==>  " + DateTime.Now.ToString());
                tw2.WriteLine("------------------------------------------------------------------------");
                tw2.Close();

            }
            catch (Exception ex)
            {
                string cMsj = "BLDatosSap-UpdateCatWC - EjecutaProceso - Error en la actualizacion ==>  " + ex.Message + ", fuente: " + ex.Source + ", fecha: " + DateTime.Now.ToString();
                TextWriter tw23 = new StreamWriter(pathLog, true);
                tw23.WriteLine(cMsj);
                tw23.Close();
                Tools tool = new Tools();
                tool.GuardarError(cnxSqlMT, ex.Message, ex.StackTrace, "EjecutaProceso", "BLDatosSap-UpdateCatWC");
            }

            return result;
        }


        public List<WorkCenter> DatosWC(DatosCnxSap cnxSap, string cnxSql)
        {
            SapRepository repoSap = new SapRepository();

            List<WorkCenter> lstWc = new List<WorkCenter>();

            lstWc = repoSap.LeeCatWorkCenter(cnxSap, cnxSql, "0007");

            return lstWc;
        }


        public int UpdateCatEstructuras(string cnxSqlMT, DatosCnxSap cnxSap, string pathLog)
        {
            List<EstruturaEquipo> lstEstruc = new List<EstruturaEquipo>();
            List<DataRow> lstTemp = new List<DataRow>();
            int result = -1;
            try
            {
                TextWriter tw21 = new StreamWriter(pathLog, true);
                tw21.WriteLine("BLDatosSap-UpdateCatEstructuras - EjecutaProceso -Inicio de la actualizacion de datos, ==>  " + ",  " + DateTime.Now.ToString());
                tw21.Close();


                //// Obtiene los datos de sap
                lstEstruc = DatosStructEquipos(cnxSap, cnxSqlMT);

                SqlRepository repoSql = new SqlRepository();

                result = repoSql.GuardarCatEESql(cnxSqlMT, lstEstruc, "CatEstrucEquipos", pathLog);

                TextWriter tw2 = new StreamWriter(pathLog, true);
                tw2.WriteLine("BLDatosSap-UpdateCatEstructuras - EjecutaProceso - Terminacion del proceso ==>  " + DateTime.Now.ToString());
                tw2.WriteLine("------------------------------------------------------------------------");
                tw2.Close();

            }
            catch (Exception ex)
            {
                string cMsj = "BLDatosSap-UpdateCatEstructuras - EjecutaProceso - Error en la actualizacion ==>  " + ex.Message + ", fuente: " + ex.Source + ", fecha: " + DateTime.Now.ToString();
                TextWriter tw23 = new StreamWriter(pathLog, true);
                tw23.WriteLine(cMsj);
                tw23.Close();
                Tools tool = new Tools();
                tool.GuardarError(cnxSqlMT, ex.Message, ex.StackTrace, "EjecutaProceso", "BLDatosSap-UpdateCatEstructuras");
            }

            return result;
        }
        public List<EstruturaEquipo> DatosStructEquipos(DatosCnxSap cnxSap, string cnxSql)
        {
            SapRepository repoSap = new SapRepository();

            List<EstruturaEquipo> lstEstruEq = new List<EstruturaEquipo>();

            List<TblSap_IFLO> lstIFLO = new List<TblSap_IFLO>();
            List<TblSap_ILOA> lstILOA = new List<TblSap_ILOA>();
            List<TblSap_EQUZ> lstEQUZ = new List<TblSap_EQUZ>();

            RfcParam param = new RfcParam();
            param.CentroCostos = "*";
            param.FuntionLoc = "E*";
            param.Planta = "1841";
            param.ObjectReferenceIndicator1 = "2";
            param.ObjectReferenceIndicator2 = " ";
            param.NomenclaEquipo = "E%";

            lstIFLO = repoSap.LeeEstrEquIFLO(cnxSap, cnxSql, param);
            lstILOA = repoSap.LeeEstrEquILOA(cnxSap, cnxSql, param);
            lstEQUZ = repoSap.LeeEstrEquEQUZ(cnxSap, cnxSql, param);

            List<Equipo> lstEq = new List<Equipo>();

            lstEq = DatosCatEquipos(cnxSap, cnxSql);

            var lstTemp = (from i in lstIFLO
                           join a in lstILOA on i.FunctionalLocation equals a.FunctionalLocation
                           join e in lstEQUZ on a.LocationAccount equals e.LocationAccount
                           join q in lstEq on e.EquipmentNumber.Trim() equals q.CodEquipo.Trim()
                           select new
                           {
                               i.FunctionalLocation,
                               i.Description,
                               i.LanguageKey,
                               i.SuperiorFunctLoc,
                               i.CostCenter,
                               i.ControllingArea,
                               i.PlannerGroup,
                               i.ObjectTypeWorkCenter,
                               i.ObjectIDWorkCenter,
                               i.ObjectNumber,
                               i.MaintPlanningPlant,
                               a.LocationAccount,
                               a.ObjectReferenceIndicator,
                               a.MaintPlant,
                               e.EquipmentNumber,
                               idEquipo = q.ObjectNumber,
                               q.DescripTechnical,
                               q.ObjectStatus,
                               q.MainWorkCenter,
                               q.WorkCenter,
                               q.ValidFromDate,
                               q.TypeTechObj,
                               q.PmObjType,
                           }).OrderBy(x => x.FunctionalLocation);


            foreach (var dr in lstTemp)
            {
                EstruturaEquipo eq = new EstruturaEquipo();
                eq.FuncionalLoc = dr.FunctionalLocation;
                eq.Descripcion = dr.Description;
                eq.LanguageKey = dr.LanguageKey;

                eq.SuperiorFuncLoc = dr.SuperiorFunctLoc;
                eq.TipoObjectoWc = dr.ObjectTypeWorkCenter;
                eq.IdObjeto = dr.ObjectNumber;
                eq.PlannerGroup = dr.PlannerGroup;
                eq.LocationAccount = dr.LocationAccount;
                eq.PlantaManto = dr.MaintPlant;
                eq.AreaControling = dr.ControllingArea;
                eq.CentroCosto = dr.CostCenter;
                eq.IdObjectPPWc = Convert.ToDecimal(dr.ObjectIDWorkCenter);
                eq.IdObjectWc = Convert.ToDecimal(dr.ObjectIDWorkCenter);
                eq.CodEquipo = dr.EquipmentNumber;
                eq.Equipodesc = dr.DescripTechnical;
                eq.IdEquipo = dr.idEquipo;
                eq.MainWorkCenter = dr.MainWorkCenter;
                eq.WorkCenter = dr.WorkCenter;
                eq.EquipoStatus = dr.ObjectStatus;
                eq.TypeTechObj = dr.TypeTechObj;
                eq.PmObjType = dr.PmObjType;
                lstEstruEq.Add(eq);
            }

            return lstEstruEq;
        }

        public int UpdateCatFunctLocat(string cnxSqlMT, DatosCnxSap cnxSap, string pathLog)
        {
            List<TblSap_IFLO> lstEstruc = new List<TblSap_IFLO>();
            List<DataRow> lstTemp = new List<DataRow>();
            int result = -1;
            try
            {
                TextWriter tw21 = new StreamWriter(pathLog, true);
                tw21.WriteLine("BLDatosSap-UpdateCatFunctLocat - EjecutaProceso -Inicio de la actualizacion de datos, ==>  " + ",  " + DateTime.Now.ToString());
                tw21.Close();


                //// Obtiene los datos de sap
                lstEstruc = DatosFunctLoc(cnxSap, cnxSqlMT);

                SqlRepository repoSql = new SqlRepository();

                result = repoSql.GuardarCatFLSql(cnxSqlMT, lstEstruc, "CatFuntLocation", pathLog);

                TextWriter tw2 = new StreamWriter(pathLog, true);
                tw2.WriteLine("BLDatosSap-UpdateCatFunctLocat - EjecutaProceso - Terminacion del proceso ==>  " + DateTime.Now.ToString());
                tw2.WriteLine("------------------------------------------------------------------------");
                tw2.Close();

            }
            catch (Exception ex)
            {
                string cMsj = "BLDatosSap-UpdateCatFunctLocat - EjecutaProceso - Error en la actualizacion ==>  " + ex.Message + ", fuente: " + ex.Source + ", fecha: " + DateTime.Now.ToString();
                TextWriter tw23 = new StreamWriter(pathLog, true);
                tw23.WriteLine(cMsj);
                tw23.Close();
                Tools tool = new Tools();
                tool.GuardarError(cnxSqlMT, ex.Message, ex.StackTrace, "EjecutaProceso", "BLDatosSap-UpdateCatFunctLocat");
            }

            return result;
        }
        public List<TblSap_IFLO> DatosFunctLoc(DatosCnxSap cnxSap, string cnxSql)
        {
            SapRepository repoSap = new SapRepository();

            List<TblSap_IFLO> lstIFLO = new List<TblSap_IFLO>();

            // Parametros para extraer los Functional location
            RfcParam param = new RfcParam();
            param.FuntionLoc = "E*";
            param.Planta = "1841";

            lstIFLO = repoSap.LeeEstrEquIFLO(cnxSap, cnxSql, param).OrderByDescending(x => x.SuperiorFunctLoc).ThenByDescending(y => y.FunctionalLocation).ToList<TblSap_IFLO>();

            return lstIFLO;
        }

        public int UpdateCatEquipos(string cnxSqlMT, DatosCnxSap cnxSap, string pathLog)
        {
            List<Equipo> lstEquipos = new List<Equipo>();
            List<DataRow> lstTemp = new List<DataRow>();
            int result = -1;
            try
            {
                TextWriter tw21 = new StreamWriter(pathLog, true);
                tw21.WriteLine("BLDatosSap-UpdateCatEquipos - EjecutaProceso -Inicio de la actualizacion de datos, ==>  " + ",  " + DateTime.Now.ToString());
                tw21.Close();

                //// Obtiene los datos de sap
                lstEquipos = DatosCatEquipos(cnxSap, cnxSqlMT);

                SqlRepository repoSql = new SqlRepository();

                result = repoSql.GuardarCatEquiposSql(cnxSqlMT, lstEquipos, "CatEquipos", pathLog);

                TextWriter tw2 = new StreamWriter(pathLog, true);
                tw2.WriteLine("BLDatosSap-UpdateCatEquipos - EjecutaProceso - Terminacion del proceso ==>  " + DateTime.Now.ToString());
                tw2.WriteLine("------------------------------------------------------------------------");
                tw2.Close();
            }

            catch (Exception ex)
            {
                string cMsj = "BLDatosSap-UpdateCatEquipos - EjecutaProceso - Error en la actualizacion ==>  " + ex.Message + ", fuente: " + ex.Source + ", fecha: " + DateTime.Now.ToString();
                TextWriter tw23 = new StreamWriter(pathLog, true);
                tw23.WriteLine(cMsj);
                tw23.Close();
                Tools tool = new Tools();
                tool.GuardarError(cnxSqlMT, ex.Message, ex.StackTrace, "EjecutaProceso", "BLDatosSap-UpdateCatWC");
            }

            return result;
        }

        public List<Equipo> DatosCatEquipos(DatosCnxSap cnxSap, string cnxSql)
        {
            SapRepository repoSap = new SapRepository();

            List<Equipo> lstEquipos = new List<Equipo>();

            List<TblSap_EQUI> lstEQUI = new List<TblSap_EQUI>();
            List<TblSap_EQUZ> lstEQUZ = new List<TblSap_EQUZ>();
            List<TblSap_ILOA> lstILOA = new List<TblSap_ILOA>();
            List<TblSap_EQKT> lstEQKT = new List<TblSap_EQKT>();
            List<TblSap_JEST> lstJEST = new List<TblSap_JEST>();
            List<TblSap_JCDS> lstJCDS = new List<TblSap_JCDS>();
            List<TblSap_CRHD> lstCRHD = new List<TblSap_CRHD>();
            List<TblSap_TJ02T> lstTJ02T = new List<TblSap_TJ02T>();


            RfcParam param = new RfcParam();
            param.CategoryEq = "M";
            param.AutTechnicalobject = "E%";
            param.Language = "EN";
            param.NomenclaEquipo = "E%";
            param.CategoryWC = "0007";
            param.Planta = "1841";
            param.FuntionLoc = "E*";
            param.CentroCostos = "*";
            param.ObjectReferenceIndicator1 = "";
            param.ObjectReferenceIndicator2 = "1";

            lstEQUI = repoSap.LeeEstrEquEQUI(cnxSap, cnxSql, param);
            lstEQUZ = repoSap.LeeEstrEquEQUZ(cnxSap, cnxSql, param);

            //TextWriter tw200 = new StreamWriter(@"c:\paso\sap\lstEQUZ.txt", true);
            //foreach (var x in lstEQUZ)
            //{
            //   tw200.WriteLine("{0},{1},{2},{3},{4}", x.EquipmentNumber, x.LocationAccount, x.ObjectIDWorkCenter, x.ObjectTypeCIMResourWC, x.ValidToDate);
            //}
            //tw200.Close();


            lstILOA = repoSap.LeeEstrEquILOA(cnxSap, cnxSql, param);

            //TextWriter tw20 = new StreamWriter(@"c:\paso\sap\lstILOA.txt", true);
            //foreach (var x in lstILOA)
            //{
            //   tw20.WriteLine("{0},{1},{2},{3},{4}", x.ObjectIDResource,  x.ObjectIDWorkCenter, x.ObjectTypesCIMResource, x.LocationAccount, x.ObjectReferenceIndicator);
            //}
            //tw20.Close();

            lstEQKT = repoSap.LeeEstrEquEQKT(cnxSap, cnxSql, param);
            lstCRHD = repoSap.LeeEstrEquCRHD(cnxSap, cnxSql, param);

            //TextWriter tw21 = new StreamWriter(@"c:\paso\sap\lstCRHD.txt", true);
            //foreach (var x in lstCRHD)
            //{
            //   tw21.WriteLine("{0},{1},{2},{3},{4}", x.ObjectTypesResource, x.ObjectIDResource, x.WorkCenter, x.WorkCenterCategory, x.PersonResponsibleWC);
            //}
            //tw21.Close();

            List<string> lstIdEqu = (from p in lstEQUI select p.ObjectNumber).ToList<string>();

            lstJEST = repoSap.LeeEstrEquJEST(cnxSap, cnxSql, param, lstIdEqu);
            lstJCDS = repoSap.LeeEstrEquJCDS(cnxSap, cnxSql, param, lstIdEqu);
            lstTJ02T = repoSap.LeeEstrEquTJ02T(cnxSap, cnxSql, param);

            #region query anterior
            //var lstTemp = (from e in lstEQUI
            //               join a in lstEQKT on e.EquipmentNumber equals a.EquipmentNumber
            //               join s in lstEQUZ on a.EquipmentNumber equals s.EquipmentNumber
            //               join i in lstILOA on s.LocationAccount equals i.LocationAccount
            //               join j in lstJEST on e.ObjectNumber.Trim() equals j.ObjectNumber.Trim()
            //               join t in lstTJ02T on j.ObjectStatus equals t.SystemStatus
            //               join c in lstCRHD on new { i.ObjectIDResource, ObjectTypesCIMResource = i.ObjectTypesCIMResource }
            //                             equals new { ObjectIDResource = c.ObjectIDResource, ObjectTypesCIMResource = c.ObjectTypesResource }
            #endregion

            var lstTemp = (from e in lstEQUI
                           join a in lstEQKT on e.EquipmentNumber equals a.EquipmentNumber
                           join s in lstEQUZ on a.EquipmentNumber equals s.EquipmentNumber
                           join i in lstILOA on s.LocationAccount equals i.LocationAccount
                           join j in lstJEST on e.ObjectNumber.Trim() equals j.ObjectNumber.Trim()
                           join t in lstTJ02T on j.ObjectStatus equals t.SystemStatus
                           select new
                           {
                               e.EquipmentNumber,
                               e.Language,
                               e.TechnicalObjectAuthorizationGroup,
                               e.EquipmentCategory,
                               e.TypeTechnicalObject,
                               e.ManufacturerAsset,
                               e.ManufacturerModelMumber,
                               e.ConsecutiveNumbering,
                               e.ObjectNumber,
                               e.MaintenancenPlan,
                               e.MeasuringPoint,
                               a.DescripTechnicalObject,
                               j.ObjectStatus,
                               j.StatusIsInactive,
                               t.SystemStatus,
                               t.IndividualStatusObject,
                               s.ValidToDate,
                               s.LocationAccount,
                               s.ValidFromDate,
                               s.SuperordinEquip,
                               s.MainPlanningPlant,
                               s.PlannerGroup,
                               ObjectIDWorkCenterMWC = s.ObjectIDWorkCenter,
                               s.TechnIdentNumber,
                               ObjectIDWorkCenter = i.ObjectIDWorkCenter,
                               i.CostCenter,
                               i.FunctionalLocation,
                               i.ObjectIDResource,
                               ObjectTypesResource = i.ObjectTypesCIMResource,
                               i.ObjectTypesCIMResource
                           }).OrderBy(x => x.EquipmentNumber).ToList();


            int cont = 1;
            foreach (var d in lstTemp)
            {
                Equipo eq = new Equipo();
                eq.Id = cont;
                eq.CodEquipo = d.EquipmentNumber;
                eq.LengEqui = d.Language;
                eq.TecObjAutGrp = d.TechnicalObjectAuthorizationGroup;
                eq.CategoryEqui = d.EquipmentCategory;
                eq.TypeTechObj = d.TypeTechnicalObject;
                eq.ObjectNumber = d.ObjectNumber;
                eq.AccountAssignment = d.LocationAccount;
                eq.ManufAsset = d.ManufacturerAsset;
                eq.ManufModelNum = d.ManufacturerModelMumber;
                eq.ConsecutiveNum = d.ConsecutiveNumbering;
                eq.ObjectNumber = d.ObjectNumber;
                eq.MaintenancePlan = d.MaintenancenPlan;
                eq.MeasuringPoint = d.MeasuringPoint;
                eq.DescripTechnical = d.DescripTechnicalObject;
                eq.StatusInactive = d.StatusIsInactive;
                eq.SystemStatus = d.SystemStatus;
                eq.LengDescrip = d.Language;
                eq.IndivStatusObject = d.IndividualStatusObject;
                eq.ObjectStatus = d.ObjectStatus;
                eq.ValidDate = DateTime.ParseExact(d.ValidToDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                eq.MaintPlanningPlant = d.MainPlanningPlant;
                eq.ValidFromDate = DateTime.ParseExact(d.ValidFromDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                eq.Superordinate = d.SuperordinEquip;
                eq.PlannerGroup = d.PlannerGroup;
                eq.IdMainWorkCenter = d.ObjectIDWorkCenterMWC;
                eq.IdWorkCenter = d.ObjectIDResource;
                eq.CostCenter = d.CostCenter;
                eq.TechIdentNumber = d.TechnIdentNumber;
                eq.FunctionalLocation = d.FunctionalLocation;
                eq.ObjectTypesResource = d.ObjectTypesResource;
                lstEquipos.Add(eq);
                cont = cont + 1;
            }

            foreach (var dr in lstEquipos)
            {
                int index = lstCRHD.FindIndex(x => x.ObjectIDResource == dr.IdWorkCenter
                                                 && x.ObjectTypesResource == dr.ObjectTypesResource
                                                 && x.WorkCenterCategory == "0007");
                if (index >= 0)
                {
                    dr.WorkCenter = lstCRHD[index].WorkCenter;
                    dr.CategoryWorkCenter = lstCRHD[index].WorkCenterCategory;
                    dr.LocationWorkCenter = lstCRHD[index].WCLocation;
                    dr.StandardTextKeyWC = lstCRHD[index].StandarTextKey;
                    dr.StandardValueKeyMWC = lstCRHD[index].Standardvaluekey;
                    dr.KeyPerformanceEfficRateWC = lstCRHD[index].KeyPerformEffRate;
                    dr.PersonResponsibleWC = lstCRHD[index].PersonResponsibleWC;
                }
            }

            foreach (var dr in lstEquipos)
            {
                int index = lstCRHD.FindIndex(x => x.ObjectIDResource == dr.IdMainWorkCenter.ToString()
                                           && x.ObjectTypesResource == dr.ObjectTypesResource
                                           && x.WorkCenterCategory == "0005");
                if (index >= 0)
                {
                    dr.MainWorkCenter = lstCRHD[index].WorkCenter;
                    dr.MainWCCategory = lstCRHD[index].WorkCenterCategory;
                    dr.MainWCLocation = lstCRHD[index].WCLocation;
                    dr.StandardTextKeyMWC = lstCRHD[index].StandarTextKey;
                    dr.StandardValueKeyMWC = lstCRHD[index].Standardvaluekey;
                    dr.KeyPerformanceEfficRateMWC = lstCRHD[index].KeyPerformEffRate;
                }
            }
            return (lstEquipos);
        }

        public List<Equipo> DatosBasicosEquipos(DatosCnxSap cnxSap, string cnxSql)
        {
            SapRepository repoSap = new SapRepository();

            List<Equipo> lstEquipos = new List<Equipo>();
            List<TblSap_EQUI> lstEQUI = new List<TblSap_EQUI>();
            List<TblSap_EQUZ> lstEQUZ = new List<TblSap_EQUZ>();
            List<TblSap_ILOA> lstILOA = new List<TblSap_ILOA>();
            List<TblSap_EQKT> lstEQKT = new List<TblSap_EQKT>();
            List<TblSap_JEST> lstJEST = new List<TblSap_JEST>();
            List<TblSap_CRHD> lstCRHD = new List<TblSap_CRHD>();
            List<TblSap_TJ02T> lstTJ02T = new List<TblSap_TJ02T>();


            RfcParam param = new RfcParam();
            param.CategoryEq = "M";
            param.AutTechnicalobject = "E%";
            param.Language = "EN";
            param.NomenclaEquipo = "E%";
            param.CategoryWC = "0007";
            param.Planta = "1841";
            param.FuntionLoc = "E*";
            param.CentroCostos = "*";
            param.ObjectReferenceIndicator1 = "2";
            param.ObjectReferenceIndicator2 = " ";


            lstEQUI = repoSap.LeeEstrEquEQUI(cnxSap, cnxSql, param);
            lstEQUZ = repoSap.LeeEstrEquEQUZ(cnxSap, cnxSql, param);
            lstILOA = repoSap.LeeEstrEquILOA(cnxSap, cnxSql, param);
            lstEQKT = repoSap.LeeEstrEquEQKT(cnxSap, cnxSql, param);
            lstCRHD = repoSap.LeeEstrEquCRHD(cnxSap, cnxSql, param);

            List<string> lstIdEqu = (from p in lstEQUI select p.ObjectNumber).ToList<string>();

            lstJEST = repoSap.LeeEstrEquJEST(cnxSap, cnxSql, param, lstIdEqu);
            lstTJ02T = repoSap.LeeEstrEquTJ02T(cnxSap, cnxSql, param);



            var lstTemp = (from e in lstEQUI
                           join a in lstEQKT on e.EquipmentNumber equals a.EquipmentNumber
                           join s in lstEQUZ on a.EquipmentNumber equals s.EquipmentNumber
                           join i in lstILOA on s.LocationAccount equals i.LocationAccount
                           join j in lstJEST on e.ObjectNumber.Trim() equals j.ObjectNumber.Trim()
                           join t in lstTJ02T on j.ObjectStatus equals t.SystemStatus
                           join c in lstCRHD on new { i.ObjectIDResource, ObjectTypesCIMResource = i.ObjectTypesCIMResource }
                                         equals new { ObjectIDResource = c.ObjectIDResource, ObjectTypesCIMResource = c.ObjectTypesResource }
                           select new
                           {
                               e.EquipmentNumber,
                               e.Language,
                               e.TechnicalObjectAuthorizationGroup,
                               e.EquipmentCategory,
                               e.TypeTechnicalObject,
                               e.ManufacturerAsset,
                               e.ManufacturerModelMumber,
                               e.ConsecutiveNumbering,
                               e.ObjectNumber,
                               e.MaintenancenPlan,
                               e.MeasuringPoint,
                               a.DescripTechnicalObject,
                               j.ObjectStatus,
                               j.StatusIsInactive,
                               t.SystemStatus,
                               t.IndividualStatusObject,
                               s.LocationAccount,
                               s.ValidFromDate,
                               s.PlannerGroup,
                               s.MainPlanningPlant,
                               s.ObjectIDWorkCenter,
                               ObjectIDWorkCenter1 = i.ObjectIDWorkCenter,
                               i.CostCenter,
                               i.FunctionalLocation,
                               c.ObjectIDResource,
                               c.ObjectTypesResource,
                               c.WorkCenterCategory,
                               c.WorkCenter,
                               c.PersonResponsibleWC,
                               c.StandarTextKey,
                               i.ObjectTypesCIMResource


                           }).OrderBy(x => x.EquipmentNumber);

            foreach (var d in lstTemp)
            {
                Equipo eq = new Equipo();

                eq.CodEquipo = d.EquipmentNumber;
                eq.ObjectNumber = d.ObjectNumber;
                eq.AccountAssignment = d.LocationAccount;
                eq.CategoryEqui = d.EquipmentCategory;
                eq.ConsecutiveNum = d.ConsecutiveNumbering;
                eq.CostCenter = d.CostCenter;
                eq.DescripTechnical = d.DescripTechnicalObject;
                eq.FunctionalLocation = d.FunctionalLocation;
                eq.IdWorkCenter = d.ObjectIDWorkCenter;
                eq.IndivStatusObject = d.ObjectStatus;
                eq.PlannerGroup = d.PlannerGroup;
                eq.TecObjAutGrp = d.TechnicalObjectAuthorizationGroup;
                eq.ValidFromDate = DateTime.ParseExact(d.ValidFromDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                eq.StatusInactive = d.StatusIsInactive;
                eq.SystemStatus = d.SystemStatus;
                eq.IndivStatusObject = d.IndividualStatusObject;
                eq.WorkCenter = d.WorkCenter;
                eq.StandardTextKeyMWC = d.StandarTextKey;
                eq.ObjectTypesResource = d.ObjectTypesResource;

                lstEquipos.Add(eq);

            }

            return (lstEquipos);

        }

    }
}

