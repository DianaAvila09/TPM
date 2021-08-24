using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using RepositorySql;

namespace BusinessLogic
{
   public class BL_CatTpm
   {
      public List<Falla> DatosCatFallas(string cnxSql, string pCodSistema, string pCodDepartamento)
      {
         SqlRepository repoSql = new SqlRepository();
         List<Falla> lstFallas = new List<Falla>();
         lstFallas = repoSql.GetCatFallas(cnxSql, pCodSistema, pCodDepartamento);
         return (lstFallas.OrderBy(x => x.CodSistema).ThenBy(y => y.Descrip).ToList());
      }
      public List<Falla> GetCatFallas(string cnxSql, string pCodDepartamento)
      {
         SqlRepository repoSql = new SqlRepository();
         List<Falla> lstFallas = new List<Falla>();
         lstFallas = repoSql.GetTodasCatFallas(cnxSql, pCodDepartamento);
         return (lstFallas.OrderBy(x => x.CodSistema).ThenBy(y => y.Descrip).ToList());
      }

      public List<ClasificFalla> DatosCatClasifFallas(string cnxSql)
      {
         SqlRepository repoSql = new SqlRepository();
         List<ClasificFalla> lstClasifFallas = new List<ClasificFalla>();
         lstClasifFallas = repoSql.GetCatClasifFallas(cnxSql);
         return (lstClasifFallas.OrderBy(x => x.Descripcion).ToList());
      }
      public List<CatStatusTicket> DatosCatStatusTicket(string cnxSql)
      {
         SqlRepository repoSql = new SqlRepository();
         List<CatStatusTicket> lstCatStat = new List<CatStatusTicket>();

         lstCatStat = repoSql.GetCatStatusTickets(cnxSql);
         return (lstCatStat.OrderBy(x => x.Descrip).ToList());
      }
      public List<Ciclos> DatosCatCiclos(string cnxSql)
      {
         SqlRepository repoSql = new SqlRepository();
         List<Ciclos> lstCiclos = new List<Ciclos>();
         lstCiclos = repoSql.LeeCatCiclos(cnxSql);
         return (lstCiclos.OrderBy(x => x.Orden).ToList());
      }

      public string DatosWcEquipo(string cnxSql, string pEquipo)
      {
         SqlRepository repoSql = new SqlRepository();
         CatEquipo result = repoSql.GetDatEquipo(cnxSql, pEquipo);
         return (result.WorkCenter);
      }

      public List<Uom> GetCatUom(string cnxSql)
      {
         SqlRepository repoSql = new SqlRepository();
         List<Uom> lstTemp = new List<Uom>();
         lstTemp = repoSql.GetCatUom(cnxSql);
         return (lstTemp.OrderBy(x => x.Descrip).ToList());
      }

   }
}
