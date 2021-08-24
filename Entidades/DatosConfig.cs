using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DatosConfig
    {
        public string CtroCtosSap { get; set; }
        public string Depto { get; set; }
        public string TituCC { get; set; }
        public string TituCA { get; set; }
        public string RutaLog { get; set; }
        public string TituArea { get; set; }
        public int Planta { get; set; }
        public int TopRedAcumMtto { get; set; }
        public int TopYellowAcumMtto { get; set; }
        public int TopGreenAcumMtto { get; set; }
        public int MesesParaFallas { get; set; }
        public decimal PrctjParaFallas { get; set; }
        public string EmailTo { get; set; }
        public decimal DiasxAno { get; set; }
        public decimal HrsxDia { get; set; }
        public decimal DiasxMes { get; set; }
        public string WebServer { get; set; }
        public string RutaFotosEquipos { get; set; }
        public string SvrSqlTpm { get; set; }
        public string BDTpm { get; set; }
        public string UserTPm { get; set; }
        public string PwdTpm { get; set; }
        public string SrvSqlHtProd { get; set; }
        public string BdHtProd { get; set; }
        public string UserHtProd { get; set; }
        public string PwdHtProd { get; set; }
        public string SrvSqlProd { get; set; }
        public string BdProd { get; set; }
        public string UserProd { get; set; }
        public string PwdProd { get; set; }
        public string SrvSqlEmpl { get; set; }
        public string BdEmpl { get; set; }
        public string UserEmpl { get; set; }
        public string PwdEmpl { get; set; }
        public string HostIPSap { get; set; }
        public string SystemIDSap { get; set; }
        public string SystemNumberSap { get; set; }
        public string ClientSap { get; set; }
        public string LanguageSap { get; set; }
        public string PoolSizeSap { get; set; }
        public string UserSap { get; set; }
        public string PwdSap { get; set; }
        public string UrlfotosUser { get; set; }
        public decimal MetaKbmCmpl { get; set; }
        public decimal MetaKbmEfic { get; set; }
        public string AdminTpm { get; set; }
        public string Emailenvio { get; set; }
        public string EmailUser { get; set; }
        public string EmailPwd { get; set; }
        public string EmailServer { get; set; }
        public string EmailUserTpmMtto { get; set; }
        public string EmailUserCalidad { get; set; }
        public string EmailGerentes { get; set; }
        public string EmailPrensas { get; set; }
        public int Turno1HrIni { get; set; }
        public int Turno1MinIni { get; set; }
        public int Turno1HrFin { get; set; }
        public int Turno1MinFin { get; set; }
        public int Turno2HrIni { get; set; }
        public int Turno2MinIni { get; set; }
        public int Turno2HrFin { get; set; }
        public int Turno2MinFin { get; set; }
        public int Turno3HrIni { get; set; }
        public int Turno3MinIni { get; set; }
        public int Turno3HrFin { get; set; }
        public int Turno3MinFin { get; set; }

        public decimal MetaMtrrAutoMin { get; set; }
        public decimal MetaMtbfAutoMin { get; set; }
        public decimal MetaMtrrMntMin { get; set; }
        public decimal MetaMtbfMntMin { get; set; }
        public decimal MetaMtrrTklMin { get; set; }
        public decimal MetaMtbfTklMin { get; set; }
        public int PkykMesesAtrasHT { get; set; }

    }
}
