using Entidades;
using RepositorySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusinessLogic
{
    public class BL_Reportes
    {
        public List<Falla> DatosCatFallas(string cnxSql, string pDepto)
        {
            SqlRepository repoSql = new SqlRepository();

            List<Falla> lstFallas = repoSql.LeeCatFallas(cnxSql, pDepto);
            return (lstFallas);
        }

        public List<Ticket> GetRepTickets(ParamRepTickets filtros, string cnxSqlMt, int planta, string cCostos, string pDepto, string rutalog)
        {
            BL_CatTpm catlg = new BL_CatTpm();
            List<Ticket> lstTick = new List<Ticket>();
            SqlRepository repoSql = new SqlRepository();
            string cadStatus = "";
            string cadHallazgo = "";
            string cadCausoParo = "";

            switch (filtros.Estatus)
            {
                case "PROC":
                    cadStatus = " and t.CodStatus = 'AAR' or t.CodStatus = 'WAS'";
                    break;
                case "TODO":
                    cadStatus = " ";
                    break;
                default:
                    cadStatus = " and t.CodStatus = '" + filtros.Estatus + "'";
                    break;
            }

            switch (filtros.Hallazgo)
            {
                case "SI":
                    cadHallazgo = " and t.HallazgoSeguridad = 'SI'";
                    break;
                case "NO":
                    cadHallazgo = " and t.HallazgoSeguridad = 'NO'";
                    break;
                case "AMBOS":
                    cadHallazgo = " ";
                    break;
            }

            switch (filtros.CausoParo)
            {
                case "SI":
                    cadCausoParo = " and t.CausoParo = 'SI'";
                    break;
                case "NO":
                    cadCausoParo = " and t.CausoParo = 'NO'";
                    break;
                case "AMBOS":
                    cadCausoParo = " ";
                    break;
            }

            lstTick = repoSql.GetRepTick(filtros, cnxSqlMt, planta, cCostos, pDepto, cadStatus, cadHallazgo, cadCausoParo, rutalog);
            return (lstTick);
        }

        public DataTable GetRepTickdt(ParamRepTickets filtros, string cnxSqlMt, int planta, string cCostos, string pDepto, string rutalog)
        {
            BL_CatTpm catlg = new BL_CatTpm();
            SqlRepository repoSql = new SqlRepository();
            string cadStatus = "";
            string cadHallazgo = "";
            string cadCausoParo = "";

            switch (filtros.Estatus)
            {
                case "PROC":
                    cadStatus = " and t.CodStatus = 'AAR' or t.CodStatus = 'WAS'";
                    break;
                case "TODO":
                    cadStatus = " ";
                    break;
                default:
                    cadStatus = " and t.CodStatus = '" + filtros.Estatus + "'";
                    break;
            }

            switch (filtros.Hallazgo)
            {
                case "SI":
                    cadHallazgo = " and t.HallazgoSeguridad = 'SI'";
                    break;
                case "NO":
                    cadHallazgo = " and t.HallazgoSeguridad = 'NO'";
                    break;
                case "AMBOS":
                    cadHallazgo = " ";
                    break;
            }

            switch (filtros.CausoParo)
            {
                case "SI":
                    cadCausoParo = " and t.CausoParo = 'SI'";
                    break;
                case "NO":
                    cadCausoParo = " and t.CausoParo = 'NO'";
                    break;
                case "AMBOS":
                    cadCausoParo = " ";
                    break;
            }
            DataTable dt = new DataTable();

            dt = repoSql.GetRepTickdt(filtros, cnxSqlMt, planta, cCostos, pDepto, cadStatus, cadHallazgo, cadCausoParo, rutalog);
            return (dt);
        }

        public List<CheckListEqEnc> GetCapchklist(string cnxSql, DateTime fi, DateTime ff, string pDepto)
        {
            SqlRepository repoSql = new SqlRepository();
            List<CheckListEqEnc> lstcapchklst = repoSql.GetCapChklst(cnxSql, fi, ff, pDepto);
            return (lstcapchklst);
        }

        public CheckListEqEnc GetDatosChkEncb(string cnxSql, int Id, string pDepto, int pPlanta)
        {
            SqlRepository repoSql = new SqlRepository();
            CheckListEqEnc lstEqConChk = repoSql.GetDatosChklstEncb(cnxSql, Id, pDepto, pPlanta);
            return (lstEqConChk);
        }

        public List<CheckListEqDt> GetDatosChkAct(string cnxSql, int Id, string pDepto)
        {
            SqlRepository repoSql = new SqlRepository();
            List<CheckListEqDt> lstChkAct = new List<CheckListEqDt>();
            lstChkAct = repoSql.GetChecklistAct(cnxSql, Id, pDepto);
            return (lstChkAct);
        }

        public List<Meses> GetMeses()
        {
            List<Meses> temp = new List<Meses>();
            temp.Add(new Meses { Mes = 1, DescripMes = "Enero" });
            temp.Add(new Meses { Mes = 2, DescripMes = "Febrero" });
            temp.Add(new Meses { Mes = 3, DescripMes = "Marzo" });
            temp.Add(new Meses { Mes = 4, DescripMes = "Abril" });
            temp.Add(new Meses { Mes = 5, DescripMes = "Mayo" });
            temp.Add(new Meses { Mes = 6, DescripMes = "Junio" });
            temp.Add(new Meses { Mes = 7, DescripMes = "Julio" });
            temp.Add(new Meses { Mes = 8, DescripMes = "Agosto" });
            temp.Add(new Meses { Mes = 9, DescripMes = "Septiembre" });
            temp.Add(new Meses { Mes = 10, DescripMes = "Octubre" });
            temp.Add(new Meses { Mes = 11, DescripMes = "Noviembre" });
            temp.Add(new Meses { Mes = 12, DescripMes = "Diciembre" });

            return (temp);

        }

        public List<EquipoPadre> GetEquiPadres(string cnxSqlMT, string pCtroCtos)
        {
            SqlRepository repoSql = new SqlRepository();
            List<EquipoPadre> temp = repoSql.GetCatEquiposPadres(cnxSqlMT, pCtroCtos);
            temp.Add(new EquipoPadre { CodEquipo = "Todos", Cod_Descrip = " Todos" });
            return (temp);
        }

        public List<RespuestaTick> GetRepAtnTick(string cnxSqlMT, DateTime fecI, DateTime fecF, int pPlanta, string pDepto)
        {
            SqlRepository repoSql = new SqlRepository();
            List<DataRow> lstDatos = new List<DataRow>();
            List<RespuestaTick> lstRespu = new List<RespuestaTick>();

            lstDatos = repoSql.GetRepAtnTick(cnxSqlMT, fecI, fecF, pPlanta, pDepto);

            foreach (DataRow dr in lstDatos)
            {
                RespuestaTick i = new RespuestaTick();

                i.Anio = (int)dr["Anio"];
                i.Mes = (int)dr["Mes"];
                i.CodWorkCenter =  dr["CodWorkCenter"].ToString();
                i.CodEquipo = dr["CodEquipo"].ToString();
                i.CodSubEquipo = dr["CodSubEquipo"].ToString();
                i.IdTicket = (int)dr["IdTicket"];
                i.TipoTicket = dr["TipoTicket"].ToString();
                i.CodClasif = dr["CodClasif"].ToString();

                if (dr["MinRepAtn"] != DBNull.Value) { i.MinRepAtn = (int)dr["MinRepAtn"]; };
                if (dr["MinAtnRepa"] != DBNull.Value) { i.MinAtnRepa = (int)dr["MinAtnRepa"]; };
                if (dr["MinRepAtn"] != DBNull.Value) { i.MinRepAtn = (int)dr["MinAtnCierre"]; };
                if (dr["MinRepCierre"] != DBNull.Value) { i.MinRepCierre = (int)dr["MinRepCierre"]; };
                if (dr["FchReporte"].ToString() != "") { i.FchReporte = Convert.ToDateTime(dr["FchReporte"]); }
                if (dr["FchAtendido"].ToString() != "") { i.FchAtendido = Convert.ToDateTime(dr["FchAtendido"]); }
                if (dr["FecEntgregaReparacion"].ToString() != "") { i.FchEntregaReparacion = Convert.ToDateTime(dr["FecEntgregaReparacion"]); }
                if (dr["FchClose"].ToString() != "") { i.FchClose = Convert.ToDateTime(dr["FchClose"]); }
               
                switch (i.TipoTicket)
                {
                    case "R":
                        i.DescripTipo = "Critico";
                        break;
                    case "A":
                        i.DescripTipo = "Alerta";
                        break;

                    case "Z":
                        i.DescripTipo = "Alerta de Contención";
                        break;
                    case "P":
                        i.DescripTipo = "Pokayoke";
                        break;

                }
                switch (i.CodClasif)
                {
                    case "MEC":
                        i.DescripCodClasif = "Mecánica";
                        break;
                    case "ELE":
                        i.DescripCodClasif = "Eléctrica";
                        break;

                    case "ETR":
                        i.DescripCodClasif = "Electrónica";
                        break;
                    case "HID":
                        i.DescripCodClasif = "Hidráulica";
                        break;
                    case "SOLD":
                        i.DescripCodClasif = "Soldadura";
                        break;
                    case "ENFR":
                        i.DescripCodClasif = "Enfiramiento";
                        break;

                }



                lstRespu.Add(i);
            }

            return (lstRespu);


        }


    }
}

