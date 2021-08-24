using Entidades;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using ToolsTpm;

namespace GetDatosSap
{
   public class GetDatos
   {
      /// <summary>
      /// Obtiene equipos productivos de sap
      /// </summary>
      /// <param name="cnxSap"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> CatLeeEquipos(DatosCnxSap cnxSap, string cadCnxSql)
      {

         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();

         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            //**************************************************************************
            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();

            rfc.Planta = "1841";
            rfc.Category = "M";
            rfc.EdoEquipo = "INST";

            lstDatos = rfc.RfcCatLeeEquiposSAP(rfcDest, cadCnxSql);


            //**************************************************************************

         }

         catch (Exception ex)
         {

            string cMsj = "Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeEquiposSAP", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);

      }


      public List<DataRow> LeeStructLocIFLO(DatosCnxSap cnxSap, string cadCnxSql, RfcParam param)
      {

         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            //**************************************************************************
            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();

            rfc.Planta = param.Planta;
            rfc.FuntionLoc = param.FuntionLoc;
            rfc.CentroCostos = param.CentroCostos;

            lstDatos = rfc.Rfc_Tabla_IFLO(rfcDest, cadCnxSql);


         }

         catch (Exception ex)
         {

            string cMsj = "GetDatos - LeeStructLocIFLO - Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeStructLocIFLO", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;

         }
         return (lstDatos);

      }

      public List<DataRow> LeeStructLocILOA(DatosCnxSap cnxSap, string cadCnxSql, RfcParam param)
      {

         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            //**************************************************************************
            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();

            rfc.Planta = param.Planta;
            rfc.FuntionLoc = param.FuntionLoc;
            rfc.CentroCostos = param.CentroCostos;
            rfc.ObjectReferenceIndicator1 = param.ObjectReferenceIndicator1;
            rfc.ObjectReferenceIndicator2 = param.ObjectReferenceIndicator2;

            lstDatos = rfc.Rfc_Tabla_ILOA(rfcDest, cadCnxSql);


         }

         catch (Exception ex)
         {

            string cMsj = "GetDatos - LeeStructLocILOA - Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeStructLocILOA", "GetDatosSap");
         }

         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }

         return (lstDatos);

      }

      public List<DataRow> LeeStructLocEQUZ(DatosCnxSap cnxSap, string cadCnxSql, RfcParam param)
      {

         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            //**************************************************************************
            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();

            rfc.Planta = param.Planta;
            rfc.FuntionLoc = param.FuntionLoc;
            rfc.CentroCostos = param.CentroCostos;
            rfc.NomenclaEquipo = param.NomenclaEquipo;

            lstDatos = rfc.Rfc_Tabla_EQUZ(rfcDest, cadCnxSql);
         }

         catch (Exception ex)
         {

            string cMsj = "GetDatos - LeeStructLocEQUZ - Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeStructLocEQUZ", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);

      }


      /// <summary>
      /// Lee cat Work center de sap
      /// </summary>
      /// <param name="cnxSap"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> LeeCatWorkCenter(DatosCnxSap cnxSap, string cadCnxSql)
      {

         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            //**************************************************************************
            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();

            rfc.Planta = "1841";
            rfc.FuntionLoc = "";
            rfc.Category = "";
            rfc.EdoEquipo = "";
            rfc.CategoriaWC = "0007";
            rfc.CategoriaMWC = "0005";

            lstDatos = rfc.RfcLeeWorkCenter(rfcDest, cadCnxSql);

         }

         catch (Exception ex)
         {
            string cMsj = "Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeCatWorkCenter", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);

      }

      /// <summary>
      /// Obtiene catalogo de planeadores de Sap
      /// </summary>
      /// <param name="cnxSap"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> LeeCatFocus(DatosCnxSap cnxSap, string cadCnxSql)
      {

         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            //**************************************************************************
            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();

            rfc.Planta = "1841";
            rfc.FuntionLoc = "";
            rfc.Category = "";
            rfc.EdoEquipo = "";
            rfc.CategoriaWC = "";
            rfc.CategoriaMWC = "";

            lstDatos = rfc.RfcLeeCatFocus(rfcDest, cadCnxSql);

         }

         catch (Exception ex)
         {

            string cMsj = "Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeCatFocus", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);

      }

      /// <summary>
      /// Obtiene el cat. de empleados de sap
      /// </summary>
      /// <param name="cnxSap"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> LeeCatEmpleados(DatosCnxSap cnxSap, string cadCnxSql)
      {

         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            //**************************************************************************
            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();

            rfc.Planta = "1841";
            rfc.FuntionLoc = "";
            rfc.Category = "";
            rfc.EdoEquipo = "";
            rfc.CategoriaWC = "";
            rfc.CategoriaMWC = "";

            lstDatos = rfc.RfcLeeCatEmpleados(rfcDest, cadCnxSql);

         }

         catch (Exception ex)
         {

            string cMsj = "Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeCatEmpleados", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);

      }

      /// <summary>
      /// Obtiene el Measuring Point de los equipos
      /// </summary>
      /// <param name="cnxSap"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> LeePMStandar(DatosCnxSap cnxSap, string cadCnxSql)
      {

         CnxToSap cfgSap = null;

         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            //**************************************************************************
            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();

            rfc.Planta = "";
            rfc.FuntionLoc = "";
            rfc.Category = "";
            rfc.EdoEquipo = "";
            rfc.CategoriaWC = "";
            rfc.CategoriaMWC = "";
            rfc.CategoriaMeasuringPoint = "M";

            lstDatos = rfc.RfcLeePMStandar(rfcDest, cadCnxSql);
            
         }

         catch (Exception ex)
         {

            string cMsj = "Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeePMStandar", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);

      }

      /// <summary>
      /// Obtiene el stock de los MRO's
      /// </summary>
      /// <param name="cnxSap"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> LeeStockMro(DatosCnxSap cnxSap, string cadCnxSql)
      {

         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
             rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            //**************************************************************************
            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();

            rfc.Planta = "1841";
            rfc.FuntionLoc = "";
            rfc.Category = "";
            rfc.EdoEquipo = "";
            rfc.CategoriaWC = "";
            rfc.CategoriaMWC = "";
            rfc.CategoriaMeasuringPoint = "";
            rfc.AlmacenMro = "AT56";

            lstDatos = rfc.RfcLeeStockMro(rfcDest, cadCnxSql);
         }

         catch (Exception ex)
         {

            string cMsj = "Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeStockMro", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);

      }

      public List<tblAufk> LeeTablaAUFK(DatosCnxSap cnxSap, string cadCnxSql)
      {

         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<tblAufk> lstDatos = new List<tblAufk>();
         Tools error = new Tools();
         try
         {
             rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            //**************************************************************************
            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();

            rfc.Planta = "1841";



            lstDatos = rfc.Rfc_Tabla_AUFK(rfcDest, cadCnxSql);
         
         }

         catch (Exception ex)
         {

            string cMsj = "GetDatos - LeeTablaAUFK - Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeTablaAUFK", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);

      }


      public List<DataRow> LeeCatEquipos_EQUI(DatosCnxSap cnxSap, string cadCnxSql, RfcParam param)
      {
         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();
            rfc.Category = param.CategoryEq;
            rfc.AutTechnicalobject = param.AutTechnicalobject;

            lstDatos = rfc.Rfc_Tabla_EQUI(rfcDest, cadCnxSql);


         }
         catch (Exception ex)
         {
            string cMsj = "GetDatos - LeeCatEquipos_EQUI - Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeEquiposEQUI", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);
      }

      public List<DataRow> LeeCatEquipos_EQKT(DatosCnxSap cnxSap, string cadCnxSql, RfcParam param)
      {
         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();
            rfc.Language = param.Language;
            rfc.NomenclaEquipo = param.NomenclaEquipo;

            lstDatos = rfc.Rfc_Tabla_EQKT(rfcDest, cadCnxSql);
         }
         catch (Exception ex)
         {
            string cMsj = "GetDatos - LeeCatEquipos_EQKT - Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeCatEquipos_EQKT", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);
      }

      public List<DataRow> LeeCatEquipos_JEST(DatosCnxSap cnxSap, string cadCnxSql, RfcParam param, List<string> lstIdEqui)
      {
         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();
            rfc.Language = param.Language;
            rfc.NomenclaEquipo = param.NomenclaEquipo;

            lstDatos = rfc.Rfc_Tabla_JEST(rfcDest, cadCnxSql, lstIdEqui);

         }
         catch (Exception ex)
         {
            string cMsj = "GetDatos - LeeCatEquipos_JEST - Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeCatEquipos_JEST", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }

         return (lstDatos);
      }

      public List<DataRow> LeeCatEquipos_JCDS(DatosCnxSap cnxSap, string cadCnxSql, RfcParam param, List<string> lstIdEqui)
      {
         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();
            rfc.Language = param.Language;
            rfc.NomenclaEquipo = param.NomenclaEquipo;

            lstDatos = rfc.Rfc_Tabla_JCDS(rfcDest, cadCnxSql, lstIdEqui);

         }
         catch (Exception ex)
         {
            string cMsj = "GetDatos - LeeCatEquipos_JCDS - Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeCatEquipos_JCDS", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }

         return (lstDatos);
      }

      public List<DataRow> LeeCatEquipos_CRHD(DatosCnxSap cnxSap, string cadCnxSql, RfcParam param)
      {
         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();
            rfc.Planta = param.Planta;
            rfc.CategoriaWC = param.CategoryWC;

            lstDatos = rfc.Rfc_Tabla_CRHD(rfcDest, cadCnxSql);

           
         }
         catch (Exception ex)
         {
            string cMsj = "GetDatos - LeeCatEquipos_CRHD - Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeCatEquipos_CRHD", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);
      }

      public List<DataRow> LeeCatEquipos_TJ02T(DatosCnxSap cnxSap, string cadCnxSql, RfcParam param)
      {
         CnxToSap cfgSap = null;
         RfcDestination rfcDest = null;
         List<DataRow> lstDatos = new List<DataRow>();
         Tools error = new Tools();
         try
         {
            rfcDest = RfcDestinationManager.TryGetDestination("Tpm");
            if (rfcDest == null)
            {
               // Conceccion con SAP
               cfgSap = new CnxToSap();
               cfgSap.Host = cnxSap.Host;
               cfgSap.SystemID = cnxSap.SystemID;
               cfgSap.SystemNumber = cnxSap.SystemNumber;
               cfgSap.Client = cnxSap.Client;
               cfgSap.Language = cnxSap.Language;
               cfgSap.PoolSize = cnxSap.PoolSize;
               cfgSap.UserPRD = cnxSap.UserPRD;
               cfgSap.PwdPRD = cnxSap.PwdPRD;
               RfcDestinationManager.RegisterDestinationConfiguration(cfgSap);
            }

            rfcDest = RfcDestinationManager.GetDestination("Tpm");

            // Creamos la clase del RFC
            RfcSap rfc = new RfcSap();
            rfc.Language = param.Language;

            lstDatos = rfc.Rfc_Tabla_TJ02T(rfcDest, cadCnxSql);

         }
         catch (Exception ex)
         {
            string cMsj = "GetDatos - LeeCatEquipos_TJ02T - Falla: ERROR AL EJECUTAR RFC, " + ex.Message;
            error.GuardarError(cadCnxSql, cMsj, ex.StackTrace, "LeeCatEquipos_TJ02T", "GetDatosSap");
         }
         finally
         {
            RfcDestinationManager.UnregisterDestinationConfiguration(cfgSap);
            rfcDest = null;
         }
         return (lstDatos);
      }

   }
}
