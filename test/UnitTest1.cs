using System;
using System.Collections.Generic;
using BusinessLogic;
using Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace test
{
   [TestClass]
   public class UnitTest1
   {
      [TestMethod]
      public void TestMethod1()
      {
         decimal[] valores = new decimal[]{89,90,78,87,90,98,99,89,90,98,95,96 };

         List<DatosTendencia> puntos = new List<DatosTendencia>();
         Tendencia trend = new Tendencia();
         var datos = trend.CalculateLinearRegression(valores); 


      }
   }
}
