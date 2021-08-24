using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using ToolsTpm;

namespace DatosSql

{
   public class OperDatosSql
   {

      Tools error = new Tools();

      public DataTable LeeConParamDt(string cnxSql, SqlCommand comandoSql)
      {
         DataTable dtDatos = new DataTable();
         SqlConnection cnx = new SqlConnection(cnxSql);
         SqlDataAdapter da = new SqlDataAdapter();

         da.SelectCommand = comandoSql;
         da.SelectCommand.Connection = cnx;
         da.Fill(dtDatos);
         //if (dtDatos.Rows.Count == 0) error.GuardarError(cnxSql, "No existen datos " + comandoSql.CommandText, "", "LeeDatos", "OperDatosSql");
         return(dtDatos);
      }


      public List<DataRow> LeeDatos(string cnxSql, string cmdSql)
      {
         DataTable dtDatos = new DataTable();

         SqlConnection cnx = new SqlConnection(cnxSql);
         SqlDataAdapter da = new SqlDataAdapter();
         da.SelectCommand = new SqlCommand(cmdSql, cnx);
         da.Fill(dtDatos);
         var lstDatos = dtDatos.AsEnumerable().ToList();

         //if (lstDatos.Count == 0) error.GuardarError(cnxSql, "No existen datos " + cmdSql, "", "LeeDatos", "OperDatosSql");

         return lstDatos;
      }

      public List<DataRow> LeeDatosOtraBD(string cnxSql, string cnxSqlMT, string cmdSql)
      {
         DataTable dtDatos = new DataTable();

         SqlConnection cnx = new SqlConnection(cnxSql);
         SqlDataAdapter da = new SqlDataAdapter();
         da.SelectCommand = new SqlCommand(cmdSql, cnx);
         da.Fill(dtDatos);
         var lstDatos = dtDatos.AsEnumerable().ToList();
         //if (lstDatos.Count == 0) error.GuardarError(cnxSqlMT, "No existen datos " + cmdSql, "", "LeeDatosOtraBD", "OperDatosSql");
         return lstDatos;
      }

      public DataRow LeeDato(string cnxSql, string cmdSql)
      {
         DataTable dtDatos = new DataTable();

         SqlConnection cnx = new SqlConnection(cnxSql);
         SqlDataAdapter da = new SqlDataAdapter();
         da.SelectCommand = new SqlCommand(cmdSql, cnx);
         da.Fill(dtDatos);
         var dr = dtDatos.AsEnumerable().ToList().FirstOrDefault();
         //if (dr == null) error.GuardarError(cnxSql, "No existen datos BD SQL " + cmdSql, "", "LeeDato", "OperDatosSql");
         return dr;
      }

      public List<DataRow> LeeDatosConParam(string cnxSql, SqlCommand comandoSql)
      {
        // Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
         DataTable dtDatos = new DataTable();
         SqlConnection cnx = new SqlConnection(cnxSql);
         SqlDataAdapter da = new SqlDataAdapter();
         da.SelectCommand = comandoSql;
         da.SelectCommand.Connection = cnx;
         da.Fill(dtDatos);
         var lstDatos = dtDatos.AsEnumerable().ToList();
         //if (lstDatos.Count == 0) error.GuardarError(cnxSql, "No existen datos " + comandoSql.CommandText, "", "LeeDatos", "OperDatosSql");
         return lstDatos;
      }

      public int Guardar(string cnxSql, SqlCommand cmd)
      {
         Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

         int result = 0;
         using (SqlConnection cnx = new SqlConnection(cnxSql))
         {
            try
            {
               cnx.Open();
               cmd.Connection = cnx;
               result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               result = -99;
               error.GuardarError(cnxSql, ex.Message, ex.StackTrace, "Guardar", "OperDatosSql");
            }
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            return result;
         }
      }

      public DataTable GetStructTabla(string cnxSql, string cmdSql)
      {
         DataTable dtDatos = new DataTable();

         SqlConnection cnx = new SqlConnection(cnxSql);
         SqlDataAdapter da = new SqlDataAdapter();
         da.SelectCommand = new SqlCommand(cmdSql, cnx);
         da.Fill(dtDatos);

         return dtDatos;
      }

      public int GuardarTablaSap(string cnxSql, DataTable tabla, string tablaDestino, string pathLog)
      {
         int result = 0;
         using (var cnx = new SqlConnection(cnxSql))
         {
            cnx.Open();
            SqlTransaction transaction = cnx.BeginTransaction();

            using (var bulkCopy = new SqlBulkCopy(cnx, SqlBulkCopyOptions.Default, transaction))
            {
             
               bulkCopy.DestinationTableName = "dbo." + tablaDestino;
               try
               {
                  bulkCopy.WriteToServer(tabla);
                  result = 0;
               }

               catch (Exception ex)
               {
                  transaction.Rollback();
                  cnx.Close();
                  result = -99;
                   error.GuardarError(cnxSql, ex.Message, ex.StackTrace, "Guardar", "OperDatosSql");
                  TextWriter twe = new StreamWriter(@pathLog, true);
                  twe.WriteLine(" OperDatosSql - GuardarTablaSap - Error: " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
                  twe.Close();
               }
            }
            transaction.Commit();
         }
         return result;
      }

  
      public int VaciarTabla(string cnxSql, string tabla, string pathLog)
      {
         int result = 0;

         string cmdSql = "Truncate table " + tabla;
         using (SqlConnection cnx = new SqlConnection(cnxSql))
         {
            try
            {
               SqlCommand sc = new SqlCommand(cmdSql, cnx);
               cnx.Open();
               sc.ExecuteNonQuery();
               result = 1;
            }
            catch (SqlException ex)
            {
               TextWriter twe = new StreamWriter(@pathLog, true);
               twe.WriteLine(" OperDatosSql - VaciarTabla:"+ tabla +" - Error: " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
               twe.Close();
               result = -1;
            }
         }
         return result;
      }

      public int Update(string cnxSql, SqlCommand cmd, string pathLog)
      {
         Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

         int result = 0;
         using (var cnx = new SqlConnection(cnxSql))
         {
            cmd.Connection = cnx;
            cnx.Open();
            SqlTransaction transaction = cnx.BeginTransaction();
            cmd.Transaction = transaction;
            try
            {
               result = cmd.ExecuteNonQuery();
               transaction.Commit();
            }

            catch (Exception ex)
            {
               transaction.Rollback();
               cnx.Close();
               result = -99;
               error.GuardarError(cnxSql, ex.Message, ex.StackTrace, "Update", "OperDatosSql");

               TextWriter twe = new StreamWriter(@pathLog, true);
               twe.WriteLine(" OperDatosSql - Update - Error: " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
               twe.Close();
            }

         }
         Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
         return result;
      }

      public int Eliminar(string cnxSql, SqlCommand cmd, string pathLog)
      {
         Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

         int result = 0;
         using (var cnx = new SqlConnection(cnxSql))
         {
            cmd.Connection = cnx;
            cnx.Open();
            SqlTransaction transaction = cnx.BeginTransaction();
            cmd.Transaction = transaction;
            try
            {
               result = cmd.ExecuteNonQuery();
               transaction.Commit();
            }

            catch (Exception ex)
            {
               transaction.Rollback();
               cnx.Close();
               result = -99;
               error.GuardarError(cnxSql, ex.Message, ex.StackTrace, "Eliminar", "OperDatosSql");

               TextWriter twe = new StreamWriter(@pathLog, true);
               twe.WriteLine(" OperDatosSql - Eliminar - Error: " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
               twe.Close();
            }
         }
         Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
         return result;
      }


      public int GuardarReturId(string cnxSql, SqlCommand cmd)
      {
         Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

         int result = 0;
         using (SqlConnection cnx = new SqlConnection(cnxSql))
         {
            try
            {
               cnx.Open();
               cmd.Connection = cnx;
               result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
               result = -99;
               error.GuardarError(cnxSql, ex.Message, ex.StackTrace, "Guardar", "OperDatosSql");
            }
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            return result;
         }
      }

      public int GuardarTablaSql(string cnxSql, DataTable tabla, string tablaDestino, string fileLog, string fileBug)
      {
         int result = 0;
         using (var cnx = new SqlConnection(cnxSql))
         {
            cnx.Open();
            SqlTransaction transaction = cnx.BeginTransaction();

            using (var bulkCopy = new SqlBulkCopy(cnx, SqlBulkCopyOptions.Default, transaction))
            {

               bulkCopy.DestinationTableName = "dbo." + tablaDestino;
               try
               {
                 bulkCopy.WriteToServer(tabla);
                  result = 1;
               }

               catch (Exception ex)
               {
                  transaction.Rollback();
                  cnx.Close();
                  result = -99;
                  error.GuardarError(cnxSql, ex.Message, ex.StackTrace, "Guardar", "OperDatosSql");
                  TextWriter twe = new StreamWriter(@fileBug, true);
                  twe.WriteLine(" OperDatosSql - GuardarTablaSql - Error: " + ex.Message + " --> fecha de " + DateTime.Now.ToString());
                  twe.Close();
               }
            }
            transaction.Commit();
         }
         return result;
      }
   }
}

