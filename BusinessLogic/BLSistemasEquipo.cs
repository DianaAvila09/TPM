using Entidades;
using RepositorySql;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
   public class BLSistemasEquipo
   {
      public List<SistemaManto> DatosCatSistManto(string cnxSql, string depto)
      {
         SqlRepository repoSql = new SqlRepository();
         List<SistemaManto> lstSm = new List<SistemaManto>();
         lstSm = repoSql.LeeCatSistManto(cnxSql, depto);
         return (lstSm.OrderBy(x => x.Sistema).ToList());
      }

      public int GuardarCatalogo(string cnxSql, List<SistemaManto> lstTemp)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.GuardarCatSistManto(cnxSql, lstTemp);
         return result;
      }

      public int Guardar(string cnxSql, SistemaManto sistema, string pRutaLog)
      {
      int result = 0;
      SqlRepository repoSql = new SqlRepository();
      result = repoSql.GuardarSist(cnxSql, sistema, pRutaLog);
      return result;
      }


      public SistemaManto DatosSistMantto(string cnxSql, int pId, string rutaLog)
      {

         SqlRepository repoSql = new SqlRepository();
         SistemaManto sm = repoSql.GetSistMantto(cnxSql, pId, rutaLog);

         return sm;
      }

      public int Update(string cnxSql, SistemaManto sistema, string pRutaLog)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.UpdateSistManto(cnxSql, sistema, pRutaLog);
         return result;
      }
   }

}

