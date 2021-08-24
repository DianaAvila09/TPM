using Entidades;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ToolsTpm;


namespace GetDatosSap
{
   public class RfcSap
   {
      #region propiedades para los parametros de sap
      public string Planta { get; set; }
      public string Category { get; set; }
      public string EdoEquipo { get; set; }
      public string FuntionLoc { get; set; }
      public string CategoriaWC { get; set; }
      public string CategoriaMWC { get; set; }
      public string CategoriaMeasuringPoint { get; set; }
      public string AlmacenMro { get; set; }
      public string CentroCostos { get; set; }
      public string Language { get; set; }
      public string NomenclaEquipo { get; set; }
      public string AutTechnicalobject { get; set; }
      public string ObjectReferenceIndicator1 { get; set; }
      public string ObjectReferenceIndicator2 { get; set; }


      #endregion

      /// <summary>
      // Lee el RFC ZPM_TPMEQUIPOS del sap y devuelve un datatable con los equipos productivos
      /// </summary>
      /// <param name="destination"></param>
      /// <param name="cnx"></param>
      /// <returns></returns>
      public List<DataRow> RfcCatLeeEquiposSAP(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstDatos = null;
         Tools error = new Tools();

         DataTable tblTempEqui = new DataTable();
         try
         {
            RfcRepository repo = destination.Repository;
            IRfcFunction bapi = repo.CreateFunction("ZPM_TPMEQUIPOS");

            #region  Definicion de las Variables de parametros

            bapi.SetValue("IN_PLANTA", Planta);
            bapi.SetValue("IN_EQTYP", Category);
            bapi.SetValue("IN_TXT04", EdoEquipo);

            #endregion

            #region Definicion parametros de la Tabla Tipos

            InputRangos rango = new InputRangos();

            rango.Sign = "I";
            rango.Opcion = "NE";
            rango.Low = "X";
            rango.High = "";

            //Get reference to table object
            IRfcTable tblTipos = bapi.GetTable("IN_INACT");
            //Create new registro en la tabla y agregamos los valores
            tblTipos.Append();
            tblTipos.SetValue("SIGN_R", rango.Sign);
            tblTipos.SetValue("OPTION_R", rango.Opcion);
            tblTipos.SetValue("LOW", rango.Low);
            tblTipos.SetValue("HIGH", rango.High);

            //agregamos la tabla de filtros de codigos
            bapi.SetValue("IN_INACT", tblTipos);

            #endregion

            #region Invocar el RFC para sustraer datos

            bapi.Invoke(destination);

            #endregion


            // Pasamos los datos a una lista generica
            IRfcTable tblEqui = bapi.GetTable("OUT_TPMEQUIPOS");
            DataTable dtDatos = new DataTable();
            tblEqui.ToList();
            dtDatos = Tools.GetDataTableFromRfcTable(tblEqui);
            lstDatos = dtDatos.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();


         }
         #region Errores de sap
         catch (RfcCommunicationException ex)
         {

            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeEquiposSAP", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeEquiposSAP", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeEquiposSAP", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeEquiposSAP", "RfcSap");
         }
         #endregion

         return (lstDatos);
      }

      /// <summary>
      /// Lee la estructura de function location de sap
      /// </summary>
      /// <param name="destination"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> RfcLeeEstructura(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstDatos = null;
         Tools error = new Tools();

         DataTable tblTempEqui = new DataTable();
         try
         {
            RfcRepository repo = destination.Repository;
            IRfcFunction bapi = repo.CreateFunction("ZPM_TPMSTRUEQU");

            #region  Definicion de las Variables de parametros

            string pIN_PLANTA = Planta;
            bapi.SetValue("IN_PLANTA", pIN_PLANTA);

            #endregion

            #region Definicion parametros de la Tabla Funtion Location
            //Get reference to table object
            IRfcTable tblTipos = bapi.GetTable("IN_TPLNR");

            tblTipos.Append();

            tblTipos.SetValue("SIGN_R", "I");
            tblTipos.SetValue("OPTION_R", "CP");
            tblTipos.SetValue("LOW", FuntionLoc);

            //agregamos la tabla de filtros de codigos
            bapi.SetValue("IN_TPLNR", tblTipos);

            IRfcTable tblFL = bapi.GetTable("IN_OWNER");

            tblFL.Append();

            tblFL.SetValue("SIGN_R", "I");
            tblFL.SetValue("OPTION_R", "EQ");
            tblFL.SetValue("LOW", "2");


            //Create new registro en la tabla y agregamos los valores
            //tblFL.Append();

            //tblFL.SetValue("SIGN_R", "I");
            //tblFL.SetValue("OPTION_R", "EQ");
            //tblFL.SetValue("LOW", " ");

            //agregamos la tabla de filtros de codigos
            bapi.SetValue("IN_OWNER", tblFL);

            #endregion

            #region Invocar el RFC para sustraer datos

            bapi.Invoke(destination);

            #endregion
            // Pasamos los datos a una lista generica
            IRfcTable tblEqui = bapi.GetTable("OUT_TPMSTRUEQU");
            DataTable dtDatos = new DataTable();
            tblEqui.ToList();
            dtDatos = Tools.GetDataTableFromRfcTable(tblEqui);
            lstDatos = dtDatos.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();


         }
         #region Errores de sap
         catch (RfcCommunicationException ex)
         {

            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeEstructura", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeEstructura", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeEstructura", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeEquiposSAP", "RfcSap");
         }
         #endregion

         return (lstDatos);
      }

      /// <summary>
      /// Lee Cat. de work center y Main work center de sap
      /// </summary>
      /// <param name="destination"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> RfcLeeWorkCenter(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstDatos = null;
         Tools error = new Tools();

         DataTable tblTempEqui = new DataTable();
         try
         {
            RfcRepository repo = destination.Repository;
            IRfcFunction bapi = repo.CreateFunction("ZPM_TPMWORKCENTER");

            #region Definicion parametros de la Tabla Funtion Location

            string pIN_PLANTA = Planta;
            bapi.SetValue("IN_PLANTA", pIN_PLANTA);



            //Get reference to table object
            IRfcTable tblWC = bapi.GetTable("IN_CATEG");

            //Create new registro en la tabla y agregamos los valores
            tblWC.Append();
            tblWC.SetValue("SIGN_R", "I");
            tblWC.SetValue("OPTION_R", "EQ");
            tblWC.SetValue("LOW", CategoriaWC);

            //Create new registro en la tabla y agregamos los valores
            tblWC.Append();
            tblWC.SetValue("SIGN_R", "I");
            tblWC.SetValue("OPTION_R", "EQ");
            tblWC.SetValue("LOW", CategoriaMWC);

            //agregamos la tabla de filtros de codigos
            bapi.SetValue("IN_CATEG", tblWC);

            #endregion

            #region Invocar el RFC para sustraer datos

            bapi.Invoke(destination);

            #endregion

            // Pasamos los datos a una lista generica
            IRfcTable tbl = bapi.GetTable("OUT_TPMWC");
            DataTable dtDatos = new DataTable();
            tbl.ToList();
            dtDatos = Tools.GetDataTableFromRfcTable(tbl);
            lstDatos = dtDatos.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();

         }
         #region Errores de sap
         catch (RfcCommunicationException ex)
         {

            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeWorkCenter", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeWorkCenter", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeWorkCenter", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeWorkCenter", "RfcSap");
         }
         #endregion

         return (lstDatos);
      }


      /// <summary>
      /// Lee Cat. de work center y Main work center de sap
      /// </summary>
      /// <param name="destination"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> RfcLeeCatFocus(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstDatos = null;
         Tools error = new Tools();

         DataTable tblTempEqui = new DataTable();
         try
         {
            RfcRepository repo = destination.Repository;
            IRfcFunction bapi = repo.CreateFunction("ZPM_TPMFOCUSFT");

            #region Definicion parametros de la Tabla Funtion Location

            bapi.SetValue("IN_PLANTA", Planta);

            #endregion

            #region Invocar el RFC para sustraer datos

            bapi.Invoke(destination);

            #endregion

            // Pasamos los datos a una lista generica
            IRfcTable tbl = bapi.GetTable("OUT_TPMFOCUSFT");
            DataTable dtDatos = new DataTable();
            tbl.ToList();
            dtDatos = Tools.GetDataTableFromRfcTable(tbl);
            lstDatos = dtDatos.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();

         }
         #region Errores de sap
         catch (RfcCommunicationException ex)
         {

            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeCatFacous", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeCatFacous", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeCatFacous", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeCatFacous", "RfcSap");
         }
         #endregion

         return (lstDatos);
      }

      /// <summary>
      /// Obtiene el cat. de empleados de sap
      /// </summary>
      /// <param name="destination"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> RfcLeeCatEmpleados(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstDatos = null;
         Tools error = new Tools();

         DataTable tblTempEqui = new DataTable();
         try
         {
            RfcRepository repo = destination.Repository;
            IRfcFunction bapi = repo.CreateFunction("ZPM_TPMEMPLEADOS");

            #region Definicion parametros de la Tabla Funtion Location

            bapi.SetValue("IN_PLANTA", Planta);

            #endregion

            #region Invocar el RFC para sustraer datos

            bapi.Invoke(destination);

            #endregion

            // Pasamos los datos a una lista generica
            IRfcTable tbl = bapi.GetTable("OUT_TPMEMPLEADOS");
            DataTable dtDatos = new DataTable();
            tbl.ToList();
            dtDatos = Tools.GetDataTableFromRfcTable(tbl);
            lstDatos = dtDatos.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();

         }
         #region Errores de sap
         catch (RfcCommunicationException ex)
         {

            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeCatEmpleados", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeCatEmpleados", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeCatEmpleados", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeCatEmpleados", "RfcSap");
         }
         #endregion

         return (lstDatos);
      }

      /// <summary>
      /// Obtiene los Measure point de los equipos
      /// </summary>
      /// <param name="destination"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> RfcLeePMStandar(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstDatos = null;
         Tools error = new Tools();

         DataTable tblTempEqui = new DataTable();
         try
         {
            RfcRepository repo = destination.Repository;
            IRfcFunction bapi = repo.CreateFunction("ZPM_TPMSTDAR");

            #region  Definicion de las Variables de parametros
            bapi.SetValue("IN_MPTYP", CategoriaMeasuringPoint);


            //Get reference to table object 
            //Structure para obtener Technical object authorization group
            IRfcTable tblMP = bapi.GetTable("IN_BEGRU");
            tblMP.Append();

            tblMP.SetValue("SIGN_R", "I");
            tblMP.SetValue("OPTION_R", "CP");
            tblMP.SetValue("LOW", "AP*");

            #endregion

            #region Invocar el RFC para sustraer datos

            bapi.Invoke(destination);

            #endregion

            // Pasamos los datos a una lista generica
            IRfcTable tbl = bapi.GetTable("OUT_TPMSTDAR");
            DataTable dtDatos = new DataTable();
            tbl.ToList();
            dtDatos = Tools.GetDataTableFromRfcTable(tbl);
            lstDatos = dtDatos.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();

         }
         #region Errores de sap
         catch (RfcCommunicationException ex)
         {

            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeePMStandar", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeePMStandar", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeePMStandar", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeePMStandar", "RfcSap");
         }
         #endregion

         return (lstDatos);
      }

      /// <summary>
      /// Obtiene el Stock actual de los MRO's del almacen AT56
      /// </summary>
      /// <param name="destination"></param>
      /// <param name="cadCnxSql"></param>
      /// <returns></returns>
      public List<DataRow> RfcLeeStockMro(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstDatos = null;
         Tools error = new Tools();

         DataTable tblTempEqui = new DataTable();
         try
         {
            RfcRepository repo = destination.Repository;
            IRfcFunction bapi = repo.CreateFunction("ZPM_TPMSTOCKMRO");

            #region  Definicion de las Variables de parametros

            bapi.SetValue("IN_PLANTA", Planta);
            bapi.SetValue("IN_LGORT", AlmacenMro);

            //Get reference to table object
            IRfcTable tblMat = bapi.GetTable("IN_MATNR");

            //Create new registro en la tabla y agregamos los valores
            tblMat.Append();

            tblMat.SetValue("SIGN_R", "I");
            tblMat.SetValue("OPTION_R", "CP");
            tblMat.SetValue("LOW", "MRO*");

            bapi.SetValue("IN_MATNR", tblMat);

            //Get reference to table object
            IRfcTable tblStatus = bapi.GetTable("IN_MSTAE");

            tblStatus.Append();
            tblStatus.SetValue("SIGN_R", "I");
            tblStatus.SetValue("OPTION_R", "EQ");
            tblStatus.SetValue("LOW", " ");

            tblStatus.Append();
            tblStatus.SetValue("SIGN_R", "I");
            tblStatus.SetValue("OPTION_R", "EQ");
            tblStatus.SetValue("LOW", "Z1");

            tblStatus.Append();
            tblStatus.SetValue("SIGN_R", "I");
            tblStatus.SetValue("OPTION_R", "EQ");
            tblStatus.SetValue("LOW", "Z2");


            bapi.SetValue("IN_MSTAE", tblStatus);

            #endregion

            #region Invocar el RFC para sustraer datos

            bapi.Invoke(destination);

            #endregion

            // Pasamos los datos a una lista generica
            IRfcTable tbl = bapi.GetTable("OUT_STOCKMRO");
            DataTable dtDatos = new DataTable();
            tbl.ToList();
            dtDatos = Tools.GetDataTableFromRfcTable(tbl);
            lstDatos = dtDatos.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }
         #region Errores de sap
         catch (RfcCommunicationException ex)
         {

            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeStockMro", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeStockMro", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeStockMro", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "RfcLeeStockMro", "RfcSap");
         }
         #endregion

         return (lstDatos);
      }


      public List<tblAufk> Rfc_Tabla_AUFK(RfcDestination destination, string cadCnxSql)
      {
         List<tblAufk> lstTblaAufk = new List<tblAufk>();
         Tools error = new Tools();
         string tabla = "AUFK";

         try
         {
            RfcRepository repo = destination.Repository; // SELECT * FROM (QUERY_TABLE) INTO <WA> WHERE (OPTIONS).
            IRfcFunction bapi = repo.CreateFunction("RFC_READ_TABLE"); // Funcion stdar para leer una tabla

            bapi.SetValue("QUERY_TABLE", tabla); // Tabla sobre la que queremos buscar
            bapi.SetValue("DELIMITER", "|"); // Delimitador de valores

            #region campos de la tabla a extraer
            // Tabla de campos a sustraer
            IRfcTable tblCampos = bapi.GetTable("FIELDS");

            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "AUFNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "AUART");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "AUTYP");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "ERNAM");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "ERDAT");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "KTEXT");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "WERKS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "KOSTV");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "IDAT2");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "SAKNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "OBJNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "SCOPE");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "ERFZEIT");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "VAPLZ");


            //agregamos la tabla 
            bapi.SetValue("FIELDS", tblCampos);

            #endregion

            #region Condiciones

            ////// Tabla de campos a sustraer
            IRfcTable tblCondicion = bapi.GetTable("OPTIONS");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", "WERKS = '1841' ");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", " and ERDAT >= '06/01/2019'");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", " and ( AUART = 'PM01' or AUART = 'PM02' or AUART = 'PM03' ");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", "  or AUART = 'PM04') ");

            #endregion

            #region Invocamos el FRC

            bapi.Invoke(destination);

            #endregion

            //#region Agregar la tabla de Wo Planning a SQL

            DataTable tblTemp = new DataTable();

            IRfcTable tblDatas = bapi.GetTable("DATA");
            foreach (IRfcStructure row in tblDatas)
            {
               // Sustraemos los datos en un arreglo de cadena

               string[] fieldValues = (row.GetValue("WA")).ToString().Split('|');

               tblAufk dr = new tblAufk();

               dr.Ordernum = fieldValues[0];
               dr.OrderType = fieldValues[1];
               dr.OrderCategory = fieldValues[2];
               dr.Creadapor = fieldValues[3];
               dr.FchaCreacion = fieldValues[4];
               dr.Descripcion = fieldValues[5];
               dr.Planta = fieldValues[6];
               dr.CentroCtos = fieldValues[7];
               dr.FchaTeco = fieldValues[8];
               dr.GLAccount = fieldValues[9];
               dr.ObjectNumber = fieldValues[10];
               dr.ObjectClass = fieldValues[11];
               dr.TimeCreate = fieldValues[12];
               dr.MainWC = fieldValues[13];

               lstTblaAufk.Add(dr);
            }

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }

         #region Errores de sap
         catch (RfcCommunicationException ex)
         {

            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_AUFK", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_AUFK", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_AUFK", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_AUFK", "RfcSap");
         }
         #endregion



         return (lstTblaAufk);
      }


      public List<DataRow> Rfc_Tabla_IFLO(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstTbla = new List<DataRow>();
         Tools error = new Tools();
         string tabla = "IFLO";

         try
         {
            RfcRepository repo = destination.Repository; // SELECT * FROM (QUERY_TABLE) INTO <WA> WHERE (OPTIONS).
            IRfcFunction bapi = repo.CreateFunction("RFC_READ_TABLE"); // Funcion stdar para leer una tabla

            bapi.SetValue("QUERY_TABLE", tabla); // Tabla sobre la que queremos buscar
            bapi.SetValue("DELIMITER", "|"); // Delimitador de valores

            #region campos de la tabla a extraer
            // Tabla de campos a sustraer
            IRfcTable tblCampos = bapi.GetTable("FIELDS");

            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "TPLNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "SPRAS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "PLTXT");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "TPLMA");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "PM_OBJTY");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "OBJNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "SWERK");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "KOKRS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "KOSTL");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "INGRP");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "LGWID");

            //agregamos la tabla 
            bapi.SetValue("FIELDS", tblCampos);

            #endregion

            #region Condiciones

            ////// Tabla de campos a sustraer
            IRfcTable tblCondicion = bapi.GetTable("OPTIONS");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", "SWERK = '" + Planta + "' ");
            //tblCondicion.Append();
            //tblCondicion.SetValue("TEXT", " and KOSTL = '" + CentroCostos + "'");
           tblCondicion.Append();
            tblCondicion.SetValue("TEXT", " and SPRAS = 'EN'");
            #endregion

            #region Invocamos el FRC

            bapi.Invoke(destination);

            #endregion

            //#region Agregar la tabla

            DataTable tblTemp = new DataTable();

            IRfcTable tblDatas = bapi.GetTable("DATA");

            tblTemp = Tools.GetDataTableFromRfcTable(tblDatas);
            lstTbla = tblTemp.AsEnumerable().ToList();


            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }

         #region Errores de sap
         catch (RfcCommunicationException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_IFLO", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_IFLO", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_IFLO", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_IFLO", "RfcSap");
         }
         #endregion

         return (lstTbla);
      }

      public List<DataRow> Rfc_Tabla_ILOA(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstTbla = new List<DataRow>();
         Tools error = new Tools();
         string tabla = "ILOA";

         try
         {
            RfcRepository repo = destination.Repository; // SELECT * FROM (QUERY_TABLE) INTO <WA> WHERE (OPTIONS).
            IRfcFunction bapi = repo.CreateFunction("RFC_READ_TABLE"); // Funcion stdar para leer una tabla

            bapi.SetValue("QUERY_TABLE", tabla); // Tabla sobre la que queremos buscar
            bapi.SetValue("DELIMITER", "|"); // Delimitador de valores

            #region campos de la tabla a extraer
            // Tabla de campos a sustraer
            IRfcTable tblCampos = bapi.GetTable("FIELDS");

            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "ILOAN");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "TPLNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "SWERK");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "PPSID");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "OWNER");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "KOSTL");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "CR_OBJTY");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "PPSID");


            //agregamos la tabla 
            bapi.SetValue("FIELDS", tblCampos);

            #endregion

            #region Condiciones

            ////// Tabla de campos a sustraer
            IRfcTable tblCondicion = bapi.GetTable("OPTIONS");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", "SWERK = '" + Planta + "' "); 
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", " and  OWNER =  ''");
            #endregion

            #region Invocamos el FRC

            bapi.Invoke(destination);

            #endregion

            //#region Agregar la tabla de Wo Planning a SQL

            //#region Agregar la tabla

            DataTable tblTemp = new DataTable();
            IRfcTable tblDatas = bapi.GetTable("DATA");
            tblTemp = Tools.GetDataTableFromRfcTable(tblDatas);
            lstTbla = tblTemp.AsEnumerable().ToList();

            
            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }

         #region Errores de sap
         catch (RfcCommunicationException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_ILOA", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_ILOA", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_ILOA", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_ILOA", "RfcSap");
         }
         #endregion

         return (lstTbla);
      }

      public List<DataRow> Rfc_Tabla_EQUZ(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstTbla = new List<DataRow>();
         Tools error = new Tools();
         string tabla = "EQUZ";

         try
         {
            RfcRepository repo = destination.Repository; // SELECT * FROM (QUERY_TABLE) INTO <WA> WHERE (OPTIONS).
            IRfcFunction bapi = repo.CreateFunction("RFC_READ_TABLE"); // Funcion stdar para leer una tabla

            bapi.SetValue("QUERY_TABLE", tabla); // Tabla sobre la que queremos buscar
            bapi.SetValue("DELIMITER", "|"); // Delimitador de valores

            #region campos de la tabla a extraer
            // Tabla de campos a sustraer
            IRfcTable tblCampos = bapi.GetTable("FIELDS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "DATBI");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "ILOAN");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "EQUNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "DATAB");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "IWERK");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "INGRP");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "PM_OBJTY");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "GEWRK");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "TIDNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "HEQUI");

            //agregamos la tabla 
            bapi.SetValue("FIELDS", tblCampos);

            #endregion

            #region Condiciones

            ////// Tabla de campos a sustraer
            IRfcTable tblCondicion = bapi.GetTable("OPTIONS");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", "IWERK = '" + Planta + "' ");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", " and RBNR <> 'ZDIES' ");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", " and DATBI = '99991231' ");

                        #endregion

            #region Invocamos el FRC

            bapi.Invoke(destination);

            #endregion

            //#region Agregar la tabla

            DataTable tblTemp = new DataTable();
            IRfcTable tblDatas = bapi.GetTable("DATA");
            tblTemp = Tools.GetDataTableFromRfcTable(tblDatas);
            lstTbla = tblTemp.AsEnumerable().ToList();




            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }

         #region Errores de sap
         catch (RfcCommunicationException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQUZ", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQUZ", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQUZ", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQUZ", "RfcSap");
         }
         #endregion

         return (lstTbla);
      }

      public List<DataRow> Rfc_Tabla_EQUI(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstTbla = new List<DataRow>();
         Tools error = new Tools();
         string tabla = "EQUI";

         try
         {
            RfcRepository repo = destination.Repository; // SELECT * FROM (QUERY_TABLE) INTO <WA> WHERE (OPTIONS).
            IRfcFunction bapi = repo.CreateFunction("RFC_READ_TABLE"); // Funcion stdar para leer una tabla

            bapi.SetValue("QUERY_TABLE", tabla); // Tabla sobre la que queremos buscar
            bapi.SetValue("DELIMITER", "|"); // Delimitador de valores

            #region campos de la tabla a extraer
            // Tabla de campos a sustraer
            IRfcTable tblCampos = bapi.GetTable("FIELDS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "EQUNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "EQASP");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "BEGRU");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "EQTYP");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "EQART");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "HERST");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "TYPBZ");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "EQLFN");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "OBJNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "WARPL");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "IMRC_POINT");

            //agregamos la tabla 
            bapi.SetValue("FIELDS", tblCampos);

            #endregion

            #region Condiciones

            ////// Tabla de campos a sustraer
            IRfcTable tblCondicion = bapi.GetTable("OPTIONS");
            //tblCondicion.Append();
            //tblCondicion.SetValue("TEXT", "EQTYP = '" + Category + "' ");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", "BEGRU LIKE '" + AutTechnicalobject + "' ");

            #endregion

            #region Invocamos el FRC

            bapi.Invoke(destination);

            #endregion

            //#region Agregar la tabla

            DataTable tblTemp = new DataTable();
            IRfcTable tblDatas = bapi.GetTable("DATA");
            tblTemp = Tools.GetDataTableFromRfcTable(tblDatas);
            lstTbla = tblTemp.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }

         #region Errores de sap
         catch (RfcCommunicationException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQUI", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQUI", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQUI", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQUI", "RfcSap");
         }
         #endregion

         return (lstTbla);
      }

      public List<DataRow> Rfc_Tabla_EQKT(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstTbla = new List<DataRow>();
         Tools error = new Tools();
         string tabla = "EQKT";

         try
         {
            RfcRepository repo = destination.Repository; // SELECT * FROM (QUERY_TABLE) INTO <WA> WHERE (OPTIONS).
            IRfcFunction bapi = repo.CreateFunction("RFC_READ_TABLE"); // Funcion stdar para leer una tabla

            bapi.SetValue("QUERY_TABLE", tabla); // Tabla sobre la que queremos buscar
            bapi.SetValue("DELIMITER", "|"); // Delimitador de valores

            #region campos de la tabla a extraer
            // Tabla de campos a sustraer
            IRfcTable tblCampos = bapi.GetTable("FIELDS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "EQUNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "SPRAS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "EQKTX");

            //agregamos la tabla 
            bapi.SetValue("FIELDS", tblCampos);

            #endregion

            #region Condiciones

            ////// Tabla de campos a sustraer
            IRfcTable tblCondicion = bapi.GetTable("OPTIONS");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", "SPRAS = '" + Language + "' ");
            //tblCondicion.Append();
            //tblCondicion.SetValue("TEXT", "and EQUNR LIKE '" + NomenclaEquipo + "' ");

            #endregion

            #region Invocamos el FRC

            bapi.Invoke(destination);

            #endregion

            //#region Agregar la tabla

            DataTable tblTemp = new DataTable();
            IRfcTable tblDatas = bapi.GetTable("DATA");
            tblTemp = Tools.GetDataTableFromRfcTable(tblDatas);
            lstTbla = tblTemp.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }

         #region Errores de sap
         catch (RfcCommunicationException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQKT", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQKT", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQKT", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_EQKT", "RfcSap");
         }
         #endregion

         return (lstTbla);
      }


      public List<DataRow> Rfc_Tabla_CRHD(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstTbla = new List<DataRow>();
         Tools error = new Tools();
         string tabla = "CRHD";

         try
         {
            RfcRepository repo = destination.Repository; // SELECT * FROM (QUERY_TABLE) INTO <WA> WHERE (OPTIONS).
            IRfcFunction bapi = repo.CreateFunction("RFC_READ_TABLE"); // Funcion stdar para leer una tabla

            bapi.SetValue("QUERY_TABLE", tabla); // Tabla sobre la que queremos buscar
            bapi.SetValue("DELIMITER", "|"); // Delimitador de valores

            #region campos de la tabla a extraer
            // Tabla de campos a sustraer
            IRfcTable tblCampos = bapi.GetTable("FIELDS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "OBJTY");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "OBJID");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "ARBPL");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "WERKS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "VERWE");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "STAND");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "VERAN");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "KTSCH");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "VGWTS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "ZGR02");

            //agregamos la tabla 
            bapi.SetValue("FIELDS", tblCampos);

            #endregion

            #region Condiciones

            ////// Tabla de campos a sustraer
            IRfcTable tblCondicion = bapi.GetTable("OPTIONS");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", "WERKS = '" + Planta + "' ");
            //tblCondicion.Append();
            //tblCondicion.SetValue("TEXT", "and VERWE = '" + CategoriaWC + "' ");

            #endregion

            #region Invocamos el FRC

            bapi.Invoke(destination);

            #endregion

            //#region Agregar la tabla

            DataTable tblTemp = new DataTable();
            IRfcTable tblDatas = bapi.GetTable("DATA");
            tblTemp = Tools.GetDataTableFromRfcTable(tblDatas);
            lstTbla = tblTemp.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }

         #region Errores de sap
         catch (RfcCommunicationException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_CRHD", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_CRHD", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_CRHD", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_CRHD", "RfcSap");
         }
         #endregion

         return (lstTbla);
      }

      public List<DataRow> Rfc_Tabla_JEST(RfcDestination destination, string cadCnxSql, List<string> lstIdEqui)
      {
         List<DataRow> lstTbla = new List<DataRow>();
         Tools error = new Tools();
         string tabla = "JEST";

         try
         {
            RfcRepository repo = destination.Repository; // SELECT * FROM (QUERY_TABLE) INTO <WA> WHERE (OPTIONS).
            IRfcFunction bapi = repo.CreateFunction("RFC_READ_TABLE"); // Funcion stdar para leer una tabla

            bapi.SetValue("QUERY_TABLE", tabla); // Tabla sobre la que queremos buscar
            bapi.SetValue("DELIMITER", "|"); // Delimitador de valores

            #region campos de la tabla a extraer
            // Tabla de campos a sustraer
            IRfcTable tblCampos = bapi.GetTable("FIELDS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "OBJNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "STAT");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "INACT");

            //agregamos la tabla 
            bapi.SetValue("FIELDS", tblCampos);

            #endregion

            #region Condiciones



            ////// Tabla de campos a sustraer
            IRfcTable tblCondicion = bapi.GetTable("OPTIONS");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", " (OBJNR = '" + lstIdEqui[0].Trim() + "' ");

            for (int i = 1; i < lstIdEqui.Count - 1; i++)
            {
               tblCondicion.Append();
               tblCondicion.SetValue("TEXT", " OR OBJNR = '" + lstIdEqui[i].Trim() + "' ");
            }
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", " OR OBJNR = '" + lstIdEqui[lstIdEqui.Count-1].Trim() + "' )");

            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", "and INACT =' ' ");

            #endregion

            #region Invocamos el FRC

            bapi.Invoke(destination);

            #endregion

            //#region Agregar la tabla

            DataTable tblTemp = new DataTable();
            IRfcTable tblDatas = bapi.GetTable("DATA");
            tblTemp = Tools.GetDataTableFromRfcTable(tblDatas);
            lstTbla = tblTemp.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }

         #region Errores de sap
         catch (RfcCommunicationException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_JEST", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_JEST", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_JEST", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_JEST", "RfcSap");
         }
         #endregion

         return (lstTbla);
      }

      public List<DataRow> Rfc_Tabla_JCDS(RfcDestination destination, string cadCnxSql, List<string> lstIdEqui)
      {
         List<DataRow> lstTbla = new List<DataRow>();
         Tools error = new Tools();
         string tabla = "JCDS";

         try
         {
            RfcRepository repo = destination.Repository; // SELECT * FROM (QUERY_TABLE) INTO <WA> WHERE (OPTIONS).
            IRfcFunction bapi = repo.CreateFunction("RFC_READ_TABLE"); // Funcion stdar para leer una tabla

            bapi.SetValue("QUERY_TABLE", tabla); // Tabla sobre la que queremos buscar
            bapi.SetValue("DELIMITER", "|"); // Delimitador de valores

            #region campos de la tabla a extraer
            // Tabla de campos a sustraer
            IRfcTable tblCampos = bapi.GetTable("FIELDS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "OBJNR");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "STAT");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "UDATE");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "UTIME");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "INACT");

            //agregamos la tabla 
            bapi.SetValue("FIELDS", tblCampos);

            #endregion

            #region Condiciones



            ////// Tabla de campos a sustraer
            IRfcTable tblCondicion = bapi.GetTable("OPTIONS");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", " (OBJNR = '" + lstIdEqui[0].Trim() + "' ");

            for (int i = 1; i < lstIdEqui.Count - 1; i++)
            {
               tblCondicion.Append();
               tblCondicion.SetValue("TEXT", " OR OBJNR = '" + lstIdEqui[i].Trim() + "' ");
            }
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", " OR OBJNR = '" + lstIdEqui[lstIdEqui.Count - 1].Trim() + "' )");

            //tblCondicion.Append();
            //tblCondicion.SetValue("TEXT", "and INACT =' ' ");

            #endregion

            #region Invocamos el FRC

            bapi.Invoke(destination);

            #endregion

            //#region Agregar la tabla

            DataTable tblTemp = new DataTable();
            IRfcTable tblDatas = bapi.GetTable("DATA");
            tblTemp = Tools.GetDataTableFromRfcTable(tblDatas);
            lstTbla = tblTemp.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }

         #region Errores de sap
         catch (RfcCommunicationException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_JCDS", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_JCDS", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_JCDS", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_JCDS", "RfcSap");
         }
         #endregion

         return (lstTbla);
      }


      public List<DataRow> Rfc_Tabla_TJ02T(RfcDestination destination, string cadCnxSql)
      {
         List<DataRow> lstTbla = new List<DataRow>();
         Tools error = new Tools();
         string tabla = "TJ02T";

         try
         {
            RfcRepository repo = destination.Repository; // SELECT * FROM (QUERY_TABLE) INTO <WA> WHERE (OPTIONS).
            IRfcFunction bapi = repo.CreateFunction("RFC_READ_TABLE"); // Funcion stdar para leer una tabla

            bapi.SetValue("QUERY_TABLE", tabla); // Tabla sobre la que queremos buscar
            bapi.SetValue("DELIMITER", "|"); // Delimitador de valores

            #region campos de la tabla a extraer
            // Tabla de campos a sustraer
            IRfcTable tblCampos = bapi.GetTable("FIELDS");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "ISTAT");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "TXT04");
            tblCampos.Append();
            tblCampos.SetValue("FIELDNAME", "TXT30");

            //agregamos la tabla 
            bapi.SetValue("FIELDS", tblCampos);

            #endregion

            #region Condiciones

            ////// Tabla de campos a sustraer
            IRfcTable tblCondicion = bapi.GetTable("OPTIONS");
            tblCondicion.Append();
            tblCondicion.SetValue("TEXT", " SPRAS = '" + Language + "' ");

            #endregion

            #region Invocamos el FRC

            bapi.Invoke(destination);

            #endregion

            //#region Agregar la tabla

            DataTable tblTemp = new DataTable();
            IRfcTable tblDatas = bapi.GetTable("DATA");
            tblTemp = Tools.GetDataTableFromRfcTable(tblDatas);
            lstTbla = tblTemp.AsEnumerable().ToList();

            //Recolectamos la basura
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }

         #region Errores de sap
         catch (RfcCommunicationException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_TJ02T", "RfcSap");
         }
         catch (RfcLogonException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_TJ02T", "RfcSap");
         }
         catch (RfcAbapRuntimeException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_TJ02T", "RfcSap");
         }
         catch (RfcAbapBaseException ex)
         {
            error.GuardarError(cadCnxSql, ex.Message, ex.StackTrace, "Rfc_Tabla_TJ02T", "RfcSap");
         }
         #endregion

         return (lstTbla);
      }
   }
}
