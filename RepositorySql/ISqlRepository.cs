using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySql
{
   interface ISqlRepository<SistemaManto>
   {
      List<DataRow> LeeDatos(string cnxSql, string cmdSql);
      bool GuardarCat(string cnxSql, List<SistemaManto> lstTempSE);
   }
}
