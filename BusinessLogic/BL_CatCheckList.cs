using Entidades;
using RepositorySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusinessLogic
{
  public class BL_CatCheckList
   {
  
      public List<CheckListEnc> GetCatCheckList(string cnxSql, string pDepto)
      {
         SqlRepository repoSql = new SqlRepository();
         List<CheckListEnc> lstCheck = new List<CheckListEnc>();
        lstCheck = repoSql.GetCatCheckList(cnxSql, pDepto);
         return (lstCheck);
      }

      public int Guardar(string cnxSql, CheckListEnc newCheck)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.GuardarCheckList(cnxSql, newCheck);
         return result;
      }

      public CheckListEnc DatosEncChk(string cnxSql, int pId, string pDepto)
      {
         SqlRepository repoSql = new SqlRepository();
         CheckListEnc Temp = repoSql.GetDatosEncChk(cnxSql, pId, pDepto);

         return Temp;
      }

      public int Update(string cnxSql, CheckListEnc Temp, string rutalog)
      {
         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.UpdateEncChecklist(cnxSql, Temp, rutalog);
         return result;
      }

      public bool ValidaClave(string cnxSql, string pCodActividad, string pDepto, string rutalog)
      {
         List<DataRow> lstDatos = null;
         SqlRepository repoSql = new SqlRepository();
         lstDatos = repoSql.Valid_CheckList(cnxSql, pCodActividad, pDepto, rutalog);

         if (lstDatos.Count > 0)
            return true;
         else
            return false;
      }

      public List<ClasifCheckList> GetClasifCheckList(string cnxSql)
      {
         SqlRepository repoSql = new SqlRepository();
         List<ClasifCheckList> lstClasifCheck = new List<ClasifCheckList>();
         lstClasifCheck = repoSql.GetCatClasifCheckList(cnxSql);
         return (lstClasifCheck);
      }

      public List<CheckListDt> GetChecklstActiv(string cnxSql, int pId, string pDepto)
      {

         SqlRepository repoSql = new SqlRepository();
         List<CheckListDt> lstTemp = repoSql.GetCheklstAct(cnxSql, pId, pDepto);

         return lstTemp;
      }
      public int GuardarActivi(string cnxSql, AltaCheckListDet newCheckDt)
      {
         BL_CatActividades blCatAct = new BL_CatActividades();        
         int result = 0;

         if (newCheckDt.ActChk.TipoActividad == "A")
         {            
            Actividad pActivi = blCatAct.DatosActiv(cnxSql, newCheckDt.ActChk.IdActividad);

            newCheckDt.ActChk.CodActividad = pActivi.CodActividad;
            newCheckDt.ActChk.CodSistema = pActivi.CodSistema;
            newCheckDt.ActChk.DescripCompo = pActivi.DescripCompo;
            newCheckDt.ActChk.TipoOperacion = pActivi.TipoOperacion;
            newCheckDt.ActChk.EqParado = pActivi.EqParado;
            newCheckDt.ActChk.ActivoActiv = pActivi.Activo;
            newCheckDt.ActChk.CodGpoActiv = newCheckDt.ActChk.TipoActividad;

            newCheckDt.ActChk.IdCheckList = newCheckDt.Encabezado.IdCheckList;
            newCheckDt.ActChk.CodCheckList = newCheckDt.Encabezado.CodCheckList;

            var lstResult = GetChecklstActiv(cnxSql, newCheckDt.Encabezado.IdCheckList, newCheckDt.Encabezado.CodDepartamento);
            int it = 1;
            int ord = 1;
            if (lstResult.Count != 0)
            {
               ord = 0;
               ord = lstResult.Max(x => x.Orden);
               if (ord == 0)
               { ord = 1; }
               else { ord = ord + 1; }

               newCheckDt.ActChk.Orden = ord;

               it = 0;
               it = lstResult.Max(x => x.Item);
               if (it == 0)
               { it = 1; }
               else { it = it + 1; }
            }
            newCheckDt.ActChk.Item = it;
            newCheckDt.ActChk.Orden = ord;
            newCheckDt.ActChk.FchAlta = DateTime.Now;


            
            SqlRepository repoSql = new SqlRepository();
            result = repoSql.GuardarCheckDt(cnxSql, newCheckDt);
         }
         if (newCheckDt.ActChk.TipoActividad == "G")
         {
            BL_GrupoActiv blGpoAct = new BL_GrupoActiv();

            GrupoActivEnc encGpo = new GrupoActivEnc();
            encGpo = blGpoAct.DatosGpoActiv(cnxSql, newCheckDt.ActChk.IdGrupoAct);

            List<GrupoActivDt> lstActGpo = new List<GrupoActivDt>();

            lstActGpo = blGpoAct.DatosGpoDet(cnxSql, newCheckDt.ActChk.IdGrupoAct);

            if(lstActGpo.Count >0)
            {
               foreach (var j in lstActGpo)
               {
                  Actividad pActivi = new Actividad();
         
                  pActivi = blCatAct.DatosActiv(cnxSql, j.IdActividad);
                  newCheckDt.ActChk.IdActividad = pActivi.IdActividad;
                  newCheckDt.ActChk.CodActividad = pActivi.CodActividad;
                  newCheckDt.ActChk.CodSistema = pActivi.CodSistema;
                  newCheckDt.ActChk.DescripCompo = pActivi.DescripCompo;
                  newCheckDt.ActChk.TipoOperacion = pActivi.TipoOperacion;
                  newCheckDt.ActChk.EqParado = pActivi.EqParado;
                  newCheckDt.ActChk.ActivoActiv = pActivi.Activo;
                  newCheckDt.ActChk.CodGpoActiv = newCheckDt.ActChk.TipoActividad;
                  newCheckDt.ActChk.CodGpoActiv = encGpo.CodGrupo;

                  newCheckDt.ActChk.IdCheckList = newCheckDt.Encabezado.IdCheckList;
                  newCheckDt.ActChk.CodCheckList = newCheckDt.Encabezado.CodCheckList;

                  var lstResult = GetChecklstActiv(cnxSql, newCheckDt.Encabezado.IdCheckList, newCheckDt.Encabezado.CodDepartamento);
                  int it = 1;
                  int ord = 1;
                  if (lstResult.Count != 0)
                  {
                     ord = 0;
                     ord = lstResult.Max(x => x.Orden);
                     if (ord == 0)
                     { ord = 1; }
                     else { ord = ord + 1; }

                     newCheckDt.ActChk.Orden = ord;

                     it = 0;
                     it = lstResult.Max(x => x.Item);
                     if (it == 0)
                     { it = 1; }
                     else { it = it + 1; }
                  }
                  newCheckDt.ActChk.Item = it;
                  newCheckDt.ActChk.Orden = ord;
                  newCheckDt.ActChk.FchAlta = DateTime.Now;



                  SqlRepository repoSql = new SqlRepository();
                  result = repoSql.GuardarCheckDt(cnxSql, newCheckDt);

               }
            }
         }

         return result;
      }

      public int ElimActChk(string cnxSqlMT, int idDetchk, int idChklst, string rutalog)
      {

         int result = 0;
         SqlRepository repoSql = new SqlRepository();
         result = repoSql.ElimActChklst(cnxSqlMT, idDetchk, idChklst, rutalog);
         return result;
      }


   }
}
