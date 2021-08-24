using System.Collections.Generic;
using System.Linq;
using Entidades;
using RepositorySql;
using RepositorySap;
using BusinessLogic;


namespace BusinessLogic
{
   
   public class BLCatPlanesManto
   {

      public List<PlanMantto> DatosCatalogo(string cnxSql, string pDepto, string pCentroCostos, string rutalog)
      {
         SqlRepository repoSql = new SqlRepository();
         SapRepository repoSap = new SapRepository();

         List<PlanMantto> lstPlanes = repoSql.LeeCatPlanMantto(cnxSql, pCentroCostos);
         List<SistemaManto> lstSistemas = repoSql.LeeCatSistManto(cnxSql, pDepto);
         List<Ciclos> lstCiclos = repoSql.LeeCatCiclos(cnxSql);

         //RfcParam param = new RfcParam();
         //param.Language = "EN";
         //param.NomenclaEquipo = "A%";

         //List<TblSap_EQKT> lstEQKT = repoSap.LeeEstrEquEQKT(cnxSap, cnxSql, param);
         List<CatEquipo> lstEQKT = repoSql.GetCatEquiposPad (cnxSql, pCentroCostos, rutalog);

         List<PlanMantto> lstPLanesManto = (from p in lstPlanes
                                            join s in lstSistemas on p.CodSistema equals s.CodSistema
                                            join c in lstCiclos on p.CodCiclo equals c.CodCiclo
                                            join k in lstEQKT on p.CodEquipo.Trim() equals k.CodEquipo.Trim()
                                            select new PlanMantto()
                                            {
                                               Id = p.Id,
                                               CodEquipo = p.CodEquipo,
                                               Equipo = k.DescripTechnical,
                                               CodSistema = p.CodSistema,
                                               Sistema = s.Sistema,
                                               CodCiclo = p.CodCiclo,
                                               Ciclo = c.Descripcion,
                                               Frecuencia = p.Frecuencia,
                                               FechaAlta = p.FechaAlta,
                                               UsuarioAlta = p.UsuarioAlta,
                                               FechaCancelacion = p.FechaCancelacion,
                                               UsuarioCancelo = p.UsuarioCancelo,
                                               Estatus = p.Estatus,
                                               FecUltEjecucion = p.FecUltEjecucion,
                                               CodWorkCenter = p.CodWorkCenter
                                            }).ToList();
         return (lstPLanesManto);
      }

      public List<Ciclos> DatosCiclos(string cnxSql)
      {
         SqlRepository repoSql = new SqlRepository();
         List<Ciclos> lstCiclos = new List<Ciclos>();


         lstCiclos = repoSql.LeeCatCiclos(cnxSql);
         return (lstCiclos.OrderBy(x => x.Descripcion).ToList<Ciclos>());
      }

      public int Guardar(string cnxSql, PlanMantto plan, string CCostos)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.GuardarPM(cnxSql, plan, CCostos);
         return result;
      }

      public int Update(string cnxSql, PlanMantto plan, string rutalog)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.UpdatePM(cnxSql, plan, rutalog);
         if (result < 0)
         {
            //mandar error de falla en la actualizacion
         }
         return result;
      }

      public PlanMantto DatosPlanManto (string cnxSql, List<CatEquipo> lstEquipos, List<SistemaManto> lstSisManto, List<Ciclos> lstCiclos, int pId)
      {
         
         SqlRepository repoSql = new SqlRepository();
         PlanMantto plan = repoSql.GetPlanManto(cnxSql, pId);

         return plan;
      }
   }
}
