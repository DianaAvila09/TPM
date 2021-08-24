using Entidades;
using RepositorySql;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusinessLogic
{
   public class BL_CatCompon
   {
      public List<Componente> GetCatCompo(string cnxSql, string pDepto, string pCtroCostos)
      {

         SqlRepository repoSql = new SqlRepository();

         List<Componente> lstCompo = repoSql.GetCatCompo(cnxSql, pDepto, pCtroCostos);
         return (lstCompo);
      }

      public int Guardar(string cnxSql, Componente newCompo)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.GuardarCompo(cnxSql, newCompo);
         return result;
      }

      public Componente DatosCompo(string cnxSql, int pId)
      {

         SqlRepository repoSql = new SqlRepository();
         Componente ActTemp = repoSql.GetDatosCompo(cnxSql, pId);

         return ActTemp;
      }

      public int Update(string cnxSql, Componente ActTemp, string rutalog)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.UpdateDatoCompo(cnxSql, ActTemp, rutalog);
         return result;
      }

      public List<Componente> GetCatCompoxSist(string cnxSql, string pCodSistema, string pCodDepartamento, string pCtroCostos)
      {
         SqlRepository repoSql = new SqlRepository();
         List<Componente> lstCompoxSis = new List<Componente>();
         lstCompoxSis = repoSql.GetCatCompoxSist(cnxSql, pCodSistema, pCodDepartamento, pCtroCostos);
         return (lstCompoxSis.OrderBy(x => x.DescripCompo).ToList());
      }

      public bool ValidaClave(string cnxSql, Componente tCompo , string rutalog)
      {
         List<DataRow> lstDatos = null;
         SqlRepository repoSql = new SqlRepository();
         lstDatos = repoSql.ValidComponente(cnxSql, tCompo.CodSistema, tCompo.DescripCompo, tCompo.CentroCostos , rutalog);

         if (lstDatos.Count > 0)
            return true;
         else
            return false;
      }

   }
}
