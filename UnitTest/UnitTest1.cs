//using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace UnitTest
{
   [TestClass]
   public class UnitTest1
   {
      //[TestMethod]
      //public void TestMethod1()
      //{
      //   //GetDatosSap.GetDatos DatosSap = new GetDatosSap.GetDatos();
      //   //DatosCnxSap cnxSap = new DatosCnxSap()
      //   //{
      //   //   Host = "10.12.0.83",
      //   //   SystemID = "P40",
      //   //   SystemNumber = "00",
      //   //   Client = "100",
      //   //   Language = "EN",
      //   //   PoolSize = "10",
      //   //   UserPRD = "ATK_RP_USER",
      //   //   PwdPRD = "INICIO01"
      //   //};

      //   string SrvSql = @"ATKSV020\ATK_PUE_PRD_001";
      //   string BD = "ProduccionDinamica";
      //   string UserSql = "UsrWOPlanning";
      //   string PwdSql = "Planning2015";
      //   string cnxSqlPD = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

      //   SrvSql = @"ATKSV020\ATK_PUE_PRD_001";
      //   BD = "TpmMtto";
      //   UserSql = "TpmManto";
      //   PwdSql = "Tpmsql2016";
      //   string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

      //   SrvSql = @"ATKSV020\ATK_PUE_PRD_001";
      //   BD = "Produccion_Dev";
      //   UserSql = "TpmManto";
      //   PwdSql = "Tpmsql2016";
      //   string cnxSqlProd = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

      //   //RfcParam param = new RfcParam();
      //   //param.CentroCostos = "";
      //   //param.FuntionLoc = "ATK*";
      //   //param.Planta = "1848";
      //   //param.ObjectReferenceIndicator1 = "2";
      //   //param.ObjectReferenceIndicator2 = " ";
      //   //param.CategoryEq = "M";
      //   //param.AutTechnicalobject = "A%";
      //   //param.Language = "EN";
      //   //param.NomenclaEquipo = "A%";
      //   //param.CategoryWC = "0007";


      //   // Enviar a guardar un error
      //   //ToolsTpm.Tools bita = new Tools();

      //   //// bita.GuardarError(cnxSqlMT, "Mensaje", "Trace", "Initest", "TPM-Mtto");
      //   // DatosCorreo correo = new DatosCorreo
      //   // {
      //   //    To = new List<string> { "pedro.sevilla@Magna.com" },
      //   //    Subject = "TPM-Mantenimiento",
      //   //    Mensaje = "Prueba de envio"
      //   // };
      //   // bita.EnviarCorreo(correo);

      //   #region test

      //   //List<SistemaManto> lstSm = new List<SistemaManto>();
      //   //SqlRepository repoSql = new SqlRepository();

      //   //lstSm = repoSql.LeeCatSistManto(cnxSqlMT);

      //   //ist <DataRow> lstSm = null;

      //   //lstEstructu = DatosSap.LeeEquiposCRHD(cnxSap, cnxSql, param);
      //   //foreach (DataRow dr in lstEstructu)
      //   //{
      //   //   Console.WriteLine("Equipo : {0}", dr.ItemArray);
      //   //}

      //   //lstEstructu = DatosSap.LeeEquiposEQKT(cnxSap, cnxSql, param);
      //   //foreach (DataRow dr in lstEstructu)
      //   //{
      //   //   Console.WriteLine("Equipo : {0}", dr.ItemArray);
      //   //}

      //   //lstEstructu = DatosSap.LeeCatEquipos_EQUI(cnxSap, cnxSql, param);
      //   //foreach (DataRow dr in lstEstructu)
      //   //{
      //   //   Console.WriteLine("Equipo : {0}", dr.ItemArray);
      //   //}

      //   //List<TblSap_IFLO> lstIFLO = new List<TblSap_IFLO>();
      //   //SapRepository repoSap = new SapRepository();

      //   //lstIFLO = repoSap.LeeEstrEquIFLO(cnxSap, cnxSql, param);
      //   //foreach (var dr in lstIFLO)       
      //   //{
      //   //   Console.WriteLine("Equipo : {0}", lstIFLO);
      //   //}

      //   //lstEstructu = DatosSap.LeeStructLocILOA(cnxSap, cnxSql, param);
      //   //foreach (DataRow dr in lstEstructu)
      //   //{
      //   //   Console.WriteLine("Equipo : {0}", dr.ItemArray);
      //   //}

      //   //lstEstructu = DatosSap.LeeStructLocEQUZ(cnxSap, cnxSql, param);
      //   //foreach (DataRow dr in lstEstructu)
      //   //{
      //   //   Console.WriteLine("Equipo : {0}", dr.ItemArray);
      //   //}

      //   //List<DataRow> lstCatWC = DatosSap.LeeCatWorkCenter(cnxSap, cnxSql);
      //   //foreach (DataRow dr in lstCatWC)
      //   //{
      //   //   Console.WriteLine("Work Center: {0} {1} {2} {3} {4} {5} {6}", dr.ItemArray[0], dr.ItemArray[1], dr.ItemArray[2], dr.ItemArray[3], dr.ItemArray[4], dr.ItemArray[5], dr.ItemArray[6]);
      //   //}

      //   //List<DataRow> lstCatFocus = DatosSap.LeeCatFocus(cnxSap, cnxSql);
      //   //foreach (DataRow dr in lstCatFocus)
      //   //{
      //   //   Console.WriteLine("Work Center: {0} {1} {2}", dr.ItemArray[0], dr.ItemArray[1], dr.ItemArray[2]);
      //   //}

      //   //List<DataRow> lstCatEmp = DatosSap.LeeCatEmpleados(cnxSap, cnxSql);
      //   //foreach (DataRow dr in lstCatEmp)
      //   //{
      //   //   Console.WriteLine("Work Center: {0} {1} {2}", dr.ItemArray[0], dr.ItemArray[1], dr.ItemArray[2]);
      //   //}

      //   //List<DataRow> lstPm = DatosSap.LeePMStandar(cnxSap, cnxSql);
      //   //foreach (DataRow dr in lstPm)
      //   //{
      //   //   Console.WriteLine("Work Center: {0}, {1}, {2}, {3}, {4}, {5}", dr.ItemArray[0], dr.ItemArray[1], dr.ItemArray[2], dr.ItemArray[3], dr.ItemArray[4], dr.ItemArray[5]);
      //   //}

      //   //List<DataRow> lstMro = DatosSap.LeeStockMro(cnxSap, cnxSql);
      //   //foreach (DataRow dr in lstMro)
      //   //{
      //   //   Console.WriteLine("Mro: {0}, {1}, {2}, {3}, {4}, {5}", dr.ItemArray[0], dr.ItemArray[1], dr.ItemArray[2], dr.ItemArray[3], dr.ItemArray[4], dr.ItemArray[5]);
      //   //}


      //   //List<tblAufk> lstwom1 = DatosSap.LeeTablaAUFK (cnxSap, cnxSql);
      //   //foreach (var dr in lstwom1)
      //   //{
      //   //   Console.WriteLine("Mro: {0}, {1}, {2}, {3}, {4}, {5}", dr.Ordernum, dr.OrderType, dr.Planta,dr.MainWC, dr.Descripcion, dr.FchaCreacion);
      //   //}


      //   //List<Equipo> lstEquipos = new List<Equipo>();

      //   //BusinessLogic.BLGetDatosTpm tpm = new BLGetDatosTpm();


      //   //lstEquipos = tpm.DatosCatEquipos(cnxSap, cnxSql);

      //   //Assert.IsNotNull(lstEquipos);

      //   //int x = 1;
      //   //foreach (var item in lstEquipos)
      //   //{
      //   //   Console.WriteLine("Codigo: {0}, Categoria: {1} , tipo Objeto: {2}", item.CodEquipo, item.CategoryEqui, item.TypeTechObj);
      //   //}

      //   //List<EstruturaEquipo> lstEquipos = new List<EstruturaEquipo>();

      //   //BusinessLogic.BLGetDatosTpm tpm = new BLGetDatosTpm();

      //   //lstEquipos = tpm.DatosStructEquipos(cnxSap, cnxSql);
      //   //Assert.IsNotNull(lstEquipos);

      //   //int x = 1;
      //   //foreach (var item in lstEquipos)
      //   //{
      //   //   Console.WriteLine("Codigo: {0}, Categoria: {1} , tipo Objeto: {2}", item.CodEquipo, item.FuncionalLoc, item.CentroCosto);
      //   //}

      //   //BusinessLogic.BLCatDeptos tpm = new BusinessLogic.BLCatDeptos();
      //   //List<PlantaSatelite> lst = new List<PlantaSatelite>();

      //   //lst = tpm.LeePalantasSat(cnxSqlMT);
      //   //Assert.IsNotNull(lst);

      //   //int x = 1;
      //   //foreach (var item in lst)
      //   //{
      //   //   Console.WriteLine("Codigo: {0}, Categoria: {1} , tipo Objeto: {2}", item.IdPlanta, item.planta, item.ClavePlanta);
      //   //}


      //   //BusinessLogic.BLCatPlanesManto tpm = new BusinessLogic.BLCatPlanesManto();
      //   //List<PlanMantto> lst = new List<PlanMantto>();

      //   //lst = tpm.DatosCatalogo(cnxSqlMT);
      //   //Assert.IsNotNull(lst);

      //   //int x = 1;
      //   //foreach (var item in lst)
      //   //{
      //   //   Console.WriteLine("dato: {0}, dato: {1} , dato: {2}", item.CodEquipo, item.CodSistema, item.Frecuencia);
      //   //}

      //   #endregion

      //   BL_TPM tpm = new BL_TPM();

      //   //List<EquipoTpm> lst = new List<EquipoTpm>();

      //   //lst = tpm.DatosTpm(cnxSqlMT, "PREN", param.CentroCostos, @"C:\atkApps\AtkSiteTpmMantto\Log\logTpmManto.txt");
      //   //Assert.IsNotNull(lst);

      //   //int x = 1;
      //   //foreach (var item in lst)
      //   //{
      //   //   Console.WriteLine("dato: {0}, dato: {1} , dato: {2}", item.CodEquipo, item.CodSistema, item.Frecuencia);
      //   //}

      //   List<FamiliaProductos> lst = new List<FamiliaProductos>();

      //   lst = tpm.getCoproductos(cnxSqlMT, cnxSqlProd);

      //   Assert.IsNotNull(lst);

      //   int x = 1;
      //   foreach (var item in lst)
      //   {
      //      Console.WriteLine("Material 1: {0}, Materal 2: {1} , Coproductos: {2}", item.Material1, item.Material2, item.Cprdts);
      //   }

      //}

      [TestMethod]
      public void TestMethod1()
      {

         Console.WriteLine("Holaaaaaaaaaaaaaa");

         //string SrvSql = @"ATKSV020\ATK_PUE_PRD_001";
         //string BD = "ProduccionDinamica";
         //string UserSql = "UsrWOPlanning";
         //string PwdSql = "Planning2015";
         //string cnxSqlPD = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

         //SrvSql = @"ATKSV020\ATK_PUE_PRD_001";
         //BD = "TpmMtto";
         //UserSql = "TpmManto";
         //PwdSql = "Tpmsql2016";
         //string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

         //SrvSql = @"ATKSV020\ATK_PUE_PRD_001";
         //BD = "Produccion_Dev";
         //UserSql = "TpmManto";
         //PwdSql = "Tpmsql2016";
         //string cnxSqlProd = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;


         //BL_TPM tpm = new BL_TPM();
        
         //List<FamiliaProductos> lst = new List<FamiliaProductos>();

         //lst = tpm.getCoproductos(cnxSqlMT, cnxSqlProd);

         //Assert.IsNotNull(lst);

         //foreach (var i in lst)
         //{
         //   Console.WriteLine("Material 1: {0}, Materal 2: {1} , Coproductos: {2}", i.Material1, i.Material2, i.Cprdts);
         //}

      }
   }
}