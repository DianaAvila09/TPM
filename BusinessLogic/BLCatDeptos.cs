using System.Collections.Generic;
using System.Linq;
using Entidades;
using RepositorySql;

namespace BusinessLogic
{
    public class BLCatDeptos
    {
        public List<Departamento> DatosCatalogo(string cnxSql, string cCostos = "")
        {
            SqlRepository repoSql = new SqlRepository();
            List<Departamento> lstDeptos = new List<Departamento>();            
            lstDeptos = repoSql.LeeCatDeptos(cnxSql, cCostos);
            return (lstDeptos);
        }

        public int GuardarCatalogo(string cnxSql, List<Departamento> lstTemp)
        {
            int result = 0;
            SqlRepository repoSql = new SqlRepository();
            result = repoSql.GuardarCatDeptos(cnxSql, lstTemp);
            return result;
        }

        public int Guardar(string cnxSql, Departamento Depto)
        {
            int result = 0;
            SqlRepository repoSql = new SqlRepository();
            result = repoSql.GuardarDepto(cnxSql, Depto);
            return result;
        }

        public List<PlantaSatelite> LeePlantasSat(string cnxSql)
        {

            SqlRepository repoSql = new SqlRepository();
            List<PlantaSatelite> lstPlantas = repoSql.LeeCatPlantas(cnxSql);
            return lstPlantas;
        }
    }
}
