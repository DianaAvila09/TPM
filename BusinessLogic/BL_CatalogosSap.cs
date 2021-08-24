using Entidades;
using RepositorySql;
using System.Collections.Generic;
using System.Linq;



namespace BusinessLogic
{
   public class BL_CatalogosSap
   {

      public List<CatEquipo> GetDatosEquiposSap(string cnxSql, string pCtroCostos, string rutalog)
      {
         SqlRepository repoSql = new SqlRepository();
         List<CatEquipo> lstEq = new List<CatEquipo>();
         List<CatEquipo> lstEqTemp = new List<CatEquipo>();

         lstEq = repoSql.GetCatEquiposSap(cnxSql, pCtroCostos, rutalog);
         lstEqTemp = repoSql.GetCatEqStatusSap(cnxSql, pCtroCostos, rutalog);
         int index = 0;
         // vamos asignar los status del equipo

         foreach (CatEquipo item in lstEqTemp)
         {
            index = lstEq.FindIndex(x => x.CodEquipo.Trim() == item.CodEquipo.Trim());
            if (index >= 0)
            {
               lstEq[index].IndivStatusObject = lstEq[index].IndivStatusObject+" "+ item.IndivStatusObject.Trim();
            }

         }

        
         return (lstEq.OrderBy(x => x.CodEquipo).ToList());

      }


      public List<CatEquipo> GetDatosEquiposPadre(string cnxSql, string pCtroCostos, string rutalog)
      {
         SqlRepository repoSql = new SqlRepository();
         List<CatEquipo> lstEq = new List<CatEquipo>();

         lstEq = repoSql.GetCatEquiposPad(cnxSql, pCtroCostos, rutalog);

         return (lstEq.OrderBy(x => x.CodEquipo).ToList());

      }



      public List<EstruturaEquipo> GetDatosEEqui(string cnxSql, string pCtroCostos, string rutalog)
      {
         int index = 0;
         SqlRepository repoSql = new SqlRepository();
         List<EstruturaEquipo> lstEstruc = new List<EstruturaEquipo>();
         lstEstruc = repoSql.GetCatEstrucEqu(cnxSql, pCtroCostos);

         List<CatEquipo> lstEqTemp = new List<CatEquipo>();
         lstEqTemp = repoSql.GetCatEqStatusSap(cnxSql, pCtroCostos, rutalog);

         // vamos asignar los status del equipo

         foreach (CatEquipo item in lstEqTemp)
         {
            index = lstEstruc.FindIndex(x => x.CodEquipo.Trim() == item.CodEquipo.Trim());
            if (index >= 0)
            {
               lstEstruc[index].EquipoStatus = lstEstruc[index].EquipoStatus + " " + item.IndivStatusObject.Trim();
            }
         }


         return (lstEstruc.OrderBy(x => x.SuperiorFuncLoc).ThenBy(c=>c.FuncionalLoc).ToList());

      }
      public List<TblSap_IFLO> GetDatosFL(string cnxSql, string pCtroCostos)
      {

         SqlRepository repoSql = new SqlRepository();
         List<TblSap_IFLO> lstFl = new List<TblSap_IFLO>();

         lstFl = repoSql.GetCatFuntLocation(cnxSql, pCtroCostos);

         return (lstFl.OrderBy(x => x.SuperiorFunctLoc).ToList());

      }

      public List<WorkCenter> GetDatosWC(string cnxSql)
      {

         SqlRepository repoSql = new SqlRepository();
         List<WorkCenter> lstWc = new List<WorkCenter>();

         lstWc = repoSql.GetCatWorkCter(cnxSql, "0007");

         return (lstWc.OrderBy(x => x.CodWorkCenter).ToList());

      }

      public CatEquipo GetDatosEquipo(string cnxSql, string codEqui)
      {

         SqlRepository repoSql = new SqlRepository();
         CatEquipo DatEq = new CatEquipo();

         DatEq = repoSql.GetDatEquipo(cnxSql, codEqui);

         return (DatEq);
      }

   }
}
