using System;
using SAP.Middleware.Connector;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;


namespace Atk_wsHistProdxDia
{
    public class RfcSap
    {
        static EnviarCorreo email = new EnviarCorreo();
        public void LeeMatNotificado(RfcDestination destination, SqlConnection cnx, DateTime pFI, DateTime pFF)
        {
            try
            {
                string cmdSql = "";
                RfcRepository repo = destination.Repository;
                IRfcFunction bapiMatNotif = repo.CreateFunction("Z_PP_PDNOTIFICADO");

                #region  Definicion de las Variables Import del RFC

                InParametros itemParametros = new InParametros();
                itemParametros.In_Planta = "1848";
                bapiMatNotif.SetValue("IN_NUM_PLANTA", itemParametros.In_Planta);

                #endregion

                #region Definicion parametros de la Tabla Periodo

                InPeriodo dFechas = new InPeriodo();

                dFechas.Sign_R = "I";
                dFechas.Opcion_R = "BT";
                dFechas.Low = pFI.Date;
                dFechas.High = pFF.Date;

                //Get reference to table object
                IRfcTable tblPeriodo = bapiMatNotif.GetTable("IN_FEC_INI_PRODUC");

                //Crear un nuevo registro en la tabla y agregar los valores
                tblPeriodo.Append();

                tblPeriodo.SetValue("SIGN_R", dFechas.Sign_R);
                tblPeriodo.SetValue("OPTION_R", dFechas.Opcion_R);
                tblPeriodo.SetValue("LOW", dFechas.Low);
                tblPeriodo.SetValue("HIGH", dFechas.High);

                //agregamos la tabla de filtros del periodo
                bapiMatNotif.SetValue("IN_FEC_INI_PRODUC", tblPeriodo);

                #endregion

                bapiMatNotif.Invoke(destination);

                #region Agrega la tabla de materiales notificados
                DataTable dtMatNotif = new DataTable();
                IRfcTable tblMatNotif = bapiMatNotif.GetTable("OUT_NOTIFICADO");
                dtMatNotif = GetDataTableFromRfcTable(tblMatNotif);

                DataTable tblMatNotificados = new DataTable();

                // Generamos las estructura de la tabla de materiales Notificados
                SqlDataAdapter da = new SqlDataAdapter();
                cmdSql = "Select * from HtProdxDia where MATNR = 'XX'";
                da.SelectCommand = new SqlCommand(cmdSql, cnx);
                da.Fill(tblMatNotificados);

                DateTime fecAct = DateTime.Now;

                foreach (DataRow row in dtMatNotif.Rows)
                {
                    DataRow item = tblMatNotificados.NewRow();

                    item["WERKS"] = row["WERKS"];
                    item["MATNR"] = row["MATNR"];
                    item["VERID"] = row["VERID"];
                    item["MDV01"] = row["MDV01"];
                    item["SPTAG"] = Convert.ToDateTime(row["SPTAG"]).Date; ;
                    item["WEMNG"] = Convert.ToDecimal(row["WEMNG"]);
                    item["AMEIN"] = row["AMEIN"];
                    item["FEC_UPDATE"] = fecAct;

                    tblMatNotificados.Rows.Add(item);
                }


                #region Borrado de las tablas de SQL: MaterialNotifi

                cmdSql = "TRUNCATE table TempHtProdxDia";
                SqlCommand cmdDele = new SqlCommand(cmdSql, cnx);
                cmdDele.ExecuteNonQuery();

                #endregion

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cnx))
                {
                    bulkCopy.DestinationTableName = "dbo.TempHtProdxDia";
                    try
                    {
                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(tblMatNotificados);
                    }
                    catch (Exception ex)
                    {
                        
                        TextWriter twe = new StreamWriter(Atk_wsHistProdxDia.Service1.pathe, true);
                        twe.WriteLine("ws_HistProdxDia FALLA de RFC: Al agregar Notificado al SQL:   --> fecha de " + DateTime.Now.ToString());
                        twe.Close();
                        EnviarCorreo email = new EnviarCorreo();
                        email.Enviar("ws_HistProdxDia FALLA de RFC: Al agregar Notificado al SQL:   --> fecha de " + DateTime.Now.ToString());
                    }
                }
                #endregion



                //Recolectamos la basura
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (RfcCommunicationException ex)
            {
                TextWriter twe = new StreamWriter(Atk_wsHistProdxDia.Service1.pathe, true);
                twe.WriteLine("ws_HistProdxDia FALLA en RFC - LeeMatNotificado: " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
                twe.Close();
                EnviarCorreo email = new EnviarCorreo();
                email.Enviar("ws_HistProdxDia FALLA en RFC: " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
            }
            catch (RfcLogonException ex)
            {
                // user could not logon...
                TextWriter twe = new StreamWriter(Atk_wsHistProdxDia.Service1.pathe, true);
                twe.WriteLine("ws_HistProdxDia FALLA en Login del RFC  - LeeMatNotificado: " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
                twe.Close();
                EnviarCorreo email = new EnviarCorreo();
                email.Enviar("ws_HistProdxDia FALLA en Login del RFC  - LeeMatNotificado: " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
            }
            catch (RfcAbapRuntimeException ex)
            {
                // serious problem on ABAP system side...
                TextWriter twe = new StreamWriter(Atk_wsHistProdxDia.Service1.pathe, true);
                twe.WriteLine("ws_HistProdxDia FALLA en RFC  - LeeMatNotificado: serious problem on ABAP system side, " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
                twe.Close();
                EnviarCorreo email = new EnviarCorreo();
                email.Enviar("ws_HistProdxDia.Program. FALLA en RFC  - LeeMatNotificado: serious problem on ABAP system side, " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
            }
            catch (RfcAbapBaseException ex)
            {
                // The function module returned an ABAP exception, an ABAP message
                // or an ABAP class-based exception...
                TextWriter twe = new StreamWriter(Atk_wsHistProdxDia.Service1.pathe, true);
                twe.WriteLine("ws_HistProdxDia.Program. FALLA en RFC - LeeMatNotificado: , " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
                twe.Close();
                EnviarCorreo email = new EnviarCorreo();
                email.Enviar("ws_HistProdxDia.Program. FALLA en RFC - LeeMatNotificado: , " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
            }

        }

        /// <summary>
        /// Convierte una tabla del un RFC de SAP a DataTable
        /// </summary>
        /// <param name="lrfcTable"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromRfcTable(IRfcTable lrfcTable)
        {
            //sapnco_util
            DataTable loTable = new DataTable();

            //... Create ADO.Net table.
            for (int liElement = 0; liElement < lrfcTable.ElementCount; liElement++)
            {
                RfcElementMetadata metadata = lrfcTable.GetElementMetadata(liElement);
                loTable.Columns.Add(metadata.Name);
            }

            //... Transfer rows from lrfcTable to ADO.Net table.
            foreach (IRfcStructure row in lrfcTable)
            {
                DataRow ldr = loTable.NewRow();
                for (int liElement = 0; liElement < lrfcTable.ElementCount; liElement++)
                {
                    RfcElementMetadata metadata = lrfcTable.GetElementMetadata(liElement);
                    ldr[metadata.Name] = row.GetString(metadata.Name);
                }
                loTable.Rows.Add(ldr);
            }
            return loTable;
        }

        /// <summary>
        /// Converts SAP table to .NET DataTable table
        /// </summary>
        /// <param name="sapTable">The SAP table to convert.</param>
        /// <returns></returns>
        public static DataTable ToDataTable(IRfcTable sapTable, string name)
        {
            DataTable adoTable = new DataTable(name);
            //... Create ADO.Net table.
            for (int liElement = 0; liElement < sapTable.ElementCount; liElement++)
            {
                RfcElementMetadata metadata = sapTable.GetElementMetadata(liElement);
                adoTable.Columns.Add(metadata.Name, GetDataType(metadata.DataType));
            }

            //Transfer rows from SAP Table ADO.Net table.
            foreach (IRfcStructure row in sapTable)
            {
                DataRow ldr = adoTable.NewRow();
                for (int liElement = 0; liElement < sapTable.ElementCount; liElement++)
                {
                    RfcElementMetadata metadata = sapTable.GetElementMetadata(liElement);

                    switch (metadata.DataType)
                    {
                        case RfcDataType.DATE:
                            ldr[metadata.Name] = row.GetString(metadata.Name).Substring(0, 4) + row.GetString(metadata.Name).Substring(5, 2) + row.GetString(metadata.Name).Substring(8, 2);
                            break;
                        case RfcDataType.BCD:
                            ldr[metadata.Name] = row.GetDecimal(metadata.Name);
                            break;
                        case RfcDataType.CHAR:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.STRING:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.INT2:
                            ldr[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.INT4:
                            ldr[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.FLOAT:
                            ldr[metadata.Name] = row.GetDouble(metadata.Name);
                            break;
                        default:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                    }
                }
                adoTable.Rows.Add(ldr);
            }
            return adoTable;
        }

        /// <summary>
        /// Obtiene el tipo de dato del RFC
        /// </summary>
        /// <param name="rfcDataType"></param>
        /// <returns></returns>
        private static Type GetDataType(RfcDataType rfcDataType)
        {
            switch (rfcDataType)
            {
                case RfcDataType.DATE:
                    return typeof(string);
                case RfcDataType.CHAR:
                    return typeof(string);
                case RfcDataType.STRING:
                    return typeof(string);
                case RfcDataType.BCD:
                    return typeof(decimal);
                case RfcDataType.INT2:
                    return typeof(int);
                case RfcDataType.INT4:
                    return typeof(int);
                case RfcDataType.FLOAT:
                    return typeof(double);
                default:
                    return typeof(string);
            }
        }


    }
}