using Entidades;
using RepositorySql;
using System.Collections.Generic;

namespace BusinessLogic
{
   public class BL_CatFallas
   {
      public List<Falla> DatosCatFallas(string cnxSql, string pDepto)
      {
         SqlRepository repoSql = new SqlRepository();
        
         List<Falla> lstFallas = repoSql.LeeCatFallas(cnxSql, pDepto);
         return (lstFallas);
      }

      public int Guardar(string cnxSql, Falla fallanew, string pDepto)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.GuardarFalla(cnxSql, fallanew, pDepto);
         return result;
      }

      public Falla DatosFalla(string cnxSql, int pId)
      {

         SqlRepository repoSql = new SqlRepository();
         Falla fallaTemp = repoSql.GetDatosFalla(cnxSql, pId);

         return fallaTemp;
      }

      public int Update(string cnxSql, Falla datFalla, string rutalog)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.UpdateDatoFalla(cnxSql, datFalla, rutalog);
         if (result < 0)
         {
            //mandar error de falla en la actualizacion
         }
         return result;
      }



   }
}
