using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using GetDatosSap;

namespace RepositorySap
{
   public class SapRepository : ISapRepository<DataRow, WorkCenter, EstruturaEquipo, TblSap_IFLO, TblSap_ILOA, TblSap_EQUZ, TblSap_EQUI, TblSap_EQKT, TblSap_CRHD, TblSap_JEST, TblSap_TJ02T>
   {

      GetDatosSap.GetDatos datSap = new GetDatosSap.GetDatos();

      public List<DataRow> LeeCatEquipos(DatosCnxSap cnxSap, string cnxSql)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<Equipo> lstEquipos = new List<Equipo>();

         lstTemp = datSap.CatLeeEquipos(cnxSap, cnxSql);
         DateTime fecAct = DateTime.Now;

         //foreach (DataRow row in lstTemp)
         //{
         //   Equipo eq = new Equipo();
         //   eq.CodEquipo = row["EQUNR"].ToString();
         //   eq.LengEqui = row["EQASP"].ToString();
         //   eq.TecObjAutGrp = row["BEGRU"].ToString();
         //   eq.CategoryEqui = row["EQTYP"].ToString();
         //   eq.TypeTechObj = row["EQART"].ToString();
         //   eq.ManufAsset = row["HERST"].ToString();
         //   eq.ManufModelNum = row["TYPBZ"].ToString();
         //   eq.ConsecutiveNum = row["EQLFN"].ToString();
         //   eq.ObjectNumber = row["OBJNR"].ToString();
         //   eq.MaintenancePlan = row["WARPL"].ToString();
         //   eq.MeasuringPoint = row["IMRC_POINT"].ToString();
         //   eq.DescripTechnical = row["EQKTX"].ToString();
         //   eq.StatusInactive = row["INACT"].ToString();
         //   eq.SystemStatus = row["ISTAT"].ToString();
         //   eq.LengDescrip = row["SPRAS"].ToString();
         //   eq.IndivStatusObject = row["TXT04"].ToString();
         //   eq.ObjectStatus = row["TXT30"].ToString();
         //   eq.ValidDate = Convert.ToDateTime(row["DATBI"]).Date;
         //   eq.NumNextEquipUsage = row["EQUZN"].ToString();
         //   eq.MaintPlanningPlant = row["IWERK"].ToString();
         //   eq.ValidFromDate = Convert.ToDateTime(row["DATAB"]).Date;
         //   eq.Superordinate = row["HEQUI"].ToString();
         //   eq.PlannerGroup = row["INGRP"].ToString();
         //   eq.PmObjType = row["PM_OBJTY"].ToString();
         //   eq.IdWorkCenter = row["GEWRK"].ToString();
         //   eq.TechIdentNumber = row["TIDNR"].ToString();
         //   eq.AccountAssignment = row["ILOAN"].ToString();
         //   eq.CatalogProfile = row["RBNR"].ToString();
         //   eq.TechnicalInformation = row["EZBER"].ToString();
         //   eq.FunctionalLocation = row["TPLNR"].ToString();
         //   eq.CrObjType = row["CR_OBJTY"].ToString();
         //   eq.ObjectIdPPWorkCenter = Convert.ToInt32(row["PPSID"]);
         //   eq.ControllingArea = row["KOKRS"].ToString();
         //   eq.CostCenter = row["KOSTL"].ToString();
         //   eq.MainWorkCenter = row["ARBPL"].ToString();
         //   eq.MainWCCategory = row["VERWE"].ToString();
         //   eq.MainWCLocation = row["STAND"].ToString();
         //   eq.PersonResponsibleMWC = row["VERAN"].ToString();
         //   eq.StandardTextKeyMWC = row["KTSCH"].ToString();
         //   eq.StandardValueKeyMWC = row["VGWTS"].ToString();
         //   eq.KeyPerformanceEfficRateMWC = row["ZGR02"].ToString();
         //   eq.WorkCenter = row["ARBPL1"].ToString();
         //   eq.CategoryWorkCenter = row["VERWE1"].ToString();
         //   eq.LocationWorkCenter = row["STAND1"].ToString();
         //   eq.PersonResponsibleWC = row["VERAN1"].ToString();
         //   eq.StandardTextKeyWC = row["KTSCH1"].ToString();
         //   eq.StandardValueKeyWC = row["VGWTS1"].ToString();
         //   eq.KeyPerformanceEfficRateWC = row["ZGR021"].ToString();

         //   lstEquipos.Add(eq);
         //}

         return (lstTemp);
      }

      public List<WorkCenter> LeeCatWorkCenter(DatosCnxSap cnxSap, string cnxSql, string tipoWc)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<WorkCenter> lstWC = new List<WorkCenter>();

         lstTemp = datSap.LeeCatWorkCenter(cnxSap, cnxSql);
         DateTime fecAct = DateTime.Now;

         foreach (DataRow row in lstTemp)
         {
            //if (row["VERWE"].ToString() == tipoWc)
            //{
               WorkCenter wc = new WorkCenter();

               wc.Planta = row["WERKS"].ToString();
               wc.TipoObjeto = row["OBJTY"].ToString();
               wc.IdWorkCenter = Convert.ToDecimal(row["OBJID"]);
               wc.CodWorkCenter = row["ARBPL"].ToString();
               wc.Categoria = row["VERWE"].ToString();
               wc.keyValorStandar = row["VGWTS"].ToString();
               wc.Administrador = row["VERAN"].ToString();
               wc.FocusFactory = row["KTSCH"].ToString();
               wc.keyValordeOperacion = row["STEUS"].ToString();
               wc.InicioVigencia = Convert.ToDateTime(row["BEGDA"].ToString());
               wc.FinVigencia = Convert.ToDateTime(row["ENDDA"].ToString());
               wc.Lenguaje = row["SPRAS"].ToString();
               wc.Descripcion = row["KTEXT"].ToString();
               wc.FechaUpdate = fecAct;

               lstWC.Add(wc);
            //}
         }

         return (lstWC);
      }

      public List<EstruturaEquipo> LeeEstructuraEquipos(DatosCnxSap cnxSap, string cnxSql)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<EstruturaEquipo> lstEstrEq = new List<EstruturaEquipo>();

         RfcParam param = new RfcParam();
         param.CentroCostos = "*";
         param.FuntionLoc = "E*";
         param.Planta = "1841";
         param.ObjectReferenceIndicator1 = "2";
         param.ObjectReferenceIndicator2 = " ";

         // lstTemp = datSap.LeeStructLocations(cnxSap, cnxSql, param);


         DateTime fecAct = DateTime.Now;

         foreach (DataRow row in lstTemp)
         {

            EstruturaEquipo dr = new EstruturaEquipo();

            dr.FuncionalLoc = row["TPLNR"].ToString();
            dr.Descripcion = row["PLTXT"].ToString();
            dr.SuperiorFuncLoc = row["TPLMA"].ToString();
            dr.TipoObjectoWc = row["PM_OBJTY"].ToString();
            dr.IdObjeto = row["OBJNR"].ToString();
            dr.LocationAccount = row["ILOAN"].ToString();
            dr.PlantaManto = row["SWERK"].ToString();
            dr.AreaControling = row["KOKRS"].ToString();
            dr.CentroCosto = row["KOSTL"].ToString();
            dr.IdObjectPPWc = Convert.ToDecimal(row["PPSID"]);
            dr.CodEquipo = row["EQUNR"].ToString();
            dr.PmObjType = row["PM_OBJTY1"].ToString();
            dr.IdObjectWc = Convert.ToDecimal(row["GEWRK"]);
            dr.FecUpdate = fecAct;

            lstEstrEq.Add(dr);
         }

         return (lstEstrEq);
      }

      public List<TblSap_IFLO> LeeEstrEquIFLO(DatosCnxSap cnxSap, string cnxSql, RfcParam param)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<TblSap_IFLO> lstIFLO = new List<TblSap_IFLO>();

         lstTemp = datSap.LeeStructLocIFLO(cnxSap, cnxSql, param);

         foreach (DataRow row in lstTemp)
         {
            //Sustraemos los datos en un arreglo de cadena

            string[] fieldValues = row["WA"].ToString().Split('|');

            TblSap_IFLO dr = new TblSap_IFLO();

            dr.FunctionalLocation = fieldValues[0];
            dr.LanguageKey = fieldValues[1];
            dr.Description = fieldValues[2];
            dr.SuperiorFunctLoc = fieldValues[3];
            dr.ObjectTypeWorkCenter = fieldValues[4];
            dr.ObjectNumber = fieldValues[5];
            dr.MaintPlanningPlant = fieldValues[6];
            dr.ControllingArea = fieldValues[7];
            dr.CostCenter = fieldValues[8];
            dr.PlannerGroup = fieldValues[9];
            dr.ObjectIDWorkCenter = fieldValues[10];

            lstIFLO.Add(dr);

         }
         var lst = lstIFLO.OrderBy(x => x.SuperiorFunctLoc).ToList();
         return (lst);
      }

      public List<TblSap_ILOA> LeeEstrEquILOA(DatosCnxSap cnxSap, string cnxSql, RfcParam param)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<TblSap_ILOA> lstILOA = new List<TblSap_ILOA>();

         lstTemp = datSap.LeeStructLocILOA(cnxSap, cnxSql, param);

         foreach (DataRow row in lstTemp)
         {
            //Sustraemos los datos en un arreglo de cadena

            string[] fieldValues = row["WA"].ToString().Split('|');

            TblSap_ILOA dr = new TblSap_ILOA();

            dr.LocationAccount = fieldValues[0];
            dr.FunctionalLocation = fieldValues[1];
            dr.MaintPlant = fieldValues[2];
            dr.ObjectIDWorkCenter = fieldValues[3];
            dr.ObjectReferenceIndicator = fieldValues[4];
            dr.CostCenter = fieldValues[5];
            dr.ObjectTypesCIMResource = fieldValues[6];
            dr.ObjectIDResource = fieldValues[7];

            lstILOA.Add(dr);

         }

         return (lstILOA);
      }

      public List<TblSap_EQUZ> LeeEstrEquEQUZ(DatosCnxSap cnxSap, string cnxSql, RfcParam param)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<TblSap_EQUZ> lstEQUZ = new List<TblSap_EQUZ>();

         lstTemp = datSap.LeeStructLocEQUZ(cnxSap, cnxSql, param);

         foreach (DataRow row in lstTemp)
         {
            //Sustraemos los datos en un arreglo de cadena

            string[] fieldValues = row["WA"].ToString().Split('|');

            TblSap_EQUZ dr = new TblSap_EQUZ();

            dr.ValidToDate = fieldValues[0];
            dr.LocationAccount = fieldValues[1];
            dr.EquipmentNumber = fieldValues[2];
            dr.ValidFromDate = fieldValues[3];     
            dr.MainPlanningPlant = fieldValues[4];
            dr.PlannerGroup = fieldValues[5];
            dr.ObjectTypeCIMResourWC = fieldValues[6];
            dr.ObjectIDWorkCenter = fieldValues[7];
            dr.TechnIdentNumber = fieldValues[8];
            dr.SuperordinEquip = fieldValues[9];


            lstEQUZ.Add(dr);

         }

         return (lstEQUZ);
      }

      public List<TblSap_EQUI> LeeEstrEquEQUI(DatosCnxSap cnxSap, string cnxSql, RfcParam param)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<TblSap_EQUI> lstEQUI = new List<TblSap_EQUI>();

         lstTemp = datSap.LeeCatEquipos_EQUI(cnxSap, cnxSql, param);

         foreach (DataRow row in lstTemp)
         {
            //Sustraemos los datos en un arreglo de cadena

            string[] fieldValues = row["WA"].ToString().Split('|');

            TblSap_EQUI dr = new TblSap_EQUI();

            dr.EquipmentNumber = fieldValues[0];
            dr.Language = fieldValues[1];
            dr.TechnicalObjectAuthorizationGroup = fieldValues[2];
            dr.EquipmentCategory = fieldValues[3];
            dr.TypeTechnicalObject = fieldValues[4];
            dr.ManufacturerAsset = fieldValues[5];
            dr.ManufacturerModelMumber = fieldValues[6];
            dr.ConsecutiveNumbering = fieldValues[7];
            dr.ObjectNumber = fieldValues[8];
            dr.MaintenancenPlan = fieldValues[9];
            dr.MeasuringPoint = fieldValues[10];

            lstEQUI.Add(dr);

         }
  
         return (lstEQUI);
      }

      public List<TblSap_EQKT> LeeEstrEquEQKT(DatosCnxSap cnxSap, string cnxSql, RfcParam param)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<TblSap_EQKT> lstEQKT = new List<TblSap_EQKT>();

         lstTemp = datSap.LeeCatEquipos_EQKT(cnxSap, cnxSql, param);

         foreach (DataRow row in lstTemp)
         {
            //Sustraemos los datos en un arreglo de cadena

            string[] fieldValues = row["WA"].ToString().Split('|');

            TblSap_EQKT dr = new TblSap_EQKT();

            dr.EquipmentNumber = fieldValues[0];
            dr.LanguageKey = fieldValues[1];
            dr.DescripTechnicalObject = fieldValues[2];

            lstEQKT.Add(dr);

         }

         return (lstEQKT);
      }

      public List<TblSap_JEST> LeeEstrEquJEST(DatosCnxSap cnxSap, string cnxSql, RfcParam param, List<string> lstIdEqui)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<TblSap_JEST> lstJEST = new List<TblSap_JEST>();

         lstTemp = datSap.LeeCatEquipos_JEST(cnxSap, cnxSql, param, lstIdEqui);

         foreach (DataRow row in lstTemp)
         {
            //Sustraemos los datos en un arreglo de cadena

            string[] fieldValues = row["WA"].ToString().Split('|');

            TblSap_JEST dr = new TblSap_JEST();

            dr.ObjectNumber = fieldValues[0];
            dr.ObjectStatus = fieldValues[1];
            dr.StatusIsInactive = fieldValues[2];

            lstJEST.Add(dr);

         }

         return (lstJEST);
      }

      public List<TblSap_JCDS> LeeEstrEquJCDS(DatosCnxSap cnxSap, string cnxSql, RfcParam param, List<string> lstIdEqui)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<TblSap_JCDS> lstJCDS = new List<TblSap_JCDS>();
         lstTemp = datSap.LeeCatEquipos_JCDS(cnxSap, cnxSql, param, lstIdEqui);

         foreach (DataRow row in lstTemp)
         {
            //Sustraemos los datos en un arreglo de cadena

            string[] fieldValues = row["WA"].ToString().Split('|');

            TblSap_JCDS dr = new TblSap_JCDS();

            dr.ObjectNumber = fieldValues[0];
            dr.ObjectStatus = fieldValues[1];
            dr.FecModfi = fieldValues[2];
            dr.TimeModfi = fieldValues[3];
            dr.StatusIsInactive = fieldValues[4];
            //dr.FecModific = DateTime.TryParseExact(dr.FecModfi + " " + dr.TimeModfi, "yyyyMMdd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out fec);
          
            lstJCDS.Add(dr);

         }

         return (lstJCDS);
      }

      public List<TblSap_CRHD> LeeEstrEquCRHD(DatosCnxSap cnxSap, string cnxSql, RfcParam param)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<TblSap_CRHD> lstCRHD = new List<TblSap_CRHD>();

         lstTemp = datSap.LeeCatEquipos_CRHD(cnxSap, cnxSql, param);

         foreach (DataRow row in lstTemp)
         {
            //Sustraemos los datos en un arreglo de cadena

            string[] fieldValues = row["WA"].ToString().Split('|');

            TblSap_CRHD dr = new TblSap_CRHD();

            dr.ObjectTypesResource = fieldValues[0];
            dr.ObjectIDResource = fieldValues[1];
            dr.WorkCenter = fieldValues[2];
            dr.Plant = fieldValues[3];
            dr.WorkCenterCategory = fieldValues[4];
            dr.WCLocation = fieldValues[5];
            dr.PersonResponsibleWC = fieldValues[6];
            dr.StandarTextKey = fieldValues[7];
            dr.Standardvaluekey = fieldValues[8];
            dr.KeyPerformEffRate = fieldValues[9];

            lstCRHD.Add(dr);

         }

         return (lstCRHD);
      }

      public List<TblSap_TJ02T> LeeEstrEquTJ02T(DatosCnxSap cnxSap, string cnxSql, RfcParam param)
      {
         List<DataRow> lstTemp = new List<DataRow>();
         List<TblSap_TJ02T> lstTJ02T = new List<TblSap_TJ02T>();

         lstTemp = datSap.LeeCatEquipos_TJ02T(cnxSap, cnxSql, param);

         foreach (DataRow row in lstTemp)
         {
            //Sustraemos los datos en un arreglo de cadena

            string[] fieldValues = row["WA"].ToString().Split('|');

            TblSap_TJ02T dr = new TblSap_TJ02T();

            dr.SystemStatus = fieldValues[0];
            dr.IndividualStatusObject = fieldValues[1];
            dr.ObjectStatus = fieldValues[2];

            lstTJ02T.Add(dr);

         }

         return (lstTJ02T);
      }

   }
}
