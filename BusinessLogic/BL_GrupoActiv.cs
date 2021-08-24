using Entidades;
using RepositorySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusinessLogic
{
   public class BL_GrupoActiv
   {
      public List<GrupoActivDt> DatosGpoDet(string cnxSql, int pId)
      {

         SqlRepository repoSql = new SqlRepository();
         List<GrupoActivDt> gpoTempDt = repoSql.GetGpoActDet(cnxSql, pId);

         return gpoTempDt;
      }

      public List<GrupoActivEnc> GetCatGpoActiv(string cnxSql, string pDepto, string pCtroCostos, bool soloActivos = false)
      {
         SqlRepository repoSql = new SqlRepository();
         List<GrupoActivEnc> lstGpoActiv = repoSql.GetCatGpoActiv(cnxSql, pDepto, pCtroCostos, soloActivos);
         return (lstGpoActiv);
      }

      public int Guardar(string cnxSql, GrupoActivEnc newGpo)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.GuardarGpoActividad(cnxSql, newGpo);
         return result;
      }

      public GrupoActivEnc DatosGpoActiv(string cnxSql, int pId)
      {
         SqlRepository repoSql = new SqlRepository();
         GrupoActivEnc ActTemp = repoSql.GetGpoActividades(cnxSql, pId);
         return ActTemp;
      }

      public int Update(string cnxSql, GrupoActivEnc GpoTemp, string rutalog)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.UpdateGpoActividades(cnxSql, GpoTemp, rutalog);
         return result;
      }

      public List<Actividad> GetCatActivSist(string cnxSql,string pCodSistemas, string pDepto, string pCtroCostos)
      {
         SqlRepository repoSql = new SqlRepository();
         List<Actividad> lstActiv = repoSql.GetCatActxSist(cnxSql, pCodSistemas, pDepto, pCtroCostos);
         return (lstActiv);
      }
      public int GuardarNewAct(string cnxSqlMT, AltaActivGrupo newGpo)
      {
         newGpo.ActNewGpo.IdGrupoAct = newGpo.GpoEncab.IdGrupoAct;
         newGpo.ActNewGpo.CodGrupo = newGpo.GpoEncab.CodGrupo;

         BL_CatActividades BlCatActiv = new BL_CatActividades();
         Actividad tempAct = BlCatActiv.DatosActiv(cnxSqlMT, newGpo.ActNewGpo.IdActividad);

         newGpo.ActNewGpo.CodActividad = tempAct.CodActividad;
         newGpo.ActNewGpo.CategoriaAct = "A";

         var lstResult = DatosGpoDet(cnxSqlMT, newGpo.GpoEncab.IdGrupoAct);
         int it = 1;
         int ord = 1;
         if (lstResult.Count != 0)
         {
            ord = 0;
            ord = lstResult.Max(x => x.Orden);
            if (ord== 0)
            { ord = 1; }
            else { ord = ord + 1; }

            newGpo.ActNewGpo.Orden = ord;

            it = 0;
            it = lstResult.Max(x => x.Item);
            if (it == 0)
            { it = 1; }
            else { it = it + 1; }
         }
         newGpo.ActNewGpo.Item = it;
         newGpo.ActNewGpo.Orden = ord;
         newGpo.ActNewGpo.FchModif = DateTime.Now;

         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.GuardarGpoDetActiv(cnxSqlMT, newGpo.ActNewGpo);
         return result;
      }

      public int EliminarActiv(string cnxSqlMT, int IdGpo, int IdAct, string rutalog)
      {

         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.EliminarDetActiv(cnxSqlMT, IdGpo, IdAct, rutalog);
         return result;
      }

      public bool ValidaClave(string cnxSql, GrupoActivEnc gpo, string rutalog)
      {
         List<DataRow> lstDatos = null;
         SqlRepository repoSql = new SqlRepository();
         lstDatos = repoSql.Valid_Grupo(cnxSql, gpo.CodGrupo, gpo.CodDepartamento, gpo.CentroCostos , rutalog);

         if (lstDatos.Count > 0)
            return true;
         else
            return false;
      }
   }
}
