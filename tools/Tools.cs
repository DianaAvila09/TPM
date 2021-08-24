using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using Entidades;
using SAP.Middleware.Connector;


namespace ToolsTpm
{
   public class Tools
   {

      public void EnviarError(string cadCnxSql, string cMsj, string pMensaje, string pStackTrace, string Program, string proyecto)
      {
         Tools datos = new Tools();
         datos.GuardarError(cadCnxSql, pMensaje, pStackTrace, Program, proyecto);

         // como usamos el mismo procedimiento para enviar varios correos
         // Debemos generar una lista y asignarla a TO

         List<string> lstTo = new List<string> { "victor.rodriguez1@magna.com" };


         DatosCorreo correo = new DatosCorreo
         {
            To = lstTo,
            Subject = "TPM-Mantenimiento - Error",
            Mensaje = cMsj
         };
         datos.EnviarCorreo(correo);
      }

      /// <summary>
      /// Enviar correo 
      /// </summary>
      /// <param name="pMensaje"></param>
      /// <param name="pTo"></param>
      /// <param name="pSubject"></param>
      public void EnviarCorreo(Entidades.DatosCorreo datos)
      {

         MailMessage email = new MailMessage();
         foreach (var address in datos.To)
         {
            email.To.Add(new MailAddress(address));
         }

         email.From = new MailAddress("TPM-Mantenimiento@magna.com");
         email.Subject = datos.Subject;
         email.Body = datos.Mensaje;
         email.IsBodyHtml = true;
         email.Priority = MailPriority.High;

         // Definir objeto SmtpClient

         SmtpClient smtp = new SmtpClient();
         smtp.Host = "Smtp.magna.global";
         smtp.Port = 25;
         smtp.EnableSsl = false;
         smtp.UseDefaultCredentials = false;
         smtp.Credentials = new NetworkCredential("email_from@example.com", "contraseña");

         string output = null;

         try
         {
            smtp.Send(email);
            email.Dispose();
         }
         catch (Exception ex)
         {
            output = "Error enviando correo electrónico: " + ex.Message;
         }
      }
      
      /// <summary>
      /// Guarda error en base de datos SQL
      /// </summary>
      /// <param name="pCnxSql"></param>
      /// <param name="pMensaje"></param>
      /// <param name="pTrace"></param>
      /// <param name="pPrograma"></param>
      /// <param name="pProyecto"></param>
      public void GuardarError(string pCnxSql, string pMensaje, string pTrace, string pPrograma, string pProyecto)
      {
         using (SqlConnection cnx = new SqlConnection(pCnxSql))
         {
            try
            {
               cnx.Open();

               SqlCommand cmd = new SqlCommand("spu_GuardarError", cnx);
               cmd.CommandType = System.Data.CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@Proyecto", pProyecto);
               cmd.Parameters.AddWithValue("@Programa", pPrograma);
               cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
               cmd.Parameters.AddWithValue("@Mensaje", pMensaje);
               cmd.Parameters.AddWithValue("@Trace", pTrace);
               cmd.Parameters.AddWithValue("@Usuario", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
               cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
               string j = ex.Message;
            }

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
