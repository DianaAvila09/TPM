using Entidades;
using RepositorySql;
using System.Collections.Generic;

namespace BusinessLogic
{
   public class BL_CatPmStd
   {
      public List<CatPMS> DatosCatPMStd(string cnxSql, string pCtroCtos, string pDepto)
      {
         SqlRepository repoSql = new SqlRepository();

         List<CatPMS> lstPMS = repoSql.LeeCatPMS(cnxSql, pCtroCtos, pDepto);
         return (lstPMS);
      }

      public List<EquipoPadre> GetEquipoPadres(string cnxSqlMT, string pCtroCtos)
      {
         SqlRepository repoSql = new SqlRepository();
        
         List<EquipoPadre> lstEquiPadre = new List<EquipoPadre>();
         lstEquiPadre = repoSql.GetCatEquiposPadres(cnxSqlMT, pCtroCtos);
         return (lstEquiPadre);
      }

      public int Guardar(string cnxSql, PmStandar pmsNew)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.GuardarPms(cnxSql, pmsNew);
         return result;
      }
      public int Update(string cnxSql, PmStandar pmsNew)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.UpdatePms(cnxSql, pmsNew);
         return result;
      }

      public PmStandar DatosPms(string cnxSql, int pId)
      {

         SqlRepository repoSql = new SqlRepository();
         PmStandar pmsTemp = repoSql.GetDatosPms(cnxSql, pId);

         return pmsTemp;
      }

      public bool ValidarPmsEquipo(string cnxSql, string pEquipo, string pCtroCtos)
      {
         bool siExiste = false;
         SqlRepository repoSql = new SqlRepository();
         siExiste = repoSql.ValidaPmsEquipo(cnxSql, pEquipo, pCtroCtos);

         return siExiste; 
      }

   }
}
