using Entidades;
using RepositorySql;
using System.Collections.Generic;
using System.Data;

namespace BusinessLogic
{
   public class BL_CatActividades
   {
      public List<Actividad> GetCatActividades(string cnxSql, string pDepto, string pCtroCostos, bool soloActivos = false)
      {

         SqlRepository repoSql = new SqlRepository();

         List<Actividad> lstActiv = repoSql.GetCatActividades(cnxSql, pDepto, pCtroCostos, soloActivos);
         return (lstActiv);
      }

      public int Guardar(string cnxSql, Actividad newAct)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.GuardarActividad(cnxSql, newAct);
         return result;
      }

      public Actividad DatosActiv(string cnxSql, int pId)
      {

         SqlRepository repoSql = new SqlRepository();
         Actividad ActTemp = repoSql.GetDatosActiv(cnxSql, pId);

         return ActTemp;
      }

      public int Update(string cnxSql, Actividad ActTemp, string rutalog)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.UpdateDatoActividad(cnxSql, ActTemp, rutalog);
         return result;
      }

      public bool ValidaClave(string cnxSql, string pCodActividad, string rutalog)
      {
         List<DataRow> lstDatos = null;
         SqlRepository repoSql = new SqlRepository();
         lstDatos = repoSql.Valid_Actividad(cnxSql, pCodActividad, rutalog);

         if (lstDatos.Count > 0)
            return true;
         else
            return false;
      }

   }
}
