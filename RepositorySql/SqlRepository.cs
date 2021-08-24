using DatosSql;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace RepositorySql
{
    public class SqlRepository

    {

        public int UpdateCheckxEqEncb(string cnxSql, CheckListEqEnc chkxEqEncb)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @" UPDATE [dbo].[CheckListxEqEnc]
                              SET Activo = @Activo, IdFrecuencia = @IdFrecuencia, Frecuencia = @Frecuencia
                                 ,UserModif = @UserModif, FchModif = @FchModif
                            WHERE IdChkEquipo = @IdChkEquipo and Planta = @Planta and CodDepartamento = @CodDepartamento and CentroCostos = @CC";

            cmd.Parameters.AddWithValue("@IdChkEquipo", chkxEqEncb.IdChkEquipo);
            cmd.Parameters.AddWithValue("@Planta", chkxEqEncb.Planta);
            cmd.Parameters.AddWithValue("@CodDepartamento", chkxEqEncb.CodDepartamento);
            cmd.Parameters.AddWithValue("@CC", chkxEqEncb.CentroCostos);
            cmd.Parameters.AddWithValue("@IdFrecuencia", chkxEqEncb.IdFrecuencia);
            cmd.Parameters.AddWithValue("@Frecuencia", chkxEqEncb.Frecuencia);
            cmd.Parameters.AddWithValue("@Activo", chkxEqEncb.Activo);
            cmd.Parameters.AddWithValue("@UserModif", chkxEqEncb.UserModif);
            cmd.Parameters.AddWithValue("@FchModif", chkxEqEncb.FchModif);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);
            return result;
        }

        public int GuardarCheckxEqAct(string cnxSql, AltaChkxEq chkxEq, string rutalog)
        {
            int result = 0;
            OperDatosSql operDatos = new OperDatosSql();

            SqlCommand cmd = new SqlCommand();
            foreach (var j in chkxEq.lstListAct)
            {
                cmd.Parameters.Clear();
                cmd.CommandText = @"INSERT INTO dbo.CheckListxEqDet
                              (IdChkEquipo,CodWorkCenter,CodEquipo,IdCheckList,CodChkList,CodGpoActiv,idActividad,CodActividad
                              ,TipoActividad,TipoOperacion,EqParado,RangoMin,OperadorMin,RangoMax,OperadorMax,Ponderacion,Activo, Orden, Item)
                              VALUES (@IdChkEquipo, @CodWorkCenter, @CodEquipo, @IdCheckList,@CodChkList,@CodGpoActiv,@idActividad,@CodActividad
                              ,@TipoActividad,@TipoOperacion,@EqParado,@RangoMin,@OperadorMin,@RangoMax,@OperadorMax,@Ponderacion,@Activo,@Orden,@Item)";

                cmd.Parameters.AddWithValue("@IdChkEquipo", chkxEq.ChklsxEq.IdChkEquipo);
                cmd.Parameters.AddWithValue("@CodWorkCenter", chkxEq.ChklsxEq.CodWorkCenter);
                cmd.Parameters.AddWithValue("@CodEquipo", chkxEq.ChklsxEq.CodEquipo);
                cmd.Parameters.AddWithValue("@IdCheckList", chkxEq.ChklsxEq.IdCheckList);
                cmd.Parameters.AddWithValue("@CodChkList", chkxEq.ChklsxEq.CodChkList);

                cmd.Parameters.AddWithValue("@CodGpoActiv", j.CodGpoActiv);
                cmd.Parameters.AddWithValue("@idActividad", j.IdActividad);
                cmd.Parameters.AddWithValue("@CodActividad", j.CodActividad);
                cmd.Parameters.AddWithValue("@TipoActividad", j.TipoActividad);
                cmd.Parameters.AddWithValue("@TipoOperacion", j.TipoOperacion);

                cmd.Parameters.AddWithValue("@EqParado", j.EqParado);
                cmd.Parameters.AddWithValue("@RangoMin", 0);
                cmd.Parameters.AddWithValue("@OperadorMin", "");
                cmd.Parameters.AddWithValue("@RangoMax", 0);
                cmd.Parameters.AddWithValue("@OperadorMax", "");
                cmd.Parameters.AddWithValue("@Ponderacion", 0);
                cmd.Parameters.AddWithValue("@Activo", j.ActivoActiv);
                cmd.Parameters.AddWithValue("@Orden", j.Orden);
                cmd.Parameters.AddWithValue("@Item", j.Item);
                result = operDatos.Guardar(cnxSql, cmd);
            }
            return result;
        }

        public int UpdateCheckxEqAct(string cnxSql, CheckListEqDt datosAct)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @" UPDATE [CheckListxEqDet]
                              SET RangoMin = @RangoMin, OperadorMin = @OperadorMin 
                                  ,RangoMax = @RangoMax, OperadorMax = @OperadorMax 
                                  ,Ponderacion = @Ponderacion , CodUom = @CodUom
                              Where IdDtCheckList = @IdDtCheckList 
                                    and IdChkEquipo = @IdChkEquipo 
                                    and idActividad = @IdActividad";

            cmd.Parameters.AddWithValue("@IdDtCheckList", datosAct.IdDtCheckList);
            cmd.Parameters.AddWithValue("@IdChkEquipo", datosAct.IdChkEquipo);
            cmd.Parameters.AddWithValue("@IdActividad", datosAct.IdActividad);

            cmd.Parameters.AddWithValue("@OperadorMin", datosAct.OperadorMin.Trim());
            cmd.Parameters.AddWithValue("@OperadorMax", datosAct.OperadorMax.Trim());
            cmd.Parameters.AddWithValue("@RangoMin", datosAct.RangoMin);
            cmd.Parameters.AddWithValue("@RangoMax", datosAct.RangoMax);
            cmd.Parameters.AddWithValue("@CodUom", datosAct.CodUom);
            cmd.Parameters.AddWithValue("@Ponderacion", datosAct.Ponderacion);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);
            return result;
        }



        public int UpdateCheckxEqProg(string cnxSql, CheckListEqEnc datosChk)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @" UPDATE [CheckListxEqEnc]
                              SET IniProgram = @IniProgram,
                                    UserActiva = @UserActiva 
                              Where IdChkEquipo = @IdChkEquipo";


            cmd.Parameters.AddWithValue("@IdChkEquipo", datosChk.IdChkEquipo);
            cmd.Parameters.AddWithValue("@IniProgram", datosChk.IniProgram);
            cmd.Parameters.AddWithValue("@UserActiva", datosChk.UserActiva);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);
            return result;
        }

        public int EliminarCheckxEqProg(string cnxSql, int IdChkEquipo, string UserCancela)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @" UPDATE [CheckListxEqEnc]
                              SET IniProgram = NULL,
                                    UserActiva = NULL,
                                    FecProgramada = NULL,
                                    FchCancel = getdate(),
                                    UserCancela = @UserCancela
                              Where IdChkEquipo = @IdChkEquipo";

            cmd.Parameters.AddWithValue("@IdChkEquipo", IdChkEquipo);
            cmd.Parameters.AddWithValue("@UserCancela", UserCancela);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);
            return result;
        }



        public int GuardarCheckxEqEncb(string cnxSql, CheckListEqEnc ChklsxEq)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @" INSERT INTO [dbo].[CheckListxEqEnc]
                                     (ChkEquipo, Planta, CentroCostos, CodWorkCenter, CodEquipo, IdCheckList, CodChkList, DescripChkList, CodClasif, EqParado, Activo, CodDepartamento,
                              UserAlta, FchAlta, UserModif, FchModif, Frecuencia, IdFrecuencia)
                              VALUES (@ChkEquipo, @Planta, @CC, @CodWorkCenter, @CodEquipo, @IdCheckList, @CodChkList, @DescripChkList, @CodClasif, @EqParado, @Activo, @CodDepartamento
                              ,@UserAlta, @FchAlta, @UserModif, @FchModif, @Frecuencia, @IdFrecuencia) 
                              SELECT SCOPE_IDENTITY()";

            cmd.Parameters.AddWithValue("@ChkEquipo", ChklsxEq.ChkEquipo);
            cmd.Parameters.AddWithValue("@Planta", ChklsxEq.Planta);
            cmd.Parameters.AddWithValue("@CC", ChklsxEq.CentroCostos);
            cmd.Parameters.AddWithValue("@CodWorkCenter", ChklsxEq.CodWorkCenter);
            cmd.Parameters.AddWithValue("@CodEquipo", ChklsxEq.CodEquipo);
            cmd.Parameters.AddWithValue("@IdCheckList", ChklsxEq.IdCheckList);
            cmd.Parameters.AddWithValue("@CodChkList", ChklsxEq.CodChkList);
            cmd.Parameters.AddWithValue("@DescripChkList", ChklsxEq.DescripChkList);
            cmd.Parameters.AddWithValue("@CodClasif", ChklsxEq.CodClasif);
            cmd.Parameters.AddWithValue("@EqParado", ChklsxEq.EqParado);
            cmd.Parameters.AddWithValue("@Activo", ChklsxEq.Activo);
            cmd.Parameters.AddWithValue("@CodDepartamento", ChklsxEq.CodDepartamento);
            cmd.Parameters.AddWithValue("@Frecuencia", ChklsxEq.Frecuencia);
            cmd.Parameters.AddWithValue("@IdFrecuencia", ChklsxEq.IdFrecuencia);
            cmd.Parameters.AddWithValue("@UserAlta", ChklsxEq.UserAlta);
            cmd.Parameters.AddWithValue("@FchAlta", ChklsxEq.FchAlta);
            cmd.Parameters.AddWithValue("@UserModif", ChklsxEq.UserModif);
            cmd.Parameters.AddWithValue("@FchModif", ChklsxEq.FchModif);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.GuardarReturId(cnxSql, cmd);
            return result;
        }

        public List<DataRow> Valid_ChkEq(string cnxSql, CheckListEqEnc checkEq, string rutalog)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();

            cmd.CommandText = @" SELECT IdChkEquipo, ChkEquipo, CodWorkCenter, CodEquipo, IdCheckList, CodChkList, CodClasif, 
                                 CodDepartamento, Frecuencia, IdFrecuencia
                              FROM [CheckListxEqEnc]
                              Where CodWorkCenter = @CodWorkCenter and CodEquipo = @CodEquipo 
                                 and ChkEquipo = @ChkEquipo
                                 and IdCheckList = @IdCheckList and CodChkList = @CodChkList
                                 and CodClasif = @CodClasif  and Frecuencia = @Frecuencia and IdFrecuencia = @IdFrecuencia 
                                 and CodDepartamento = @CodDepartamento and Planta = @Planta";

            cmd.Parameters.AddWithValue("@ChkEquipo", checkEq.ChkEquipo);
            cmd.Parameters.AddWithValue("@CodWorkCenter", checkEq.CodWorkCenter);
            cmd.Parameters.AddWithValue("@CodEquipo", checkEq.CodEquipo);
            cmd.Parameters.AddWithValue("@IdCheckList", checkEq.IdCheckList);
            cmd.Parameters.AddWithValue("@CodChkList", checkEq.CodChkList);
            cmd.Parameters.AddWithValue("@CodClasif", checkEq.CodClasif);
            cmd.Parameters.AddWithValue("@CodDepartamento", checkEq.CodDepartamento);
            cmd.Parameters.AddWithValue("@Frecuencia", checkEq.Frecuencia);
            cmd.Parameters.AddWithValue("@IdFrecuencia", checkEq.IdFrecuencia);
            cmd.Parameters.AddWithValue("@Planta", checkEq.Planta);

            lstDatos = operDatos.LeeDatosConParam(cnxSql, cmd);

            return lstDatos;
        }
        public int GuardarCheckDt(string cnxSql, AltaCheckListDet newChk)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"INSERT INTO [dbo].[CatCheckListDet]
               (IdCheckList,CodCheckList,CodGpoActiv,idActividad,CodActividad,TipoActividad,TipoOperacion,EqParado, Activo,Item,Orden, FchAlta)
               VALUES (@IdCheckList, @CodCheckList, @CodGpoActiv, @idActividad, @CodActividad, @TipoActividad,@TipoOperacion, @EqParado, @Activo, @Item, @Orden, @FchAlta)";

            cmd.Parameters.AddWithValue("@IdCheckList", newChk.Encabezado.IdCheckList);
            cmd.Parameters.AddWithValue("@CodCheckList", newChk.Encabezado.CodCheckList);
            cmd.Parameters.AddWithValue("@CodGpoActiv", newChk.ActChk.CodGpoActiv);
            cmd.Parameters.AddWithValue("@idActividad", newChk.ActChk.IdActividad);
            cmd.Parameters.AddWithValue("@CodActividad", newChk.ActChk.CodActividad);
            cmd.Parameters.AddWithValue("@TipoActividad", newChk.ActChk.TipoActividad);
            cmd.Parameters.AddWithValue("@TipoOperacion", newChk.ActChk.TipoOperacion);
            cmd.Parameters.AddWithValue("@EqParado", newChk.ActChk.EqParado);
            cmd.Parameters.AddWithValue("@Activo", newChk.ActChk.ActivoActiv);
            cmd.Parameters.AddWithValue("@Item", newChk.ActChk.Item);
            cmd.Parameters.AddWithValue("@Orden", newChk.ActChk.Orden);
            cmd.Parameters.AddWithValue("@FchAlta", newChk.ActChk.FchAlta);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public List<CheckListDt> GetCheklstAct(string cnxSql, int pId, string pDepto)
        {
            List<CheckListDt> lstItem = new List<CheckListDt>();

            List<DataRow> lstDatos = null;

            string cmdSql = @" SELECT ch.IdDetCheckList, ch.Item, ch.IdCheckList, ch.CodCheckList, 
                              ch.CodGpoActiv, g.DescripGpo, 
                              ch.idActividad, ch.CodActividad, a.DescripcionAct, 
                              s.CodSistema, s.Sistema as DescripSistema , c.IdComponente, c.DescripCompo,
                              ch.TipoActividad, ch.TipoOperacion, ch.EqParado, ch.Activo as ActivoActiv,  ch.Orden
                              FROM [CatCheckListDet] ch
                              left join  [CatGrupoActEnc] g on g.CodGrupo = ch.CodGpoActiv
                              inner join  [CatActivChkLst] a on a.IdActividad = ch.idActividad
                              inner join  [CatSistemasEquipos] s on s.CodSistema = a.CodSistema
                              inner join  [CatComponentes] c on c.IdComponente = a.IdComponente
                              Where  IdCheckList = @pId
                              order by ch.orden";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@pId", pId);
            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            foreach (DataRow dr in lstDatos)
            {
                CheckListDt item = new CheckListDt();

                item.IdDetCheckList = (int)dr["IdDetCheckList"];
                item.Item = (int)dr["Item"];
                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodCheckList = dr["CodCheckList"].ToString();
                item.CodGpoActiv = dr["CodGpoActiv"].ToString();
                item.DescripGpo = dr["DescripGpo"].ToString();
                item.IdActividad = (int)dr["IdActividad"];
                item.CodActividad = dr["CodActividad"].ToString();
                item.DescripAct = dr["DescripcionAct"].ToString();
                item.IdComponente = (int)dr["IdComponente"];
                item.DescripCompo = dr["DescripCompo"].ToString();
                item.CodSistema = dr["CodSistema"].ToString();
                item.DescripSistema = dr["DescripSistema"].ToString();
                item.TipoActividad = dr["TipoActividad"].ToString();
                item.TipoOperacion = dr["TipoOperacion"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.ActivoActiv = (bool)dr["ActivoActiv"];
                item.Orden = (int)dr["Orden"];


                lstItem.Add(item);
            }

            return (lstItem);
        }

        public int UpdateEncChecklist(string cnxSql, CheckListEnc datos, string pRutaLog)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();

            string cmdTxt = @"Update [CatCheckListEnc]
                           set   DescripCheckList = @DescripCheckList, 
                                 CodClasif = @CodClasif,
                                 EqParado = @EqParado, 
                                 Activo = @Activo,
                                 UserModif = @UserModif, 
                                 FchModif = @FchModif
                           Where IdCheckList = @IdCheckList";

            cmd.CommandText = cmdTxt;
            cmd.Parameters.AddWithValue("@IdCheckList", datos.IdCheckList);
            cmd.Parameters.AddWithValue("@DescripCheckList", datos.DescripCheckList);
            cmd.Parameters.AddWithValue("@CodClasif", datos.CodClasif);
            cmd.Parameters.AddWithValue("@Activo", datos.Activo);
            cmd.Parameters.AddWithValue("@EqParado", datos.EqParado);
            cmd.Parameters.AddWithValue("@UserModif", datos.UserModif);
            cmd.Parameters.AddWithValue("@FchModif", datos.FchModif);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Update(cnxSql, cmd, pRutaLog);

            return result;
        }

        public CheckListEnc GetDatosEncChk(string cnxSql, int pId, string pDepto)
        {
            CheckListEnc item = new CheckListEnc();
            List<DataRow> lstDatos = null;

            string cmdSql = @" SELECT IdCheckList, CodCheckList, DescripCheckList, CodClasif, 
                            EqParado, Activo, CodDepartamento, UserAlta, FchAlta, UserModif, FchModif
                            FROM [CatCheckListEnc]
                            Where CodDepartamento = @pDepto and IdCheckList =  @pId ";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@pId", pId);
            sqlcmd.Parameters.AddWithValue("@pDepto", pDepto);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];

                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodCheckList = dr["CodCheckList"].ToString();
                item.DescripCheckList = dr["DescripCheckList"].ToString();
                item.CodClasif = dr["CodClasif"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.UserAlta = dr["UserAlta"].ToString();
                if (dr["FchAlta"].ToString() != "")
                { item.FchAlta = Convert.ToDateTime(dr["FchAlta"]); }
                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }

            }

            return item;
        }

        public List<ClasifCheckList> GetCatClasifCheckList(string cnxSql)
        {
            List<DataRow> lstDatos = null;
            List<ClasifCheckList> lstClasifChk = new List<ClasifCheckList>();

            string cmdSql = @" SELECT IdClasif, CodClasif, DescripClasif
                             FROM [CatClasifChecklist]
                             Where Estatus = 1";

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);
            foreach (DataRow dr in lstDatos)
            {
                ClasifCheckList item = new ClasifCheckList();

                item.IdClasif = (int)dr["IdClasif"];
                item.CodClasif = dr["CodClasif"].ToString();
                item.DescripClasif = dr["DescripClasif"].ToString();

                lstClasifChk.Add(item);
            }
            return (lstClasifChk);
        }

        public int GuardarCheckList(string cnxSql, CheckListEnc newChk)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"INSERT INTO [dbo].[CatCheckListEnc]
                            (CodCheckList, DescripCheckList, CodClasif, EqParado, Activo, CodDepartamento, UserAlta, FchAlta, UserModif, FchModif)
                            VALUES (@CodCheckList, @DescripCheckList, @ClasifCheckList, @EqParado, @Activo, @CodDepartamento, @UserAlta, @FchAlta, @UserModif, @FchModif)";

            cmd.Parameters.AddWithValue("@CodCheckList", newChk.CodCheckList.ToUpper());
            cmd.Parameters.AddWithValue("@DescripCheckList", newChk.DescripCheckList);
            cmd.Parameters.AddWithValue("@ClasifCheckList", newChk.CodClasif);
            cmd.Parameters.AddWithValue("@EqParado", newChk.EqParado);
            cmd.Parameters.AddWithValue("@Activo", newChk.Activo);
            cmd.Parameters.AddWithValue("@CodDepartamento", newChk.CodDepartamento);
            cmd.Parameters.AddWithValue("@UserAlta", newChk.UserAlta);
            cmd.Parameters.AddWithValue("@FchAlta", newChk.FchAlta);
            cmd.Parameters.AddWithValue("@UserModif", newChk.UserModif);
            cmd.Parameters.AddWithValue("@FchModif", newChk.FchModif);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public List<DataRow> Valid_CheckList(string cnxSql, string codigo, string pDepto, string rutalog)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();

            cmd.CommandText = @"SELECT IdCheckList, CodCheckList
                             FROM [CatCheckListEnc]
                             Where CodDepartamento = @pDepto
                             and CodCheckList = @codCheck";

            cmd.Parameters.AddWithValue("@codCheck", codigo);
            cmd.Parameters.AddWithValue("@pDepto", pDepto);

            lstDatos = operDatos.LeeDatosConParam(cnxSql, cmd);

            return lstDatos;
        }

        public List<CheckListEnc> GetCatCheckList(string cnxSql, string pDepto)
        {
            List<DataRow> lstDatos = null;
            List<CheckListEnc> lstChk = new List<CheckListEnc>();

            string cmdSql = @" SELECT IdCheckList, CodCheckList, DescripCheckList, CodClasif, 
                            EqParado, Activo, UserAlta, FchAlta, UserModif, FchModif
                            FROM [CatCheckListEnc]";

            if (pDepto != "")
                cmdSql = cmdSql + " Where CodDepartamento = '" + pDepto + "' ";

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);
            foreach (DataRow dr in lstDatos)
            {
                CheckListEnc item = new CheckListEnc();

                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodCheckList = dr["CodCheckList"].ToString();
                item.DescripCheckList = dr["DescripCheckList"].ToString();
                item.CodClasif = dr["CodClasif"].ToString();

                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.UserAlta = dr["UserAlta"].ToString();

                if (dr["FchAlta"].ToString() != "")
                { item.FchAlta = Convert.ToDateTime(dr["FchAlta"]); }

                item.UserModif = dr["UserModif"].ToString();

                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }

                lstChk.Add(item);
            }
            return (lstChk);
        }

        public List<DataRow> Valid_Grupo(string cnxSql, string pCodGrupo, string pCodDepartamento, string pCtroCostos, string rutalog)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();

            cmd.CommandText = @"SELECT CodGrupo, CodDepartamento 
                              FROM [CatGrupoActEnc]
                              Where CodGrupo = @codGrupo and CodDepartamento = @Depto and CentroCostos = @CtroCostos";

            cmd.Parameters.AddWithValue("@codGrupo", pCodGrupo);
            cmd.Parameters.AddWithValue("@Depto", pCodDepartamento);
            cmd.Parameters.AddWithValue("@CtroCostos", pCtroCostos);

            lstDatos = operDatos.LeeDatosConParam(cnxSql, cmd);

            return lstDatos;
        }


        public List<DataRow> ValidComponente(string cnxSql, string pCodSistemas, string pDescripCompo, string pCtroCostos, string rutalog)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();

            cmd.CommandText = @" SELECT DescripCompo, CodSistema 
                              FROM [CatComponentes]
                              Where DescripCompo = @cod and CodSistema = @CodSist and CentroCostos = @CtroCostos";

            cmd.Parameters.AddWithValue("@CodSist", pCodSistemas);
            cmd.Parameters.AddWithValue("@cod", pDescripCompo);
            cmd.Parameters.AddWithValue("@CtroCostos", pCtroCostos);

            lstDatos = operDatos.LeeDatosConParam(cnxSql, cmd);

            return lstDatos;
        }

        public List<DataRow> Valid_Actividad(string cnxSql, string codigo, string rutalog)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();

            cmd.CommandText = @"SELECT CodActividad
                             FROM [CatActivChkLst]
                             Where CodActividad = @cod";

            cmd.Parameters.AddWithValue("@cod", codigo);

            lstDatos = operDatos.LeeDatosConParam(cnxSql, cmd);

            return lstDatos;
        }

        public int EliminarDetActiv(string cnxSql, int idGpo, int idAct, string rutalog)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();

            cmd.CommandText = @" Delete FROM [CatGrupoActDt]
                              Where IdActividad = @IdActividad and IdGrupoAct =  @IdGrupoAct";

            cmd.Parameters.AddWithValue("@IdGrupoAct", idGpo);
            cmd.Parameters.AddWithValue("@IdActividad", idAct);


            result = operDatos.Eliminar(cnxSql, cmd, rutalog);

            return result;
        }

        public List<Actividad> GetCatActxSist(string cnxSql, string pCodSistema, string pDepto, string pCtroCostos)
        {
            List<DataRow> lstDatos = null;
            List<Actividad> lstCompo = new List<Actividad>();

            string cmdSql = @" SELECT a.IdActividad, a.CodActividad, a.DescripcionAct, a.IdComponente, c.DescripCompo, a.CodSistema, 
                           a.TipoOperacion,'['+a.CodActividad +'] ['+c.DescripCompo +']  '+ a.DescripcionAct as llave
                           FROM [CatActivChkLst] a
                           inner join [CatComponentes] c on c.IdComponente = a.IdComponente 
                           and c.CodSistema = a.CodSistema and c.CodDepartamento = a.CodDepartamento
                           inner join [CatSistemasEquipos] s on s.CodSistema = a.CodSistema 
                           and s.CodDepartamento = a.CodDepartamento
                           inner join [CatDepartamentos] d on d.CodDepartamento = a.CodDepartamento and c.CentroCostos = d.CentroCostosSap                           
                           Where a.CodDepartamento = '" + pDepto + "' and  a.CodSistema = '" + pCodSistema + "' and a.Activo = 1";

            if (pCtroCostos != "")
                cmdSql = cmdSql + " and c.CentroCostos = '" + pCtroCostos + "'";

            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                Actividad item = new Actividad();

                item.IdActividad = (int)dr["IdActividad"];
                item.CodActividad = dr["CodActividad"].ToString();
                item.DescripcionAct = dr["DescripcionAct"].ToString();
                item.IdComponente = (int)dr["IdComponente"];
                item.DescripCompo = dr["DescripCompo"].ToString();
                item.CodSistema = dr["CodSistema"].ToString();
                item.TipoOperacion = dr["TipoOperacion"].ToString();
                item.Llave = dr["llave"].ToString();

                lstCompo.Add(item);
            }

            return (lstCompo);
        }


        public List<Componente> GetCatCompoxSist(string cnxSql, string pCodSistema, string pDepto, string pCtroCostos)
        {
            List<DataRow> lstDatos = null;
            List<Componente> lstCompo = new List<Componente>();

            string cmdSql = @" SELECT c.IdComponente, c.DescripCompo, c.CodDepartamento, d.Descrip as DescripDepto, c.CodSistema, s.Sistema
                           FROM [CatComponentes] c
                           inner join [CatSistemasEquipos] s on s.CodSistema = c.CodSistema
                           inner join [CatDepartamentos] d on d.CodDepartamento = c.CodDepartamento and c.CentroCostos = d.CentroCostosSap
                           Where c.StatusCompo = 1  and  c.CodSistema = '" + pCodSistema + "'";
            if (pCtroCostos != "")
                cmdSql = cmdSql + " and  c.CodDepartamento = '" + pDepto + "' and c.CentroCostos = '" + pCtroCostos + "'";


            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                Componente item = new Componente();

                item.IdComponente = (int)dr["IdComponente"];
                item.DescripCompo = dr["DescripCompo"].ToString();
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.DescripDepto = dr["DescripDepto"].ToString();
                item.CodSistema = dr["CodSistema"].ToString();
                item.DescripSist = dr["Sistema"].ToString();

                lstCompo.Add(item);
            }

            return (lstCompo);
        }

        public int UpdateDatoCompo(string cnxSql, Componente datos, string pRutaLog)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();

            string cmdTxt = @" UPDATE [dbo].[CatComponentes]
                              SET StatusCompo = @StatusCompo,
                              Usuario = @Usuario, 
                              FchModif = @FchModif
                              Where IdComponente = @IdComponente";


            cmd.CommandText = cmdTxt;
            cmd.Parameters.AddWithValue("@IdComponente", datos.IdComponente);
            cmd.Parameters.AddWithValue("@StatusCompo", datos.StatusCompo);
            cmd.Parameters.AddWithValue("@Usuario", datos.Usuario);
            cmd.Parameters.AddWithValue("@FchModif", datos.FchModif);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Update(cnxSql, cmd, pRutaLog);

            return result;
        }

        public Componente GetDatosCompo(string cnxSql, int pId)
        {
            Componente item = new Componente();

            List<DataRow> lstDatos = null;

            string cmdSql = @" SELECT IdComponente, DescripCompo, CodDepartamento, CodSistema, StatusCompo, Usuario, FchAlta, FchModif
                            FROM [CatComponentes]
                             Where IdComponente =  @pId ";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@pId", pId);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];

                item.IdComponente = (int)dr["IdComponente"];
                item.DescripCompo = dr["DescripCompo"].ToString();
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.CodSistema = dr["CodSistema"].ToString();
                item.StatusCompo = (bool)dr["StatusCompo"];
                item.Usuario = dr["Usuario"].ToString();
                if (dr["FchAlta"].ToString() != "")
                { item.FchAlta = Convert.ToDateTime(dr["FchAlta"]); }
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }

            }

            return item;
        }
        public int GuardarCompo(string cnxSql, Componente newCompo)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"INSERT INTO [dbo].[CatComponentes]
                             (DescripCompo, CodDepartamento, CodSistema, StatusCompo, Usuario, FchAlta, FchModif, CentroCostos)
                             VALUES ( @DescripCompo, @CodDepartamento, @CodSistema, @StatusCompo, @Usuario, @FchAlta, @FchModif, @CentroCostos)";


            cmd.Parameters.AddWithValue("@DescripCompo", newCompo.DescripCompo);
            cmd.Parameters.AddWithValue("@CodDepartamento", newCompo.CodDepartamento);
            cmd.Parameters.AddWithValue("@CodSistema", newCompo.CodSistema);
            cmd.Parameters.AddWithValue("@StatusCompo", newCompo.StatusCompo);
            cmd.Parameters.AddWithValue("@Usuario", newCompo.Usuario);
            cmd.Parameters.AddWithValue("@FchAlta", newCompo.FchAlta);
            cmd.Parameters.AddWithValue("@FchModif", newCompo.FchModif);
            cmd.Parameters.AddWithValue("@CentroCostos", newCompo.CentroCostos);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public List<Componente> GetCatCompo(string cnxSql, string pDepto, string pCtroCostos)
        {
            List<DataRow> lstDatos = null;
            List<Componente> lstCompo = new List<Componente>();

            string cmdSql = @" SELECT c.IdComponente, c.DescripCompo, c.CodDepartamento, d.Descrip as DescripDepto, c.CodSistema, s.Sistema, 
                                 c.StatusCompo, c.Usuario, c.FchAlta, c.FchModif, cf.WorkCenter
                            FROM [CatComponentes] c
                               inner join [CatSistemasEquipos] s on s.CodSistema = c.CodSistema
                               inner join [CatDepartamentos] d on d.CodDepartamento = c.CodDepartamento and c.CentroCostos = d.CentroCostosSap
                               inner join [ConfigTpm] cf on cf.CtroCtosSap = c.CentroCostos";

            if (pCtroCostos != "")
            {
                cmdSql = cmdSql + " Where c.CodDepartamento = '" + pDepto + "' and c.CentroCostos = '" + pCtroCostos + "'";
            }


            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                Componente item = new Componente();

                item.IdComponente = (int)dr["IdComponente"];
                item.DescripCompo = dr["DescripCompo"].ToString();
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.DescripDepto = dr["DescripDepto"].ToString();
                item.CodSistema = dr["CodSistema"].ToString();
                item.DescripSist = dr["Sistema"].ToString();
                item.StatusCompo = (bool)dr["StatusCompo"];
                item.Usuario = dr["Usuario"].ToString();
                if (dr["FchAlta"].ToString() != "")
                { item.FchAlta = Convert.ToDateTime(dr["FchAlta"]); }
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }
                item.WorkCenter = dr["WorkCenter"].ToString();
                lstCompo.Add(item);
            }

            return (lstCompo);
        }
        //********************************************************

        public List<GrupoActivDt> GetGpoActDet(string cnxSql, int pId)
        {
            List<GrupoActivDt> lstItem = new List<GrupoActivDt>();

            List<DataRow> lstDatos = null;

            string cmdSql = @" Select IdDetGrupo, d.idGrupoAct, d.CodGrupo , d.IdActividad, d.CodActividad, d.Orden, d.Item, d.CategoriaAct,
                              a.DescripcionAct, a.TipoOperacion, a.EqParado, a.Activo as ActivoActiv, a.CodSistema, s.Sistema as DescripSist
                            from  [CatGrupoActDt] d
                            join  [CatActivChkLst] a on a.IdActividad = d.IdActividad
                            join  [CatSistemasEquipos] s on s.CodSistema = a.CodSistema
                            Where d.idGrupoAct = @pId
                            order by d.Orden";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@pId", pId);
            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            foreach (DataRow dr in lstDatos)
            {
                GrupoActivDt item = new GrupoActivDt();
                item.IdDetGrupo = (int)dr["IdDetGrupo"];
                item.IdGrupoAct = (int)dr["IdGrupoAct"];
                item.CodGrupo = dr["CodGrupo"].ToString();
                item.IdGrupoAct = (int)dr["IdGrupoAct"];
                item.IdActividad = (int)dr["IdActividad"];
                item.CodActividad = dr["CodActividad"].ToString();
                item.Orden = (int)dr["Orden"];
                item.Item = (int)dr["Item"];
                item.CategoriaAct = dr["CategoriaAct"].ToString();
                item.DescripAct = dr["DescripcionAct"].ToString();
                item.TipoOperacion = dr["TipoOperacion"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.ActivoActiv = (bool)dr["ActivoActiv"];
                item.CodSistema = dr["CodSistema"].ToString();
                item.DescripSist = dr["DescripSist"].ToString();

                lstItem.Add(item);
            }

            return (lstItem);
        }


        public int UpdateGpoActividades(string cnxSql, GrupoActivEnc datos, string pRutaLog)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();

            string cmdTxt = @"Update [CatGrupoActEnc]
                           set   DescripGpo = @DescripGpo, 
                                 EqParado = @EqParado, 
                                 Activo = @Activo,
                                 UserModif = @UserModif, 
                                 FchModif = @FchModif
                           Where idGrupoAct = @idGrupoAct";


            cmd.CommandText = cmdTxt;
            cmd.Parameters.AddWithValue("@idGrupoAct", datos.IdGrupoAct);
            cmd.Parameters.AddWithValue("@DescripGpo", datos.DescripGpo);
            cmd.Parameters.AddWithValue("@Activo", datos.Activo);
            cmd.Parameters.AddWithValue("@EqParado", datos.EqParado);
            cmd.Parameters.AddWithValue("@UserModif", datos.UserModif);
            cmd.Parameters.AddWithValue("@FchModif", datos.FchModif);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Update(cnxSql, cmd, pRutaLog);

            return result;
        }

        public GrupoActivEnc GetGpoActividades(string cnxSql, int pId)
        {
            GrupoActivEnc item = new GrupoActivEnc();

            List<DataRow> lstDatos = null;

            string cmdSql = @" SELECT idGrupoAct, CodGrupo, DescripGpo, EqParado, Activo, CodDepartamento, UserAlta, FchAlta, UserModif, FchModif
                             FROM[CatGrupoActEnc]
                             Where idGrupoAct =  @pId ";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@pId", pId);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];

                item.IdGrupoAct = (int)dr["IdGrupoAct"];
                item.CodGrupo = dr["CodGrupo"].ToString();
                item.DescripGpo = dr["DescripGpo"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.UserAlta = dr["UserAlta"].ToString();
                if (dr["FchAlta"].ToString() != "")
                { item.FchAlta = Convert.ToDateTime(dr["FchAlta"]); }
                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }

            }

            return item;
        }

        public int GuardarGpoActividad(string cnxSql, GrupoActivEnc newGpo)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @" INSERT INTO  [CatGrupoActEnc]
                                     (CodGrupo, DescripGpo, EqParado, Activo, UserAlta, FchAlta, UserModif, FchModif, CodDepartamento, CentroCostos)
                              VALUES ( @CodGrupo, @DescripGpo, @EqParado, @Activo, @UserAlta, @FchAlta, @UserModif, @FchModif, @CodDepartamento, @CtroCostos)";

            cmd.Parameters.AddWithValue("@CodGrupo", newGpo.CodGrupo.ToUpper());
            cmd.Parameters.AddWithValue("@DescripGpo", newGpo.DescripGpo);
            cmd.Parameters.AddWithValue("@EqParado", newGpo.EqParado);
            cmd.Parameters.AddWithValue("@Activo", newGpo.Activo);
            cmd.Parameters.AddWithValue("@CodDepartamento", newGpo.CodDepartamento);
            cmd.Parameters.AddWithValue("@CtroCostos", newGpo.CentroCostos);
            cmd.Parameters.AddWithValue("@UserAlta", newGpo.UserAlta);
            cmd.Parameters.AddWithValue("@FchAlta", newGpo.FchAlta);
            cmd.Parameters.AddWithValue("@UserModif", newGpo.UserModif);
            cmd.Parameters.AddWithValue("@FchModif", newGpo.FchModif);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }
        public int GuardarGpoDetActiv(string cnxSql, GrupoActivDt newGpoDt)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();

            cmd.CommandText = @" INSERT INTO [dbo].[CatGrupoActDt]
                              (IdGrupoAct, CodGrupo, IdActividad, CodActividad, Item, Orden, CategoriaAct, FchModif)
                              VALUES (@IdGrupoAct, @CodGrupo, @IdActividad, @CodActividad, @Item, @Orden, @CategoriaAct, @FchModif)";

            cmd.Parameters.AddWithValue("@IdGrupoAct", newGpoDt.IdGrupoAct);
            cmd.Parameters.AddWithValue("@CodGrupo", newGpoDt.CodGrupo);
            cmd.Parameters.AddWithValue("@IdActividad", newGpoDt.IdActividad);
            cmd.Parameters.AddWithValue("@CodActividad", newGpoDt.CodActividad);
            cmd.Parameters.AddWithValue("@Item", newGpoDt.Item);
            cmd.Parameters.AddWithValue("@Orden", newGpoDt.Orden);
            cmd.Parameters.AddWithValue("@CategoriaAct", newGpoDt.CategoriaAct);
            cmd.Parameters.AddWithValue("@FchModif", newGpoDt.FchModif);

            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public List<GrupoActivEnc> GetCatGpoActiv(string cnxSql, string pDepto, string pCtroCostos, bool SoloActivos = false)
        {
            List<DataRow> lstDatos = null;
            List<GrupoActivEnc> lstAct = new List<GrupoActivEnc>();
            string cmdSql = "";

            if (SoloActivos)
            {
                cmdSql = @" SELECT idGrupoAct, CodGrupo, DescripGpo, EqParado, Activo, UserAlta, FchAlta, UserModif, FchModif
                             FROM [CatGrupoActEnc]
                              Where Activo = 1";
                if (pCtroCostos != "")
                    cmdSql = cmdSql + " and CodDepartamento = '" + pDepto + "' and CentroCostos = '" + pCtroCostos + "'";
            }
            else
            {
                cmdSql = @" SELECT idGrupoAct, CodGrupo, DescripGpo, EqParado, Activo, UserAlta, FchAlta, UserModif, FchModif
                             FROM [CatGrupoActEnc]";
                if (pCtroCostos != "")
                    cmdSql = cmdSql + " Where CodDepartamento = '" + pDepto + "' and CentroCostos = '" + pCtroCostos + "'";
            }
            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);
            foreach (DataRow dr in lstDatos)
            {
                GrupoActivEnc item = new GrupoActivEnc();

                item.IdGrupoAct = (int)dr["IdGrupoAct"];
                item.CodGrupo = dr["CodGrupo"].ToString();
                item.DescripGpo = dr["DescripGpo"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];

                item.UserAlta = dr["UserAlta"].ToString();
                if (dr["FchAlta"].ToString() != "")
                { item.FchAlta = Convert.ToDateTime(dr["FchAlta"]); }
                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }
                item.Llave = "[" + item.CodGrupo + "]  " + item.DescripGpo;

                lstAct.Add(item);
            }
            return (lstAct);
        }

        public int UpdateDatoActividad(string cnxSql, Actividad datos, string pRutaLog)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();

            string cmdTxt = @" Update [CatActivChkLst]
                           set DescripcionAct = @DescripcionAct, TipoOperacion = @TipoOperacion, 
                              EqParado = @EqParado, Activo = @Activo,
                              UserModif = @UserModif,  FchModif = @FchModif
                           Where IdActividad = @IdActividad";


            cmd.CommandText = cmdTxt;
            cmd.Parameters.AddWithValue("@IdActividad", datos.IdActividad);
            cmd.Parameters.AddWithValue("@DescripcionAct", datos.DescripcionAct);
            cmd.Parameters.AddWithValue("@TipoOperacion", datos.TipoOperacion);
            cmd.Parameters.AddWithValue("@Activo", datos.Activo);
            cmd.Parameters.AddWithValue("@EqParado", datos.EqParado);
            cmd.Parameters.AddWithValue("@UserModif", datos.UserModif);
            cmd.Parameters.AddWithValue("@FchModif", datos.FchModif);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Update(cnxSql, cmd, pRutaLog);

            return result;
        }

        public Actividad GetDatosActiv(string cnxSql, int pId)
        {
            Actividad item = new Actividad();

            List<DataRow> lstDatos = null;

            string cmdSql = @" SELECT a.IdActividad, a.CodActividad, a.DescripcionAct, a.CodSistema,s.Sistema as DescripSistema , a.IdComponente, c.DescripCompo, 
                           a.TipoOperacion, a.EqParado, a.Activo, a.CodDepartamento,d.Descrip as DescripDepto, a.UserAlta, a.FchAlta,
                           a.UserModif, a.FchModif
                           FROM [CatActivChkLst] a
                           inner join [CatComponentes] c on c.IdComponente = a.IdComponente 
                           and c.CodSistema = a.CodSistema and c.CodDepartamento = a.CodDepartamento
                           inner join [CatSistemasEquipos] s on s.CodSistema = a.CodSistema 
                           and s.CodDepartamento = a.CodDepartamento
                           inner join [CatDepartamentos] d on d.CodDepartamento = a.CodDepartamento and c.CentroCostos = d.CentroCostosSap                          
                           Where IdActividad =  @pId ";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@pId", pId);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];

                item.IdActividad = (int)dr["IdActividad"];
                item.CodActividad = dr["CodActividad"].ToString();
                item.DescripcionAct = dr["DescripcionAct"].ToString();
                item.CodSistema = dr["CodSistema"].ToString();
                item.DescripSistema = dr["DescripSistema"].ToString();
                item.IdComponente = (int)dr["IdComponente"];
                item.DescripCompo = dr["DescripCompo"].ToString();
                item.TipoOperacion = dr["TipoOperacion"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.DescripDepto = dr["DescripDepto"].ToString();
                item.UserAlta = dr["UserAlta"].ToString();
                if (dr["FchAlta"].ToString() != "")
                { item.FchAlta = Convert.ToDateTime(dr["FchAlta"]); }
                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }
            }

            return item;
        }
        public int GuardarActividad(string cnxSql, Actividad newAct)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"INSERT INTO  [CatActivChkLst]
                              (CodActividad,DescripcionAct,CodSistema,CodDepartamento,IdComponente,TipoOperacion,EqParado,Activo,UserAlta,FchAlta,UserModif,FchModif)
                              VALUES (@CodActividad,@DescripcionAct,@CodSistema,@CodDepartamento,@IdComponente,@TipoOperacion,@EqParado,@Activo,@UserAlta,@FchAlta,@UserModif,@FchModif)";

            cmd.Parameters.AddWithValue("@CodActividad", newAct.CodActividad.ToUpper());
            cmd.Parameters.AddWithValue("@DescripcionAct", newAct.DescripcionAct);
            cmd.Parameters.AddWithValue("@CodSistema", newAct.CodSistema);
            cmd.Parameters.AddWithValue("@CodDepartamento", newAct.CodDepartamento);
            cmd.Parameters.AddWithValue("@IdComponente", (int)newAct.IdComponente);
            cmd.Parameters.AddWithValue("@TipoOperacion", newAct.TipoOperacion);
            cmd.Parameters.AddWithValue("@EqParado", newAct.EqParado);
            cmd.Parameters.AddWithValue("@Activo", newAct.Activo);
            cmd.Parameters.AddWithValue("@UserAlta", newAct.UserAlta);
            cmd.Parameters.AddWithValue("@FchAlta", newAct.FchAlta);
            cmd.Parameters.AddWithValue("@UserModif", newAct.UserModif);
            cmd.Parameters.AddWithValue("@FchModif", newAct.FchModif);


            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public List<Actividad> GetCatActividades(string cnxSql, string pDepto, string pCtroCostos, bool soloActivos = false)
        {
            List<DataRow> lstDatos = null;
            List<Actividad> lstAct = new List<Actividad>();
            string cmdSql = "";
            if (soloActivos)
            {
                cmdSql = @"SELECT a.IdActividad, a.CodActividad, a.DescripcionAct, a.CodSistema,s.Sistema as DescripSistema , a.IdComponente, c.DescripCompo, 
                           a.TipoOperacion, a.EqParado, a.Activo, a.CodDepartamento,d.Descrip as DescripDepto, a.UserAlta, a.FchAlta,
                           a.UserModif, a.FchModif
                           FROM [CatActivChkLst] a
                           inner join [CatComponentes] c on c.IdComponente = a.IdComponente 
                           and c.CodSistema = a.CodSistema and c.CodDepartamento = a.CodDepartamento
                           inner join [CatSistemasEquipos] s on s.CodSistema = a.CodSistema 
                           and s.CodDepartamento = a.CodDepartamento
                           inner join [CatDepartamentos] d on d.CodDepartamento = a.CodDepartamento and c.CentroCostos = d.CentroCostosSap
                           Where a.Activo = 1";

                if (pCtroCostos != "")
                    cmdSql = cmdSql + " and  a.CodDepartamento = '" + pDepto + "' and c.CentroCostos = '" + pCtroCostos + "'";

            }
            else
            {
                cmdSql = @"SELECT a.IdActividad, a.CodActividad, a.DescripcionAct, a.CodSistema,s.Sistema as DescripSistema , a.IdComponente, c.DescripCompo, 
                           a.TipoOperacion, a.EqParado, a.Activo, a.CodDepartamento,d.Descrip as DescripDepto, a.UserAlta, a.FchAlta,
                           a.UserModif, a.FchModif
                           FROM [CatActivChkLst] a
                           inner join [CatComponentes] c on c.IdComponente = a.IdComponente 
                           and c.CodSistema = a.CodSistema and c.CodDepartamento = a.CodDepartamento
                           inner join [CatSistemasEquipos] s on s.CodSistema = a.CodSistema 
                           and s.CodDepartamento = a.CodDepartamento
                           inner join [CatDepartamentos] d on d.CodDepartamento = a.CodDepartamento and c.CentroCostos = d.CentroCostosSap";

                if (pCtroCostos != "")
                    cmdSql = cmdSql + " where  a.CodDepartamento = '" + pDepto + "' and c.CentroCostos = '" + pCtroCostos + "'";
            }



            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                Actividad item = new Actividad();

                item.IdActividad = (int)dr["IdActividad"];
                item.CodActividad = dr["CodActividad"].ToString();
                item.DescripcionAct = dr["DescripcionAct"].ToString();
                item.CodSistema = dr["CodSistema"].ToString();
                item.DescripSistema = dr["DescripSistema"].ToString();
                item.IdComponente = (int)dr["IdComponente"];
                item.DescripCompo = dr["DescripCompo"].ToString();
                item.TipoOperacion = dr["TipoOperacion"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.DescripDepto = dr["DescripDepto"].ToString();
                item.UserAlta = dr["UserAlta"].ToString();
                if (dr["FchAlta"].ToString() != "")
                { item.FchAlta = Convert.ToDateTime(dr["FchAlta"]); }
                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }
                item.Llave = "[" + item.CodActividad + "]  " + item.DescripcionAct;

                lstAct.Add(item);
            }

            return (lstAct);
        }
        public ProducNotificada GetWcCiclos(string cnxSqlMT, string cnxSqlProd, EquipoTpmBasico wc)
        {
            List<DataRow> lstDatos = null;
            ProducNotificada ciclos = new ProducNotificada();

            ciclos.WorkCenter = wc.CodWorkCenter;
            ciclos.PiezasProducidas = 0;

            if (!string.IsNullOrEmpty(wc.CodWorkCenter))
            {
                string cmdSql = @"SELECT WorkCenter, sum(ciclos) as ciclos
                          FROM [TpmCycles].[dbo].[tpm_ProductionDay]
                          Where WorkCenter = @wc
                          and Convert(date,Fecha) >= Convert(date,@fecha)
                          Group by WorkCenter";

                SqlCommand sqlcmd = new SqlCommand(cmdSql);
                sqlcmd.Parameters.AddWithValue("@wc", wc.CodWorkCenter);
                sqlcmd.Parameters.AddWithValue("@fecha", wc.FecUltManto);

                OperDatosSql datosSql = new OperDatosSql();
                lstDatos = datosSql.LeeDatosConParam(cnxSqlProd, sqlcmd);
                DataRow dr = null;

                if (lstDatos.Count > 0)
                {

                    dr = lstDatos[0];
                    ciclos.WorkCenter = dr["WorkCenter"].ToString();
                    ciclos.PiezasProducidas = Convert.ToDecimal(dr["ciclos"]);
                }
            }

            return (ciclos);
        }

        public List<WcReportables> GetWcReport(string cnxSqlMT, string pCtroCostos)
        {
            List<DataRow> lstDatos = null;
            List<WcReportables> lstWcr = new List<WcReportables>();

            string cmdSql = @"SELECT Id, WcReportable, WcHijo, StatusHijo      
                          FROM [CatWcReportables]
                          Where StatusHijo = 1";
            if (pCtroCostos != "")
                cmdSql = cmdSql + " and CentroCostosSap = '" + pCtroCostos + "'";


            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSqlMT, cmdSql);


            foreach (DataRow dr in lstDatos)
            {
                WcReportables dato = new WcReportables();

                dato.Id = (int)dr["id"];
                dato.WcReportable = dr["WcReportable"].ToString().Trim();
                dato.WcHijo = dr["WcHijo"].ToString().Trim();
                dato.StatusHijo = (bool)dr["StatusHijo"];

                lstWcr.Add(dato);
            }

            return (lstWcr);
        }

        public List<Coproducto> GetCoproduc(string cnxSqlMT, string cnxSqlProd)
        {
            List<DataRow> lstDatos = null;
            List<Coproducto> lstcoprod = new List<Coproducto>();

            string cmdSql = @" SELECT id_int, Material, Co_Productos, Linea, TWB
                             FROM [Produccion_Dev].[dbo].[tblMateriales]
                             order by id_int ";

            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSqlProd, cmdSql);


            foreach (DataRow dr in lstDatos)
            {
                Coproducto dato = new Coproducto();

                dato.Id = (int)dr["id_int"];
                dato.Material = dr["Material"].ToString();
                dato.Co_Productos = dr["Co_Productos"].ToString();
                dato.WorkCenter = dr["Linea"].ToString();
                dato.Twb = (int)dr["TWB"];
                lstcoprod.Add(dato);
            }

            return (lstcoprod);
        }

        public List<TipoQr> GetTiposQr(string cnxSql)
        {
            List<DataRow> lstDatos = null;
            List<TipoQr> lstTipos = new List<TipoQr>();

            string cmdSql = @" SELECT Tipo,Aplicacion
                              FROM [dbo].[CatTiposCodeQr]
                              Where Estatus = 1";

            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                TipoQr item = new TipoQr();

                item.Tipo = dr["Tipo"].ToString();
                item.Aplicacion = dr["Aplicacion"].ToString();
                item.Descrip = "[" + item.Tipo + "]  " + item.Aplicacion;
                lstTipos.Add(item);
            }

            return (lstTipos);
        }
        public List<Falla> GetCatFallas(string cnxSql, string pCodSistema, string pCodDepartamento)
        {
            List<DataRow> lstDatos = null;
            List<Falla> lstplt = new List<Falla>();

            string cmdSql = @" SELECT IdFalla, CodFalla, Descrip, CodSistema, CodDepartamento
                           FROM [CatFallas]
                           Where CodSistema = '" + pCodSistema + "' and " +
                              "CodDepartamento = '" + pCodDepartamento + "' and Status = 1";

            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                Falla item = new Falla();

                item.IdFalla = (int)dr["IdFalla"];
                item.CodFalla = dr["CodFalla"].ToString();
                item.Descrip = dr["Descrip"].ToString();
                item.CodSistema = dr["CodSistema"].ToString();
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                lstplt.Add(item);
            }

            return (lstplt);
        }

        public List<Falla> GetTodasCatFallas(string cnxSql, string pCodDepartamento)
        {
            List<DataRow> lstDatos = null;
            List<Falla> lstplt = new List<Falla>();

            string cmdSql = @" SELECT IdFalla, CodFalla, Descrip, CodSistema, CodDepartamento
                           FROM [CatFallas]
                           Where CodDepartamento = '" + pCodDepartamento + "' and Status = 1";

            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                Falla item = new Falla();

                item.IdFalla = (int)dr["IdFalla"];
                item.CodFalla = dr["CodFalla"].ToString();
                item.Descrip = dr["Descrip"].ToString();
                item.CodSistema = dr["CodSistema"].ToString();
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                lstplt.Add(item);
            }

            return (lstplt);
        }

        public List<ClasificFalla> GetCatClasifFallas(string cnxSql)
        {
            List<DataRow> lstDatos = null;
            List<ClasificFalla> lstClasif = new List<ClasificFalla>();

            string cmdSql = @" SELECT IdClasif, CodClasif, Descripcion
                             FROM [CatClasifFallas]
                             Where Status = 1";

            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                ClasificFalla item = new ClasificFalla();

                item.IdClasif = (int)dr["IdClasif"];
                item.CodClasif = dr["CodClasif"].ToString();
                item.Descripcion = dr["Descripcion"].ToString();
                lstClasif.Add(item);
            }

            return (lstClasif);
        }

        public CatEquipo GetDatEquipo(string cnxSql, string pCodEquipo)
        {
            List<DataRow> lstDatos = new List<DataRow>();
            CatEquipo e = new CatEquipo();

            string cmdSql = " SELECT WorkCenter, ObjectNumber, CodEquipo, DescripTechnical, CostCenter, TecObjAutGrp, IndivStatusObject, FunctionalLocation, ";
            cmdSql = cmdSql + " PlannerGroup, MainWorkCenter, StandardTextKeyWC, TypeTechObj, ManufAsset, ManufModelNum, ValidFromDate, Superordinate, KeyPerformanceEfficRateWC";
            cmdSql = cmdSql + " FROM[CatEquipos] ";
            cmdSql = cmdSql + " Where CodEquipo = '" + pCodEquipo + "'";

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];

                e.WorkCenter = dr["WorkCenter"].ToString();
                e.ObjectNumber = dr["ObjectNumber"].ToString();
                e.CodEquipo = dr["CodEquipo"].ToString().Trim();
                e.DescripTechnical = dr["DescripTechnical"].ToString();
                e.CostCenter = dr["CostCenter"].ToString();
                e.TecObjAutGrp = dr["TecObjAutGrp"].ToString();
                e.IndivStatusObject = dr["IndivStatusObject"].ToString();
                e.FunctionalLocation = dr["FunctionalLocation"].ToString();
                e.PlannerGroup = dr["PlannerGroup"].ToString();
                e.MainWorkCenter = dr["MainWorkCenter"].ToString();
                e.StandardTextKeyWC = dr["StandardTextKeyWC"].ToString();
                e.TypeTechObj = dr["TypeTechObj"].ToString();
                e.ManufAsset = dr["ManufAsset"].ToString();
                e.ManufModelNum = dr["ManufModelNum"].ToString();
                e.ValidFromDate = dr["ValidFromDate"].ToString();
                e.Superordinate = dr["Superordinate"].ToString();
                e.KeyPerformanceEfficRateWC = dr["KeyPerformanceEfficRateWC"].ToString();
                e.Cod_Descrip = "[" + dr["CodEquipo"].ToString().Trim() + "]  " + dr["DescripTechnical"].ToString().Trim();
            }
            return (e);
        }

        public List<CatEquipo> GetCatEquiposSap(string cnxSql, string pCtroCostos, string rutalog)
        {
            List<DataRow> lstDatos = null;
            List<CatEquipo> lstEq = new List<CatEquipo>();

            string cmdSql = @" SELECT Distinct WorkCenter, ObjectNumber, CodEquipo, DescripTechnical, CostCenter, TecObjAutGrp, FunctionalLocation,
                             PlannerGroup, MainWorkCenter, StandardTextKeyWC, TypeTechObj, ManufAsset, ManufModelNum, ValidFromDate, Superordinate, KeyPerformanceEfficRateWC,FecUpdate
                              FROM[CatEquipos] 
                             Where PATINDEX('%DOWN%', upper(FunctionalLocation)) = 0";

            if (pCtroCostos != "")
                cmdSql = cmdSql + " and CostCenter = '" + pCtroCostos + "'";


            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@CentroCostos", pCtroCostos);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    CatEquipo e = new CatEquipo();

                    e.WorkCenter = dr["WorkCenter"].ToString();
                    e.ObjectNumber = dr["ObjectNumber"].ToString();
                    e.CodEquipo = dr["CodEquipo"].ToString().Trim();
                    e.DescripTechnical = dr["DescripTechnical"].ToString();
                    e.CostCenter = dr["CostCenter"].ToString();
                    e.TecObjAutGrp = dr["TecObjAutGrp"].ToString();
                    e.IndivStatusObject = "";
                    e.FunctionalLocation = dr["FunctionalLocation"].ToString();
                    e.PlannerGroup = dr["PlannerGroup"].ToString();
                    e.MainWorkCenter = dr["MainWorkCenter"].ToString();
                    e.StandardTextKeyWC = dr["StandardTextKeyWC"].ToString();
                    e.TypeTechObj = dr["TypeTechObj"].ToString();
                    e.ManufAsset = dr["ManufAsset"].ToString();
                    e.ManufModelNum = dr["ManufModelNum"].ToString();
                    e.ValidFromDate = dr["ValidFromDate"].ToString();
                    e.Superordinate = dr["Superordinate"].ToString();
                    e.KeyPerformanceEfficRateWC = dr["KeyPerformanceEfficRateWC"].ToString();
                    e.FecUpdate = Convert.ToDateTime(dr["FecUpdate"]);
                    e.Cod_Descrip = "[" + dr["CodEquipo"].ToString().Trim() + "]  " + dr["DescripTechnical"].ToString().Trim();

                    lstEq.Add(e);
                }
            }
            return (lstEq);
        }

        public List<CatEquipo> GetCatEqStatusSap(string cnxSql, string pCtroCostos, string rutalog)
        {
            List<DataRow> lstDatos = null;
            List<CatEquipo> lstEq = new List<CatEquipo>();

            string cmdSql = @" SELECT Distinct WorkCenter, ObjectNumber, CodEquipo, IndivStatusObject
                              FROM[CatEquipos] 
                             Where PATINDEX('%DOWN%', upper(FunctionalLocation)) = 0";

            if (pCtroCostos != "")
                cmdSql = cmdSql + " and CostCenter = @CentroCostos";


            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@CentroCostos", pCtroCostos);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    CatEquipo e = new CatEquipo();

                    e.WorkCenter = dr["WorkCenter"].ToString();
                    e.ObjectNumber = dr["ObjectNumber"].ToString();
                    e.CodEquipo = dr["CodEquipo"].ToString().Trim();
                    e.IndivStatusObject = dr["IndivStatusObject"].ToString();

                    lstEq.Add(e);
                }
            }
            return (lstEq);
        }

        public List<CatEquipo> GetCatEquiposPad(string cnxSql, string pCtroCostos, string rutalog)
        {
            List<DataRow> lstDatos = null;
            List<CatEquipo> lstEq = new List<CatEquipo>();

            //string cmdSql = @" SELECT WorkCenter, ObjectNumber, CodEquipo, DescripTechnical, CostCenter, TecObjAutGrp, IndivStatusObject, FunctionalLocation,
            //PlannerGroup, MainWorkCenter, StandardTextKeyWC, TypeTechObj, ManufAsset, ManufModelNum, ValidFromDate, Superordinate, KeyPerformanceEfficRateWC
            // FROM[CatEquipos] 
            //Where CostCenter = @CentroCostos
            //      and PATINDEX('%DOWN%', upper(FunctionalLocation)) = 0";

            string cmdSql = @"Select DISTINCT WorkCenter, ObjectNumber, CodEquipo, DescripTechnical, CostCenter, TecObjAutGrp, IndivStatusObject, 
                           FunctionalLocation, PlannerGroup, MainWorkCenter, StandardTextKeyWC, TypeTechObj, ManufAsset, ManufModelNum, ValidFromDate, 
                           Superordinate, KeyPerformanceEfficRateWC
                           FROM[CatEquipos]
                           Where ((@CentroCostos = '') or (CostCenter = @CentroCostos)) and Superordinate = ''
                           and CodEquipo not in (Select CodEquipo  FROM[CatEquipos]
                                                Where ((@CentroCostos = '') or (CostCenter = @CentroCostos)) and Superordinate = ''
                                                and(IndivStatusObject in ('DLFL','INAC') or PATINDEX('%DOWN%', upper(FunctionalLocation)) != 0)
                                                group by CodEquipo)";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@CentroCostos", pCtroCostos);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    CatEquipo e = new CatEquipo();

                    e.WorkCenter = dr["WorkCenter"].ToString();
                    e.ObjectNumber = dr["ObjectNumber"].ToString();
                    e.CodEquipo = dr["CodEquipo"].ToString().Trim();
                    e.DescripTechnical = dr["DescripTechnical"].ToString();
                    e.CostCenter = dr["CostCenter"].ToString();
                    e.TecObjAutGrp = dr["TecObjAutGrp"].ToString();
                    e.IndivStatusObject = dr["IndivStatusObject"].ToString();
                    e.FunctionalLocation = dr["FunctionalLocation"].ToString();
                    e.PlannerGroup = dr["PlannerGroup"].ToString();
                    e.MainWorkCenter = dr["MainWorkCenter"].ToString();
                    e.StandardTextKeyWC = dr["StandardTextKeyWC"].ToString();
                    e.TypeTechObj = dr["TypeTechObj"].ToString();
                    e.ManufAsset = dr["ManufAsset"].ToString();
                    e.ManufModelNum = dr["ManufModelNum"].ToString();
                    e.ValidFromDate = dr["ValidFromDate"].ToString();
                    e.Superordinate = dr["Superordinate"].ToString();
                    e.KeyPerformanceEfficRateWC = dr["KeyPerformanceEfficRateWC"].ToString();
                    e.Cod_Descrip = "[" + dr["CodEquipo"].ToString().Trim() + "]  " + dr["DescripTechnical"].ToString().Trim();

                    lstEq.Add(e);
                }
            }
            return (lstEq);
        }

        public List<EquipoPadre> GetCatEquiposPadres(string cnxSql, string pCtroCostos)
        {
            List<DataRow> lstDatos = null;
            List<EquipoPadre> lstEq = new List<EquipoPadre>();

            // Vamos as generar una lista de equipos padres para mostrar la descripcion a nivel de work center
            // Con el fin de que el operador carge los tickets a un solo equipo
            // el campo valioso es Superordinate, si esta en blanco quiere decir que es el equipo padre
            // para esto en sap hay que ubicarlo correctamente en los functional location
            // No se consideraran los que estan en la funct. location Down,los que tienen estatus de Inactivos y/o bajas ('DLFL','INAC') 

            //string cmdSql = @"SELECT DISTINCT WorkCenter, CodEquipo, TypeTechObj, ObjectNumber, DescripTechnical, MaintPlanningPlant, 
            //                  FunctionalLocation, CostCenter, MainWorkCenter, Superordinate
            //                  FROM[CatEquipos]
            //                  Where CostCenter = @CentroCostos and (IndivStatusObject = 'INST' ) and Superordinate = ''
            //                  and PATINDEX('%DOWN%', upper(FunctionalLocation)) = 0
            //                  order by WorkCenter";
            string cmdSql = " SELECT DISTINCT ";

            //if (pCtroCostos == "")
            //{
            //    cmdSql = cmdSql + " TOP 200 ";
            //}

            cmdSql = cmdSql + @" WorkCenter, CodEquipo, IndivStatusObject,FunctionalLocation, TypeTechObj, ObjectNumber, 
               DescripTechnical, MaintPlanningPlant, CostCenter, MainWorkCenter, Superordinate, ValidFromDate, ManufAsset,
               ManufModelNum, PlannerGroup, MainWorkCenter, Superordinate, StandardTextKeyWC
               FROM[CatEquipos]
                     Where Superordinate = ''
               and CodEquipo not in (Select DISTINCT CodEquipo
                                       FROM [CatEquipos]
                                             Where Superordinate = ''";
            if (pCtroCostos != "")
            {
                cmdSql = cmdSql + " and CostCenter = @CentroCostos";
            }
            cmdSql = cmdSql + " and (IndivStatusObject in ('DLFL','INAC') or PATINDEX('%DOWN%', upper(FunctionalLocation)) != 0))";
            if (pCtroCostos != "")
                cmdSql = cmdSql + " and CostCenter = @CentroCostos ";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@CentroCostos", pCtroCostos);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    EquipoPadre e = new EquipoPadre();

                    e.CodWorkCenter = dr["WorkCenter"].ToString();
                    e.CodEquipo = dr["CodEquipo"].ToString().Trim();
                    e.TypeTechObj = dr["TypeTechObj"].ToString();
                    e.ObjectNumber = dr["ObjectNumber"].ToString();
                    e.DescripTechnical = dr["DescripTechnical"].ToString();
                    e.MaintPlanningPlant = dr["MaintPlanningPlant"].ToString();
                    e.FunctionalLocation = dr["FunctionalLocation"].ToString();
                    e.CostCenter = dr["CostCenter"].ToString();
                    e.MainWorkCenter = dr["MainWorkCenter"].ToString();
                    e.Superordinate = dr["Superordinate"].ToString();
                    e.Cod_Descrip = "[" + dr["CodEquipo"].ToString().Trim() + "]  " + dr["DescripTechnical"].ToString().Trim();
                    e.ValidFromDate = dr["ValidFromDate"].ToString();
                    e.IndivStatusObject = dr["IndivStatusObject"].ToString();
                    e.ManufAsset = dr["ManufAsset"].ToString();
                    e.ManufModelNum = dr["ManufModelNum"].ToString();
                    e.StandardTextKeyWC = dr["StandardTextKeyWC"].ToString();
                    e.PlannerGroup = dr["PlannerGroup"].ToString();
                    lstEq.Add(e);
                }
            }
            return (lstEq);
        }

        public List<EstruturaEquipo> GetCatEstrucEqu(string cnxSql, string pCtroCostos)
        {
            List<DataRow> lstDatos = null;
            List<EstruturaEquipo> lstEE = new List<EstruturaEquipo>();

            string cmdSql = @" SELECT DISTINCT FuncionalLoc, Descripcion, LanguageKey, SuperiorFuncLoc, TipoObjectoWc, IdObjeto, LocationAccount, PlantaManto, AreaControling, 
         CentroCosto, IdObjectPPWc, Propietario, CodEquipo, PmObjType, IdObjectWc, Equipodesc, IdEquipo, EquipoValidFromDate,
         MainWorkCenter, WorkCenter, TypeTechObj, PlannerGroup, FecUpdate
         FROM [CatEstrucEquipos] 
         Where ((@CentroCostos = '') or (CentroCosto = @CentroCostos))";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@CentroCostos", pCtroCostos);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    EstruturaEquipo e = new EstruturaEquipo();

                    e.FuncionalLoc = dr["FuncionalLoc"].ToString();
                    e.Descripcion = dr["Descripcion"].ToString();
                    e.LanguageKey = dr["LanguageKey"].ToString();
                    e.SuperiorFuncLoc = dr["SuperiorFuncLoc"].ToString();
                    e.TipoObjectoWc = dr["TipoObjectoWc"].ToString();
                    e.IdObjeto = dr["IdObjeto"].ToString();
                    e.LocationAccount = dr["LocationAccount"].ToString();
                    e.PlantaManto = dr["PlantaManto"].ToString();
                    e.AreaControling = dr["AreaControling"].ToString();
                    e.CentroCosto = dr["CentroCosto"].ToString();
                    e.IdObjectPPWc = (decimal)dr["IdObjectPPWc"];
                    e.Propietario = dr["Propietario"].ToString();
                    e.CodEquipo = dr["CodEquipo"].ToString();
                    e.PmObjType = dr["PmObjType"].ToString();
                    e.IdObjectWc = (decimal)dr["IdObjectWc"];
                    e.Equipodesc = dr["Equipodesc"].ToString();
                    e.IdEquipo = dr["IdEquipo"].ToString();
                    e.EquipoStatus = "";
                    e.EquipoValidFromDate = dr["EquipoValidFromDate"].ToString();
                    e.MainWorkCenter = dr["MainWorkCenter"].ToString();
                    e.WorkCenter = dr["WorkCenter"].ToString();
                    e.TypeTechObj = dr["TypeTechObj"].ToString();
                    e.PlannerGroup = dr["PlannerGroup"].ToString();
                    e.FecUpdate = Convert.ToDateTime(dr["FecUpdate"].ToString());
                    //e.Id = (int)dr["Id"];

                    lstEE.Add(e);
                }
            }
            return (lstEE);
        }

        public List<CatEquipo> GetCatEstrucEquWC(string cnxSql, string pWc, string pCodEquipo)
        {
            List<DataRow> lstDatos = null;
            List<CatEquipo> lstEE = new List<CatEquipo>();

            // Solo los equipos que no tengan estatus de inactivos y marcados para borrar
            string cmdSql = @"SELECT DISTINCT WorkCenter, CodEquipo, DescripTechnical, IndivStatusObject, ObjectNumber, TypeTechObj
                           FROM[CatEquipos]
                           Where WorkCenter = @Wc
                           and CodEquipo = @CodEquipo
                           and CodEquipo not in (Select CodEquipo  FROM[CatEquipos]
                                          Where WorkCenter = @Wc
                                          and(IndivStatusObject in ('DLFL','INAC') or PATINDEX('%DOWN%', upper(FunctionalLocation)) != 0)
                                          group by CodEquipo  )
                           order by WorkCenter";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@Wc", pWc);
            sqlcmd.Parameters.AddWithValue("@CodEquipo", pCodEquipo);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    CatEquipo e = new CatEquipo();

                    e.WorkCenter = dr["WorkCenter"].ToString();
                    e.CodEquipo = dr["CodEquipo"].ToString();
                    e.DescripTechnical = dr["DescripTechnical"].ToString();
                    e.IndivStatusObject = dr["IndivStatusObject"].ToString();
                    e.ObjectNumber = dr["ObjectNumber"].ToString();
                    e.TypeTechObj = dr["TypeTechObj"].ToString();
                    e.Cod_Descrip = "[ " + dr["CodEquipo"].ToString() + " ]       " + dr["DescripTechnical"].ToString().Trim();
                    lstEE.Add(e);
                }
            }
            return (lstEE);
        }
        public List<TblSap_IFLO> GetCatFuntLocation(string cnxSql, string pCtroCostos)
        {
            List<DataRow> lstDatos = null;
            List<TblSap_IFLO> lstFl = new List<TblSap_IFLO>();

            string cmdSql = @"SELECT FunctionalLocation, LanguageKey, Description, SuperiorFunctLoc, ObjectTypeWorkCenter, ObjectNumber, MaintPlanningPlant, 
                                 ControllingArea, CostCenter, PlannerGroup, ObjectIDWorkCenter, Id, fecUpdate 
                                 FROM[CatFuntLocation] ";


            if (pCtroCostos != "")
                cmdSql = cmdSql + " where CostCenter = '" + pCtroCostos + "'";


            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    TblSap_IFLO e = new TblSap_IFLO();

                    e.FunctionalLocation = dr["FunctionalLocation"].ToString();
                    e.LanguageKey = dr["LanguageKey"].ToString();
                    e.Description = dr["Description"].ToString();
                    e.SuperiorFunctLoc = dr["SuperiorFunctLoc"].ToString();
                    e.ObjectTypeWorkCenter = dr["ObjectTypeWorkCenter"].ToString();
                    e.ObjectNumber = dr["ObjectNumber"].ToString();
                    e.MaintPlanningPlant = dr["MaintPlanningPlant"].ToString();
                    e.ControllingArea = dr["ControllingArea"].ToString();
                    e.CostCenter = dr["CostCenter"].ToString();
                    e.PlannerGroup = dr["PlannerGroup"].ToString();
                    e.ObjectIDWorkCenter = dr["ObjectIDWorkCenter"].ToString();
                    e.fecUpdate = Convert.ToDateTime(dr["fecUpdate"].ToString());

                    lstFl.Add(e);
                }
            }
            return (lstFl);
        }

        public List<SistemaManto> LeeCatSistManto(string cnxSql, string pDepto)
        {
            List<DataRow> lstDatos = null;
            List<SistemaManto> lstSm = new List<SistemaManto>();

            string cmdSql = " SELECT Id, CodSistema, Sistema, CodDepartamento, Estatus FROM [CatSistemasEquipos] ";

            if (pDepto != "")
                cmdSql = cmdSql + " Where CodDepartamento = '" + pDepto.Trim() + "'";

            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                SistemaManto item = new SistemaManto();

                item.Id = dr[0].ToString();
                item.CodSistema = dr[1].ToString();
                item.Sistema = dr[2].ToString();
                item.CodDepartamento = dr[3].ToString();
                item.Estatus = (bool)dr[4];
                item.keySistemas = "[" + item.CodSistema.Trim() + "] " + item.Sistema;
                lstSm.Add(item);
            }

            return (lstSm);
        }

        public int GuardarCatSistManto(string cnxSql, List<SistemaManto> lstTempSE)
        {

            int result = 0;
            SqlCommand cmd = new SqlCommand();

            string cmdSql = " Update [CatSistemasEquipos]";
            cmdSql = cmdSql + "Set CodSistema = @CodSistema ,";
            cmdSql = cmdSql + "     Sistema = @Sistema, ";
            cmdSql = cmdSql + "     CodDepartamento = @CodDepto, ";
            cmdSql = cmdSql + "     Estatus = @Estatus ";
            cmdSql = cmdSql + "Where Id = @Id";
            cmd.CommandText = cmdSql;

            OperDatosSql operDatos = new OperDatosSql();

            foreach (var dr in lstTempSE)
            {
                cmd.Parameters.AddWithValue("@Id", dr.Id);
                cmd.Parameters.AddWithValue("@CodSistema", dr.CodSistema);
                cmd.Parameters.AddWithValue("@Sistema", dr.Sistema);
                cmd.Parameters.AddWithValue("@CodDepto", dr.CodDepartamento);
                cmd.Parameters.AddWithValue("@Estatus", dr.Estatus);

                result = operDatos.Guardar(cnxSql, cmd);
                cmd.Parameters.Clear();
            }

            return result;
        }

        public int GuardarSist(string cnxSql, SistemaManto datoSist, string pRutaLog)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " Insert into [CatSistemasEquipos] values (@CodSistema,@Sistema,@CodDepto,@Estatus)";
            cmd.Parameters.AddWithValue("@CodSistema", datoSist.CodSistema);
            cmd.Parameters.AddWithValue("@Sistema", datoSist.Sistema);
            cmd.Parameters.AddWithValue("@CodDepto", datoSist.CodDepartamento);
            cmd.Parameters.AddWithValue("@Estatus", datoSist.Estatus);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public int UpdateSistManto(string cnxSql, SistemaManto datoSist, string pRutaLog)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();

            string cmdtxt = " Update [CatSistemasEquipos] ";
            cmdtxt = cmdtxt + " set CodSistema = @CodSistema,  ";
            cmdtxt = cmdtxt + "     Sistema = @Sistema, ";
            cmdtxt = cmdtxt + "     CodDepartamento = @CodDepto, ";
            cmdtxt = cmdtxt + "     Estatus = @Estatus ";
            cmdtxt = cmdtxt + " Where Id = @Id ";

            cmd.CommandText = cmdtxt;

            cmd.Parameters.AddWithValue("@Id", datoSist.Id);
            cmd.Parameters.AddWithValue("@CodSistema", datoSist.CodSistema);
            cmd.Parameters.AddWithValue("@Sistema", datoSist.Sistema);
            cmd.Parameters.AddWithValue("@CodDepto", datoSist.CodDepartamento);
            cmd.Parameters.AddWithValue("@Estatus", datoSist.Estatus);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Update(cnxSql, cmd, pRutaLog);

            return result;
        }

        public List<WorkCenter> GetCatWorkCter(string cnxSql, string pCategoria)
        {
            List<DataRow> lstDatos = null;
            List<WorkCenter> lstWc = new List<WorkCenter>();

            string cmdSql = " SELECT Planta, TipoObjeto, IdWorkCenter, CodWorkCenter, Categoria, keyValorStandar, Administrador, FocusFactory, keyValordeOperacion,";
            cmdSql = cmdSql + " InicioVigencia, FinVigencia, Lenguaje, Descripcion, FechaUpdate, Id ";
            cmdSql = cmdSql + " FROM [CatWorkCenter] ";
            cmdSql = cmdSql + " Where Categoria = @Categoria";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@Categoria", pCategoria);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    WorkCenter i = new WorkCenter();

                    i.Planta = dr["Planta"].ToString();
                    i.TipoObjeto = dr["TipoObjeto"].ToString();
                    i.IdWorkCenter = (decimal)dr["IdWorkCenter"];
                    i.CodWorkCenter = dr["CodWorkCenter"].ToString();
                    i.Categoria = dr["Categoria"].ToString();
                    i.keyValorStandar = dr["keyValorStandar"].ToString();
                    i.Administrador = dr["Administrador"].ToString();
                    i.FocusFactory = dr["FocusFactory"].ToString();
                    i.keyValordeOperacion = dr["keyValordeOperacion"].ToString();
                    i.InicioVigencia = Convert.ToDateTime(dr["InicioVigencia"].ToString());
                    i.FinVigencia = Convert.ToDateTime(dr["FinVigencia"]);
                    i.Lenguaje = dr["Lenguaje"].ToString();
                    i.Descripcion = dr["Descripcion"].ToString();
                    i.FechaUpdate = Convert.ToDateTime(dr["FechaUpdate"]);
                    i.Id = (int)dr["Id"];

                    lstWc.Add(i);
                }
            }
            return (lstWc);
        }

        public List<Departamento> LeeCatDeptos(string cnxSql, string pCtroCostos)
        {
            List<DataRow> lstDatos = null;
            List<Departamento> lstDpt = new List<Departamento>();

            string cmdSql = @" SELECT ";
            //if (pCtroCostos == "")
            //    cmdSql = cmdSql + " Top 1 ";
            cmdSql = cmdSql + " Id, IdPlanta, PlantaSatelite, CodDepartamento, Descrip, CentroCostos, Estatus, CentroCostosSap FROM [CatDepartamentos] ";
            if (pCtroCostos != "")
                cmdSql = cmdSql + " Where CentroCostosSap = '" + pCtroCostos + "'";

            //SqlCommand sqlcmd = new SqlCommand(cmdSql);
            //sqlcmd.Parameters.AddWithValue("@CentroCostos", pCtroCostos);

            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                Departamento item = new Departamento();

                item.Id = Convert.ToInt32(dr["Id"]);
                item.IdPlanta = Convert.ToInt32(dr["IdPlanta"]);
                item.PlantaSatelite = dr["PlantaSatelite"].ToString();
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.Descrip = dr["Descrip"].ToString();
                item.CentroCostos = dr["CentroCostosSap"].ToString();
                item.Estatus = (bool)dr["Estatus"];
                item.KeyDescrip = "[" + dr["CentroCostosSap"].ToString() + "] " + dr["Descrip"].ToString();

                lstDpt.Add(item);
            }

            return (lstDpt);
        }

        public int GuardarCatDeptos(string cnxSql, List<Departamento> lstTempSE)
        {

            int result = 0;
            SqlCommand cmd = new SqlCommand();

            string cmdSql = " Update [CatDepartamentos]";
            cmdSql = cmdSql + "Set PlantaSatelite = @PlantaSatelite ,";
            cmdSql = cmdSql + "    CodDepartamento = @CodDepartamento, ";
            cmdSql = cmdSql + "    Descrip = @Descrip, ";
            cmdSql = cmdSql + "    CentroCostos = @CentroCostos, ";
            cmdSql = cmdSql + "    Estatus = @Estatus ";
            cmdSql = cmdSql + "Where Id = @Id";
            cmd.CommandText = cmdSql;

            OperDatosSql operDatos = new OperDatosSql();

            foreach (var dr in lstTempSE)
            {
                cmd.Parameters.AddWithValue("@Id", dr.Id);
                cmd.Parameters.AddWithValue("@PlantaSatelite", dr.PlantaSatelite);
                cmd.Parameters.AddWithValue("@CodDepartamento", dr.CodDepartamento);
                cmd.Parameters.AddWithValue("@Descrip", dr.Descrip);
                cmd.Parameters.AddWithValue("@CentroCostos", dr.CentroCostos);
                cmd.Parameters.AddWithValue("@Estatus", dr.Estatus);

                result = operDatos.Guardar(cnxSql, cmd);
                cmd.Parameters.Clear();
            }

            return result;
        }

        public int GuardarDepto(string cnxSql, Departamento dato)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " Insert into [CatDepartamentos] values (@PlantaSatelite,@CodDepartamento,@Descrip,@CentroCostos,@Estatus)";
            cmd.Parameters.AddWithValue("@PlantaSatelite", dato.PlantaSatelite);
            cmd.Parameters.AddWithValue("@CodDepartamento", dato.CodDepartamento);
            cmd.Parameters.AddWithValue("@Descrip", dato.Descrip);
            cmd.Parameters.AddWithValue("@CentroCostos", dato.CentroCostos);
            cmd.Parameters.AddWithValue("@Estatus", dato.Estatus);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public List<PlantaSatelite> LeeCatPlantas(string cnxSql)
        {
            List<DataRow> lstDatos = null;
            List<PlantaSatelite> lstplt = new List<PlantaSatelite>();

            string cmdSql = " SELECT idPlanta, ClavePlanta, planta, Estatus ";
            cmdSql = cmdSql + " FROM [CatPlantas]";
            //cmdSql = cmdSql + " Where Estatus = 1  ";

            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                PlantaSatelite item = new PlantaSatelite();

                item.IdPlanta = Convert.ToInt32(dr[0]);
                item.ClavePlanta = dr[1].ToString();
                item.planta = dr[2].ToString();
                item.Estatus = (bool)dr[3];

                lstplt.Add(item);
            }

            return (lstplt);
        }

        public List<PlanMantto> LeeCatPlanMantto(string cnxSql, string pCtroCostos)
        {
            List<DataRow> lstDr = null;
            List<PlanMantto> lstDatos = new List<PlanMantto>();

            string cmdSql =
               @"SELECT p.Id,p.CodEquipo,p.CodSistema,p.CodCiclo,p.Frecuencia,p.FechaAlta,p.UsuarioAlta,p.FechaCancelacion,
               p.UsuarioCancelo,p.Estatus,p.FecUltEjecucion,p.CentroCostos, e.WorkCenter
               FROM [CatPlanesMantto] p
               inner join (select Distinct CodEquipo, WorkCenter 
							from [CatEquipos]) e on e.CodEquipo = p.CodEquipo";

            if (pCtroCostos != "")
                cmdSql = cmdSql + " where p.CentroCostos = '" + pCtroCostos + "'";




            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDr)
            {
                PlanMantto i = new PlanMantto();

                i.Id = Convert.ToInt32(dr[0]);
                i.CodEquipo = dr[1].ToString();
                i.CodSistema = dr[2].ToString();
                i.CodCiclo = dr[3].ToString();
                i.Frecuencia = Convert.ToInt32(dr[4]);
                i.FechaAlta = Convert.ToDateTime(dr[5]);
                i.UsuarioAlta = dr[6].ToString();
                if (dr[7].ToString() != "")
                {
                    i.FechaCancelacion = Convert.ToDateTime(dr[7]);
                }


                i.UsuarioCancelo = dr[8].ToString();
                i.Estatus = Convert.ToBoolean(dr[9]);
                i.FecUltEjecucion = Convert.ToDateTime(dr[10]);
                i.CodWorkCenter = dr[12].ToString();

                lstDatos.Add(i);
            }

            return (lstDatos);
        }

        public List<PlanMantto> LeeCatPlanManttoWc(string cnxSql, string pCtroCostos)
        {
            List<DataRow> lstDr = null;
            List<PlanMantto> lstDatos = new List<PlanMantto>();

            string cmdSql = @"SELECT p.id, p.CodEquipo, p.CodSistema, p.CodCiclo, p.Frecuencia, p.FechaAlta, p.UsuarioAlta, p.FechaCancelacion, 
                                    p.UsuarioCancelo, p.Estatus, p.FecUltEjecucion, p.CentroCostos, e.WorkCenter
                           FROM[CatPlanesMantto] p, [CatEquipos] e
                           Where p.CodEquipo = e.CodEquipo
                           Where  p.Estatus = 1";

            if (pCtroCostos != "")
                cmdSql = cmdSql + " and p.CentroCostos = '" + pCtroCostos + "'";



            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDr)
            {
                PlanMantto i = new PlanMantto();

                i.Id = Convert.ToInt32(dr[0]);
                i.CodEquipo = dr[1].ToString();
                i.CodSistema = dr[2].ToString();
                i.CodCiclo = dr[3].ToString();
                i.Frecuencia = Convert.ToInt32(dr[4]);
                i.FechaAlta = Convert.ToDateTime(dr[5]);
                i.UsuarioAlta = dr[6].ToString();
                if (dr[7].ToString() != "")
                {
                    i.FechaCancelacion = Convert.ToDateTime(dr[7]);
                }


                i.UsuarioCancelo = dr[8].ToString();
                i.Estatus = Convert.ToBoolean(dr[9]);
                i.FecUltEjecucion = Convert.ToDateTime(dr[10]);

                lstDatos.Add(i);
            }

            return (lstDatos);
        }

        public List<Ciclos> LeeCatCiclos(string cnxSql)
        {
            List<DataRow> lstDr = null;
            List<Ciclos> lstDatos = new List<Ciclos>();

            string cmdSql = @"  SELECT idCiclo, CodCiclo, Descripcion, Estatus, Orden
                              FROM [CatCiclos]
                              Where Estatus = 1  Order by idCiclo ";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDr)
            {
                Ciclos i = new Ciclos();

                i.IdCiclo = Convert.ToInt32(dr[0]);
                i.CodCiclo = dr[1].ToString();
                i.Descripcion = dr[2].ToString();
                i.Estatus = Convert.ToBoolean(dr[3]);
                i.Orden = Convert.ToInt32(dr[4]);

                lstDatos.Add(i);
            }

            return (lstDatos);
        }

        public List<PmStandar> LeePMStandar(string cnxSql, string pCtroCostos)
        {
            List<DataRow> lstDr = null;
            List<PmStandar> lstDatos = new List<PmStandar>();

            string cmdSql = @" SELECT Id, WorkCenter, CodEquipo, CodSistemas, CodCiclo, Ppm, Estatus, CentroCostos
                               FROM[CatPMStandar] 
                               Where Estatus = 1 ";

            if (pCtroCostos != "")
                cmdSql = cmdSql + " and CentroCostos = '" + pCtroCostos + "'";


            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDr)
            {
                PmStandar i = new PmStandar();

                i.Id = (int)dr["Id"];
                i.WorkCenter = dr["WorkCenter"].ToString();
                i.CodEquipo = dr["CodEquipo"].ToString();
                i.CodSistemas = dr["CodSistemas"].ToString();
                i.CodCiclo = dr["CodCiclo"].ToString();
                i.Ppm = (decimal)dr["Ppm"];
                i.PMStandar = 0;
                i.Estatus = (bool)dr["Estatus"];
                i.CentroCostos = dr["CentroCostos"].ToString();
                lstDatos.Add(i);
            }

            return (lstDatos);
        }
        public List<PlanMantto> LeeCatPM(string cnxSql, string pCtroCostos)
        {
            List<DataRow> lstDr = null;
            List<PlanMantto> lstDatos = new List<PlanMantto>();

            string cmdSql = @"SELECT CodEquipo, CodSistema, CodCiclo, Frecuencia, Estatus, FecUltEjecucion, CentroCostos
                           from   [CatPlanesMantto]
                           Where Estatus = 1";

            if (pCtroCostos != "")
                cmdSql = cmdSql + " and CentroCostos = '" + pCtroCostos + "'";



            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDr)
            {
                PlanMantto i = new PlanMantto();

                i.CodEquipo = dr["CodEquipo"].ToString();
                i.CodSistema = dr["CodSistema"].ToString();
                i.CodCiclo = dr["CodCiclo"].ToString();
                i.Frecuencia = (int)dr["Frecuencia"];
                i.FecUltEjecucion = Convert.ToDateTime(dr["FecUltEjecucion"]);
                i.Estatus = (bool)dr["Estatus"];

                lstDatos.Add(i);
            }

            return (lstDatos);
        }
        public int GuardarPM(string cnxSql, PlanMantto plan, string pCentroCostos)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " Insert into [CatPlanesMantto] values (@CodEquipo,@CodSistema,@CodCiclo,@Frecuencia,@FechaAlta,@UsuarioAlta,@FechaCancelacion,@UsuarioCancelo,@Estatus,@FecUltEjecucion,@CentroCostos)";
            cmd.Parameters.AddWithValue("@CodEquipo", plan.CodEquipo);
            cmd.Parameters.AddWithValue("@CodSistema", plan.CodSistema);
            cmd.Parameters.AddWithValue("@CodCiclo", plan.CodCiclo);
            cmd.Parameters.AddWithValue("@Frecuencia", plan.Frecuencia);
            cmd.Parameters.AddWithValue("@FechaAlta", plan.FechaAlta);
            cmd.Parameters.AddWithValue("@UsuarioAlta", plan.UsuarioAlta);
            cmd.Parameters.AddWithValue("@Estatus", plan.Estatus);
            cmd.Parameters.AddWithValue("@FechaCancelacion", DBNull.Value);
            cmd.Parameters.AddWithValue("@UsuarioCancelo", "");

            cmd.Parameters.AddWithValue("@FecUltEjecucion", plan.FecUltEjecucion);
            cmd.Parameters.AddWithValue("@CentroCostos", pCentroCostos);


            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public int UpdatePM(string cnxSql, PlanMantto plan, string pRutaLog)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();

            string cmdTxt = "Update  [CatPlanesMantto] ";
            cmdTxt = cmdTxt + " Set CodCiclo = @CodCiclo, Frecuencia = @Frecuencia,  FechaAlta = @FechaAlta, UsuarioAlta = @UsuarioAlta, ";
            cmdTxt = cmdTxt + " UsuarioCancelo = @UsuarioCancelo, Estatus = @Estatus , FecUltEjecucion = @FecUltEjecucion, FechaCancelacion = @FechaCancelacion ";
            cmdTxt = cmdTxt + " Where Id = @id ";


            cmd.CommandText = cmdTxt;
            cmd.Parameters.AddWithValue("@id", plan.Id);
            cmd.Parameters.AddWithValue("@CodCiclo", plan.CodCiclo);
            cmd.Parameters.AddWithValue("@Frecuencia", plan.Frecuencia);
            cmd.Parameters.AddWithValue("@FechaAlta", plan.FechaAlta);
            cmd.Parameters.AddWithValue("@UsuarioAlta", plan.UsuarioAlta);

            if (plan.FechaCancelacion.ToString("MM/dd/yyyy").Substring(0, 10) != "01/01/0001")
            {
                cmd.Parameters.AddWithValue("@FechaCancelacion", plan.FechaCancelacion);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FechaCancelacion", DBNull.Value);
            }

            if (plan.UsuarioCancelo != null)
            {
                cmd.Parameters.AddWithValue("@UsuarioCancelo", plan.UsuarioCancelo);
            }
            else
            {
                cmd.Parameters.AddWithValue("@UsuarioCancelo", " ");
            }

            cmd.Parameters.AddWithValue("@Estatus", plan.Estatus);
            cmd.Parameters.AddWithValue("@FecUltEjecucion", plan.FecUltEjecucion);


            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Update(cnxSql, cmd, pRutaLog);

            return result;
        }

        public int GuardarCatEquiposSql(string cnxSql, List<Equipo> lstEquipos, string nameTabla, string pathLog)
        {
            int result = 0;
            int resultDelete = 0;
            DataTable dt = null;

            string cmdSql = " SELECT * ";
            cmdSql = cmdSql + "  FROM [" + nameTabla + "] ";
            cmdSql = cmdSql + " Where CategoryEqui = 'x' ";

            OperDatosSql datosSql = new OperDatosSql();
            DateTime fec = DateTime.Now;

            dt = datosSql.GetStructTabla(cnxSql, cmdSql);

            foreach (Equipo e in lstEquipos)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = e.Id;
                dr["CodEquipo"] = e.CodEquipo;
                dr["LengEqui"] = e.LengEqui;
                dr["TecObjAutGrp"] = e.TecObjAutGrp;
                dr["CategoryEqui"] = e.CategoryEqui;
                dr["TypeTechObj"] = e.TypeTechObj;
                dr["ManufAsset"] = e.ManufAsset;
                dr["ManufModelNum"] = e.ManufModelNum;
                dr["ConsecutiveNum"] = e.ConsecutiveNum;
                dr["ObjectNumber"] = e.ObjectNumber;
                dr["MaintenancePlan"] = e.MaintenancePlan;
                dr["MeasuringPoint"] = e.MeasuringPoint;
                dr["DescripTechnical"] = e.DescripTechnical;
                dr["StatusInactive"] = e.StatusInactive;
                dr["SystemStatus"] = e.SystemStatus;
                dr["LengDescrip"] = e.LengDescrip;
                dr["IndivStatusObject"] = e.IndivStatusObject;
                dr["ObjectStatus"] = e.ObjectStatus;
                dr["ValidDate"] = e.ValidDate;
                dr["NumNextEquipUsage"] = e.NumNextEquipUsage;
                dr["MaintPlanningPlant"] = e.MaintPlanningPlant;
                dr["ValidFromDate"] = e.ValidFromDate;
                dr["Superordinate"] = e.Superordinate;
                dr["PlannerGroup"] = e.PlannerGroup;
                dr["PmObjType"] = e.PmObjType;
                dr["IdWorkCenter"] = e.IdWorkCenter;
                dr["TechIdentNumber"] = e.TechIdentNumber;
                dr["AccountAssignment"] = e.AccountAssignment;
                dr["CatalogProfile"] = e.CatalogProfile;
                dr["TechnicalInformation"] = e.TechnicalInformation;
                dr["FunctionalLocation"] = e.FunctionalLocation;
                dr["CrObjType"] = e.CrObjType;
                dr["ObjectIdPPWorkCenter"] = e.ObjectIdPPWorkCenter;
                dr["ControllingArea"] = e.ControllingArea;
                dr["CostCenter"] = e.CostCenter;
                dr["MainWorkCenter"] = e.MainWorkCenter;
                dr["MainWCCategory"] = e.MainWCCategory;
                dr["MainWCLocation"] = e.MainWCLocation;
                dr["PersonResponsibleMWC"] = e.PersonResponsibleMWC;
                dr["StandardTextKeyMWC"] = e.StandardTextKeyMWC;
                dr["StandardValueKeyMWC"] = e.StandardValueKeyMWC;
                dr["KeyPerformanceEfficRateMWC"] = e.KeyPerformanceEfficRateMWC;
                dr["WorkCenter"] = e.WorkCenter;
                dr["CategoryWorkCenter"] = e.CategoryWorkCenter;
                dr["LocationWorkCenter"] = e.LocationWorkCenter;
                dr["PersonResponsibleWC"] = e.PersonResponsibleWC;
                dr["StandardTextKeyWC"] = e.StandardTextKeyWC;
                dr["StandardValueKeyWC"] = e.StandardValueKeyWC;
                dr["KeyPerformanceEfficRateWC"] = e.KeyPerformanceEfficRateWC;
                dr["ObjectTypesResource"] = e.ObjectTypesResource;
                dr["FecUpdate"] = fec;

                dt.Rows.Add(dr);
            }



            resultDelete = datosSql.VaciarTabla(cnxSql, nameTabla, pathLog);
            if (resultDelete > 0)
                result = datosSql.GuardarTablaSap(cnxSql, dt, nameTabla, pathLog);
            else
                result = -99;

            return result;
        }

        public int GuardarCatWCSql(string cnxSql, List<WorkCenter> lstWc, string nameTabla, string pathLog)
        {
            int result = 0;
            int resultDelete = 0;
            DataTable dt = null;

            string cmdSql = " SELECT * ";
            cmdSql = cmdSql + "  FROM [CatWorkCenter] ";
            cmdSql = cmdSql + " Where Planta = '9999' ";

            OperDatosSql datosSql = new OperDatosSql();
            DateTime fec = DateTime.Now;

            dt = datosSql.GetStructTabla(cnxSql, cmdSql);
            int cnt = 1;
            foreach (WorkCenter e in lstWc)
            {
                DataRow dr = dt.NewRow();

                dr["Planta"] = e.Planta;
                dr["TipoObjeto"] = e.TipoObjeto;
                dr["IdWorkCenter"] = e.IdWorkCenter;
                dr["CodWorkCenter"] = e.CodWorkCenter;
                dr["Categoria"] = e.Categoria;
                dr["keyValorStandar"] = e.keyValorStandar;
                dr["Administrador"] = e.Administrador;
                dr["FocusFactory"] = e.FocusFactory;
                dr["keyValordeOperacion"] = e.keyValordeOperacion;
                dr["InicioVigencia"] = e.InicioVigencia;
                dr["FinVigencia"] = e.FinVigencia;
                dr["Lenguaje"] = e.Lenguaje;
                dr["Descripcion"] = e.Descripcion;
                dr["FechaUpdate"] = fec;
                dr["Id"] = cnt;

                dt.Rows.Add(dr);
                cnt = cnt + 1;
            }

            resultDelete = datosSql.VaciarTabla(cnxSql, "CatWorkCenter", pathLog);

            if (resultDelete > 0)
                result = datosSql.GuardarTablaSap(cnxSql, dt, nameTabla, pathLog);
            else
                result = -99;

            return result;
        }

        public int GuardarCatEESql(string cnxSql, List<EstruturaEquipo> lstEE, string nameTabla, string pathLog)
        {
            int result = 0;
            int resultDelete = 0;
            DataTable dt = null;

            string cmdSql = " SELECT * ";
            cmdSql = cmdSql + "  FROM [CatEstrucEquipos] ";
            cmdSql = cmdSql + " Where FuncionalLoc = '9999' ";

            OperDatosSql datosSql = new OperDatosSql();
            DateTime fec = DateTime.Now;

            dt = datosSql.GetStructTabla(cnxSql, cmdSql);
            int x = 1;
            foreach (EstruturaEquipo e in lstEE)
            {
                DataRow dr = dt.NewRow();

                dr["FuncionalLoc"] = e.FuncionalLoc;
                dr["Descripcion"] = e.Descripcion;
                dr["LanguageKey"] = e.LanguageKey;
                dr["SuperiorFuncLoc"] = e.SuperiorFuncLoc;
                dr["TipoObjectoWc"] = e.TipoObjectoWc;
                dr["IdObjeto"] = e.IdObjeto;
                dr["LocationAccount"] = e.LocationAccount;
                dr["PlantaManto"] = e.PlantaManto;
                dr["AreaControling"] = e.AreaControling;
                dr["CentroCosto"] = e.CentroCosto;
                dr["IdObjectPPWc"] = e.IdObjectPPWc;
                dr["Propietario"] = e.Propietario;
                dr["CodEquipo"] = e.CodEquipo;
                dr["PmObjType"] = e.PmObjType;
                dr["IdObjectWc"] = e.IdObjectWc;
                dr["Equipodesc"] = e.Equipodesc;
                dr["IdEquipo"] = e.IdEquipo;
                dr["EquipoStatus"] = e.EquipoStatus;
                dr["EquipoValidFromDate"] = string.IsNullOrEmpty(e.EquipoValidFromDate) ? "" : e.EquipoValidFromDate;
                dr["MainWorkCenter"] = e.MainWorkCenter;
                dr["WorkCenter"] = e.WorkCenter;
                dr["TypeTechObj"] = e.TypeTechObj;
                dr["PlannerGroup"] = e.PlannerGroup;
                dr["FecUpdate"] = fec;
                dr["Id"] = x;

                dt.Rows.Add(dr);
                x = x + 1;
            }



            resultDelete = datosSql.VaciarTabla(cnxSql, "dbo." + nameTabla, pathLog);
            if (resultDelete > 0)
                result = datosSql.GuardarTablaSap(cnxSql, dt, nameTabla, pathLog);
            else
                result = -99;

            return result;
        }

        public int GuardarCatFLSql(string cnxSql, List<TblSap_IFLO> lstFl, string nameTabla, string pathLog)
        {
            int result = 0;
            int resultDelete = 0;
            DataTable dt = null;

            string cmdSql = " SELECT * ";
            cmdSql = cmdSql + "  FROM [" + nameTabla + "]";
            cmdSql = cmdSql + " Where FunctionalLocation = '9999' ";

            OperDatosSql datosSql = new OperDatosSql();
            DateTime fec = DateTime.Now;

            dt = datosSql.GetStructTabla(cnxSql, cmdSql);
            int cnt = 1;
            foreach (TblSap_IFLO e in lstFl)
            {
                DataRow dr = dt.NewRow();

                dr["FunctionalLocation"] = e.FunctionalLocation;
                dr["LanguageKey"] = e.LanguageKey;
                dr["Description"] = e.Description;
                dr["SuperiorFunctLoc"] = e.SuperiorFunctLoc;
                dr["ObjectTypeWorkCenter"] = e.ObjectTypeWorkCenter;
                dr["ObjectNumber"] = e.ObjectNumber;
                dr["MaintPlanningPlant"] = e.MaintPlanningPlant;
                dr["ControllingArea"] = e.ControllingArea;
                dr["CostCenter"] = e.CostCenter;
                dr["PlannerGroup"] = e.PlannerGroup;
                dr["ObjectIDWorkCenter"] = e.ObjectIDWorkCenter;
                dr["Id"] = cnt;
                dr["fecUpdate"] = fec;
                dt.Rows.Add(dr);
                cnt = cnt + 1;

            }

            resultDelete = datosSql.VaciarTabla(cnxSql, nameTabla, pathLog);
            if (resultDelete > 0)
                result = datosSql.GuardarTablaSap(cnxSql, dt, nameTabla, pathLog);
            else
                result = -99;

            return result;
        }

        public PlanMantto GetPlanManto(string cnxSql, int pId)
        {
            PlanMantto dPm = new PlanMantto();
            List<DataRow> lstDatos = null;

            string cmdSql = "  SELECT id , CodEquipo, CodSistema, CodCiclo, Frecuencia, FechaAlta, UsuarioAlta, FechaCancelacion, UsuarioCancelo, Estatus, FecUltEjecucion ";
            cmdSql = cmdSql + " FROM[CatPlanesMantto]";
            cmdSql = cmdSql + "Where id = @Id ";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@Id", pId);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];

                dPm.Id = Convert.ToInt32(dr["Id"]);
                dPm.CodEquipo = dr["CodEquipo"].ToString();
                dPm.CodSistema = dr["CodSistema"].ToString();
                dPm.CodCiclo = dr["CodCiclo"].ToString();
                dPm.Frecuencia = Convert.ToInt32(dr["Frecuencia"]);
                dPm.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                dPm.UsuarioAlta = dr["UsuarioAlta"].ToString();
                if (dr["FechaCancelacion"].ToString() != "") { dPm.FechaCancelacion = Convert.ToDateTime(dr["FechaCancelacion"]); }

                dPm.UsuarioCancelo = dr["UsuarioCancelo"].ToString();
                dPm.Estatus = Convert.ToBoolean(dr["Estatus"]);
                dPm.FecUltEjecucion = Convert.ToDateTime(dr["FecUltEjecucion"]);
            }
            return dPm;
        }

        public SistemaManto GetSistMantto(string cnxSql, int pId, string rutaLog)
        {
            SistemaManto dSm = new SistemaManto();
            List<DataRow> lstDatos = null;

            string cmdSql = " SELECT Id, CodSistema, Sistema, CodDepartamento, Estatus ";
            cmdSql = cmdSql + " FROM [CatSistemasEquipos] ";
            cmdSql = cmdSql + "Where Id = @Id";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@Id", pId);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];

                dSm.Id = dr["Id"].ToString(); ;
                dSm.CodSistema = dr["CodSistema"].ToString();
                dSm.Sistema = dr["Sistema"].ToString();
                dSm.CodDepartamento = dr["CodDepartamento"].ToString();
                dSm.Estatus = Convert.ToBoolean(dr["Estatus"]);

            }
            return dSm;
        }

        public List<Ticket> GetUltimaFalla(string cnxSql, string rutaLog, string depto, int planta)
        {
            List<DataRow> lstDr = null;
            List<Ticket> lstDatos = new List<Ticket>();

            // Query para obtener la ultima falla por equipo
            string cmdSql = @" SELECT t.IdPlanta, j.IdTicket, t.TipoTicket, t.CodDepartamento, t.CodWorkCenter, t.CodEquipo, t.CodSubEquipo, t.FallaReportada, t.FchReporte, t.UsuarioReporto, t.Diagnostico, t.CodClasif,
         t.CodSistema, t.CodFalla, t.Falla, t.CodTipoFalla, t.CodStatus, t.FecStatus, t.UsrAtendio, t.AccionPlan, t.FchAccionplan, t.WorkerAsignado, t.FecIniReparacion, FecEntgregaReparacion,
         t.CodTipoWom, t.WOM, t.FchaPromesa, t.FchClose, t.PCReporto, t.Notas, t.CausoParo, HallazgoSeguridad, Sensor
         FROM [Tickets] t ,
               (SELECT count(t.IdTicket) as Idticket, t.CodEquipo
			   FROM[Tickets] t, [CatStsTickets] s
                     Where t.CodStatus = s.codStatus and s.ActivoAbierto = 1
	                  group by CodEquipo) j
         Where t.CodEquipo = j.CodEquipo and t.CodDepartamento = '" + depto + "' and t.IdPlanta = " + planta.ToString();


            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDr)
            {
                Ticket e = new Ticket();

                e.IdPlanta = (int)dr["IdPlanta"];
                e.IdTicket = (int)dr["IdTicket"];
                e.TipoTicket = dr["TipoTicket"].ToString();
                e.CodDepartamento = dr["CodDepartamento"].ToString();
                e.CodWorkCenter = dr["CodWorkCenter"].ToString();
                e.CodEquipo = dr["CodEquipo"].ToString();
                e.CodSubEquipo = dr["CodSubEquipo"].ToString();
                e.FallaReportada = dr["FallaReportada"].ToString();
                e.UsuarioReporto = dr["UsuarioReporto"].ToString();
                e.Diagnostico = dr["Diagnostico"].ToString();
                e.CodClasif = dr["CodClasif"].ToString();
                e.CodSistema = dr["CodSistema"].ToString();
                e.CodFalla = dr["CodFalla"].ToString();
                e.Falla = dr["Falla"].ToString();
                e.CodTipoFalla = dr["CodTipoFalla"].ToString();
                e.CodStatus = dr["CodStatus"].ToString();
                e.UsrAtendio = dr["UsrAtendio"].ToString();
                e.AccionPlan = dr["AccionPlan"].ToString();
                e.WorkerAsignado = dr["WorkerAsignado"].ToString();
                e.CodTipoWom = dr["CodTipoWom"].ToString();
                e.WOM = dr["WOM"].ToString();
                e.PCReporto = dr["PCReporto"].ToString();
                e.Notas = dr["Notas"].ToString();
                e.CausoParo = dr["CausoParo"].ToString();
                e.HallazgoSeguridad = dr["HallazgoSeguridad"].ToString();
                e.Sensor = dr["Sensor"].ToString();

                if (dr["FchReporte"].ToString() != "") { e.FchReporte = Convert.ToDateTime(dr["FchReporte"]); }
                if (dr["FecStatus"].ToString() != "") { e.FecStatus = Convert.ToDateTime(dr["FecStatus"]); }
                if (dr["FchAccionplan"].ToString() != "") { e.FchAccionplan = Convert.ToDateTime(dr["FchAccionplan"]); }
                if (dr["FecIniReparacion"].ToString() != "") { e.FecIniReparacion = Convert.ToDateTime(dr["FecIniReparacion"]); }
                if (dr["FecEntgregaReparacion"].ToString() != "") { e.FecEntgregaReparacion = Convert.ToDateTime(dr["FecEntgregaReparacion"]); }
                if (dr["FchaPromesa"].ToString() != "") { e.FchaPromesa = Convert.ToDateTime(dr["FchaPromesa"]); }
                if (dr["FchClose"].ToString() != "") { e.FchClose = Convert.ToDateTime(dr["FchClose"]); }

                lstDatos.Add(e);
            }

            return (lstDatos);

        }

        public List<CantFallasEquipo> GetFallasxEquipo(string cnxSql, int mesesFallas, string rutaLog)
        {
            List<DataRow> lstDr = null;
            List<CantFallasEquipo> lstDatos = new List<CantFallasEquipo>();

            // Query para obtener la ultima falla por equipo menos los de mantenimiento
            string cmdSql = @"   SELECT  t.CodWorkCenter, t.CodEquipo, COUNT(t.IdTicket) as NumFallas
                              FROM [Tickets] t 
                              Where Convert(date,t.FchReporte) >= Convert(date,DATEADD(mm, @meses, GETDATE()))
		                              and Convert(date, t.FchReporte) <= Convert(date,GETDATE())
                                    and t.TipoTicket <> 'M'                                   
		                        group by t.CodWorkCenter, t.CodEquipo
		                        order by t.CodWorkCenter ";


            //string cmdSql = @"   SELECT  t.CodWorkCenter, t.CodEquipo, COUNT(t.IdTicket) as NumFallas
            //                  FROM [Tickets] t 
            //                  Where Convert(date,t.FchReporte) >= Convert(date,DATEADD(mm, @meses, GETDATE()))
            //                    and Convert(date, t.FchReporte) <= Convert(date,GETDATE())
            //                        and t.TipoTicket <> 'M'
            //              group by t.CodWorkCenter, t.CodEquipo
            //              order by t.CodWorkCenter ";


            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@meses", mesesFallas);

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            foreach (DataRow dr in lstDr)
            {
                CantFallasEquipo e = new CantFallasEquipo();


                e.CodWorkCenter = dr["CodWorkCenter"].ToString();
                e.CodEquipo = dr["CodEquipo"].ToString();
                e.NumFallas = (int)dr["NumFallas"];



                lstDatos.Add(e);
            }

            return (lstDatos);

        }

        public List<CantFallasEquipo> GetFallasSeguridad(string cnxSql, string rutaLog)
        {
            List<DataRow> lstDr = null;
            List<CantFallasEquipo> lstDatos = new List<CantFallasEquipo>();

            // Query para obtener los tickets abiertos clasificados como hallazgos de seguridad
            string cmdSql = @"SELECT t.CodWorkCenter, t.CodEquipo, COUNT(t.IdTicket) as Hallazgos
                           FROM [Tickets] t
                           inner join  [CatStsTickets] s on  s.CodStatus = t.CodStatus
                           Where HallazgoSeguridad = 'SI' and s.ActivoAbierto = 1
                           group by t.CodWorkCenter,  t.CodEquipo";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDr)
            {
                CantFallasEquipo e = new CantFallasEquipo();


                e.CodWorkCenter = dr["CodWorkCenter"].ToString();
                e.CodEquipo = dr["CodEquipo"].ToString();
                e.NumFallas = (int)dr["Hallazgos"];

                lstDatos.Add(e);
            }

            return (lstDatos);

        }
        public List<Ticket> ListaTickxEqu(string cnxSql, string rutaLog, string pCodEquipo, string pCodWorkCenter, bool pStatusTicket)
        {
            List<DataRow> lstDr = null;
            List<Ticket> lstTickets = new List<Ticket>();
            string cmdSql = "";

            if (pStatusTicket)
            {

                cmdSql = @" SELECT t.IdPlanta, t.IdTicket, t.TipoTicket, t.CodDepartamento, t.CodWorkCenter, t.CodEquipo,t.CodSubEquipo, t.FallaReportada, t.FchReporte, t.UsuarioReporto, t.Diagnostico, 
                     t.CodClasif, t.CodSistema, t.CodFalla, t.Falla, t.CodTipoFalla, t.CodStatus, t.FecStatus, t.UsrAtendio, t.AccionPlan, t.FchAccionplan, t.WorkerAsignado,
                     t.FecIniReparacion, FecEntgregaReparacion, t.CodTipoWom, t.WOM, t.FchaPromesa, t.FchClose, t.PCReporto, t.Notas, t.CausoParo, HallazgoSeguridad, Sensor
                     FROM [Tickets] t, 
                          [CatStsTickets] s
                     Where t.CodStatus = s.codStatus 
                        and s.ActivoAbierto = @StatusTicket
                        and t.CodEquipo = @pCodEquipo";
            }
            else
            {

                cmdSql = @" SELECT t.IdPlanta, t.IdTicket, t.TipoTicket, t.CodDepartamento, t.CodWorkCenter, t.CodEquipo,t.CodSubEquipo, t.FallaReportada, t.FchReporte, t.UsuarioReporto, t.Diagnostico, 
                     t.CodClasif, t.CodSistema, t.CodFalla, t.Falla, t.CodTipoFalla, t.CodStatus, t.FecStatus, t.UsrAtendio, t.AccionPlan, t.FchAccionplan, t.WorkerAsignado,
                     t.FecIniReparacion, FecEntgregaReparacion, t.CodTipoWom, t.WOM, t.FchaPromesa, t.FchClose, t.PCReporto, t.Notas, t.CausoParo, HallazgoSeguridad, Sensor
                     FROM [Tickets] t, 
                          [CatStsTickets] s
                     Where t.CodStatus = s.codStatus 
                        and t.CodEquipo = @pCodEquipo";
            }


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdSql;
            cmd.Parameters.AddWithValue("@StatusTicket", pStatusTicket);
            cmd.Parameters.AddWithValue("@pCodEquipo", pCodEquipo);

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatosConParam(cnxSql, cmd);

            foreach (DataRow dr in lstDr)
            {
                Ticket e = new Ticket();

                e.IdPlanta = (int)dr["IdPlanta"];
                e.IdTicket = (int)dr["IdTicket"];
                e.TipoTicket = dr["TipoTicket"].ToString();
                e.CodDepartamento = dr["CodDepartamento"].ToString();
                e.CodWorkCenter = dr["CodWorkCenter"].ToString();
                e.CodEquipo = dr["CodEquipo"].ToString();
                e.CodSubEquipo = dr["CodSubEquipo"].ToString();
                e.FallaReportada = dr["FallaReportada"].ToString();
                e.UsuarioReporto = dr["UsuarioReporto"].ToString();
                e.Diagnostico = dr["Diagnostico"].ToString();
                e.CodClasif = dr["CodClasif"].ToString();
                e.CodSistema = dr["CodSistema"].ToString();
                e.CodFalla = dr["CodFalla"].ToString();
                e.Falla = dr["Falla"].ToString();
                e.CodTipoFalla = dr["CodTipoFalla"].ToString();
                e.CodStatus = dr["CodStatus"].ToString();
                e.UsrAtendio = dr["UsrAtendio"].ToString();
                e.AccionPlan = dr["AccionPlan"].ToString();
                e.WorkerAsignado = dr["WorkerAsignado"].ToString();
                e.CodTipoWom = dr["CodTipoWom"].ToString();
                e.WOM = dr["WOM"].ToString();
                e.PCReporto = dr["PCReporto"].ToString();
                e.Notas = dr["Notas"].ToString();
                e.CausoParo = dr["CausoParo"].ToString();
                e.HallazgoSeguridad = dr["HallazgoSeguridad"].ToString();
                e.Sensor = dr["Sensor"].ToString();

                if (dr["FchReporte"].ToString() != "") { e.FchReporte = Convert.ToDateTime(dr["FchReporte"]); }
                if (dr["FecStatus"].ToString() != "") { e.FecStatus = Convert.ToDateTime(dr["FecStatus"]); }
                if (dr["FchAccionplan"].ToString() != "") { e.FchAccionplan = Convert.ToDateTime(dr["FchAccionplan"]); }
                if (dr["FecIniReparacion"].ToString() != "") { e.FecIniReparacion = Convert.ToDateTime(dr["FecIniReparacion"]); }
                if (dr["FecEntgregaReparacion"].ToString() != "") { e.FecEntgregaReparacion = Convert.ToDateTime(dr["FecEntgregaReparacion"]); }
                if (dr["FchaPromesa"].ToString() != "") { e.FchaPromesa = Convert.ToDateTime(dr["FchaPromesa"]); }
                if (dr["FchClose"].ToString() != "") { e.FchClose = Convert.ToDateTime(dr["FchClose"]); }

                lstTickets.Add(e);
            }

            return (lstTickets);

        }

        public int GuardarTicket(string cnxSql, Ticket datoTick, string pRutaLog, bool todo)
        {
            int result = 0;
            if (!todo)
            {
                // ticket nuevo
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @" insert into [Tickets] 
                              ( IdPlanta,  TipoTicket, CodDepartamento,  CodWorkCenter,  CodEquipo, CodSubEquipo,   CodSistema, FallaReportada, 
                              FchReporte,  UsuarioReporto,  CodStatus,  FecStatus,  CodFalla, HallazgoSeguridad, Sensor, CentroCostos)
                              Values(@IdPlanta, @TipoTicket, @CodDepartamento, @CodWorkCenter, @CodEquipo,@CodSubEquipo, @CodSistema, @FallaReportada, 
                              @FchReporte, @UsuarioReporto, @CodStatus, @FecStatus, @CodFalla, @HallazgoSeguridad, @Sensor, @CtroCostos)";

                cmd.Parameters.AddWithValue("@IdPlanta", datoTick.IdPlanta);
                cmd.Parameters.AddWithValue("@CodDepartamento", datoTick.CodDepartamento);
                cmd.Parameters.AddWithValue("@CodWorkCenter", datoTick.CodWorkCenter);
                cmd.Parameters.AddWithValue("@CodEquipo", datoTick.CodEquipo);
                cmd.Parameters.AddWithValue("@CodSubEquipo", datoTick.CodSubEquipo);
                cmd.Parameters.AddWithValue("@CodSistema", datoTick.CodSistema);
                cmd.Parameters.AddWithValue("@FallaReportada", datoTick.FallaReportada);
                cmd.Parameters.AddWithValue("@FchReporte", datoTick.FchReporte);
                cmd.Parameters.AddWithValue("@UsuarioReporto", datoTick.UsuarioReporto);
                cmd.Parameters.AddWithValue("@CodStatus", datoTick.CodStatus);
                cmd.Parameters.AddWithValue("@FecStatus", datoTick.FecStatus);
                cmd.Parameters.AddWithValue("@CodFalla", datoTick.CodFalla);
                cmd.Parameters.AddWithValue("@TipoTicket", datoTick.TipoTicket);
                cmd.Parameters.AddWithValue("@HallazgoSeguridad", datoTick.HallazgoSeguridad);
                cmd.Parameters.AddWithValue("@Sensor", datoTick.Sensor);
                cmd.Parameters.AddWithValue("@CtroCostos", datoTick.CentroCostos);
                OperDatosSql operDatos = new OperDatosSql();
                result = operDatos.Guardar(cnxSql, cmd);
            }
            else
            {
                // Edicion de un ticket
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = @" Update [Tickets]
                                 Set 
                                 Diagnostico = @Diagnostico,         
                                 CodClasif   = @CodClasif,     
                                 CodSistema	= @CodSistema,                              
                                 CodFalla		= @CodFalla,            
                                 CausoParo   = @CausoParo,  
                                 TiempoReparacion  = @TiempoReparacion,                                   
                                 UnidadTiempoRep   = @UnidadTiempoRep,   
                                 UsrAtendio  = @UsrAtendio,     
                                 FchAtendido	= @FchAtendido,                          
                                 CodStatus	= @CodStatus,           
                                 FecStatus   = @FecStatus,   
                                 AccionPlan  = @AccionPlan,
                                 FchAccionplan	= @FchAccionplan, 
                                 WorkerAsignado = @WorkerAsignado,    
                                 UsrAsigno      = @UsrAsigno,                                    
                                 FchAsignacion	= @FchAsignacion, 
                                 FecIniReparacion      = @FecIniReparacion,
                                 FecEntgregaReparacion = @FecEntgregaReparacion, 
                                 UsrEntrReparacion = @UsrEntrReparacion,
                                 WOM			 = @WOM, 
                                 UsrAsignoWom = @UsrAsignoWom, 
                                 FchWom	    = @FchWom,
                                 FchaPromesa	 = @FchaPromesa, 
                                 FchClose     = @FchClose, 
                                 UsrCerroTicket = @UsrCerroTicket,
                                 Notas			= @Notas
                                 Where IdTicket = @IdTicket 
                                 and IdPlanta = @IdPlanta";

                cmd.Parameters.AddWithValue("@IdPlanta", datoTick.IdPlanta);
                cmd.Parameters.AddWithValue("@IdTicket", datoTick.IdTicket);
                cmd.Parameters.AddWithValue("@Diagnostico", datoTick.Diagnostico);
                cmd.Parameters.AddWithValue("@CodClasif", datoTick.CodClasif);
                cmd.Parameters.AddWithValue("@CodSistema", datoTick.CodSistema);
                cmd.Parameters.AddWithValue("@CodFalla", datoTick.CodFalla);

                cmd.Parameters.AddWithValue("@CausoParo", datoTick.CausoParo);
                cmd.Parameters.AddWithValue("@TiempoReparacion", datoTick.TiempoReparacion);
                cmd.Parameters.AddWithValue("@UnidadTiempoRep", datoTick.UnidadTiempoRep);
                cmd.Parameters.AddWithValue("@UsrAtendio", datoTick.UsrAtendio);
                cmd.Parameters.AddWithValue("@FchAtendido", (datoTick.FchAtendido.ToString("dd/MM/yyyy") == "01/01/0001") ? (object)DBNull.Value : datoTick.FchAtendido);

                cmd.Parameters.AddWithValue("@CodStatus", datoTick.CodStatus);
                cmd.Parameters.AddWithValue("@FecStatus", DateTime.Now);
                cmd.Parameters.AddWithValue("@AccionPlan", datoTick.AccionPlan.Trim());
                cmd.Parameters.AddWithValue("@FchAccionplan", (datoTick.FchAccionplan.ToString("dd/MM/yyyy") == "01/01/0001") ? (object)DBNull.Value : datoTick.FchAccionplan);

                cmd.Parameters.AddWithValue("@WorkerAsignado", datoTick.WorkerAsignado);
                cmd.Parameters.AddWithValue("@UsrAsigno", string.IsNullOrEmpty(datoTick.UsrAsigno) ? "" : datoTick.UsrAsigno);
                cmd.Parameters.AddWithValue("@FchAsignacion", (datoTick.FchAsignacion.ToString("dd/MM/yyyy") == "01/01/0001") ? (object)DBNull.Value : datoTick.FchAsignacion);

                cmd.Parameters.AddWithValue("@FecIniReparacion", (datoTick.FecIniReparacion.ToString("dd/MM/yyyy") == "01/01/0001") ? (object)DBNull.Value : datoTick.FecIniReparacion);
                cmd.Parameters.AddWithValue("@FecEntgregaReparacion", (datoTick.FecEntgregaReparacion.ToString("dd/MM/yyyy") == "01/01/0001") ? (object)DBNull.Value : datoTick.FecEntgregaReparacion);
                cmd.Parameters.AddWithValue("@UsrEntrReparacion", string.IsNullOrEmpty(datoTick.UsrEntrReparacion) ? "" : datoTick.UsrEntrReparacion);

                cmd.Parameters.AddWithValue("@WOM", datoTick.WOM);
                cmd.Parameters.AddWithValue("@UsrAsignoWom", string.IsNullOrEmpty(datoTick.UsrAsignoWom) ? "" : datoTick.UsrAsignoWom);
                cmd.Parameters.AddWithValue("@FchWom", (datoTick.FchWom.ToString("dd/MM/yyyy") == "01/01/0001") ? (object)DBNull.Value : datoTick.FchWom);

                cmd.Parameters.AddWithValue("@FchaPromesa", (datoTick.FchaPromesa.ToString("dd/MM/yyyy") == "01/01/0001") ? (object)DBNull.Value : datoTick.FchaPromesa);
                cmd.Parameters.AddWithValue("@FchClose", (datoTick.FchClose.ToString("dd/MM/yyyy") == "01/01/0001") ? (object)DBNull.Value : datoTick.FchClose);
                cmd.Parameters.AddWithValue("@UsrCerroTicket", string.IsNullOrEmpty(datoTick.UsrCerroTicket) ? "" : datoTick.UsrCerroTicket);
                cmd.Parameters.AddWithValue("@Notas", datoTick.Notas);

                OperDatosSql operDatos = new OperDatosSql();
                result = operDatos.Guardar(cnxSql, cmd);
            }
            return result;
        }

        public List<CatStatusTicket> GetCatStatusTickets(string cnxSql)
        {
            List<DataRow> lstDr = null;
            List<CatStatusTicket> lstDatos = new List<CatStatusTicket>();

            string cmdSql = @" SELECT CodStatus, Descrip, ActivoAbierto
                             FROM[CatStsTickets]
                             Where Estatus = 1";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDr)
            {
                CatStatusTicket st = new CatStatusTicket();

                st.CodStatus = dr["CodStatus"].ToString(); ;
                st.Descrip = dr["Descrip"].ToString();
                st.ActivoAbierto = (bool)dr["ActivoAbierto"];

                lstDatos.Add(st);
            }

            return (lstDatos);

        }

        public string LeeTipoTicketDesc(string cnxSql, string tipoTick)
        {
            List<DataRow> lstDr = null;
            string DatoDescrip = "S/Dato";


            string cmdSql = @" SELECT Descrip FROM [CatTipoFallas] Where CodTipoFalla = '" + tipoTick + "'";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDr.Count > 0)
            {
                DataRow dr = lstDr[0];
                DatoDescrip = dr["Descrip"].ToString();
            }
            return (DatoDescrip);

        }

        public Ticket GetDatosTicket(string cnxSql, int idTick, string rutaLog)
        {
            DataRow dr = null;
            Ticket DatosTicket = new Ticket();
            string cmdSql = "";


            // Query para obteners la ultima falla por equipo
            cmdSql = @"SELECT t.IdPlanta, p.Planta, t.IdTicket, t.TipoTicket, t.CodDepartamento, d.Descrip as Depto, t.CodWorkCenter, t.CodEquipo, t.CodSubEquipo, 
                           e.DescripTechnical as SubEquipo, t.FallaReportada, t.FchReporte, t.UsuarioReporto, t.Diagnostico, isnull(t.CodClasif, '') as CodClasif, 
                           isnull(c.Descripcion,'') as ClasifDescrip, t.CodSistema, s.Sistema, t.CodFalla, isnull(f.Descrip,'') as ClasifFalla, t.Falla, 
                           isnull(t.CodTipoFalla,'') as CodTipoFalla, isnull(tf.Descrip,'') as tipoFalla, t.CodStatus, st.Descrip as status, t.FecStatus, 
                           t.UsrAtendio, t.FchAtendido, t.AccionPlan, t.FchAccionplan, t.WorkerAsignado, t.UsrAsigno, t.FchAsignacion,   t.FecIniReparacion, 
                           FecEntgregaReparacion, t.UsrEntrReparacion, t.CodTipoWom, isnull(tw.Descrip,'') as CodTipoWomDescrip, t.WOM, t.FchWom, t.UsrAsignoWom, t.FchaPromesa, 
                           t.FchClose, t.UsrCerroTicket, t.PCReporto, t.Notas, t.CausoParo, t.HallazgoSeguridad, t.Sensor ,
                           isnull(t.TiempoReparacion,0) as TiempoReparacion, t.UnidadTiempoRep, isnull(cl.Descripcion,'') as DescripcionUT
                           FROM [Tickets] t
                           inner join [CatPlantas] p on p.IdPlanta = t.IdPlanta
                           inner join [CatDepartamentos] d on d.CodDepartamento = t.CodDepartamento and d.IdPlanta = t.IdPlanta
                           inner join [CatEquipos] e on e.CodEquipo = t.CodSubEquipo
                           inner join [CatSistemasEquipos] s on s.CodSistema = t.CodSistema and s.CodDepartamento = t.CodDepartamento
                           inner join [CatStsTickets] st on st.CodStatus = t.CodStatus
                           left join [CatFallas] f on f.CodFalla = t.CodFalla and f.CodDepartamento = t.CodDepartamento                 
                           left join [CatClasifFallas] c on c.CodClasif = t.CodClasif 
                           left join [CatTipoFallas] tf on tf.CodTipoFalla = t.CodTipoFalla
                           left join [CatTipoWom] tw on tw.CodTipoWom = t.CodTipoWom
                           left join [CatCiclos] cl on cl.CodCiclo = t.UnidadTiempoRep
                           Where t.IdTicket = " + idTick.ToString();

            OperDatosSql datosSql = new OperDatosSql();

            dr = datosSql.LeeDato(cnxSql, cmdSql);

            Ticket tick = new Ticket();
            if (dr != null)
            {
                tick.IdPlanta = (int)dr["IdPlanta"];
                tick.Planta = dr["Planta"].ToString();
                tick.IdTicket = (int)dr["IdTicket"];
                tick.TipoTicket = dr["TipoTicket"].ToString();
                tick.CodDepartamento = dr["CodDepartamento"].ToString();
                tick.Depto = dr["Depto"].ToString();
                tick.CodWorkCenter = dr["CodWorkCenter"].ToString();
                tick.CodEquipo = dr["CodEquipo"].ToString();
                tick.CodSubEquipo = dr["CodSubEquipo"].ToString();
                tick.CodEquipoDescrip = dr["SubEquipo"].ToString();
                tick.FallaReportada = dr["FallaReportada"].ToString();
                tick.UsuarioReporto = dr["UsuarioReporto"].ToString();

                tick.Diagnostico = dr["Diagnostico"].ToString();
                tick.CodClasif = dr["CodClasif"].ToString();
                tick.ClasifDescrip = dr["ClasifDescrip"].ToString();
                tick.CodSistema = dr["CodSistema"].ToString();
                tick.CodSistemaDescrip = dr["Sistema"].ToString();
                tick.CodFalla = dr["CodFalla"].ToString();
                tick.CodFallaDescrip = dr["ClasifFalla"].ToString();
                tick.CodTipoFalla = dr["CodTipoFalla"].ToString();
                tick.CodTipoFallaDescrip = dr["TipoFalla"].ToString();
                tick.CodStatus = dr["CodStatus"].ToString();
                tick.CodStatusDescrip = dr["Status"].ToString();

                tick.UsrAtendio = dr["UsrAtendio"].ToString();

                tick.AccionPlan = dr["AccionPlan"].ToString();
                tick.WorkerAsignado = dr["WorkerAsignado"].ToString();
                tick.UsrAsigno = dr["UsrAsigno"].ToString();
                tick.UsrEntrReparacion = dr["UsrEntrReparacion"].ToString();

                tick.CodTipoWom = dr["CodTipoWom"].ToString();
                tick.CodTipoWomDescrip = dr["CodTipoWomDescrip"].ToString();
                tick.WOM = dr["WOM"].ToString();
                tick.UsrAsignoWom = dr["UsrAsignoWom"].ToString();
                tick.UsrCerroTicket = dr["UsrCerroTicket"].ToString();

                tick.PCReporto = dr["PCReporto"].ToString();
                tick.Notas = dr["Notas"].ToString();
                tick.CausoParo = dr["CausoParo"].ToString();
                tick.HallazgoSeguridad = dr["HallazgoSeguridad"].ToString();
                tick.Sensor = dr["Sensor"].ToString();
                tick.TiempoReparacion = (int)dr["TiempoReparacion"];
                tick.UnidadTiempoRep = dr["UnidadTiempoRep"].ToString();
                tick.DescripcionUT = dr["DescripcionUT"].ToString();

                if (dr["FchReporte"].ToString() != "") { tick.FchReporte = Convert.ToDateTime(dr["FchReporte"]); }
                if (dr["FecStatus"].ToString() != "") { tick.FecStatus = Convert.ToDateTime(dr["FecStatus"]); }
                if (dr["FchAsignacion"].ToString() != "") { tick.FchAsignacion = Convert.ToDateTime(dr["FchAsignacion"]); }
                if (dr["FchAtendido"].ToString() != "") { tick.FchAtendido = Convert.ToDateTime(dr["FchAtendido"]); }
                if (dr["FchAsignacion"].ToString() != "") { tick.FchAsignacion = Convert.ToDateTime(dr["FchAsignacion"]); }
                if (dr["FchWom"].ToString() != "") { tick.FchWom = Convert.ToDateTime(dr["FchWom"]); }

                if (dr["FchAccionplan"].ToString() != "") { tick.FchAccionplan = Convert.ToDateTime(dr["FchAccionplan"]); }
                if (dr["FecIniReparacion"].ToString() != "") { tick.FecIniReparacion = Convert.ToDateTime(dr["FecIniReparacion"]); }
                if (dr["FecEntgregaReparacion"].ToString() != "") { tick.FecEntgregaReparacion = Convert.ToDateTime(dr["FecEntgregaReparacion"]); }
                if (dr["FchaPromesa"].ToString() != "") { tick.FchaPromesa = Convert.ToDateTime(dr["FchaPromesa"]); }
                if (dr["FchClose"].ToString() != "") { tick.FchClose = Convert.ToDateTime(dr["FchClose"]); }

            }

            return (tick);

        }

        public string GetDescripEquipo(string cnxSql, string codigo)
        {
            List<DataRow> lstDr = null;
            string DatoDescrip = "S/Dato";


            string cmdSql = @" SELECT CodEquipo, Equipodesc , IdEquipo, WorkCenter
                          FROM [CatEstrucEquipos]
                          Where CodEquipo = '" + codigo + "'";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDr.Count >= 1)
            {
                DataRow dr = lstDr[0];
                DatoDescrip = dr["Equipodesc"].ToString();
            }
            return (DatoDescrip);

        }

        public string GetNombreEmpl(string cnxSql, string pNumControl)
        {
            List<DataRow> lstDr = null;
            string DatoNombre = "";

            // Situacion = 1 solo activos
            string cmdSql = @"SELECT Nc, Nombre, Apellidos, Scorecard
                             FROM [Refectorio].[dbo].[EmpleadoAbc]
                             Where Situacion = 1
                             and Nc = '" + pNumControl + "'";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDr.Count >= 1)
            {
                DataRow dr = lstDr[0];
                DatoNombre = dr["Nombre"].ToString() + " " + dr["Apellidos"].ToString();
            }

            return (DatoNombre);

        }

        public List<ProducNotificada> GetProducMatxWc(string cnxSql, string pWc, DateTime fecProduc)
        {
            List<ProducNotificada> lstPNotif = new List<ProducNotificada>();
            List<DataRow> lstDr = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = @"SELECT MATNR as Material, MDV01 as WorkCenter, Sum(WEMNG) as PzsProduc
                              FROM [ProdHist].[dbo].[HtProdxDia]
                                where MDV01 = @Wc
                                and  Convert(date,SPTAG) >= @fec
                                Group by MATNR, MDV01 ";

            cmd.Parameters.AddWithValue("@Wc", pWc);
            cmd.Parameters.AddWithValue("@fec", fecProduc.Date);

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatosConParam(cnxSql, cmd);

            if (lstDr.Count > 0)
            {
                foreach (var t in lstDr)
                {
                    ProducNotificada dato = new ProducNotificada();

                    dato.Material = t["Material"].ToString();
                    dato.WorkCenter = t["WorkCenter"].ToString();
                    // dato.Uom = t["Uom"].ToString();
                    dato.PiezasProducidas = Convert.ToDecimal(t["PzsProduc"]);
                    lstPNotif.Add(dato);
                }
            }
            return (lstPNotif);
        }

        public decimal GetProducMatxWc(string cnxSql, string pWc)
        {
            //List<ProducNotificada> lstPNotif = new List<ProducNotificada>();
            decimal lstPNotif = 0;
            List<DataRow> lstDr = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = @"SELECT isnull(Sum(WEMNG),0) as PzsProduc
                              FROM [ProdHist].[dbo].[HtProdxDia]
                              WHERE MDV01 = @Wc                                
                                ";

            cmd.Parameters.AddWithValue("@Wc", pWc);

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatosConParam(cnxSql, cmd);

            if (lstDr.Count > 0)
            {
                foreach (var t in lstDr)
                {
                    lstPNotif = Convert.ToDecimal(t["PzsProduc"]);
                }
            }
            return (lstPNotif);
        }

        public DataRow GetDatosUsuario(string cnxSql, UsuarioAcceso usuario, string pCtroCostos)
        {
            List<DataRow> lstDr = null;
            DataRow dr = null;

            // Situacion = 1 solo activos
            string cmdSql = @"SELECT u.Id, u.NumControl, u.Nombre, u.Password, u.StatusEmpTpm, u.ClaveRol, 
                           r.catTpm, r.EditarTicket, r.CatChecklist, r.CapturaChecklist, u.CentroCostos
                           FROM [UsuariosTpm] u
                           inner join [CatRoles] r on r.ClaveRol = u.ClaveRol 
                           Where StatusEmpTpm = 1 and NumControl = " + usuario.NumControl;

            if (pCtroCostos != "")
                cmdSql = cmdSql + " and CentroCostos = '" + pCtroCostos + "'";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDr.Count >= 1)
            {
                dr = lstDr[0];
            }

            return (dr);

        }

        public DataRow GetDatosUsuario(string cnxSql, UsuarioAcceso usuario)
        {
            List<DataRow> lstDr = null;
            DataRow dr = null;

            // Situacion = 1 solo activos
            string cmdSql = @"SELECT u.Id, u.NumControl, u.Nombre, u.Password, u.StatusEmpTpm, u.ClaveRol, 
                           r.catTpm, r.EditarTicket, r.CatChecklist, r.CapturaChecklist, u.CentroCostos
                           FROM [UsuariosTpm] u
                           inner join [CatRoles] r on r.ClaveRol = u.ClaveRol 
                           Where StatusEmpTpm = 1 and NumControl = " + usuario.NumControl; // +
                                                                                           //" and CentroCostos = '" + pCtroCostos + "'";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDr.Count >= 1)
            {
                dr = lstDr[0];
            }

            return (dr);

        }

        public DataRow GetDatosUsuario(string cnxSql, UsuarioTpm usuario)
        {
            List<DataRow> lstDr = null;
            DataRow dr = null;

            // Situacion = 1 solo activos
            string cmdSql = @"SELECT u.Id, u.NumControl, u.Nombre, u.StatusEmpTpm, u.CentroCostos
                           FROM [UsuariosTpm] u
                           Where NumControl = " + usuario.NumControl;
            // + " and CentroCostos = '" + usuario.CentroCostos + "'";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDr.Count >= 1)
            {
                dr = lstDr[0];
            }

            return (dr);

        }

        public List<string> GetEmailTpm(string cnxSql, string rutalog, string pCtroCtos)
        {
            List<DataRow> lstDr = null;
            List<string> lstEm = new List<string>();

            // Situacion = 1 solo activos
            string cmdSql = @"Select Email, Tipo, Status, CentroCostos
                             From [CatEmail]
                             Where  Status = 1
                                 and CentroCostos = '" + pCtroCtos + "'";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDr.Count >= 1)
            {
                foreach (var item in lstDr)
                {
                    string x = item["Email"].ToString();
                    lstEm.Add(x);
                }
            }

            return (lstEm);

        }

        public DatosConfig GetConfigTpm(string cnxSql, string ctroCtos)
        {
            List<DataRow> lstDr = null;
            DatosConfig datos = new DatosConfig();

            string cmdSql = @" SELECT TOP 1 * FROM [ConfigTpm]
                                where StatusConfig  = 1 ";
            if (ctroCtos != "")
                cmdSql = cmdSql + " and Convert(int,CtroCtosSap) = '" + ctroCtos + "'";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDr.Count > 0)
            {
                DataRow dr = lstDr[0];
                datos.CtroCtosSap = dr["CtroCtosSap"].ToString();
                datos.Depto = dr["Depto"].ToString();
                datos.TituCC = dr["TituCC"].ToString();
                datos.TituCA = dr["TituCA"].ToString();
                datos.TituArea = dr["TituArea"].ToString();
                datos.RutaLog = dr["RutaLog"].ToString();
                datos.Planta = (int)dr["Planta"];
                datos.TopRedAcumMtto = (int)dr["TopRedAcumMtto"];
                datos.TopYellowAcumMtto = (int)dr["TopYellowAcumMtto"];
                datos.TopGreenAcumMtto = (int)dr["TopGreenAcumMtto"];
                datos.MesesParaFallas = (int)dr["MesesParaFallas"];
                datos.PrctjParaFallas = (decimal)dr["PrctjParaFallas"];
                datos.EmailTo = dr["EmailTo"].ToString();
                datos.DiasxAno = (decimal)dr["DiasxAno"];
                datos.HrsxDia = (decimal)dr["HrsxDia"];
                datos.DiasxMes = (decimal)dr["DiasxMes"];
                datos.RutaFotosEquipos = dr["RutaFotosEquipos"].ToString();
                datos.WebServer = dr["WebServer"].ToString();
                datos.SvrSqlTpm = dr["SvrSqlTpm"].ToString();
                datos.BDTpm = dr["BDTpm"].ToString();
                datos.UserTPm = dr["UserTPm"].ToString();
                datos.PwdTpm = dr["PwdTpm"].ToString();
                datos.SrvSqlHtProd = dr["SrvSqlHtProd"].ToString();
                datos.BdHtProd = dr["BDHtProd"].ToString();
                datos.UserHtProd = dr["userHtProd"].ToString();
                datos.PwdHtProd = dr["PwdHtProd"].ToString();

                datos.SrvSqlProd = dr["SrvSqlProd"].ToString();
                datos.BdProd = dr["BdProd"].ToString();
                datos.UserProd = dr["userProd"].ToString();
                datos.PwdProd = dr["PwdProd"].ToString();

                datos.SrvSqlEmpl = dr["SrvSqlEmpl"].ToString();
                datos.BdEmpl = dr["BdEmpl"].ToString();
                datos.UserEmpl = dr["UserEmpl"].ToString();
                datos.PwdEmpl = dr["PwdEmpl"].ToString();
                datos.HostIPSap = dr["HostIPSap"].ToString();
                datos.SystemIDSap = dr["SystemIDSap"].ToString();
                datos.SystemNumberSap = dr["SystemNumberSap"].ToString();
                datos.ClientSap = dr["ClientSap"].ToString();
                datos.LanguageSap = dr["LanguageSap"].ToString();
                datos.PoolSizeSap = dr["PoolSizeSap"].ToString();
                datos.UserSap = dr["UserSap"].ToString();
                datos.PwdSap = dr["PwdSap"].ToString();

                datos.UrlfotosUser = dr["UrlfotosUser"].ToString();
                datos.MetaKbmCmpl = (decimal)dr["MetaKbmCmpl"];
                datos.MetaKbmEfic = (decimal)dr["MetaKbmEfic"];
                datos.AdminTpm = dr["AdminTpm"].ToString();
                datos.Emailenvio = dr["Emailenvio"].ToString();
                datos.EmailUser = dr["EmailUser"].ToString();
                datos.EmailPwd = dr["EmailPwd"].ToString();
                datos.EmailServer = dr["EmailServer"].ToString();
                datos.EmailUserTpmMtto = dr["EmailUserTpmMtto"].ToString();
                datos.EmailUserCalidad = dr["EmailUserCalidad"].ToString();
                datos.EmailGerentes = dr["EmailGerentes"].ToString();
                datos.EmailPrensas = dr["EmailPrensas"].ToString();

                datos.Turno1HrIni = (int)dr["Turno1HrIni"];
                datos.Turno1MinIni = (int)dr["Turno1MinIni"];
                datos.Turno1HrFin = (int)dr["Turno1HrFin"];
                datos.Turno1MinFin = (int)dr["Turno1MinFin"];
                datos.Turno2HrIni = (int)dr["Turno2HrIni"];
                datos.Turno2MinIni = (int)dr["Turno2MinIni"];
                datos.Turno2HrFin = (int)dr["Turno2HrFin"];
                datos.Turno2MinFin = (int)dr["Turno2MinFin"];
                datos.Turno3HrIni = (int)dr["Turno3HrIni"];
                datos.Turno3MinIni = (int)dr["Turno3MinIni"];
                datos.Turno3HrFin = (int)dr["Turno3HrFin"];
                datos.Turno3MinFin = (int)dr["Turno3MinFin"];

                datos.MetaMtrrAutoMin = (decimal)dr["MetaMtrrAutoMin"];
                datos.MetaMtbfAutoMin = (decimal)dr["MetaMtbfAutoMin"];
                datos.MetaMtrrMntMin = (decimal)dr["MetaMtrrMntMin"];
                datos.MetaMtbfMntMin = (decimal)dr["MetaMtbfMntMin"];
                datos.MetaMtrrTklMin = (decimal)dr["MetaMtrrTklMin"];
                datos.MetaMtbfTklMin = (decimal)dr["MetaMtbfTklMin"];
            }
            return (datos);
        }

        public int ActualizarCiclo(string pCnxSql, string pCodEquipo, string pCodSistema, DateTime pFecCierre, string pRutaLog)
        {
            List<DataRow> lstDr = null;
            int result = 0;

            string cmdSql = @"SELECT Id,CodEquipo, CodSistema, CodCiclo, Frecuencia, Estatus, FecUltEjecucion, CentroCostos
                          FROM [CatPlanesMantto]
                          Where CodEquipo = '" + pCodEquipo + "' and CodSistema = '" + pCodSistema + "'";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(pCnxSql, cmdSql);

            if (lstDr.Count >= 1)
            {
                string cmdTxt = @"Update [CatPlanesMantto]
                        set FecUltEjecucion = @FchCierre
                        Where CodEquipo = @CodEquipo and CodSistema = @CodSistema";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = cmdTxt;
                cmd.Parameters.AddWithValue("@CodEquipo", pCodEquipo);
                cmd.Parameters.AddWithValue("@CodSistema", pCodSistema);
                cmd.Parameters.AddWithValue("@FchCierre", pFecCierre);


                OperDatosSql operDatos = new OperDatosSql();
                result = operDatos.Update(pCnxSql, cmd, pRutaLog);
            }

            return (result);
        }

        public List<Falla> LeeCatFallas(string cnxSql, string pDepto)
        {
            List<DataRow> lstDr = null;
            List<Falla> lstDatos = new List<Falla>();

            string cmdSql = @"SELECT f.IdFalla, f.CodFalla, f.Descrip, f.CodSistema, f.CodDepartamento, f.Tipo, f.Status, f.FecAlta, f.FecActualizacion,
                        f.UsuarioAlta,  isnull(s.Sistema,'N/A')  as Sistema, isnull(s.Estatus, 0) as StsSistema
                        FROM [CatFallas] f
                        left join [CatSistemasEquipos] s 
                        on s.CodSistema = f.CodSistema and s.CodDepartamento = f.CodDepartamento ";

            if (pDepto != "")
                cmdSql = cmdSql + " Where f.CodDepartamento = '" + pDepto.Trim() + "' ";

            cmdSql = cmdSql + " order by isnull(FecActualizacion, FecAlta) desc";

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDr)
            {
                Falla i = new Falla();

                i.IdFalla = Convert.ToInt32(dr["IdFalla"]);
                i.CodFalla = dr["CodFalla"].ToString();
                i.Descrip = dr["Descrip"].ToString();
                i.CodSistema = dr["CodSistema"].ToString();
                i.Sistema = dr["Sistema"].ToString();
                i.StsSistema = Convert.ToBoolean(dr["StsSistema"]);
                i.CodDepartamento = dr["CodDepartamento"].ToString();
                i.Tipo = dr["Tipo"].ToString();
                i.StatusFalla = Convert.ToBoolean(dr["Status"]);
                if (dr["FecAlta"].ToString() != "") { i.FecAlta = Convert.ToDateTime(dr["FecAlta"]); }
                if (dr["FecActualizacion"].ToString() != "") { i.FecActualizacion = Convert.ToDateTime(dr["FecActualizacion"]); }
                i.UsuarioAlta = dr["UsuarioAlta"].ToString();

                lstDatos.Add(i);
            }

            return (lstDatos);
        }

        public int GuardarFalla(string cnxSql, Falla fallanew, string pDepto)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"INSERT INTO  [CatFallas]
                           (CodFalla, Descrip, CodSistema, CodDepartamento, Tipo, Status, FecAlta, FecActualizacion, UsuarioAlta)
                           VALUES(@CodFalla, @Descrip, @CodSistema, @CodDepartamento, @Tipo, @Status, @FecAlta, @FecActualizacion, @UsuarioAlta)";

            cmd.Parameters.AddWithValue("@CodFalla", fallanew.CodFalla);
            cmd.Parameters.AddWithValue("@Descrip", fallanew.Descrip);
            cmd.Parameters.AddWithValue("@CodSistema", fallanew.CodSistema);
            cmd.Parameters.AddWithValue("@CodDepartamento", fallanew.CodDepartamento);
            cmd.Parameters.AddWithValue("@Tipo", "");
            cmd.Parameters.AddWithValue("@Status", fallanew.StatusFalla);
            cmd.Parameters.AddWithValue("@FecAlta", fallanew.FecAlta);
            cmd.Parameters.AddWithValue("@FecActualizacion", fallanew.FecActualizacion);
            cmd.Parameters.AddWithValue("@UsuarioAlta", fallanew.UsuarioAlta);


            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public Falla GetDatosFalla(string cnxSql, int pId)
        {
            Falla f = new Falla();
            List<DataRow> lstDatos = null;

            string cmdSql = @" SELECT IdFalla, CodFalla, Descrip, CodSistema, CodDepartamento, Tipo, Status, FecAlta, FecActualizacion, UsuarioAlta
                            FROM [CatFallas]
                            Where IdFalla = @pId ";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@pId", pId);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];

                f.IdFalla = Convert.ToInt32(dr["IdFalla"]);
                f.CodFalla = dr["CodFalla"].ToString();
                f.Descrip = dr["Descrip"].ToString();
                f.CodSistema = dr["CodSistema"].ToString();
                f.CodDepartamento = dr["CodDepartamento"].ToString();
                f.StatusFalla = Convert.ToBoolean(dr["Status"]);
                if (dr["FecAlta"].ToString() != "") { f.FecAlta = Convert.ToDateTime(dr["FecAlta"]); }
                if (dr["FecActualizacion"].ToString() != "") { f.FecActualizacion = Convert.ToDateTime(dr["FecActualizacion"]); }
                f.UsuarioAlta = dr["UsuarioAlta"].ToString();
            }

            return f;
        }

        public int UpdateDatoFalla(string cnxSql, Falla df, string pRutaLog)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();

            string cmdTxt = @"UPDATE  [CatFallas]  
                           SET Descrip = @Descrip, Status = @Status, FecActualizacion = @FecActualizacion
                           WHERE IdFalla = @IdFalla";


            cmd.CommandText = cmdTxt;
            cmd.Parameters.AddWithValue("@IdFalla", df.IdFalla);
            cmd.Parameters.AddWithValue("@Descrip", df.Descrip);
            cmd.Parameters.AddWithValue("@Status", df.StatusFalla);
            cmd.Parameters.AddWithValue("@FecActualizacion", df.FecActualizacion);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Update(cnxSql, cmd, pRutaLog);

            return result;
        }

        public List<UsuarioTpm> LeeCatUsuarios(string cnxSql, string pCtroCostos)
        {
            List<DataRow> lstDr = null;
            List<UsuarioTpm> lstDatos = new List<UsuarioTpm>();

            string cmdSql = @"SELECT u.Id, u.NumControl, u.Nombre, u.Password, u.StatusEmpTpm, u.FecAlta, u.Agrego, u.CentroCostos, d.Descrip as DesripCCostos
                           ,isnull(u.ClaveRol,'') as ClaveRol, isnull(r.Descripcion,'') as DescripRol, isnull(r.CatTpm,0) as CatTpm, 
                           isnull(r.EditarTicket,0) as EditarTicket, isnull(r.CatChecklist,0) as CatChecklist,isnull(r.CatChecklist,0) as CatChecklist, 
                           isnull(r.CapturaChecklist,0) as CapturaChecklist,   isnull(r.EstatusRol,0) as EstatusRol
                           FROM [UsuariosTpm] u
                           left join [CatRoles] r on r.ClaveRol = u.ClaveRol
                           left join [CatDepartamentos] d on Convert(int,d.CentroCostos) = Convert(int,u.CentroCostos)
                           Where u.CentroCostos = '" + pCtroCostos + "'";

            OperDatosSql datosSql = new OperDatosSql();
            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);
            foreach (DataRow dr in lstDr)
            {
                UsuarioTpm i = new UsuarioTpm();

                i.Id = Convert.ToInt32(dr["Id"]);
                i.NumControl = dr["NumControl"].ToString();
                i.Nombre = dr["Nombre"].ToString();
                i.Password = dr["Password"].ToString();
                i.StatusEmpTpm = Convert.ToBoolean(dr["StatusEmpTpm"]);
                if (dr["FecAlta"].ToString() != "") { i.FecAlta = Convert.ToDateTime(dr["FecAlta"]); }
                i.Agrego = dr["Agrego"].ToString();
                i.CentroCostos = dr["CentroCostos"].ToString();
                i.DesripCCostos = dr["DesripCCostos"].ToString();
                i.ClaveRol = dr["ClaveRol"].ToString();
                i.DespcripRol = dr["DescripRol"].ToString();
                i.EstatusRol = Convert.ToBoolean(dr["EstatusRol"]);
                i.CatTpm = Convert.ToBoolean(dr["CatTpm"]);
                i.EditarTicket = Convert.ToBoolean(dr["EditarTicket"]);
                i.CatChecklist = Convert.ToBoolean(dr["CatChecklist"]);
                i.CapturaChecklist = Convert.ToBoolean(dr["CapturaChecklist"]);
                lstDatos.Add(i);
            }

            return (lstDatos);
        }

        public List<RolAcceso> LeeCatRolles(string cnxSql)
        {
            List<DataRow> lstDr = null;
            List<RolAcceso> lstRoles = new List<RolAcceso>();

            string cmdSql = @"SELECT rol, ClaveRol, Descripcion, CatTpm, EditarTicket, CatChecklist, CapturaChecklist, EstatusRol
                           FROM [CatRoles]";

            OperDatosSql datosSql = new OperDatosSql();
            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);
            foreach (DataRow dr in lstDr)
            {
                RolAcceso i = new RolAcceso();

                i.rol = Convert.ToInt32(dr["rol"]);
                i.ClaveRol = dr["ClaveRol"].ToString();
                i.Descripcion = dr["Descripcion"].ToString();
                i.CatTpm = Convert.ToBoolean(dr["CatTpm"]);
                i.EditarTicket = Convert.ToBoolean(dr["EditarTicket"]);
                i.CatChecklist = Convert.ToBoolean(dr["CatChecklist"]);
                i.CapturaChecklist = Convert.ToBoolean(dr["CapturaChecklist"]);
                i.EstatusRol = Convert.ToBoolean(dr["EstatusRol"]);

                lstRoles.Add(i);
            }

            return (lstRoles);
        }
        public List<Empleado> LeeCatEmpleados(string cnxSqlMt, string cnxSqlRefec, string rutalog)
        {
            List<DataRow> lstDr = null;
            List<Empleado> lstemp = new List<Empleado>();

            string cmdSql = @"SELECT a.Nc, a.Nombre + ' '+ a.Apellidos as Nombre, '['+CONVERT(varchar(6),a.NC)+'] ' +a.Nombre+' '+ a.Apellidos as NombreKey,
                        a.Puesto , p.Descripcion as DescripPuesto, a.cc,c.Cc as descripCc, a.Scorecard
	                     FROM [Refectorio].[dbo].[EmpleadoAbc] a,
		                     [Refectorio].[dbo].[CentroCostos] c,
		                     [Refectorio].[dbo].[Cat_Puestos] p
	                     Where a.cc = c.IdCc and a.Puesto = p.Id and a.Situacion = 1";

            OperDatosSql datosSql = new OperDatosSql();
            lstDr = datosSql.LeeDatosOtraBD(cnxSqlRefec, cnxSqlMt, cmdSql);
            foreach (DataRow dr in lstDr)
            {
                Empleado i = new Empleado();

                i.NumControl = dr["Nc"].ToString();
                i.Nombre = dr["Nombre"].ToString();
                i.Nombrekey = dr["NombreKey"].ToString();
                i.Puesto = dr["Puesto"].ToString();
                i.DescripPuesto = dr["DescripPuesto"].ToString();
                i.CentroCosto = dr["cc"].ToString();
                i.DescripCC = dr["descripCc"].ToString();
                i.ClaveTpm = dr["Scorecard"].ToString();
                lstemp.Add(i);
            }
            return (lstemp);
        }
        public int GuardarUsuarioTPM(string cnxSql, UsuarioTpm UsuarioNew)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"  INSERT INTO [dbo].[UsuariosTpm] (NumControl,Nombre,Password,StatusEmpTpm,CentroCostos,ClaveRol,FecAlta,Agrego)
                              VALUES (@NumControl,@Nombre,@Password,@StatusEmpTpm,@CentroCostos,@ClaveRol,@FecAlta,@Agrego)";

            cmd.Parameters.AddWithValue("@NumControl", UsuarioNew.NumControl);
            cmd.Parameters.AddWithValue("@Nombre", UsuarioNew.Nombre);
            cmd.Parameters.AddWithValue("@Password", UsuarioNew.Password);
            cmd.Parameters.AddWithValue("@StatusEmpTpm", UsuarioNew.StatusEmpTpm);
            cmd.Parameters.AddWithValue("@CentroCostos", UsuarioNew.CentroCostos);
            cmd.Parameters.AddWithValue("@ClaveRol", UsuarioNew.ClaveRol);
            cmd.Parameters.AddWithValue("@FecAlta", UsuarioNew.FecAlta);
            cmd.Parameters.AddWithValue("@Agrego", UsuarioNew.Agrego);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public UsuarioTpm LeeDatosUsuarioTpm(string cnxSql, int id)
        {
            List<DataRow> lstDr = null;
            DataRow dr = null;
            UsuarioTpm i = new UsuarioTpm();

            string cmdSql = @"SELECT Id, NumControl, Nombre, Password, StatusEmpTpm, CentroCostos, ClaveRol, FecAlta, Agrego
                          FROM [UsuariosTpm]
                          Where Id = " + id.ToString();

            OperDatosSql datosSql = new OperDatosSql();

            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDr.Count >= 1)
            {
                dr = lstDr[0];
                i.Id = Convert.ToInt32(dr["Id"]);
                i.NumControl = dr["NumControl"].ToString();
                i.Nombre = dr["Nombre"].ToString();
                i.Password = dr["Password"].ToString();
                i.StatusEmpTpm = Convert.ToBoolean(dr["StatusEmpTpm"]);
                i.CentroCostos = dr["CentroCostos"].ToString();
                i.ClaveRol = dr["ClaveRol"].ToString();
                if (dr["FecAlta"].ToString() != "") { i.FecAlta = Convert.ToDateTime(dr["FecAlta"]); }
                i.Agrego = dr["Agrego"].ToString();
            }

            return (i);

        }

        public int UpdateUsuarioTPM(string cnxSql, UsuarioTpm Usuario)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"Update [dbo].[UsuariosTpm] Set Password = @Password, StatusEmpTpm = @StatusEmpTpm,
                              ClaveRol = @ClaveRol Where Id = @Id";

            cmd.Parameters.AddWithValue("@Id", Usuario.Id);
            cmd.Parameters.AddWithValue("@Password", Usuario.Password);
            cmd.Parameters.AddWithValue("@StatusEmpTpm", Usuario.StatusEmpTpm);
            cmd.Parameters.AddWithValue("@ClaveRol", Usuario.ClaveRol);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public List<CatPMS> LeeCatPMS(string cnxSql, string pCtroCtos, string pDepto)
        {
            List<DataRow> lstDr = null;
            List<CatPMS> lstDatos = new List<CatPMS>();

            string cmdSql = @" SELECT p.Id, p.WorkCenter, p.CodEquipo, e.DescripTechnical, p.CodSistemas, s.Sistema, p.CodCiclo, c.Descripcion,
                             p.Ppm, p.FecAlta, p.FecModif, p.UsuarioAlta, p.Estatus, p.CentroCostos, s.CodDepartamento
                             FROM [CatPMStandar] p
                             left join (Select CodEquipo, DescripTechnical from [CatEquipos] group by CodEquipo, DescripTechnical ) e on e.CodEquipo = p.CodEquipo
                             left join [CatSistemasEquipos] s on s.CodSistema = p.CodSistemas " +
                                "left join [CatCiclos] c on c.CodCiclo = p.CodCiclo ";

            if (pCtroCtos != "")
                cmdSql = cmdSql + " Where p.CentroCostos = '" + pCtroCtos + "' and s.CodDepartamento = '" + pDepto + "'";

            OperDatosSql datosSql = new OperDatosSql();
            lstDr = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDr)
            {
                CatPMS i = new CatPMS();

                i.Id = Convert.ToInt32(dr["Id"]);
                i.WorkCenter = dr["WorkCenter"].ToString();
                i.CodEquipo = dr["CodEquipo"].ToString();
                i.DescripEquipo = dr["DescripTechnical"].ToString();
                i.CodSistemas = dr["CodSistemas"].ToString();
                i.DescripSistema = dr["Sistema"].ToString();
                i.CodCiclo = dr["CodCiclo"].ToString();
                i.DescripCiclo = dr["Descripcion"].ToString();
                i.Ppm = (decimal)dr["Ppm"];
                if (dr["FecAlta"].ToString() != "") { i.FecAlta = Convert.ToDateTime(dr["FecAlta"]); }
                if (dr["FecModif"].ToString() != "") { i.FecModif = Convert.ToDateTime(dr["FecModif"]); }
                i.UsuarioAlta = dr["UsuarioAlta"].ToString();
                i.Estatus = Convert.ToBoolean(dr["Estatus"]);
                i.CentroCostos = dr["CentroCostos"].ToString();
                i.CodDepartamento = dr["CodDepartamento"].ToString();
                lstDatos.Add(i);
            }

            return (lstDatos);
        }

        public int GuardarPms(string cnxSql, PmStandar pmsNew)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"Insert into [CatPMStandar]
                              (WorkCenter,CodEquipo,CodSistemas,CodCiclo,Ppm,FecAlta,FecModif,UsuarioAlta,Estatus,CentroCostos)
                              VALUES (@WorkCenter, @CodEquipo, @CodSistemas, @CodCiclo, @Ppm, @FecAlta, @FecModif, @UsuarioAlta, @Estatus, @CentroCostos)";

            cmd.Parameters.AddWithValue("@WorkCenter", pmsNew.WorkCenter);
            cmd.Parameters.AddWithValue("@CodEquipo", pmsNew.CodEquipo);
            cmd.Parameters.AddWithValue("@CodSistemas", pmsNew.CodSistemas);
            cmd.Parameters.AddWithValue("@CodCiclo", pmsNew.CodCiclo);
            cmd.Parameters.AddWithValue("@Ppm", pmsNew.Ppm);
            cmd.Parameters.AddWithValue("@FecAlta", pmsNew.FecAlta);
            cmd.Parameters.AddWithValue("@FecModif", pmsNew.FecModif);
            cmd.Parameters.AddWithValue("@UsuarioAlta", pmsNew.UsuarioAlta);
            cmd.Parameters.AddWithValue("@Estatus", pmsNew.Estatus);
            cmd.Parameters.AddWithValue("@CentroCostos", pmsNew.CentroCostos);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public int UpdatePms(string cnxSql, PmStandar pmsNew)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"Update [dbo].[CatPMStandar]
                              Set Ppm =  @Ppm, FecModif = @FecModif, UsuarioAlta = @UsuarioAlta, Estatus = @Estatus
                              Where Id = @Id";

            cmd.Parameters.AddWithValue("@Id", pmsNew.Id);
            cmd.Parameters.AddWithValue("@Ppm", pmsNew.Ppm);
            cmd.Parameters.AddWithValue("@FecModif", pmsNew.FecModif);
            cmd.Parameters.AddWithValue("@UsuarioAlta", pmsNew.UsuarioAlta);
            cmd.Parameters.AddWithValue("@Estatus", pmsNew.Estatus);


            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);

            return result;
        }

        public PmStandar GetDatosPms(string cnxSql, int pId)
        {
            PmStandar f = new PmStandar();
            List<DataRow> lstDatos = null;

            string cmdSql = @"SELECT Id, WorkCenter, CodEquipo, CodSistemas, CodCiclo, Ppm, FecAlta, FecModif, UsuarioAlta, Estatus, CentroCostos
                             FROM [CatPMStandar]
                             Where Id = @pId";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@pId", pId);
            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];
                f.Id = Convert.ToInt32(dr["Id"]);
                f.WorkCenter = dr["WorkCenter"].ToString();
                f.CodEquipo = dr["CodEquipo"].ToString();
                f.CodSistemas = dr["CodSistemas"].ToString();
                f.CodCiclo = dr["CodCiclo"].ToString();
                f.Ppm = (decimal)dr["Ppm"];
                f.Estatus = Convert.ToBoolean(dr["Estatus"]);
                f.CentroCostos = dr["CentroCostos"].ToString();
                if (dr["FecAlta"].ToString() != "") { f.FecAlta = Convert.ToDateTime(dr["FecAlta"]); }
                if (dr["FecModif"].ToString() != "") { f.FecModif = Convert.ToDateTime(dr["FecModif"]); }
                f.UsuarioAlta = dr["UsuarioAlta"].ToString();
            }

            return f;
        }

        public bool ValidaPmsEquipo(string cnxSql, string pEquipo, string pCtroCtos)
        {
            List<DataRow> lstDatos = null;

            string cmdSql = @"SELECT Id, WorkCenter, CodEquipo, CodSistemas, CodCiclo, Ppm, FecAlta, FecModif, UsuarioAlta, Estatus, CentroCostos
                             FROM [CatPMStandar]
                             Where CodEquipo = @CodEquipo and CentroCostos = @CtroCtos";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@CodEquipo", pEquipo);
            sqlcmd.Parameters.AddWithValue("@CtroCtos", pCtroCtos);
            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                return true;
            }

            return false;
        }

        public List<Ticket> GetRepTick(ParamRepTickets filtros, string cnxSql, int pIdPlanta, string pCentroCostos, string pCodDepartamento,
                       string cadStatus, string cadHallazgo, string cadCausoParo, string pRutaLog)
        {
            List<DataRow> lstDatos = null;
            List<Ticket> lstTicket = new List<Ticket>();

            string cmdSql = "SELECT ";
            if (pCentroCostos == "")
            {
                cmdSql = cmdSql + "TOP 1000 ";
            }

            cmdSql = cmdSql + @"t.IdPlanta, p.Planta, t.IdTicket, t.TipoTicket, t.CodDepartamento, d.Descrip as Depto, t.CodWorkCenter, t.CodEquipo, t.CodSubEquipo, 
                     e.DescripTechnical as SubEquipo, t.FallaReportada, t.FchReporte, t.UsuarioReporto, t.Diagnostico, isnull(t.CodClasif, '') as CodClasif, 
                     isnull(c.Descripcion,'') as ClasifDescrip, t.CodSistema, s.Sistema, t.CodFalla, isnull(f.Descrip,'') as ClasifFalla, t.Falla, 
                     isnull(t.CodTipoFalla,'') as CodTipoFalla, isnull(tf.Descrip,'') as tipoFalla, t.CodStatus, st.Descrip as status, t.FecStatus, t.UsrAtendio, t.AccionPlan, 
                     t.FchAccionplan, t.WorkerAsignado, t.FecIniReparacion, FecEntgregaReparacion, t.CodTipoWom, isnull(tw.Descrip,'') as CodTipoWomDescrip, t.WOM, t.FchaPromesa, 
                     t.FchClose, t.PCReporto, t.Notas, t.CausoParo, t.HallazgoSeguridad, t.Sensor, isnull(t.TiempoReparacion,0) as TiempoReparacion, t.UnidadTiempoRep, 
                     isnull(cl.Descripcion,'') as DescripcionUT
                     FROM [Tickets] t
                     inner join [CatPlantas] p on p.IdPlanta = t.IdPlanta
                     inner join [CatDepartamentos] d on d.CodDepartamento = t.CodDepartamento and d.IdPlanta = t.IdPlanta
                     inner join [CatEquipos] e on e.CodEquipo = t.CodSubEquipo
                     inner join [CatSistemasEquipos] s on s.CodSistema = t.CodSistema and s.CodDepartamento = t.CodDepartamento
                     inner join [CatStsTickets] st on st.CodStatus = t.CodStatus
                     left join [CatFallas] f on f.CodFalla = t.CodFalla and f.CodDepartamento = t.CodDepartamento                 
                     left join [CatClasifFallas] c on c.CodClasif = t.CodClasif 
                     left join [CatTipoFallas] tf on tf.CodTipoFalla = t.CodTipoFalla
                     left join [CatTipoWom] tw on tw.CodTipoWom = t.CodTipoWom
                     left join [CatCiclos] cl on cl.CodCiclo = t.UnidadTiempoRep
                     Where  t.IdPlanta = @IdPlanta 
                     and ((@CodDepartamento = '') or (t.CodDepartamento = @CodDepartamento))
                     and ((@CentroCostos = '') or (t.CentroCostos = @CentroCostos))
                     and convert(date,FchReporte) >= convert(date,@fecInicial) and convert(date,FchReporte) <= convert(date,@fecFinal) " +
                         cadStatus + cadHallazgo + cadCausoParo
                        + " Order by FchReporte";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdSql;
            cmd.Parameters.AddWithValue("@IdPlanta", pIdPlanta);
            cmd.Parameters.AddWithValue("@CodDepartamento", pCodDepartamento);
            cmd.Parameters.AddWithValue("@CentroCostos", pCentroCostos);
            cmd.Parameters.AddWithValue("@fecInicial", filtros.FecInicial);
            cmd.Parameters.AddWithValue("@fecFinal", filtros.FecFinal);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, cmd);


            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    Ticket tick = new Ticket();

                    tick.IdPlanta = (int)dr["IdPlanta"];
                    tick.Planta = dr["Planta"].ToString();
                    tick.IdTicket = (int)dr["IdTicket"];
                    tick.TipoTicket = dr["TipoTicket"].ToString();
                    tick.CodDepartamento = dr["CodDepartamento"].ToString();
                    tick.Depto = dr["Depto"].ToString();
                    tick.CodWorkCenter = dr["CodWorkCenter"].ToString();
                    tick.CodEquipo = dr["CodEquipo"].ToString();
                    tick.CodSubEquipo = dr["CodSubEquipo"].ToString();
                    tick.SubEquipo = dr["SubEquipo"].ToString();
                    tick.FallaReportada = dr["FallaReportada"].ToString();
                    tick.UsuarioReporto = dr["UsuarioReporto"].ToString();
                    tick.Diagnostico = dr["Diagnostico"].ToString();
                    tick.CodClasif = dr["CodClasif"].ToString();
                    tick.ClasifDescrip = dr["ClasifDescrip"].ToString();
                    tick.CodSistema = dr["CodSistema"].ToString();
                    tick.CodSistemaDescrip = dr["Sistema"].ToString();
                    tick.CodFalla = dr["CodFalla"].ToString();
                    tick.CodFallaDescrip = dr["ClasifFalla"].ToString();
                    tick.CodTipoFalla = dr["CodTipoFalla"].ToString();
                    tick.CodTipoFallaDescrip = dr["TipoFalla"].ToString();
                    tick.CodStatus = dr["CodStatus"].ToString();
                    tick.CodStatusDescrip = dr["Status"].ToString();
                    tick.UsrAtendio = dr["UsrAtendio"].ToString();
                    tick.AccionPlan = dr["AccionPlan"].ToString();
                    tick.WorkerAsignado = dr["WorkerAsignado"].ToString();
                    tick.CodTipoWom = dr["CodTipoWom"].ToString();
                    tick.CodTipoWomDescrip = dr["CodTipoWomDescrip"].ToString();
                    tick.WOM = dr["WOM"].ToString();
                    tick.PCReporto = dr["PCReporto"].ToString();
                    tick.Notas = dr["Notas"].ToString();
                    tick.CausoParo = dr["CausoParo"].ToString();
                    tick.Sensor = dr["Sensor"].ToString();
                    tick.HallazgoSeguridad = dr["HallazgoSeguridad"].ToString();
                    tick.TiempoReparacion = (int)dr["TiempoReparacion"];
                    tick.UnidadTiempoRep = dr["UnidadTiempoRep"].ToString();
                    tick.DescripcionUT = dr["DescripcionUT"].ToString();

                    if (dr["FchReporte"].ToString() != "") { tick.FchReporte = Convert.ToDateTime(dr["FchReporte"]); }
                    if (dr["FecStatus"].ToString() != "") { tick.FecStatus = Convert.ToDateTime(dr["FecStatus"]); }
                    if (dr["FchAccionplan"].ToString() != "") { tick.FchAccionplan = Convert.ToDateTime(dr["FchAccionplan"]); }
                    if (dr["FecIniReparacion"].ToString() != "") { tick.FecIniReparacion = Convert.ToDateTime(dr["FecIniReparacion"]); }
                    if (dr["FecEntgregaReparacion"].ToString() != "") { tick.FecEntgregaReparacion = Convert.ToDateTime(dr["FecEntgregaReparacion"]); }
                    if (dr["FchaPromesa"].ToString() != "") { tick.FchaPromesa = Convert.ToDateTime(dr["FchaPromesa"]); }
                    if (dr["FchClose"].ToString() != "") { tick.FchClose = Convert.ToDateTime(dr["FchClose"]); }

                    lstTicket.Add(tick);
                }
            }

            return (lstTicket);

        }
        public DataTable GetRepTickdt(ParamRepTickets filtros, string cnxSql, int pIdPlanta, string pCentroCostos, string pCodDepartamento,
                       string cadStatus, string cadHallazgo, string cadCausoParo, string pRutaLog)
        {
            DataTable dt = new DataTable();


            string cmdSql = "SELECT ";
            if (pCentroCostos == "")
            {
                cmdSql = cmdSql + "TOP 1000 ";
            }

            cmdSql = cmdSql + @"SELECT t.IdPlanta, p.Planta, t.IdTicket, t.TipoTicket, t.CodDepartamento, d.Descrip as Depto, t.CodWorkCenter, t.CodEquipo, t.CodSubEquipo, 
                     e.DescripTechnical as SubEquipo, t.FallaReportada, t.FchReporte, t.UsuarioReporto, t.Diagnostico, isnull(t.CodClasif, '') as CodClasif, 
                     isnull(c.Descripcion,'') as ClasifDescrip, t.CodSistema, s.Sistema, t.CodFalla, isnull(f.Descrip,'') as ClasifFalla, t.Falla, 
                     isnull(t.CodTipoFalla,'') as CodTipoFalla, isnull(tf.Descrip,'') as tipoFalla, t.CodStatus, st.Descrip as status, t.FecStatus, t.UsrAtendio, t.AccionPlan, 
                     t.FchAccionplan, t.WorkerAsignado, t.FecIniReparacion, FecEntgregaReparacion, t.CodTipoWom, isnull(tw.Descrip,'') as CodTipoWomDescrip, t.WOM, t.FchaPromesa, 
                     t.FchClose, t.PCReporto, t.Notas, t.CausoParo, t.Sensor, t.HallazgoSeguridad, isnull(t.TiempoReparacion,0) as TiempoReparacion, t.UnidadTiempoRep, 
                     isnull(cl.Descripcion,'') as DescripcionUT
                     FROM [Tickets] t
                     inner join [CatPlantas] p on p.IdPlanta = t.IdPlanta
                     inner join [CatDepartamentos] d on d.CodDepartamento = t.CodDepartamento and d.IdPlanta = t.IdPlanta
                     inner join [CatEquipos] e on e.CodEquipo = t.CodSubEquipo
                     inner join [CatSistemasEquipos] s on s.CodSistema = t.CodSistema and s.CodDepartamento = t.CodDepartamento
                     inner join [CatStsTickets] st on st.CodStatus = t.CodStatus
                     left join [CatFallas] f on f.CodFalla = t.CodFalla and f.CodDepartamento = t.CodDepartamento                 
                     left join [CatClasifFallas] c on c.CodClasif = t.CodClasif 
                     left join [CatTipoFallas] tf on tf.CodTipoFalla = t.CodTipoFalla
                     left join [CatTipoWom] tw on tw.CodTipoWom = t.CodTipoWom
                     left join [CatCiclos] cl on cl.CodCiclo = t.UnidadTiempoRep
                     Where  t.IdPlanta = @IdPlanta 
                     and ((@CodDepartamento = '') or (t.CodDepartamento = @CodDepartamento))
                     and ((@CentroCostos = '') or (t.CentroCostos = @CentroCostos))
                     and convert(date,FchReporte) >= convert(date,@fecInicial) and convert(date,FchReporte) <= convert(date,@fecFinal) " +
                         cadStatus + cadHallazgo + cadCausoParo
                        + " Order by FchReporte";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdSql;
            cmd.Parameters.AddWithValue("@IdPlanta", pIdPlanta);
            cmd.Parameters.AddWithValue("@CodDepartamento", pCodDepartamento);
            cmd.Parameters.AddWithValue("@CentroCostos", pCentroCostos);
            cmd.Parameters.AddWithValue("@fecInicial", filtros.FecInicial);
            cmd.Parameters.AddWithValue("@fecFinal", filtros.FecFinal);

            OperDatosSql datosSql = new OperDatosSql();
            dt = datosSql.LeeConParamDt(cnxSql, cmd);

            return (dt);

        }

        public List<CheckListEqEnc> GetCatChklstxEq(string cnxSql, int pPlanta, string pDepto, string pCostos, bool SoloActivos = false)
        {
            List<DataRow> lstDatos = null;
            List<CheckListEqEnc> lstTemp = new List<CheckListEqEnc>();
            string cmdSql = "";

            if (SoloActivos)
            {
                cmdSql = @" SELECT e.IdChkEquipo, e.ChkEquipo, e.CodWorkCenter, e.CodEquipo, e.IdCheckList, e.CodChkList, e.DescripChkList,
                        e.CodClasif, e.EqParado, e.Activo, e.IdFrecuencia, c.Descripcion as DesripFrencu, e.Frecuencia, 
                        e.CodDepartamento, e.UserAlta, e.FchAlta, e.UserModif, e.FchModif, e.IniProgram, e.UltimaEjec, e.UserEjecuto
                        FROM [CheckListxEqEnc] e
                        left Join [CatCiclos] c on c.CodCiclo = e.IdFrecuencia
                        Where e.Activo = 1 
                            and e.Planta = @pPlanta
                            and ((@pDepto = '') or (e.CodDepartamento = @pDepto))
                            and ((@pCostos = '') or (e.CentroCostos = @pCostos))";
            }
            else
            {
                cmdSql = @" SELECT e.IdChkEquipo, e.ChkEquipo, e.CodWorkCenter, e.CodEquipo, e.IdCheckList, e.CodChkList, e.DescripChkList,
                        e.CodClasif, e.EqParado, e.Activo, e.IdFrecuencia, c.Descripcion as DesripFrencu, e.Frecuencia, 
                        e.CodDepartamento, e.UserAlta, e.FchAlta, e.UserModif, e.FchModif, e.IniProgram, e.UltimaEjec, e.UserEjecuto
                        FROM [CheckListxEqEnc] e
                        left Join [CatCiclos] c on c.CodCiclo = e.IdFrecuencia
                        Where e.Planta = @pPlanta
                            and ((@pDepto = '') or (e.CodDepartamento = @pDepto))
                            and ((@pCostos = '') or (e.CentroCostos = @pCostos))";
            }

            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandText = cmdSql;

            sqlcmd.Parameters.AddWithValue("@pPlanta", pPlanta);
            sqlcmd.Parameters.AddWithValue("@pDepto", pDepto);
            sqlcmd.Parameters.AddWithValue("@pCostos", pCostos);


            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            foreach (DataRow dr in lstDatos)
            {
                CheckListEqEnc item = new CheckListEqEnc();

                item.IdChkEquipo = (int)dr["IdChkEquipo"];
                item.ChkEquipo = dr["ChkEquipo"].ToString();
                item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                item.CodEquipo = dr["CodEquipo"].ToString();
                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodChkList = dr["CodChkList"].ToString();
                item.DescripChkList = dr["DescripChkList"].ToString();
                item.CodClasif = dr["CodClasif"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.IdFrecuencia = dr["IdFrecuencia"].ToString();
                item.DesripFrencu = dr["DesripFrencu"].ToString();
                item.Frecuencia = (int)dr["Frecuencia"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.UserAlta = dr["UserAlta"].ToString();
                if (dr["FchAlta"].ToString() != "")
                { item.FchAlta = Convert.ToDateTime(dr["FchAlta"]); }
                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["IniProgram"].ToString() != "")
                { item.IniProgram = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["UltimaEjec"].ToString() != "")
                { item.UltimaEjec = Convert.ToDateTime(dr["UltimaEjec"]); }
                item.UserEjecuto = dr["UserEjecuto"].ToString();

                lstTemp.Add(item);
            }
            return (lstTemp);
        }

        public CheckListEqEnc GetChklstxEq(string cnxSql, int Id, string pDepto)
        {
            List<DataRow> lstDatos = null;
            CheckListEqEnc item = new CheckListEqEnc();
            string cmdSql = "";

            cmdSql = @" SELECT e.IdChkEquipo, e.ChkEquipo, e.CodWorkCenter, e.CodEquipo, e.IdCheckList, e.CodChkList, e.DescripChkList, 
                        e.CodClasif, e.EqParado, e.Activo, e.CodDepartamento, e.IdFrecuencia, c.Descripcion as DesripFrencu,
                        e.Frecuencia, e.UserModif, e.FchModif, e.IniProgram
                        FROM [CheckListxEqEnc] e
                         left Join [CatCiclos] c on c.CodCiclo = e.IdFrecuencia
                        Where  e.CodDepartamento = '" + pDepto + "' " + " and e.IdChkEquipo =" + Id.ToString();

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];

                item.IdChkEquipo = (int)dr["IdChkEquipo"];
                item.ChkEquipo = dr["ChkEquipo"].ToString();
                item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                item.CodEquipo = dr["CodEquipo"].ToString();
                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodChkList = dr["CodChkList"].ToString();
                item.DescripChkList = dr["DescripChkList"].ToString();
                item.CodClasif = dr["CodClasif"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.IdFrecuencia = dr["IdFrecuencia"].ToString();
                item.DesripFrencu = dr["DesripFrencu"].ToString();
                item.Frecuencia = (int)dr["Frecuencia"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["IniProgram"].ToString() != "")
                { item.IniProgram = Convert.ToDateTime(dr["IniProgram"]); }


            }

            return (item);
        }

        public List<CheckListEqDt> GetChkxEqDet(string cnxSql, int pId, string pDepto)
        {
            List<DataRow> lstDatos = null;
            List<CheckListEqDt> lstDt = new List<CheckListEqDt>();

            string cmdSql = @"SELECT d.IdDtCheckList, d.IdChkEquipo, d.CodWorkCenter, d.CodEquipo, d.IdCheckList, d.CodChkList, d.CodGpoActiv, d.idActividad,
                           d.CodActividad ,a.DescripcionAct, s.CodSistema,s.Sistema as DescripSistema ,c.IdComponente,c.DescripCompo,
                           d.Orden, d.TipoActividad, d.TipoOperacion, d.EqParado, d.RangoMin, d.OperadorMin, d.RangoMax, d.OperadorMax, isnull(d.CodUom, '') as CodUom, isnull(u.Descrip,'') as DescripUom,
                           d.Ponderacion, d.Activo, d.Item, d.CodUom
                           FROM [CheckListxEqDet]  d
                           inner join[CatActivChkLst] a on a.IdActividad = d.idActividad
                           inner join[CatSistemasEquipos] s on s.CodSistema = a.CodSistema
                           inner join[CatComponentes] c on c.IdComponente = a.IdComponente
                           left join[CatUom] u on u.CodUom = d.CodUom
                           Where d.IdChkEquipo =  @pId
                           order by d.Orden";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdSql;
            cmd.Parameters.AddWithValue("@pId", pId);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, cmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    CheckListEqDt item = new CheckListEqDt();

                    item.IdDtCheckList = (int)dr["IdDtCheckList"];
                    item.IdChkEquipo = (int)dr["IdChkEquipo"];
                    item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                    item.CodEquipo = dr["CodEquipo"].ToString();
                    item.IdCheckList = (int)dr["IdCheckList"];
                    item.CodChkList = dr["CodChkList"].ToString();
                    item.CodGpoActiv = dr["CodGpoActiv"].ToString();
                    item.IdActividad = (int)dr["idActividad"];
                    item.CodActividad = dr["CodActividad"].ToString();
                    item.DescripcionAct = dr["DescripcionAct"].ToString();
                    item.CodSistema = dr["CodSistema"].ToString();
                    item.DescripSistema = dr["DescripSistema"].ToString();
                    item.IdComponente = (int)dr["IdComponente"];
                    item.DescripCompo = dr["DescripCompo"].ToString();
                    item.Orden = (int)dr["Orden"];
                    item.TipoActividad = dr["TipoActividad"].ToString();
                    item.TipoOperacion = dr["TipoOperacion"].ToString();
                    item.EqParado = (bool)dr["EqParado"];
                    item.RangoMin = (decimal)dr["RangoMin"];
                    item.OperadorMin = dr["OperadorMin"].ToString();
                    item.RangoMax = (decimal)dr["RangoMax"];
                    item.OperadorMax = dr["OperadorMax"].ToString();
                    item.CodUom = dr["CodUom"].ToString();
                    item.DescripUom = dr["DescripUom"].ToString();
                    item.Ponderacion = (decimal)dr["Ponderacion"];
                    item.Activo = (bool)dr["Activo"];
                    item.Item = (int)dr["Item"];
                    item.ResultVisual = null;
                    item.ResultMedible = null;
                    lstDt.Add(item);
                }
            }
            return (lstDt);
        }

        public CheckListEqDt GetChklstxEqAct(string cnxSql, int idDetChkEq, int idChkEq, int IdActiv)
        {
            List<DataRow> lstDatos = null;

            string cmdSql = @"SELECT d.IdDtCheckList, d.IdChkEquipo, d.CodWorkCenter, d.CodEquipo, d.IdCheckList, d.CodChkList, d.CodGpoActiv, d.idActividad,
                           d.CodActividad ,a.DescripcionAct, s.CodSistema,s.Sistema as DescripSistema ,c.IdComponente,c.DescripCompo,
                           d.Orden, d.TipoActividad, d.TipoOperacion, d.EqParado, d.RangoMin, d.OperadorMin, d.RangoMax, d.OperadorMax,
                           d.Ponderacion, d.Activo, d.Item, d.CodUoM
                           FROM[CheckListxEqDet]  d
                           inner join[CatActivChkLst] a on a.IdActividad = d.idActividad
                           inner join[CatSistemasEquipos] s on s.CodSistema = a.CodSistema
                           inner join[CatComponentes] c on c.IdComponente = a.IdComponente
                           Where d.IdDtCheckList = @idDetChkEq and d.IdChkEquipo =  @idChkEq and d.idActividad = @IdActiv";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdSql;
            cmd.Parameters.AddWithValue("@idDetChkEq", idDetChkEq);
            cmd.Parameters.AddWithValue("@idChkEq", idChkEq);
            cmd.Parameters.AddWithValue("@IdActiv", IdActiv);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, cmd);
            CheckListEqDt item = new CheckListEqDt();

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];

                item.IdDtCheckList = (int)dr["IdDtCheckList"];
                item.IdChkEquipo = (int)dr["IdChkEquipo"];
                item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                item.CodEquipo = dr["CodEquipo"].ToString();
                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodChkList = dr["CodChkList"].ToString();
                item.CodGpoActiv = dr["CodGpoActiv"].ToString();
                item.IdActividad = (int)dr["idActividad"];
                item.CodActividad = dr["CodActividad"].ToString();
                item.DescripcionAct = dr["DescripcionAct"].ToString();
                item.CodSistema = dr["CodSistema"].ToString();
                item.DescripSistema = dr["DescripSistema"].ToString();
                item.IdComponente = (int)dr["IdComponente"];
                item.DescripCompo = dr["DescripCompo"].ToString();
                item.Orden = (int)dr["Orden"];
                item.TipoActividad = dr["TipoActividad"].ToString();
                item.TipoOperacion = dr["TipoOperacion"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.RangoMin = (decimal)dr["RangoMin"];
                item.OperadorMin = dr["OperadorMin"].ToString();
                item.RangoMax = (decimal)dr["RangoMax"];
                item.OperadorMax = dr["OperadorMax"].ToString();
                item.Ponderacion = (decimal)dr["Ponderacion"];
                item.Activo = (bool)dr["Activo"];
                item.Item = (int)dr["Item"];
                item.CodUom = dr["CodUom"].ToString();
            }

            return (item);
        }

        public int GuardarCapCklstEncb(string cnxSql, CheckListEqEnc chkxEq, int pPlanta)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @" INSERT INTO [CheckListCapEnc]
                              (CodWorkCenter,CodEquipo,IdCheckList,CodChkList,DescripChkList,CodClasif,EqParado,Activo,CodDepartamento,
                              UserModif,FchModif,Frecuencia,IdFrecuencia,IniProgram,FchEjecucion,UserEjecuto, Observaciones, Planta, CentroCostos)
                              VALUES (@CodWorkCenter,@CodEquipo,@IdCheckList,@CodChkList,@DescripChkList,@CodClasif,@EqParado,@Activo,@CodDepartamento,
                              @UserModif,@FchModif,@Frecuencia,@IdFrecuencia,@IniProgram,@FchEjecucion,@UserEjecuto, @Observaciones, @Planta, @CentroCostos)";

            cmd.Parameters.AddWithValue("@CodWorkCenter", chkxEq.CodWorkCenter);
            cmd.Parameters.AddWithValue("@CodEquipo", chkxEq.CodEquipo);
            cmd.Parameters.AddWithValue("@IdCheckList", chkxEq.IdCheckList);
            cmd.Parameters.AddWithValue("@CodChkList", chkxEq.CodChkList);
            cmd.Parameters.AddWithValue("@DescripChkList", chkxEq.DescripChkList);
            cmd.Parameters.AddWithValue("@CodClasif", chkxEq.CodClasif);
            cmd.Parameters.AddWithValue("@EqParado", chkxEq.EqParado);
            cmd.Parameters.AddWithValue("@Activo", chkxEq.Activo);
            cmd.Parameters.AddWithValue("@CodDepartamento", chkxEq.CodDepartamento);
            cmd.Parameters.AddWithValue("@Frecuencia", chkxEq.Frecuencia);
            cmd.Parameters.AddWithValue("@IdFrecuencia", chkxEq.IdFrecuencia);
            cmd.Parameters.AddWithValue("@UserModif", chkxEq.UserModif);
            cmd.Parameters.AddWithValue("@FchModif", chkxEq.FchModif);
            cmd.Parameters.AddWithValue("@CentroCostos", chkxEq.CentroCostos);


            // Sino a sido programado, lo guardamos en null
            if (chkxEq.IniProgram.ToShortDateString() != "01/01/0001") { cmd.Parameters.AddWithValue("@IniProgram", chkxEq.IniProgram); }
            else { cmd.Parameters.AddWithValue("@IniProgram", DBNull.Value); }

            if (chkxEq.FchEjecucion != null && chkxEq.UserEjecuto == "")
                chkxEq.UserEjecuto = "ADMINISTRADOR SISTEMA";
            cmd.Parameters.AddWithValue("@FchEjecucion", chkxEq.FchEjecucion);
            cmd.Parameters.AddWithValue("@UserEjecuto", chkxEq.UserEjecuto);
            cmd.Parameters.AddWithValue("@Observaciones", chkxEq.Observaciones);
            cmd.Parameters.AddWithValue("@Planta", chkxEq.Planta);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);
            return result;
        }

        public List<DataRow> Valid_ChkNewCap(string cnxSql, CheckListEqEnc chekEq, string rutalog)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();

            cmd.CommandText = @" SELECT IdChkEquipo, CodWorkCenter, CodEquipo, IdCheckList, CodChkList, CodClasif, 
                                 CodDepartamento, Frecuencia, IdFrecuencia, FchEjecucion
                              FROM [CheckListCapEnc]
                              Where CodWorkCenter = @CodWorkCenter and  CodEquipo = @CodEquipo and  IdCheckList = @IdCheckList and CodChkList = @CodChkList
                                 and CodClasif =	@CodClasif and CodDepartamento = @CodDepartamento and Frecuencia = @Frecuencia and IdFrecuencia = @IdFrecuencia
                                 and FchEjecucion = @FchEjecucion";

            cmd.Parameters.AddWithValue("@CodWorkCenter", chekEq.CodWorkCenter);
            cmd.Parameters.AddWithValue("@CodEquipo", chekEq.CodEquipo);
            cmd.Parameters.AddWithValue("@IdCheckList", chekEq.IdCheckList);
            cmd.Parameters.AddWithValue("@CodChkList", chekEq.CodChkList);
            cmd.Parameters.AddWithValue("@CodClasif", chekEq.CodClasif);
            cmd.Parameters.AddWithValue("@CodDepartamento", chekEq.CodDepartamento);
            cmd.Parameters.AddWithValue("@Frecuencia", chekEq.Frecuencia);
            cmd.Parameters.AddWithValue("@IdFrecuencia", chekEq.IdFrecuencia);
            cmd.Parameters.AddWithValue("@FchEjecucion", chekEq.FchEjecucion);

            lstDatos = operDatos.LeeDatosConParam(cnxSql, cmd);

            return lstDatos;
        }

        public int GuardarNewCapAct(string cnxSql, CapturaChklst NewChkAct, string rutalog)
        {
            int result = 0;
            OperDatosSql operDatos = new OperDatosSql();

            SqlCommand cmd = new SqlCommand();
            foreach (var j in NewChkAct.lstChckActEq)
            {

                cmd.Parameters.Clear();
                cmd.CommandText = @"INSERT INTO  [CheckListCapDet]
                              (IdChkEquipo,CodWorkCenter,CodEquipo,IdCheckList,CodChkList,CodGpoActiv,idActividad,CodActividad
                              ,TipoActividad,TipoOperacion,EqParado,RangoMin,OperadorMin,RangoMax,OperadorMax,Ponderacion,Activo, Orden, Item, CodUom, ResultVisual,ResultMedible)
                              VALUES (@IdChkEquipo, @CodWorkCenter, @CodEquipo, @IdCheckList,@CodChkList,@CodGpoActiv,@idActividad,@CodActividad
                              ,@TipoActividad,@TipoOperacion,@EqParado,@RangoMin,@OperadorMin,@RangoMax,@OperadorMax,@Ponderacion,@Activo,@Orden,@Item, @CodUom, @ResultVisual, @ResultMedible)";

                cmd.Parameters.AddWithValue("@IdChkEquipo", NewChkAct.ChklsxEq.IdChkEquipo);
                cmd.Parameters.AddWithValue("@CodWorkCenter", NewChkAct.ChklsxEq.CodWorkCenter);
                cmd.Parameters.AddWithValue("@CodEquipo", NewChkAct.ChklsxEq.CodEquipo);
                cmd.Parameters.AddWithValue("@IdCheckList", NewChkAct.ChklsxEq.IdCheckList);
                cmd.Parameters.AddWithValue("@CodChkList", NewChkAct.ChklsxEq.CodChkList);

                cmd.Parameters.AddWithValue("@CodGpoActiv", j.CodGpoActiv);
                cmd.Parameters.AddWithValue("@idActividad", j.IdActividad);
                cmd.Parameters.AddWithValue("@CodActividad", j.CodActividad);

                cmd.Parameters.AddWithValue("@TipoActividad", j.TipoActividad);
                cmd.Parameters.AddWithValue("@TipoOperacion", j.TipoOperacion);

                cmd.Parameters.AddWithValue("@EqParado", j.EqParado);
                cmd.Parameters.AddWithValue("@RangoMin", j.RangoMin);
                cmd.Parameters.AddWithValue("@OperadorMin", j.OperadorMin);
                cmd.Parameters.AddWithValue("@RangoMax", j.RangoMax);
                cmd.Parameters.AddWithValue("@OperadorMax", j.OperadorMax);
                cmd.Parameters.AddWithValue("@Ponderacion", j.Ponderacion);
                cmd.Parameters.AddWithValue("@Activo", j.Activo);
                cmd.Parameters.AddWithValue("@Orden", j.Orden);
                cmd.Parameters.AddWithValue("@Item", j.Item);
                cmd.Parameters.AddWithValue("@CodUom", j.CodUom);

                cmd.Parameters.AddWithValue("@ResultVisual", j.ResultVisual);
                cmd.Parameters.AddWithValue("@ResultMedible", j.ResultMedible);

                result = operDatos.Guardar(cnxSql, cmd);
            }

            return result;
        }

        public List<CheckListEqEnc> GetCapChklst(string cnxSql, DateTime fi, DateTime ff, string pDepto)
        {

            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();
            List<CheckListEqEnc> lstTemp = new List<CheckListEqEnc>();

            cmd.CommandText = @" SELECT e.IdChkEquipo, e.CodWorkCenter, e.CodEquipo, q.DescripTechnical as DescripEquipo, e.IdCheckList, e.CodChkList, 
                     cle.ChkEquipo as DescripChkList, e.CodClasif, e.EqParado, e.Activo, e.CodDepartamento, e.IdFrecuencia, c.Descripcion as DesripFrencu, 
                     e.Frecuencia, e.UserModif, e.FchModif, e.IniProgram,  e.FchEjecucion, e.UserEjecuto
                       FROM [CheckListCapEnc] e
                       left join [CheckListxEqEnc] cle on cle.CodEquipo = e.CodEquipo and e.IdCheckList = cle.IdCheckList
                       left Join [CatCiclos] c on c.CodCiclo = e.IdFrecuencia
                       left join [CatEquipos] q on q.CodEquipo = e.CodEquipo
                       where Convert(date,fchEjecucion) >= @fi and Convert(date,fchEjecucion) <= @ff 
                       and ((@pDepto = '') or (e.CodDepartamento = @pDepto))";

            cmd.Parameters.AddWithValue("@fi", fi);
            cmd.Parameters.AddWithValue("@ff", ff);
            cmd.Parameters.AddWithValue("@pDepto", pDepto);

            lstDatos = operDatos.LeeDatosConParam(cnxSql, cmd);

            foreach (DataRow dr in lstDatos)
            {
                CheckListEqEnc item = new CheckListEqEnc();

                item.IdChkEquipo = (int)dr["IdChkEquipo"];
                item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                item.CodEquipo = dr["CodEquipo"].ToString();
                item.DescripEquipo = dr["DescripEquipo"].ToString();
                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodChkList = dr["CodChkList"].ToString();
                item.DescripChkList = dr["DescripChkList"].ToString();
                item.CodClasif = dr["CodClasif"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.IdFrecuencia = dr["IdFrecuencia"].ToString();
                item.DesripFrencu = dr["DesripFrencu"].ToString();
                item.Frecuencia = (int)dr["Frecuencia"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();

                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["IniProgram"].ToString() != "")
                { item.IniProgram = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["FchEjecucion"].ToString() != "")
                { item.FchEjecucion = Convert.ToDateTime(dr["FchEjecucion"]); }
                item.UserEjecuto = dr["UserEjecuto"].ToString();

                lstTemp.Add(item);
            }
            return (lstTemp);
        }

        public List<CheckListEqEnc> GetCapChklstEnc(string cnxSql, DateTime fi, DateTime ff, string pDepto)
        {

            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();
            List<CheckListEqEnc> lstTemp = new List<CheckListEqEnc>();

            cmd.CommandText = @" SELECT e.IdChkEquipo, e.CodWorkCenter, e.CodEquipo, q.DescripTechnical as DescripEquipo, e.IdCheckList, e.CodChkList, 
                     e.DescripChkList, e.CodClasif, e.EqParado, e.Activo, e.CodDepartamento, e.IdFrecuencia, c.Descripcion as DesripFrencu, 
                     e.Frecuencia, e.UserModif, e.FchModif, e.IniProgram,  e.FchEjecucion, e.UserEjecuto
                       FROM [CheckListCapEnc] e
                       left Join [CatCiclos] c on c.CodCiclo = e.IdFrecuencia
                       left join [CatEquipos] q on q.CodEquipo = e.CodEquipo
                       where Convert(date,fchEjecucion) >= @fi and Convert(date,fchEjecucion) <= @ff and CodDepartamento = @pDepto";

            cmd.Parameters.AddWithValue("@fi", fi);
            cmd.Parameters.AddWithValue("@ff", ff);
            cmd.Parameters.AddWithValue("@pDepto", pDepto);

            lstDatos = operDatos.LeeDatosConParam(cnxSql, cmd);

            foreach (DataRow dr in lstDatos)
            {
                CheckListEqEnc item = new CheckListEqEnc();

                item.IdChkEquipo = (int)dr["IdChkEquipo"];
                item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                item.CodEquipo = dr["CodEquipo"].ToString();
                item.DescripEquipo = dr["DescripEquipo"].ToString();
                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodChkList = dr["CodChkList"].ToString();
                item.DescripChkList = dr["DescripChkList"].ToString();
                item.CodClasif = dr["CodClasif"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.IdFrecuencia = dr["IdFrecuencia"].ToString();
                item.DesripFrencu = dr["DesripFrencu"].ToString();
                item.Frecuencia = (int)dr["Frecuencia"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();

                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["IniProgram"].ToString() != "")
                { item.IniProgram = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["FchEjecucion"].ToString() != "")
                { item.FchEjecucion = Convert.ToDateTime(dr["FchEjecucion"]); }
                item.UserEjecuto = dr["UserEjecuto"].ToString();

                lstTemp.Add(item);
            }
            return (lstTemp);
        }

        public CheckListEqEnc GetDatosChklstEncb(string cnxSql, int Id, string pDepto, int pPlanta)
        {
            List<DataRow> lstDatos = null;
            CheckListEqEnc item = new CheckListEqEnc();

            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();

            cmd.CommandText = @" SELECT e.IdChkEquipo, e.CodWorkCenter, e.CodEquipo, q.DescripTechnical as DescripEquipo, e.IdCheckList, e.CodChkList, 
                     e.DescripChkList, e.CodClasif, e.EqParado, e.Activo, e.CodDepartamento, e.IdFrecuencia, c.Descripcion as DesripFrencu, 
                     e.Frecuencia, e.UserModif, e.FchModif, e.IniProgram,  e.FchEjecucion, e.UserEjecuto, e.Observaciones, e.Planta, FecProgramada
                       FROM [CheckListCapEnc] e
                       left Join [CatCiclos] c on c.CodCiclo = e.IdFrecuencia
                       left join [CatEquipos] q on q.CodEquipo = e.CodEquipo
                        Where e.Planta = @pPlanta 
                        and ((@pDepto = '') or (e.CodDepartamento = @pDepto)) 
                        and e.IdChkEquipo = @Id";

            cmd.Parameters.AddWithValue("@pPlanta", pPlanta);
            cmd.Parameters.AddWithValue("@pDepto", pDepto);
            cmd.Parameters.AddWithValue("@Id", Id);

            lstDatos = operDatos.LeeDatosConParam(cnxSql, cmd);

            if (lstDatos.Count > 0)
            {
                DataRow dr = lstDatos[0];

                item.IdChkEquipo = (int)dr["IdChkEquipo"];
                item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                item.CodEquipo = dr["CodEquipo"].ToString();
                item.DescripEquipo = dr["DescripEquipo"].ToString();
                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodChkList = dr["CodChkList"].ToString();
                item.DescripChkList = dr["DescripChkList"].ToString();
                item.CodClasif = dr["CodClasif"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.IdFrecuencia = dr["IdFrecuencia"].ToString();
                item.DesripFrencu = dr["DesripFrencu"].ToString();
                item.Frecuencia = (int)dr["Frecuencia"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.Planta = (int)dr["Planta"];
                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["IniProgram"].ToString() != "")
                { item.IniProgram = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["FchEjecucion"].ToString() != "")
                { item.FchEjecucion = Convert.ToDateTime(dr["FchEjecucion"]); }
                item.UserEjecuto = dr["UserEjecuto"].ToString();
                item.Observaciones = dr["Observaciones"].ToString();
                if (dr["FecProgramada"].ToString() != "")
                { item.FecProgramada = Convert.ToDateTime(dr["FecProgramada"]); }
            }

            return (item);
        }

        public List<CheckListEqDt> GetChecklistAct(string cnxSql, int pId, string pDepto)
        {
            List<DataRow> lstDatos = null;
            List<CheckListEqDt> lstDt = new List<CheckListEqDt>();

            string cmdSql = @"SELECT d.IdDtCheckList, d.IdChkEquipo, d.CodWorkCenter, d.CodEquipo, d.IdCheckList, d.CodChkList, d.CodGpoActiv, d.idActividad,
                           d.CodActividad ,a.DescripcionAct, s.CodSistema,s.Sistema as DescripSistema ,c.IdComponente,c.DescripCompo,
                           d.Orden, d.TipoActividad, d.TipoOperacion, d.EqParado, d.RangoMin, d.OperadorMin, d.RangoMax, d.OperadorMax, 
                           isnull(d.CodUom, '') as CodUom, isnull(u.Descrip,'') as DescripUom,
                           d.Ponderacion, d.Activo, d.Item, ResultVisual, ResultMedible, ResultActiv
                           FROM [CheckListCapDet]  d
                           inner join[CatActivChkLst] a on a.IdActividad = d.idActividad
                           inner join[CatSistemasEquipos] s on s.CodSistema = a.CodSistema
                           inner join[CatComponentes] c on c.IdComponente = a.IdComponente
                           left join[CatUom] u on u.CodUom = d.CodUom
                           Where d.IdChkEquipo =  @pId
                           order by d.Orden";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdSql;
            cmd.Parameters.AddWithValue("@pId", pId);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, cmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    CheckListEqDt item = new CheckListEqDt();

                    item.IdDtCheckList = (int)dr["IdDtCheckList"];
                    item.IdChkEquipo = (int)dr["IdChkEquipo"];
                    item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                    item.CodEquipo = dr["CodEquipo"].ToString();
                    item.IdCheckList = (int)dr["IdCheckList"];
                    item.CodChkList = dr["CodChkList"].ToString();
                    item.CodGpoActiv = dr["CodGpoActiv"].ToString();
                    item.IdActividad = (int)dr["idActividad"];
                    item.CodActividad = dr["CodActividad"].ToString();
                    item.DescripcionAct = dr["DescripcionAct"].ToString();
                    item.CodSistema = dr["CodSistema"].ToString();
                    item.DescripSistema = dr["DescripSistema"].ToString();
                    item.IdComponente = (int)dr["IdComponente"];
                    item.DescripCompo = dr["DescripCompo"].ToString();
                    item.Orden = (int)dr["Orden"];
                    item.TipoActividad = dr["TipoActividad"].ToString();
                    item.TipoOperacion = dr["TipoOperacion"].ToString();
                    item.EqParado = (bool)dr["EqParado"];
                    item.RangoMin = (decimal)dr["RangoMin"];
                    item.OperadorMin = dr["OperadorMin"].ToString();
                    item.RangoMax = (decimal)dr["RangoMax"];
                    item.OperadorMax = dr["OperadorMax"].ToString();
                    item.Ponderacion = (decimal)dr["Ponderacion"];
                    item.Activo = (bool)dr["Activo"];
                    item.Item = (int)dr["Item"];
                    if (dr["ResultVisual"] == DBNull.Value) { item.ResultVisual = null; }
                    else { item.ResultVisual = (bool)dr["ResultVisual"]; }
                    if (dr["ResultMedible"] == DBNull.Value) { item.ResultMedible = null; }
                    else { item.ResultMedible = (decimal)dr["ResultMedible"]; }
                    item.CodUom = dr["CodUom"].ToString();
                    item.DescripUom = dr["DescripUom"].ToString();
                    item.Criterio = "";
                    if (item.TipoOperacion == "M")
                    {
                        if (!string.IsNullOrEmpty(item.OperadorMin))
                        {
                            item.Criterio = item.OperadorMin.Trim() + " " + item.RangoMin.ToString().Trim();
                        }
                        if (!string.IsNullOrEmpty(item.OperadorMax))
                        {
                            item.Criterio = item.Criterio + " y " + item.OperadorMax.Trim() + " " + item.RangoMax.ToString().Trim();
                        }
                    }

                    if (dr["ResultActiv"] == DBNull.Value) { item.ResultActiv = null; }
                    else { item.ResultActiv = (bool)dr["ResultActiv"]; }

                    lstDt.Add(item);
                }
            }
            return (lstDt);
        }

        public int ElimActChklst(string cnxSql, int idDetchk, int idChklst, string rutalog)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();

            cmd.CommandText = @" Delete FROM [CatCheckListDet]
                              Where IdDetCheckList = @IdDetCheckList and IdCheckList =  @IdCheckList";

            cmd.Parameters.AddWithValue("@IdDetCheckList", idDetchk);
            cmd.Parameters.AddWithValue("@IdCheckList", idChklst);

            result = operDatos.Eliminar(cnxSql, cmd, rutalog);

            return result;
        }

        public List<Uom> GetCatUom(string cnxSql)
        {
            List<DataRow> lstDatos = null;
            List<Uom> lstTemp = new List<Uom>();

            string cmdSql = @" SELECT Id, CodUom, Descrip, Orden, GrupoUom
                              FROM [CatUom]
                                Where Status = 1
                                order by Descrip";

            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                Uom item = new Uom();

                item.Id = (int)dr["Id"];
                item.CodUom = dr["CodUom"].ToString();
                item.Descrip = dr["Descrip"].ToString();
                item.Orden = (int)dr["Orden"];
                item.GrupoUom = dr["GrupoUom"].ToString();
                lstTemp.Add(item);
            }

            return (lstTemp);
        }

        public List<CheckListEqEnc> GetCheckProg(string cnxSql, int pPlanta, string pDepto, string pCtroCtos)
        {
            List<DataRow> lstDatos = null;
            List<CheckListEqEnc> lstTemp = new List<CheckListEqEnc>();
            string cmdSql = "";

            cmdSql = @" SELECT e.IdChkEquipo, e.CodWorkCenter, e.CodEquipo, e.IdCheckList, e.CodChkList, e.DescripChkList,
                        e.CodClasif, e.EqParado, e.Activo, e.IdFrecuencia, c.Descripcion as DesripFrencu, e.Frecuencia, 
                        e.CodDepartamento, e.UserAlta, e.FchAlta, e.UserModif, e.FchModif, e.IniProgram, e.UltimaEjec, e.UserEjecuto,
						      CodDepartamento, Planta, FecProgramada
                        FROM [CheckListxEqEnc] e
                        left Join [CatCiclos] c on c.CodCiclo = e.IdFrecuencia
                        Where e.Activo = 1  and e.IniProgram is not null and e.IniProgram < getdate() 
                              and e.CodDepartamento = @pDepto 
                              and Planta = @pPlanta
                        order by e.IdChkEquipo";

            OperDatosSql datosSql = new OperDatosSql();
            SqlCommand sc = new SqlCommand();

            sc.CommandText = cmdSql;
            sc.Parameters.AddWithValue("@pDepto", pDepto);
            sc.Parameters.AddWithValue("@pPlanta", pPlanta);


            lstDatos = datosSql.LeeDatosConParam(cnxSql, sc);

            foreach (DataRow dr in lstDatos)
            {
                CheckListEqEnc item = new CheckListEqEnc();

                item.IdChkEquipo = (int)dr["IdChkEquipo"];
                item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                item.CodEquipo = dr["CodEquipo"].ToString();
                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodChkList = dr["CodChkList"].ToString();
                item.DescripChkList = dr["DescripChkList"].ToString();
                item.CodClasif = dr["CodClasif"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.IdFrecuencia = dr["IdFrecuencia"].ToString();
                item.DesripFrencu = dr["DesripFrencu"].ToString();
                item.Frecuencia = (int)dr["Frecuencia"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.UserAlta = dr["UserAlta"].ToString();
                if (dr["FchAlta"].ToString() != "")
                { item.FchAlta = Convert.ToDateTime(dr["FchAlta"]); }
                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["IniProgram"].ToString() != "")
                { item.IniProgram = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["UltimaEjec"].ToString() != "")
                { item.UltimaEjec = Convert.ToDateTime(dr["UltimaEjec"]); }
                item.UserEjecuto = dr["UserEjecuto"].ToString();
                item.Planta = (int)dr["Planta"];
                if (dr["FecProgramada"].ToString() != "")
                { item.FecProgramada = Convert.ToDateTime(dr["FecProgramada"]); }
                else
                { item.FecProgramada = null; }
                item.Planta = (int)dr["Planta"];
                item.CentroCostos = pCtroCtos;

                lstTemp.Add(item);
            }
            return (lstTemp);
        }

        public List<CheckListEqEnc> GetCheckProgAll(string cnxSql)
        {
            List<DataRow> lstDatos = null;
            List<CheckListEqEnc> lstTemp = new List<CheckListEqEnc>();
            string cmdSql = "";

            cmdSql = @" SELECT e.IdChkEquipo, e.CodWorkCenter, e.CodEquipo, e.IdCheckList, e.CodChkList, e.DescripChkList,
                        e.CodClasif, e.EqParado, e.Activo, e.IdFrecuencia, c.Descripcion as DesripFrencu, e.Frecuencia, 
                        e.CodDepartamento, e.UserAlta, e.FchAlta, e.UserModif, e.FchModif, e.IniProgram, e.UltimaEjec, e.UserEjecuto,
						      CodDepartamento, Planta, FecProgramada, e.CentroCostos
                        FROM [CheckListxEqEnc] e
                        left Join [CatCiclos] c on c.CodCiclo = e.IdFrecuencia
                        Where e.Activo = 1  and e.IniProgram is not null and e.IniProgram < getdate()                              
                        order by e.IdChkEquipo";

            OperDatosSql datosSql = new OperDatosSql();
            SqlCommand sc = new SqlCommand();


            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                CheckListEqEnc item = new CheckListEqEnc();

                item.IdChkEquipo = (int)dr["IdChkEquipo"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.CentroCostos = dr["CentroCostos"].ToString();
                item.Planta = (int)dr["Planta"];
                item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                item.CodEquipo = dr["CodEquipo"].ToString();
                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodChkList = dr["CodChkList"].ToString();
                item.DescripChkList = dr["DescripChkList"].ToString();
                item.CodClasif = dr["CodClasif"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.IdFrecuencia = dr["IdFrecuencia"].ToString();
                item.DesripFrencu = dr["DesripFrencu"].ToString();

                item.UserAlta = dr["UserAlta"].ToString();
                if (dr["FchAlta"].ToString() != "")
                { item.FchAlta = Convert.ToDateTime(dr["FchAlta"]); }
                item.UserModif = dr["UserModif"].ToString();
                if (dr["FchModif"].ToString() != "")
                { item.FchModif = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["IniProgram"].ToString() != "")
                { item.IniProgram = Convert.ToDateTime(dr["FchModif"]); }
                if (dr["UltimaEjec"].ToString() != "")
                { item.UltimaEjec = Convert.ToDateTime(dr["UltimaEjec"]); }
                item.UserEjecuto = dr["UserEjecuto"].ToString();
                item.Planta = (int)dr["Planta"];
                if (dr["FecProgramada"].ToString() != "")
                { item.FecProgramada = Convert.ToDateTime(dr["FecProgramada"]); }
                else
                { item.FecProgramada = null; }


                item.Frecuencia = (int)dr["Frecuencia"];
                lstTemp.Add(item);
            }
            return (lstTemp);
        }

        public int GuardarchkEncProg(string cnxSql, CheckListEqEnc chkxEq)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @" INSERT INTO [CheckListCapEnc]
                                    (CodWorkCenter, CodEquipo, IdCheckList, CodChkList, DescripChkList, CodClasif, EqParado, Activo, CodDepartamento,
                                      UserModif, FchModif, Frecuencia, IdFrecuencia, IniProgram, Planta, CentroCostos, FecProgramada )
                              VALUES (@CodWorkCenter, @CodEquipo, @IdCheckList, @CodChkList, @DescripChkList, @CodClasif, @EqParado, @Activo, @CodDepartamento,
                                    @UserModif, @FchModif, @Frecuencia, @IdFrecuencia, @IniProgram, @planta, @CentroCostos, @FecProgram) 
                                    SELECT SCOPE_IDENTITY() ";

            cmd.Parameters.AddWithValue("@CodWorkCenter", chkxEq.CodWorkCenter);
            cmd.Parameters.AddWithValue("@CodEquipo", chkxEq.CodEquipo);
            cmd.Parameters.AddWithValue("@IdCheckList", chkxEq.IdCheckList);
            cmd.Parameters.AddWithValue("@CodChkList", chkxEq.CodChkList);
            cmd.Parameters.AddWithValue("@DescripChkList", chkxEq.DescripChkList);
            cmd.Parameters.AddWithValue("@CodClasif", chkxEq.CodClasif);
            cmd.Parameters.AddWithValue("@EqParado", chkxEq.EqParado);
            cmd.Parameters.AddWithValue("@Activo", chkxEq.Activo);
            cmd.Parameters.AddWithValue("@CodDepartamento", chkxEq.CodDepartamento);
            cmd.Parameters.AddWithValue("@Frecuencia", chkxEq.Frecuencia);
            cmd.Parameters.AddWithValue("@IdFrecuencia", chkxEq.IdFrecuencia);
            cmd.Parameters.AddWithValue("@UserModif", chkxEq.UserModif);
            cmd.Parameters.AddWithValue("@FchModif", chkxEq.FchModif);

            cmd.Parameters.AddWithValue("@IniProgram", chkxEq.IniProgram);
            //cmd.Parameters.AddWithValue("@FchEjecucion", chkxEq.FchEjecucion);
            //cmd.Parameters.AddWithValue("@UserEjecuto", chkxEq.UserEjecuto);
            //cmd.Parameters.AddWithValue("@Observaciones", chkxEq.Observaciones);
            cmd.Parameters.AddWithValue("@planta", chkxEq.Planta);
            cmd.Parameters.AddWithValue("@FecProgram", chkxEq.FecProgramada);
            cmd.Parameters.AddWithValue("@CentroCostos", chkxEq.CentroCostos);


            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.GuardarReturId(cnxSql, cmd);
            return result;
        }

        public int GuardarchkActProg(string cnxSql, CheckListEqDt newChkAct, string rutalog)
        {
            int result = 0;
            OperDatosSql operDatos = new OperDatosSql();

            SqlCommand cmd = new SqlCommand();

            cmd.Parameters.Clear();
            if (newChkAct.ResultVisual != null)
            {
                cmd.CommandText = @"INSERT INTO  [CheckListCapDet]
                              (IdChkEquipo,CodWorkCenter,CodEquipo,IdCheckList,CodChkList,CodGpoActiv,idActividad,CodActividad
                              ,TipoActividad,TipoOperacion,EqParado,RangoMin,OperadorMin,RangoMax,OperadorMax,Ponderacion,Activo, Orden, Item, CodUom, ResultVisual,ResultMedible, ResultActiv)
                              VALUES (@IdChkEquipo, @CodWorkCenter, @CodEquipo, @IdCheckList,@CodChkList,@CodGpoActiv,@idActividad,@CodActividad
                              ,@TipoActividad,@TipoOperacion,@EqParado,@RangoMin,@OperadorMin,@RangoMax,@OperadorMax,@Ponderacion,@Activo,@Orden,@Item, @CodUom, @ResultVisual, @ResultMedible, @ResultActiv)";
            }
            else
            {
                cmd.CommandText = @"INSERT INTO  [CheckListCapDet]
                              (IdChkEquipo,CodWorkCenter,CodEquipo,IdCheckList,CodChkList,CodGpoActiv,idActividad,CodActividad
                              ,TipoActividad,TipoOperacion,EqParado,RangoMin,OperadorMin,RangoMax,OperadorMax,Ponderacion,Activo, Orden, Item, CodUom)
                              VALUES (@IdChkEquipo, @CodWorkCenter, @CodEquipo, @IdCheckList,@CodChkList,@CodGpoActiv,@idActividad,@CodActividad
                              ,@TipoActividad,@TipoOperacion,@EqParado,@RangoMin,@OperadorMin,@RangoMax,@OperadorMax,@Ponderacion,@Activo,@Orden,@Item, @CodUom)";
            }
            cmd.Parameters.AddWithValue("@IdChkEquipo", newChkAct.IdChkEquipo);
            cmd.Parameters.AddWithValue("@CodWorkCenter", newChkAct.CodWorkCenter);
            cmd.Parameters.AddWithValue("@CodEquipo", newChkAct.CodEquipo);
            cmd.Parameters.AddWithValue("@IdCheckList", newChkAct.IdCheckList);
            cmd.Parameters.AddWithValue("@CodChkList", newChkAct.CodChkList);

            cmd.Parameters.AddWithValue("@CodGpoActiv", newChkAct.CodGpoActiv);
            cmd.Parameters.AddWithValue("@idActividad", newChkAct.IdActividad);
            cmd.Parameters.AddWithValue("@CodActividad", newChkAct.CodActividad);

            cmd.Parameters.AddWithValue("@TipoActividad", newChkAct.TipoActividad);
            cmd.Parameters.AddWithValue("@TipoOperacion", newChkAct.TipoOperacion);

            cmd.Parameters.AddWithValue("@EqParado", newChkAct.EqParado);
            cmd.Parameters.AddWithValue("@RangoMin", newChkAct.RangoMin);
            cmd.Parameters.AddWithValue("@OperadorMin", newChkAct.OperadorMin);
            cmd.Parameters.AddWithValue("@RangoMax", newChkAct.RangoMax);
            cmd.Parameters.AddWithValue("@OperadorMax", newChkAct.OperadorMax);
            cmd.Parameters.AddWithValue("@Ponderacion", newChkAct.Ponderacion);
            cmd.Parameters.AddWithValue("@Activo", newChkAct.Activo);
            cmd.Parameters.AddWithValue("@Orden", newChkAct.Orden);
            cmd.Parameters.AddWithValue("@Item", newChkAct.Item);
            cmd.Parameters.AddWithValue("@CodUom", newChkAct.CodUom);
            if (newChkAct.ResultVisual != null)
            {
                cmd.Parameters.AddWithValue("@ResultVisual", newChkAct.ResultVisual);
                cmd.Parameters.AddWithValue("@ResultMedible", newChkAct.ResultMedible);
                cmd.Parameters.AddWithValue("@ResultActiv", newChkAct.ResultActiv);

            }
            result = operDatos.Guardar(cnxSql, cmd);


            return result;
        }

        public int updateFecProg(string cnxSqlMT, CheckListEqEnc re, string pathLog)
        {
            int result = 0;
            OperDatosSql operDatos = new OperDatosSql();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"update[CheckListxEqEnc]
                              set FecProgramada = @fechaProg
                              , UltimaEjec = getdate()
                              Where IdChkEquipo = @id";

            cmd.Parameters.AddWithValue("@id", re.IdChkEquipo);
            cmd.Parameters.AddWithValue("@fechaProg", re.FecProgramada);

            result = operDatos.Update(cnxSqlMT, cmd, pathLog);

            return (result);
        }

        public List<CheckListEqEnc> GetChklstxEqProg(string cnxSql, string pDepto, int pPlanta, string pCodEquipo, string pCtroCostos)
        {
            List<DataRow> lstDatos = null;
            List<CheckListEqEnc> lstTemp = new List<CheckListEqEnc>();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = @" SELECT TOP 100 e.IdChkEquipo, e.Planta, e.CodDepartamento, e.CodWorkCenter, e.CodEquipo, e.IdCheckList,
               e.CodChkList, e.DescripChkList,e.CodClasif, e.EqParado, e.Activo, e.IdFrecuencia,
               c.Descripcion as DesripFrencu, e.Frecuencia, e.FecProgramada, f.ChkEquipo
               FROM [CheckListCapEnc] e
               LEFT JOIN (SELECT DISTINCT IdCheckList, ChkEquipo, CentroCostos, CodEquipo FROM [CheckListxEqEnc]) f
			   ON f.IdCheckList = e.IdCheckList and f.CentroCostos = e.CentroCostos and f.CodEquipo = e.CodEquipo
               LEFT JOIN [CatCiclos] c ON c.CodCiclo = e.IdFrecuencia
               WHERE e.Planta = @pPlanta
               AND ((@pDepto = '') or (e.CodDepartamento = @pDepto)) 
               AND ((@pCtroCostos = '') or (e.CentroCostos = @pCtroCostos)) 
               AND e.Activo = 1 
               AND Convert(date,FecProgramada) <= Convert(date,Getdate()) and FchEjecucion is null";

            if (pCodEquipo != null)
            {
                cmd.CommandText = cmd.CommandText + " and e.CodEquipo = @CodEquipo";

                cmd.Parameters.AddWithValue("@CodEquipo", pCodEquipo);
            }

            cmd.CommandText = cmd.CommandText + " order by Convert(date,FecProgramada)  desc";

            cmd.Parameters.AddWithValue("@pPlanta", pPlanta);
            cmd.Parameters.AddWithValue("@pCtroCostos", pCtroCostos);
            cmd.Parameters.AddWithValue("@pDepto", pDepto);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, cmd);
            foreach (DataRow dr in lstDatos)
            {
                CheckListEqEnc item = new CheckListEqEnc();

                item.IdChkEquipo = (int)dr["IdChkEquipo"];
                item.Planta = (int)dr["Planta"];
                item.CodDepartamento = dr["CodDepartamento"].ToString();
                item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                item.CodEquipo = dr["CodEquipo"].ToString();
                item.IdCheckList = (int)dr["IdCheckList"];
                item.CodChkList = dr["CodChkList"].ToString();
                item.DescripChkList = dr["DescripChkList"].ToString();
                item.CodClasif = dr["CodClasif"].ToString();
                item.EqParado = (bool)dr["EqParado"];
                item.Activo = (bool)dr["Activo"];
                item.IdFrecuencia = dr["IdFrecuencia"].ToString();
                item.DesripFrencu = dr["DesripFrencu"].ToString();
                item.Frecuencia = (int)dr["Frecuencia"];
                item.FecProgramada = Convert.ToDateTime(dr["FecProgramada"]);
                item.ChkEquipo = dr["ChkEquipo"].ToString();

                lstTemp.Add(item);
            }
            return (lstTemp);
        }

        public int UpdateCapCklstEncbProg(string cnxSql, CheckListEqEnc chkxEq)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"Update [CheckListCapEnc]
                              set FchEjecucion = @FchEjecucion,
                                  UserEjecuto = @UserEjecuto, 
                                  Observaciones = @Observaciones
                              Where  IdChkEquipo = @Id
							            and Planta = @pPlanta
							            and CodDepartamento = @pDepto";


            cmd.Parameters.AddWithValue("@Id", chkxEq.IdChkEquipo);
            cmd.Parameters.AddWithValue("@pPlanta", chkxEq.Planta);
            cmd.Parameters.AddWithValue("@pDepto", chkxEq.CodDepartamento);
            cmd.Parameters.AddWithValue("@FchEjecucion", chkxEq.FchEjecucion);
            if (chkxEq.FchEjecucion != null && chkxEq.UserEjecuto == "")
                chkxEq.UserEjecuto = "ADMINISTRADOR SISTEMA";
            cmd.Parameters.AddWithValue("@UserEjecuto", chkxEq.UserEjecuto);
            cmd.Parameters.AddWithValue("@Observaciones", chkxEq.Observaciones);

            OperDatosSql operDatos = new OperDatosSql();
            result = operDatos.Guardar(cnxSql, cmd);
            return result;
        }

        public int UpdateCapActProg(string cnxSql, CapturaChklst NewChkAct, string rutalog)
        {
            int result = 0;
            OperDatosSql operDatos = new OperDatosSql();

            SqlCommand cmd = new SqlCommand();
            foreach (var j in NewChkAct.lstChckActEq)
            {

                cmd.Parameters.Clear();
                cmd.CommandText = @"Update [CheckListCapDet]
                                 Set ResultVisual = @RV,
	                                 ResultMedible = @RM,
	                                 ResultActiv = @RA
                                 where IdDtCheckList  = @IdDt 
                                  and  IdChkEquipo = @IdChkEquipo";


                cmd.Parameters.AddWithValue("@IdDt", j.IdDtCheckList);
                cmd.Parameters.AddWithValue("@IdChkEquipo", j.IdChkEquipo);
                cmd.Parameters.AddWithValue("@RV", j.ResultVisual);
                cmd.Parameters.AddWithValue("@RM", j.ResultMedible);
                cmd.Parameters.AddWithValue("@RA", j.ResultActiv);

                result = operDatos.Guardar(cnxSql, cmd);
            }

            return result;
        }

        public List<CheckListEqDt> GetChecklistActProg(string cnxSql, int pId, string pDepto)
        {
            List<DataRow> lstDatos = null;
            List<CheckListEqDt> lstDt = new List<CheckListEqDt>();

            string cmdSql = @"SELECT d.IdDtCheckList, d.IdChkEquipo, d.CodWorkCenter, d.CodEquipo, d.IdCheckList, d.CodChkList, d.CodGpoActiv, d.idActividad,
                           d.CodActividad ,a.DescripcionAct, s.CodSistema,s.Sistema as DescripSistema ,c.IdComponente,c.DescripCompo,
                           d.Orden, d.TipoActividad, d.TipoOperacion, d.EqParado, d.RangoMin, d.OperadorMin, d.RangoMax, d.OperadorMax, 
                           isnull(d.CodUom, '') as CodUom, isnull(u.Descrip,'') as DescripUom,
                           d.Ponderacion, d.Activo, d.Item, ResultVisual, ResultMedible
                           FROM [CheckListCapDet]  d
                           inner join[CatActivChkLst] a on a.IdActividad = d.idActividad
                           inner join[CatSistemasEquipos] s on s.CodSistema = a.CodSistema
                           inner join[CatComponentes] c on c.IdComponente = a.IdComponente
                           left join[CatUom] u on u.CodUom = d.CodUom
                           Where d.IdChkEquipo =  @pId
                           order by d.Orden";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdSql;
            cmd.Parameters.AddWithValue("@pId", pId);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, cmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    CheckListEqDt item = new CheckListEqDt();

                    item.IdDtCheckList = (int)dr["IdDtCheckList"];
                    item.IdChkEquipo = (int)dr["IdChkEquipo"];
                    item.CodWorkCenter = dr["CodWorkCenter"].ToString();
                    item.CodEquipo = dr["CodEquipo"].ToString();
                    item.IdCheckList = (int)dr["IdCheckList"];
                    item.CodChkList = dr["CodChkList"].ToString();
                    item.CodGpoActiv = dr["CodGpoActiv"].ToString();
                    item.IdActividad = (int)dr["idActividad"];
                    item.CodActividad = dr["CodActividad"].ToString();
                    item.DescripcionAct = dr["DescripcionAct"].ToString();
                    item.CodSistema = dr["CodSistema"].ToString();
                    item.DescripSistema = dr["DescripSistema"].ToString();
                    item.IdComponente = (int)dr["IdComponente"];
                    item.DescripCompo = dr["DescripCompo"].ToString();
                    item.Orden = (int)dr["Orden"];
                    item.TipoActividad = dr["TipoActividad"].ToString();
                    item.TipoOperacion = dr["TipoOperacion"].ToString();
                    item.EqParado = (bool)dr["EqParado"];
                    item.RangoMin = (decimal)dr["RangoMin"];
                    item.OperadorMin = dr["OperadorMin"].ToString();
                    item.RangoMax = (decimal)dr["RangoMax"];
                    item.OperadorMax = dr["OperadorMax"].ToString();
                    item.Ponderacion = (decimal)dr["Ponderacion"];
                    item.Activo = (bool)dr["Activo"];
                    item.Item = (int)dr["Item"];
                    item.ResultVisual = null;
                    item.ResultMedible = null;
                    item.ResultActiv = null;
                    item.CodUom = dr["CodUom"].ToString();
                    item.DescripUom = dr["DescripUom"].ToString();
                    item.Criterio = "";
                    if (item.TipoOperacion == "M")
                    {
                        if (!string.IsNullOrEmpty(item.OperadorMin))
                        {
                            item.Criterio = item.OperadorMin.Trim() + " " + item.RangoMin.ToString().Trim();
                        }
                        if (!string.IsNullOrEmpty(item.OperadorMax))
                        {
                            item.Criterio = item.Criterio + " y " + item.OperadorMax.Trim() + " " + item.RangoMax.ToString().Trim();
                        }
                    }

                    lstDt.Add(item);
                }
            }
            return (lstDt);
        }

        public List<DataRow> GetInfoChlst(string cnxSqlMT, string periIni, string periFin, string ctroCostos, string depto, string workCenter)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();

            if (workCenter != null)
            {
                cmd.CommandText = @" SELECT Convert(varChar,year(FecProgramada)) +'-'+ REPLACE(STR(month(FecProgramada), 2), SPACE(1), '0') as periodo 
                                     ,Planta, CentroCostos, CodDepartamento, 
                                     IdChkEquipo, CodWorkCenter, CodEquipo, IdCheckList, CodChkList, DescripChkList, CodClasif,
                                     IdFrecuencia, Frecuencia, Observaciones, FecProgramada, FchEjecucion
                                 FROM [CheckListCapEnc]
                                 where ((@cc = '') or (CentroCostos = @cc)) 
                                     and ((@Depto = '') or (CodDepartamento = @Depto)) 
                                     and CodEquipo = @wc
                                     and Convert(varChar,year(FecProgramada)) + REPLACE(STR(month(FecProgramada), 2), SPACE(1), '0')  >= @periIni 
                                     and Convert(varChar,year(FecProgramada)) + REPLACE(STR(month(FecProgramada), 2), SPACE(1), '0')  <= @periFin ";

                cmd.Parameters.AddWithValue("@cc", ctroCostos);
                cmd.Parameters.AddWithValue("@Depto", depto);
                cmd.Parameters.AddWithValue("@periIni", periIni);
                cmd.Parameters.AddWithValue("@pEriFin", periFin);
                cmd.Parameters.AddWithValue("@wc", workCenter);

            }
            else
            {
                cmd.CommandText = @" SELECT Convert(varChar,year(FecProgramada)) +'-'+ REPLACE(STR(month(FecProgramada), 2), SPACE(1), '0') as periodo 
                                     ,Planta, CentroCostos, CodDepartamento, 
                                     IdChkEquipo, CodWorkCenter, CodEquipo, IdCheckList, CodChkList, DescripChkList, CodClasif,
                                     IdFrecuencia, Frecuencia, Observaciones, FecProgramada, FchEjecucion
                                 FROM [CheckListCapEnc]
                                  where ((@cc = '') or (CentroCostos = @cc))
                                     and ((@Depto = '') or (CodDepartamento = @Depto)) 
                                     and Convert(varChar,year(FecProgramada)) + REPLACE(STR(month(FecProgramada), 2), SPACE(1), '0')  >= @periIni 
                                     and Convert(varChar,year(FecProgramada)) + REPLACE(STR(month(FecProgramada), 2), SPACE(1), '0')  <= @periFin ";

                cmd.Parameters.AddWithValue("@cc", ctroCostos);
                cmd.Parameters.AddWithValue("@Depto", depto);
                cmd.Parameters.AddWithValue("@periIni", periIni);
                cmd.Parameters.AddWithValue("@pEriFin", periFin);

            }
            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSqlMT, cmd);

            return (lstDatos);
        }

        public int ElimActChklstEq(string cnxSql, int idDetchkEq, int idChklstEq, string rutalog)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand();
            OperDatosSql operDatos = new OperDatosSql();

            cmd.CommandText = @"Delete FROM [CheckListxEqDet]
                             Where IdDtCheckList = @IdDtCheckList and IdChkEquipo =  @IdChkEquipo";

            cmd.Parameters.AddWithValue("@IdDtCheckList", idDetchkEq);
            cmd.Parameters.AddWithValue("@IdChkEquipo", idChklstEq);

            result = operDatos.Eliminar(cnxSql, cmd, rutalog);

            return result;
        }

        public List<Mes> GetCatMeses(string cnxSql)
        {
            List<DataRow> lstDatos = null;
            List<Mes> lstTemp = new List<Mes>();

            string cmdSql = @" SELECT Mes, DescripEsp, DescripIng, AbrevEsp, AbrevIng, DiaInicio, DiaFin,  DiaInicioEsp, DiaFinEsp 
                            FROM [CatMeses]";

            OperDatosSql datosSql = new OperDatosSql();

            lstDatos = datosSql.LeeDatos(cnxSql, cmdSql);

            foreach (DataRow dr in lstDatos)
            {
                Mes item = new Mes();

                item.NumMes = (int)dr["Mes"];
                item.DescripEsp = dr["DescripEsp"].ToString();
                item.DescripIng = dr["DescripIng"].ToString();
                item.AbrevEsp = dr["AbrevEsp"].ToString();
                item.AbrevIng = dr["AbrevIng"].ToString();
                item.DiaInicio = dr["DiaInicio"].ToString();
                item.DiaFin = dr["DiaFin"].ToString();
                item.DiaInicioEsp = dr["DiaInicioEsp"].ToString();
                item.DiaFinEsp = dr["DiaFinEsp"].ToString();
                lstTemp.Add(item);
            }

            return (lstTemp);
        }

        public int GuardarTpmHisto(string cnxSql, int pPlanta, string pDepto, string pCtroCostos, List<EquipoTpmBasico> lstTpm, string fileLog, string fileBug)
        {
            int result = 0;
            DataTable dt = null;

            string cmdSql = " SELECT * ";
            cmdSql = cmdSql + "  FROM [TpmCalcHistorico] ";
            cmdSql = cmdSql + " Where Planta = '9999' ";

            OperDatosSql datosSql = new OperDatosSql();
            DateTime fec = DateTime.Now;

            dt = datosSql.GetStructTabla(cnxSql, cmdSql);
            int cnt = 1;
            foreach (var e in lstTpm)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = cnt;
                dr["FecCalculo"] = fec;
                dr["Planta"] = pPlanta;
                dr["CentroCostos"] = pCtroCostos;
                dr["CodDepartamento"] = pDepto;
                dr["CodWorkCenter"] = e.CodWorkCenter;
                dr["CodEquipo"] = e.CodEquipo;
                dr["DescripTechnical"] = e.DescripTechnical;
                dr["IndivStatusObject"] = e.IndivStatusObject;
                dr["TypeTechObj"] = e.TypeTechObj;
                dr["PlannerGroup"] = e.PlannerGroup;
                dr["CostCenter"] = e.CostCenter;
                dr["MainWorkCenter"] = e.MainWorkCenter;
                dr["NombreMwc"] = e.NombreMwc;
                dr["FunctionalLocation"] = e.FunctionalLocation;
                dr["ManufAsset"] = e.ManufAsset;
                dr["ManufModelNum"] = e.ManufModelNum;
                dr["ValidFromDate"] = e.ValidFromDate;
                dr["Superordinate"] = e.Superordinate;
                dr["StandardTextKeyWC"] = e.StandardTextKeyWC;
                dr["PorcAvance"] = e.PorcAvance;
                dr["Avance"] = e.Avance;
                dr["NumFallas"] = e.NumFallas;
                dr["PmStandar"] = e.PmStandar;
                dr["PmReal"] = e.PmReal;
                dr["UltFalla"] = e.UltFalla;
                dr["TipoFalla"] = e.TipoFalla;
                dr["Codciclo"] = e.Codciclo;
                dr["Ciclo"] = e.Ciclo;
                dr["Frecuencia"] = e.Frecuencia;
                dr["PzasProducidas"] = e.PzasProducidas;
                dr["PzasScrap"] = e.PzasScrap;
                if (e.FecUltManto == Convert.ToDateTime("01/01/0001"))
                {
                    dr["fecUltManto"] = DBNull.Value;
                }
                else
                {
                    dr["fecUltManto"] = e.FecUltManto;
                }
                dr["HallazgoSeguridad"] = e.HallazgoSeguridad;
                dr["Sensor"] = e.Sensor;

                dt.Rows.Add(dr);
                cnt = cnt + 1;
            }

            result = datosSql.GuardarTablaSql(cnxSql, dt, "TpmCalcHistorico", fileLog, fileBug);

            return result;
        }

        public List<Periodos> GenPeriodos(List<Mes> lstMeses, int aIni, int mIni, int aFin, int mFin)
        {
            List<Periodos> lstTemp = new List<Periodos>();
            int x = aIni;
            int m = mIni;
            int index = 0;
            while (x <= aFin)
            {
                if (x == aFin)
                {
                    while (m <= mFin)
                    {
                        index = lstMeses.FindIndex(j => j.NumMes == m);
                        if (index > -1)
                        {
                            lstTemp.Add(new Periodos
                            {
                                Periodo = x.ToString().Trim() + "-" + m.ToString().Trim().PadLeft(2, '0'),
                                Anio = x,
                                Mes = m,
                                IniMes = lstMeses[index].DiaInicioEsp,
                                FinMes = lstMeses[index].DiaFinEsp,
                                DescripEsp = lstMeses[index].DescripEsp,
                                AbrevEsp = lstMeses[index].AbrevEsp
                            });
                        }
                        else
                        {
                            lstTemp.Add(new Periodos
                            {
                                Periodo = x.ToString().Trim() + "-" + m.ToString().Trim().PadLeft(2, '0'),
                                Anio = x,
                                Mes = m
                            });
                        }
                        m = m + 1;
                    }
                }
                else
                {
                    while (m <= 12)
                    {
                        index = lstMeses.FindIndex(j => j.NumMes == m);
                        if (index > -1)
                        {
                            lstTemp.Add(new Periodos
                            {
                                Periodo = x.ToString().Trim() + "-" + m.ToString().Trim().PadLeft(2, '0'),
                                Anio = x,
                                Mes = m,
                                IniMes = lstMeses[index].DiaInicio,
                                FinMes = lstMeses[index].DiaFin,
                                DescripEsp = lstMeses[index].DescripEsp,
                                AbrevEsp = lstMeses[index].AbrevEsp
                            });
                        }
                        else
                        {
                            lstTemp.Add(new Periodos
                            {
                                Periodo = x.ToString().Trim() + "-" + m.ToString().Trim().PadLeft(2, '0'),
                                Anio = x,
                                Mes = m
                            });
                        }

                        m = m + 1;
                    }
                }

                x = x + 1;
                m = 1;
            }


            return (lstTemp);
        }


        public List<EquipoTpmBasico> GetEqCalculo(string cnxSql, int planta, string depto, string ctroCostos, int anio, int mes)
        {
            List<DataRow> lstDatos = null;
            List<EquipoTpmBasico> lstTemp = new List<EquipoTpmBasico>();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = @" SELECT h.FecCalculo, h.Planta, h.CentroCostos, h.CodDepartamento, h.CodWorkCenter, h.CodEquipo, h.DescripTechnical, h.StandardTextKeyWC,
                              h.PorcAvance, h.Avance, h.NumFallas, h.PmStandar, h.PmReal, h.PzasProducidas, h.fecUltManto, h.UltFalla, h.TipoFalla,
                              iif(h.PmStandar >0, h.PMReal/ h.PMStandar,0) as Efic 
                              FROM [TpmCalcHistorico] h, 
	                                  (Select Max(FecCalculo) as fecCalcu 
                              FROM [TpmCalcHistorico]
                              Where year(Convert(date,FecCalculo))= @anio
                              and month(Convert(date,FecCalculo))= @mes
                              and ((@ctroCtos = '') or (CentroCostos = @ctroCtos))) j
                                  Where h.Planta = @planta
                                  and ((@ctroCtos = '') or (h.CentroCostos = @ctroCtos))
                                  and ((@depto = '') or (h.CodDepartamento = @depto))
                                  and h.FecCalculo = j.fecCalcu";

            cmd.Parameters.AddWithValue("@planta", planta);
            cmd.Parameters.AddWithValue("@ctroCtos", ctroCostos);
            cmd.Parameters.AddWithValue("@depto", depto);

            cmd.Parameters.AddWithValue("@anio", anio);
            cmd.Parameters.AddWithValue("@mes", mes);


            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, cmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    EquipoTpmBasico i = new EquipoTpmBasico();

                    i.Planta = (int)dr["Planta"];
                    i.CodCentroCostos = dr["CentroCostos"].ToString();
                    i.CodDepartamento = dr["CodDepartamento"].ToString();
                    i.FecCalculo = Convert.ToDateTime(dr["FecCalculo"]);
                    i.CodWorkCenter = dr["CodWorkCenter"].ToString();
                    i.CodEquipo = dr["CodEquipo"].ToString();
                    i.DescripTechnical = dr["DescripTechnical"].ToString();
                    i.StandardTextKeyWC = dr["StandardTextKeyWC"].ToString();
                    i.PorcAvance = (int)dr["PorcAvance"];
                    i.Avance = (int)dr["Avance"];
                    i.NumFallas = (int)dr["NumFallas"];
                    i.PmStandar = (decimal)dr["PmStandar"];
                    i.PmReal = (decimal)dr["PmReal"];
                    i.UltFalla = dr["UltFalla"].ToString();
                    i.TipoFalla = dr["TipoFalla"].ToString(); ;
                    i.PzasProducidas = (decimal)dr["PzasProducidas"];
                    if (dr["fecUltManto"].ToString() != "") i.FecUltManto = Convert.ToDateTime(dr["FecUltManto"]);
                    i.Eficiencia = Math.Round((decimal)dr["Efic"], 0) * 100;

                    lstTemp.Add(i);
                }
            }

            return (lstTemp);
        }
        public List<DataRow> GenResumenTick(string cnxSql, int pPlanta, string pDepto, string pCtroCostos, DateTime pFi, DateTime pFf)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = @" SELECT Year(FchReporte)  as Anio, Month(FchReporte) as Mes,
                              Sum(iif(TipoTicket <> 'M',1,0)) as TotTickets, 
                              Sum(iif( CodStatus = 'CER' and TipoTicket <> 'M',1,0)) as Cerrados,
                              Sum(iif( CodStatus <> 'CER' and TipoTicket <> 'M',1,0)) as Abiertos,
                              Sum(iif( CodStatus = 'NVO' and TipoTicket <> 'M',1,0)) as Nuevos,
                              Sum(iif( CodStatus <> 'NVO' and CodStatus <> 'CER' and TipoTicket <> 'M',1,0)) as EnProceso,
                              Sum(iif( TipoTicket = 'A' ,1,0)) as Alertas,  Sum(iif( TipoTicket = 'R' ,1,0)) as Criticos,
                              Sum(iif( TipoTicket = 'P' ,1,0)) as Pky,      Sum(iif( TipoTicket = 'Z' ,1,0)) as Calidad,
                              Sum(iif( TipoTicket = 'G' ,1,0)) as Mejoras,  Sum(iif( TipoTicket = 'M' ,1,0)) as Manto
                              FROM [Tickets]                              
                              Where IdPlanta = @planta 
                              and ((@depto = '') or (CodDepartamento = @depto))
                              and ((@CtroCostos = '') or (CentroCostos = @CtroCostos))
                              and convert(date,FchReporte) >= convert(date,@fi) and convert(date,FchReporte) <= convert(date,@ff) 
                              and CodStatus <> 'CAN'
                              group by Year(FchReporte), Month(FchReporte)
                              order by  Year(FchReporte), Month(FchReporte)";

            cmd.Parameters.AddWithValue("@fi", pFi);
            cmd.Parameters.AddWithValue("@ff", pFf);
            cmd.Parameters.AddWithValue("@planta", pPlanta);
            cmd.Parameters.AddWithValue("@depto", pDepto);
            cmd.Parameters.AddWithValue("@CtroCostos", pCtroCostos);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, cmd);

            return (lstDatos);
        }

        public List<DataRow> GenTop5WcTick(string cnxSql, int pPlanta, string pDepto, string pCtroCostos, int pAnio, int pMes)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = @"SELECT Top 5 Year(FchReporte)  as Anio,  Month(FchReporte) as Mes, CodWorkCenter,
                               Sum(iif(TipoTicket <> 'M',1,0)) as TotTickets, 
                               Sum(iif( CodStatus = 'CER' and TipoTicket <> 'M',1,0)) as Cerrados,
                               Sum(iif( CodStatus <> 'CER' and TipoTicket <> 'M',1,0)) as Abiertos
                             FROM [Tickets]                            
                             Where IdPlanta = @planta and CodDepartamento = @depto and CentroCostos = @CtroCostos                             
                                and Year(FchReporte) = @anio
                                and Month(FchReporte) = @mes 
                                and CodStatus <> 'CAN'
                                group by Year(FchReporte), Month(FchReporte), CodWorkCenter
                                order by  totTickets desc";

            cmd.Parameters.AddWithValue("@anio", pAnio);
            cmd.Parameters.AddWithValue("@mes", pMes);
            cmd.Parameters.AddWithValue("@planta", pPlanta);
            cmd.Parameters.AddWithValue("@depto", pDepto);
            cmd.Parameters.AddWithValue("@CtroCostos", pCtroCostos);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, cmd);

            return (lstDatos);
        }

        public List<EquipoPadre> GetCatEqyWc(string cnxSql, string pCtroCostos)
        {
            List<DataRow> lstDatos = null;
            List<EquipoPadre> lstEq = new List<EquipoPadre>();

            // Vamos as generar una lista de equipos padres para mostrar la descripcion a nivel de work center
            // Con el fin de que el operador carge los tickets a un solo equipo
            // el campo valioso es Superordinate, si esta en blanco quiere decir que es el equipo padre
            // para esto en sap hay que ubicarlo correctamente en los functional location
            // No se consideraran los que estan en la funct. location Down,los que tienen estatus de Inactivos y/o bajas ('DLFL','INAC') 


            string cmdSql = @"Select DISTINCT WorkCenter, CodEquipo, IndivStatusObject,FunctionalLocation, TypeTechObj, ObjectNumber, 
                               DescripTechnical, MainWorkCenter, ManufAsset, ManufModelNum, PlannerGroup, StandardTextKeyWC
                               FROM[CatEquipos]
                                     Where CostCenter = @centroCostos 
					                 and Superordinate = ''
					                 and WorkCenter is not null
					                and CodEquipo not in (Select CodEquipo
										                   FROM [CatEquipos]
												                 Where CostCenter = @centroCostos and Superordinate = ''
										                   and (IndivStatusObject in ('DLFL','INAC') or PATINDEX('%DOWN%', upper(FunctionalLocation)) != 0)
										                   group by CodEquipo)";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@CentroCostos", pCtroCostos);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, sqlcmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    EquipoPadre e = new EquipoPadre();

                    e.CodWorkCenter = dr["WorkCenter"].ToString();
                    e.CodEquipo = dr["CodEquipo"].ToString().Trim();
                    e.IndivStatusObject = dr["IndivStatusObject"].ToString();
                    e.FunctionalLocation = dr["FunctionalLocation"].ToString();
                    e.TypeTechObj = dr["TypeTechObj"].ToString();
                    e.ObjectNumber = dr["ObjectNumber"].ToString();
                    e.DescripTechnical = dr["DescripTechnical"].ToString();
                    e.MainWorkCenter = dr["MainWorkCenter"].ToString();
                    e.ManufAsset = dr["ManufAsset"].ToString();
                    e.ManufModelNum = dr["ManufModelNum"].ToString();
                    e.PlannerGroup = dr["PlannerGroup"].ToString();
                    e.StandardTextKeyWC = dr["StandardTextKeyWC"].ToString();
                    e.FocusFactory = dr["StandardTextKeyWC"].ToString();
                    e.Cod_Descrip = "[" + dr["WorkCenter"].ToString().Trim() + "]  " + dr["DescripTechnical"].ToString().Trim();
                    lstEq.Add(e);
                }
            }
            return (lstEq);
        }

        public List<DataRow> GenKpiTmMin(string cnxSql, int pTipo, int pPlanta, string pDepto, string pcCostos, int pAnio, int pMes, string pWc = "")
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();

            switch (pTipo)
            {
                case 1:
                    {
                        cmd.CommandText = @"SELECT Year(fecha_prod) as Anio, Month(fecha_prod) as Mes
                                      ,sum(Automatizacion)			as Automatizacion     
                                      ,sum(Mantto)					as Mantto    
                                      ,sum(Programado + CambioHerr)	as Programado
                                      ,sum(Troqueles)				as Troqueles
	                                  ,sum(ParoProd)				 as ParoProd
	                                  ,sum(Logistica)				as Logistica
	                                  ,sum(Error)					 as Error
	                                  ,sum(Corriendo)				 as Corriendo
	                                  ,sum(Calidad) 				 as Calidad
	                                  ,Sum(Automatizacion+Calidad+Corriendo+Error+Logistica+Mantto+ParoProd+Programado+Troqueles+CambioHerr+MinCtMeta) as TotalMinutos
                                 FROM [Produccion_Dev].[dbo].[Rep_Prod_Hora]
                                 Where Year(fecha_prod) = @anio and Month(fecha_prod) = @mes
                                 Group by Year(fecha_prod), Month(fecha_prod) ";

                        cmd.Parameters.AddWithValue("@anio", pAnio);
                        cmd.Parameters.AddWithValue("@mes", pMes);
                        break;
                    }
                case 2:
                    {
                        cmd.CommandText = @"SELECT Year(fecha_prod) as Anio, Month(fecha_prod) as Mes
                                      ,sum(Automatizacion)			as Automatizacion     
                                      ,sum(Mantto)					as Mantto    
                                      ,sum(Programado + CambioHerr)	as Programado
                                      ,sum(Troqueles)				as Troqueles
	                                  ,sum(ParoProd)				 as ParoProd
	                                  ,sum(Logistica)				as Logistica
	                                  ,sum(Error)					 as Error
	                                  ,sum(Corriendo)				 as Corriendo
	                                  ,sum(Calidad) 				 as Calidad
	                                  ,Sum(Automatizacion+Calidad+Corriendo+Error+Logistica+Mantto+ParoProd+Programado+Troqueles+CambioHerr+MinCtMeta) as TotalMinutos
                                 FROM [Produccion_Dev].[dbo].[Rep_Prod_Hora]
                                 Where Year(fecha_prod) = @anio and Month(fecha_prod) = @mes
                                       and maquina = (SELECT top 1 Maquina
                                                      FROM [Produccion_Dev].[dbo].[tblMaquinas]
                                                      where NombreSAP_Programa = @pWc)    
                                 Group by Year(fecha_prod), Month(fecha_prod) ";

                        cmd.Parameters.AddWithValue("pWc", pWc);
                        cmd.Parameters.AddWithValue("@anio", pAnio);
                        cmd.Parameters.AddWithValue("@mes", pMes);
                        break;
                    }
            }

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, cmd);


            return (lstDatos);
        }

        public List<DataRow> GenKpiTmEventos(string cnxSql, int pTipo, int pPlanta, string pDepto, string pcCostos, int pAnio, int pMes, string pWc = "")
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            switch (pTipo)
            {
                case 1:
                    {
                        cmd.CommandText = @"SELECT e.Status, s.AreaTM, count(e.IdTm) as Eventos
                                FROM (SELECT Status, IdTm FROM [Produccion_Dev].[dbo].[Rep_EventosxDiaTm]
		                                Where Year(Fecha) = @anio and Month(Fecha) = @mes group by Status, IdTm) e
                                inner join [Produccion_Dev].[dbo].[CatStatus] s on  s.codigo = e.Status
                                Group by e.Status,s.AreaTM
                                order by e.Status,s.AreaTM";

                        cmd.Parameters.AddWithValue("@anio", pAnio);
                        cmd.Parameters.AddWithValue("@mes", pMes);
                        break;
                    }
                case 2:
                    {
                        cmd.CommandText = @"SELECT e.Status, s.AreaTM, count(e.IdTm) as Eventos
                                            FROM (SELECT Status, IdTm FROM [Produccion_Dev].[dbo].[Rep_EventosxDiaTm]
		                                            Where Year(Fecha) = @anio and Month(Fecha) = @mes 
		                                            and Maquina = (SELECT top 1 Maquina
					                                               FROM [Produccion_Dev].[dbo].[tblMaquinas]
					                                               where NombreSAP_Programa = @pWc                                                                     )     
		                                            group by Status, IdTm) e
                                            inner join [Produccion_Dev].[dbo].[CatStatus] s on  s.codigo = e.Status
                                            Group by e.Status,s.AreaTM
                                            order by e.Status,s.AreaTM";

                        cmd.Parameters.AddWithValue("pWc", pWc);
                        cmd.Parameters.AddWithValue("@anio", pAnio);
                        cmd.Parameters.AddWithValue("@mes", pMes);
                        break;
                    }
            }

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSql, cmd);
            return (lstDatos);
        }

        public List<DataRow> GetpkykMes(string cnxSqlMT, int pPlanta, string pDepto, string pTipoTick, int pMesesAtras)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();

            int pMeses = pMesesAtras * (-1);


            cmd.CommandText = @"SELECT CodWorkCenter, CodEquipo,  Sensor, Count(IdTicket) as eventos
                             FROM [Tickets]               
                             Where CodDepartamento = @Depto and IdPlanta = @Planta
		                     and TipoTicket =  @TipoTicket and CodWorkCenter <> ''
		                     and  (Sensor <> '')
		                     and Convert(date,FchReporte)  >= Convert(date,DATEADD(MM, @meses , GETDATE()))
		                     and Convert(date, FchReporte) <= Convert(date,GETDATE())
		                     group by CodWorkCenter, CodEquipo,  Sensor
		                     order by CodWorkCenter, CodEquipo,  Sensor";

            cmd.Parameters.AddWithValue("@Planta", pPlanta);
            cmd.Parameters.AddWithValue("@Depto", pDepto);
            cmd.Parameters.AddWithValue("@TipoTicket", pTipoTick);
            cmd.Parameters.AddWithValue("@meses", pMeses);


            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSqlMT, cmd);

            return (lstDatos);

        }

        public List<DataRow> GetHtpkyk(string cnxSqlMT, int pPlanta, string pDepto, int pAnio, int pMes, string pTipoTick)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            DateTime pFecHt = Convert.ToDateTime("01/" + pMes.ToString() + "/" + pAnio.ToString(), CultureInfo.InvariantCulture);

            cmd.CommandText = @"SELECT CodWorkCenter, CodEquipo, Sensor, Count(IdTicket) as eventos
                                 FROM [Tickets]               
                                 Where CodDepartamento = @Depto and IdPlanta = @Planta
		                         and TipoTicket = @TipoTicket and CodWorkCenter <> ''
		                          and (Sensor <> '')
		                          and FchReporte > = @FecHt
		                         group by CodWorkCenter, CodEquipo,  Sensor
		                         order by CodWorkCenter, CodEquipo,  Sensor";

            cmd.Parameters.AddWithValue("@Planta", pPlanta);
            cmd.Parameters.AddWithValue("@Depto", pDepto);
            cmd.Parameters.AddWithValue("@TipoTicket", pTipoTick);
            cmd.Parameters.AddWithValue("@FecHt", pFecHt);


            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSqlMT, cmd);

            return (lstDatos);

        }

        public List<DataRow> GetEventosxMes(string cnxSqlMT, int pPlanta, string pDepto, int pAnio, int pMes, int pMesesAtras, string pTipoTick, List<Mes> lstMeses, bool UltMes)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            int index = 0;

            DateTime pFecIni;
            DateTime pFecFin;
            string finMes = "";

            string iniMes = "";

            if (!UltMes)
            {

                index = lstMeses.FindIndex(x => x.NumMes == pMes);
                // Sacamos el Fin del mes que solicita
                finMes = lstMeses[index].DiaFin;

                // vamos a crear los ragos de fecha de meses completos
                pFecFin = Convert.ToDateTime(finMes + "/" + pAnio.ToString(), CultureInfo.InvariantCulture);

                // le restamos los meses hacia atras
                var t = pFecFin.AddMonths(pMesesAtras);
                var m = t.Month;
                var a = t.Year;

                index = lstMeses.FindIndex(x => x.NumMes == m);
                // Sacamos el Fin del mes que solicita
                iniMes = lstMeses[index].DiaInicio;
                pFecIni = Convert.ToDateTime(iniMes + "/" + a.ToString(), CultureInfo.InvariantCulture);
            }
            else
            {
                index = lstMeses.FindIndex(x => x.NumMes == pMes);
                // Sacamos el Fin del mes que solicita
                finMes = lstMeses[index].DiaFin;

                // vamos a crear los ragos de fecha de meses completos
                pFecFin = Convert.ToDateTime(finMes + "/" + pAnio.ToString(), CultureInfo.InvariantCulture);

                // Si el ultimo mes es el mes actual..
                // para el ultimo mes vamos a restar 30 dias
                // solicitud de IB 14 abril 2021
                if (pMes == DateTime.Now.Month && pAnio == DateTime.Now.Year)
                {
                    pFecFin = DateTime.Now;
                    pFecIni = pFecFin.AddDays(-30);
                }
                else
                {
                    // le restamos los meses hacia atras
                    var t = pFecFin.AddMonths(pMesesAtras);
                    var m = t.Month;
                    var a = t.Year;

                    index = lstMeses.FindIndex(x => x.NumMes == m);
                    // Sacamos el Fin del mes que solicita
                    iniMes = lstMeses[index].DiaInicio;
                    pFecIni = Convert.ToDateTime(iniMes + "/" + a.ToString(), CultureInfo.InvariantCulture);

                }
            }

            cmd.CommandText = @" SELECT Count(IdTicket) as eventos
                                 FROM [Tickets]               
                                 Where CodDepartamento = @Depto and IdPlanta = @Planta
		                         and TipoTicket =  @TipoTicket and CodWorkCenter <> ''
		                         and  (Sensor <> '')
		                         and Convert(date,FchReporte)  >= @FecIni and Convert(date,FchReporte) <= @FecFin ";

            cmd.Parameters.AddWithValue("@Planta", pPlanta);
            cmd.Parameters.AddWithValue("@Depto", pDepto);
            cmd.Parameters.AddWithValue("@TipoTicket", pTipoTick);
            cmd.Parameters.AddWithValue("@FecIni", pFecIni);
            cmd.Parameters.AddWithValue("@FecFin", pFecFin);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSqlMT, cmd);

            return (lstDatos);
        }

        public List<DataRow> GetPkykTop5(string cnxSqlMT, int pPlanta, string pDepto, int pAnio, int pMes, int pMesesAtras, string pTipoTick, List<Mes> lstMeses, string pPareto)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            int index = 0;

            DateTime pFecIni = DateTime.Now;
            DateTime pFecFin = DateTime.Now;

            string iniMes = "";
            string finMes = "";

            if (pPareto == "ACTU")
            {
                index = lstMeses.FindIndex(x => x.NumMes == pMes);
                // Sacamos el Fin del mes que solicita
                finMes = lstMeses[index].DiaFin;

                // vamos a crear los ragos de fecha de meses completos
                pFecFin = Convert.ToDateTime(finMes + "/" + pAnio.ToString(), CultureInfo.InvariantCulture);

                // le restamos los meses hacia atras
                var t = pFecFin.AddMonths(pMesesAtras);
                var m = t.Month;
                var a = t.Year;

                index = lstMeses.FindIndex(x => x.NumMes == m);
                // Sacamos el Fin del mes que solicita
                iniMes = lstMeses[index].DiaInicio;
                pFecIni = Convert.ToDateTime(iniMes + "/" + a.ToString(), CultureInfo.InvariantCulture);

            }
            else
            {
                if (pPareto == "ULT30")
                {
                    index = lstMeses.FindIndex(x => x.NumMes == pMes);
                    // Sacamos el Fin del mes que solicita
                    finMes = lstMeses[index].DiaFin;

                    // vamos a crear los ragos de fecha de meses completos
                    pFecFin = Convert.ToDateTime(finMes + "/" + pAnio.ToString(), CultureInfo.InvariantCulture);

                    // Si el ultimo mes es el mes actual..
                    // para el ultimo mes vamos a restar 30 dias
                    // solicitud de IB 14 abril 2021
                    if (pMes == DateTime.Now.Month && pAnio == DateTime.Now.Year)
                    {
                        pFecFin = DateTime.Now;
                        pFecIni = pFecFin.AddDays(-30);
                    }
                    else
                    {
                        // le restamos los meses hacia atras
                        var t = pFecFin.AddMonths(pMesesAtras);
                        var m = t.Month;
                        var a = t.Year;

                        index = lstMeses.FindIndex(x => x.NumMes == m);
                        // Sacamos el Fin del mes que solicita
                        iniMes = lstMeses[index].DiaInicio;
                        pFecIni = Convert.ToDateTime(iniMes + "/" + a.ToString(), CultureInfo.InvariantCulture);

                    }
                }
            }

            cmd.CommandText = @"SELECT top 5 CodWorkCenter, CodEquipo,  Sensor, Count(IdTicket) as eventos
                                FROM [Tickets]               
                                Where CodDepartamento = @Depto and IdPlanta = @Planta
                                and TipoTicket =  @TipoTicket and CodWorkCenter <> ''
                                and  (Sensor <> '')
                                and Convert(date,FchReporte)  >= @FecIni and Convert(date,FchReporte) <= @FecFin 
                                group by CodWorkCenter, CodEquipo,  Sensor
                                order by eventos desc";

            cmd.Parameters.AddWithValue("@Planta", pPlanta);
            cmd.Parameters.AddWithValue("@Depto", pDepto);
            cmd.Parameters.AddWithValue("@TipoTicket", pTipoTick);
            cmd.Parameters.AddWithValue("@FecIni", pFecIni);
            cmd.Parameters.AddWithValue("@FecFin", pFecFin);


            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSqlMT, cmd);

            return (lstDatos);
        }


        public List<DataRow> GetCatTurnos(string cnxSqlMT, int pPlanta, string pDepto, string pCtroCtos)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT CtroCtosSap, Depto, Planta           
	                                ,Turno1HrIni, Turno1MinIni, Turno1HrFin, Turno1MinFin
	                                ,Turno2HrIni, Turno2MinIni, Turno2HrFin, Turno2MinFin
	                                ,Turno3HrIni, Turno3MinIni, Turno3HrFin, Turno3MinFin
                                FROM [ConfigTpm]
                                Where StatusConfig = 1
	                                and CtroCtosSap = @ctroCtosSap
	                                and Depto = @depto
	                                and Planta = @planta";

            cmd.Parameters.AddWithValue("@Planta", pPlanta);
            cmd.Parameters.AddWithValue("@Depto", pDepto);
            cmd.Parameters.AddWithValue("@ctroCtosSap", pCtroCtos);


            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSqlMT, cmd);

            return (lstDatos);

        }

        public List<DataRow> GetRepAtnTick(string cnxSqlMT, DateTime fecI, DateTime fecF, int pPlanta, string pDepto)
        {
            List<DataRow> lstDatos = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = @"SELECT Year(FchReporte)  as Anio, Month(FchReporte) as Mes, CodWorkCenter, CodEquipo, CodSubEquipo, IdTicket, UsrEntrReparacion, TipoTicket, CodClasif,
                                FchReporte, FchAtendido, FecEntgregaReparacion , FchClose,
                                datediff(Minute,  FchReporte, FchAtendido) as MinRepAtn,
                                datediff(Minute,  FchAtendido, FecEntgregaReparacion) as MinAtnRepa,
                                datediff(Minute,  FchReporte, fchClose) as MinRepCierre,
                                datediff(Minute,  FchAtendido, fchClose) as MinAtnCierre
                                FROM [Tickets]
                                Where IdPlanta = @planta  
                                and ((@Depto = '') or (CodDepartamento = @Depto)) 
                                and CodStatus = 'CER' and TipoTicket <> 'M'
                                and convert(date,FchReporte) >= convert(date,@fi) and convert(date,FchReporte) <= convert(date,@ff) ";

            cmd.Parameters.AddWithValue("@Planta", pPlanta);
            cmd.Parameters.AddWithValue("@Depto", pDepto);
            cmd.Parameters.AddWithValue("@fi", fecI);
            cmd.Parameters.AddWithValue("@ff", fecF);


            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSqlMT, cmd);

            return (lstDatos);
        }

        /// <summary>
        /// Obtiene solo el WC como catalogo
        /// </summary>
        /// <param name="cnxSql"></param>
        /// <param name="pCategoria"></param>
        /// <returns></returns>
        public List<WorkCter> GetCatWC(string cnxSqlMt, string pCategoria, string pCtroCtos)
        {

            List<DataRow> lstDatos = null;
            List<WorkCter> lstWc = new List<WorkCter>();

            // Vamos as generar una lista de equipos padres para mostrar la descripcion a nivel de work center                
            // el campo valioso es Superordinate, si esta en blanco quiere decir que es el equipo padre
            // para esto en sap hay que ubicarlo correctamente en los functional location
            // No se consideraran los que estan en la funct. location Down,los que tienen estatus de Inactivos y/o bajas ('DLFL','INAC')                

            string cmdSql = @"SELECT Distinct  MaintPlanningPlant as Planta, WorkCenter  
                  FROM [CatEquipos]
                 Where Convert(int,CostCenter) = @CentroCostos
                 and WorkCenter is not null and  Superordinate = ''
                 and (IndivStatusObject  not in ('DLFL','INAC') or PATINDEX('%DOWN%', upper(FunctionalLocation)) = 0)";

            SqlCommand sqlcmd = new SqlCommand(cmdSql);
            sqlcmd.Parameters.AddWithValue("@CentroCostos", pCtroCtos);

            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatosConParam(cnxSqlMt, sqlcmd);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    WorkCter e = new WorkCter();
                    e.Planta = dr["Planta"].ToString();
                    e.CodWorkCenter = dr["WorkCenter"].ToString();
                    e.LlaveWc = dr["WorkCenter"].ToString();

                    lstWc.Add(e);
                }
            }
            return (lstWc);
        }

        public List<StsBitSensor> GetCatStsBitacSensores(string cnxSqlMt)
        {

            List<DataRow> lstDatos = null;
            List<StsBitSensor> lstSts = new List<StsBitSensor>();

            // Vamos as generar una lista de equipos padres para mostrar la descripcion a nivel de work center                
            // el campo valioso es Superordinate, si esta en blanco quiere decir que es el equipo padre
            // para esto en sap hay que ubicarlo correctamente en los functional location
            // No se consideraran los que estan en la funct. location Down,los que tienen estatus de Inactivos y/o bajas ('DLFL','INAC')                

            string cmdSql = @"SELECT Clave, Descrip
                              FROM [CatStsBitaSensores]
                              Where Status = 1";


            OperDatosSql datosSql = new OperDatosSql();
            lstDatos = datosSql.LeeDatos(cnxSqlMt, cmdSql);

            if (lstDatos.Count > 0)
            {
                foreach (DataRow dr in lstDatos)
                {
                    StsBitSensor e = new StsBitSensor();

                    e.Clave = dr["Clave"].ToString();
                    e.Descrip = dr["Descrip"].ToString();

                    lstSts.Add(e);
                }
            }
            return (lstSts);
        }

        public List<UsuarioCtroCostos> GetUsuarioCtroCostos(string cnxSqlMt, int pIdUsuario, string pCtroCostos)
        {

            List<DataRow> lstDatos = null;
            List<UsuarioCtroCostos> lstSts = new List<UsuarioCtroCostos>();
            string cmdSql = "";
            OperDatosSql datosSql = new OperDatosSql();
            UsuarioCtroCostos e = new UsuarioCtroCostos();

            if (pCtroCostos != "")
            {
                cmdSql = @"SELECT Distinct CtroCtosSap, WorkCenter
                              FROM [ConfigTpm] Where CtroCtosSap = '" + pCtroCostos + "'";




                lstDatos = datosSql.LeeDatos(cnxSqlMt, cmdSql);

                if (lstDatos.Count > 0)
                {
                    foreach (DataRow dr in lstDatos)
                    {
                        e = new UsuarioCtroCostos();
                        e.Id = pIdUsuario;
                        e.CtroCostos = dr["CtroCtosSap"].ToString(); ;
                        e.WorkCenter = dr["WorkCenter"].ToString();
                        e.Orden = 1;
                        lstSts.Add(e);
                    }
                }


            }

            e = new UsuarioCtroCostos();
            e.Id = pIdUsuario;
            e.CtroCostos = "";
            e.WorkCenter = "[ -- Todos -- ]";
            e.Orden = 1;
            lstSts.Add(e);

            if (pIdUsuario == 0)
            {

                cmdSql = @"SELECT Distinct CtroCtosSap, WorkCenter
                              FROM [ConfigTpm]";

                if (pCtroCostos != "")
                    cmdSql = cmdSql + " Where CtroCtosSap <> '" + pCtroCostos + "'";

                lstDatos = datosSql.LeeDatos(cnxSqlMt, cmdSql);

                if (lstDatos.Count > 0)
                {
                    foreach (DataRow dr in lstDatos)
                    {
                        e = new UsuarioCtroCostos();
                        e.Id = pIdUsuario;
                        e.CtroCostos = dr["CtroCtosSap"].ToString(); ;
                        e.WorkCenter = dr["WorkCenter"].ToString();
                        e.Orden = 1;
                        lstSts.Add(e);
                    }
                }
            }
            else
            {
                cmdSql = @"SELECT a.CentroCostos, b.WorkCenter
                              FROM [UsuariosCentroCostos] a
                              INNER JOIN [ConfigTpm] b
                              ON a.CentroCostos = b.CtroCtosSap
                              Where a.IdUsuario = " + pIdUsuario.ToString() +
                                  " and a.CentroCostos <> '" + pCtroCostos + "'";

                lstDatos = datosSql.LeeDatos(cnxSqlMt, cmdSql);

                if (lstDatos.Count > 0)
                {
                    int i = 2;
                    foreach (DataRow dr in lstDatos)
                    {
                        UsuarioCtroCostos f = new UsuarioCtroCostos();

                        f.Id = pIdUsuario;
                        f.CtroCostos = dr["CentroCostos"].ToString();
                        f.WorkCenter = dr["WorkCenter"].ToString();
                        f.Orden = i;
                        lstSts.Add(f);
                        i++;
                    }
                }
            }
            return (lstSts);
        }
    }
}