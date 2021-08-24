using BusinessLogic;
using Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTestProject1
{
   [TestClass]
   public class UnitTest1
   {
      [TestMethod]
      public void TestMethod1()
      {
         string SrvSql = @"ATKSV020\ATK_PUE_PRD_001";
         string BD = "ProduccionDinamica";
         string UserSql = "UsrWOPlanning";
         string PwdSql = "Planning2015";
         string cnxSqlPD = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

         SrvSql = @"ATKSV020\ATK_PUE_PRD_001";
         BD = "TpmMtto";
         UserSql = "TpmManto";
         PwdSql = "Tpmsql2016";
         string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

         SrvSql = @"ATKSV020\ATK_PUE_PRD_001";
         BD = "Produccion_Dev";
         UserSql = "TpmManto";
         PwdSql = "Tpmsql2016";
         string cnxSqlProd = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

         BL_TPM tpm = new BL_TPM();

         List<FamiliaProductos> lst = new List<FamiliaProductos>();

         lst = tpm.getCoproductos(cnxSqlMT, cnxSqlProd);

         Assert.IsNotNull(lst);

         foreach (var i in lst)
         {
            Console.WriteLine("Material 1: {0}, Materal 2: {1} , Coproductos: {2}", i.Material1, i.Material2, i.Cprdts);
         }


      }
   }
}
