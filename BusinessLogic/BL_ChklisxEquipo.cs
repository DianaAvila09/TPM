using Entidades;
using RepositorySql;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessLogic
{
    public class BL_ChklisxEquipo
    {
        public List<CheckListEqDt> GetDatosChkxEqDet(string cnxSql, int Id, string pDepto)
        {
            SqlRepository repoSql = new SqlRepository();

            List<CheckListEqDt> lstChkEqDet = repoSql.GetChkxEqDet(cnxSql, Id, pDepto);

            return (lstChkEqDet);
        }

        public CheckListEqEnc GetDatosChkxEq(string cnxSql, int Id, string pDepto)
        {
            SqlRepository repoSql = new SqlRepository();

            CheckListEqEnc lstEqConChk = repoSql.GetChklstxEq(cnxSql, Id, pDepto);
            return (lstEqConChk);
        }

        public List<CheckListEqEnc> GetCatChkxEq(string cnxSql, int Planta, string pDepto, string pCostos, bool soloActivos = false)
        {
            SqlRepository repoSql = new SqlRepository();
            List<CheckListEqEnc> lstEqConChk = repoSql.GetCatChklstxEq(cnxSql, Planta, pDepto, pCostos, soloActivos);
            return (lstEqConChk);
        }

        public int Guardar(string cnxSql, AltaChkxEq newChkEq, string rutalog)
        {
            int result = 0;
            int IdNew = 0;
            SqlRepository repoSql = new SqlRepository();
            IdNew = repoSql.GuardarCheckxEqEncb(cnxSql, newChkEq.ChklsxEq);

            //int tempId = GetNewChekxEq(cnxSql, newChkEq.ChklsxEq, rutalog);

            newChkEq.ChklsxEq.IdChkEquipo = IdNew;
            result = repoSql.GuardarCheckxEqAct(cnxSql, newChkEq, rutalog);

            return result;
        }

        public int UpdateEncb(string cnxSql, CheckListEqEnc newChkEqEnc, string rutalog)
        {
            int result = 0;
            SqlRepository repoSql = new SqlRepository();
            result = repoSql.UpdateCheckxEqEncb(cnxSql, newChkEqEnc);

            return result;
        }
        public bool ValidaChekxEq(string cnxSql, CheckListEqEnc chkEqui, string rutalog)
        {
            List<DataRow> lstDatos = null;
            SqlRepository repoSql = new SqlRepository();
            lstDatos = repoSql.Valid_ChkEq(cnxSql, chkEqui, rutalog);

            if (lstDatos.Count > 0)
                return true;
            else
                return false;
        }
        public int GetNewChekxEq(string cnxSql, CheckListEqEnc chkEqui, string rutalog)
        {
            int newId = 0;
            List<DataRow> lstDatos = null;
            SqlRepository repoSql = new SqlRepository();
            lstDatos = repoSql.Valid_ChkEq(cnxSql, chkEqui, rutalog);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];
                newId = (int)dr["IdChkEquipo"];
            }
            else
            {
                newId = 0;
            }
            return (newId);

        }

        public CheckListEqDt GetDatosEqAct(string cnxSqlMT, int idDetChkEq, int idChkEq, int IdActiv)
        {
            SqlRepository repoSql = new SqlRepository();

            CheckListEqDt chkeqAct = repoSql.GetChklstxEqAct(cnxSqlMT, idDetChkEq, idChkEq, IdActiv);

            return (chkeqAct);

        }

        public int GuardarAct(string cnxSql, CheckListEqDt datosAct)
        {
            int result = 0;
            SqlRepository repoSql = new SqlRepository();

            if (datosAct.TipoOperacion == "V") // Visual
            {
                datosAct.RangoMin = 0;
                datosAct.RangoMax = 0;
                datosAct.OperadorMin = "";
                datosAct.OperadorMax = "";
                datosAct.CodUom = "";
            }
            if (datosAct.TipoOperacion == "M") // Medible
            {
                datosAct.OperadorMin = datosAct.OperadorMin == null ? "" : datosAct.OperadorMin;
                datosAct.OperadorMax = datosAct.OperadorMax == null ? "" : datosAct.OperadorMax;
            }

            result = repoSql.UpdateCheckxEqAct(cnxSql, datosAct);

            return result;

        }

        public int GuardarProgram(string cnxSqlMT, CheckListEqEnc datosChk)
        {
            int result = 0;
            SqlRepository repoSql = new SqlRepository();
            result = repoSql.UpdateCheckxEqProg(cnxSqlMT, datosChk);

            return result;
        }

        public int EliminarProgram(string cnxSqlMT, int idCkEqEnc, string usuario)
        {
            int result = 0;
            SqlRepository repoSql = new SqlRepository();            
            result = repoSql.EliminarCheckxEqProg(cnxSqlMT, idCkEqEnc, usuario);

            return result;
        }


        public int ElimActChk(string cnxSqlMT, int idChkEqDet, int idCkEqEnc, int idActiv, string rutalog)
        {
            int result = 0;
            SqlRepository repoSql = new SqlRepository();
            result = repoSql.ElimActChklstEq(cnxSqlMT, idChkEqDet, idCkEqEnc, rutalog);
            return result;
        }
    }
}
