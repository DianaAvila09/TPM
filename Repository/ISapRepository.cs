using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;


// Especificar solo acciones a realizar 
namespace RepositorySap
{
   public interface ISapRepository<Equipo, WorkCenter, EstruturaEquipo, TblSap_IFLO, TblSap_ILOA, TblSap_EQUZ, TblSap_EQUI, TblSap_EQKT, TblSap_CRHD, TblSap_JEST, TblSap_TJ02T>
   {
      List<Equipo> LeeCatEquipos(DatosCnxSap cnxSap, string cnxSql);
      List<WorkCenter> LeeCatWorkCenter(DatosCnxSap cnxSap, string cnxSql, string tipoWc);
      List<EstruturaEquipo> LeeEstructuraEquipos(DatosCnxSap cnxSap, string cnxSql);
      List<TblSap_IFLO> LeeEstrEquIFLO(DatosCnxSap cnxSap, string cnxSql, RfcParam param);
      List<TblSap_ILOA> LeeEstrEquILOA(DatosCnxSap cnxSap, string cnxSql, RfcParam param);
      List<TblSap_EQUZ> LeeEstrEquEQUZ(DatosCnxSap cnxSap, string cnxSql, RfcParam param);
      List<TblSap_EQUI> LeeEstrEquEQUI(DatosCnxSap cnxSap, string cnxSql, RfcParam param);
      List<TblSap_EQKT> LeeEstrEquEQKT(DatosCnxSap cnxSap, string cnxSql, RfcParam param);

      List<TblSap_CRHD> LeeEstrEquCRHD(DatosCnxSap cnxSap, string cnxSql, RfcParam param);
      List<TblSap_JEST> LeeEstrEquJEST(DatosCnxSap cnxSap, string cnxSql, RfcParam param, List<string> lstIdEqui);
      List<TblSap_TJ02T> LeeEstrEquTJ02T(DatosCnxSap cnxSap, string cnxSql, RfcParam param);


   }
}
