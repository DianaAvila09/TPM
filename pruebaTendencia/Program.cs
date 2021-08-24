using System;
using System.Collections.Generic;
using BusinessLogic;
using Entidades;
using System.Diagnostics;
using System.Linq;

namespace pruebaTendencia
{
   class Program
   {
      static void Main(string[] args)
      {
            //    Tendencia t = new Tendencia();   

            //var dat = t.CalculateLinearRegression(new string[] { "", "", "78", "", "", "95", "", "89", "78", "88", "89", "90" });

            //    Console.WriteLine("CalculateLinearRegression  Y = a + bX ");
            //    Console.WriteLine("(a)Intercept: {0}", dat.Intercept);
            //    Console.WriteLine("(B)Slope: {0}", dat.Slope);

            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (1 * dat.Slope) );
            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (2 * dat.Slope) );
            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (3 * dat.Slope) );
            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (4 * dat.Slope) );
            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (5 * dat.Slope) );
            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (6 * dat.Slope) );
            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (7 * dat.Slope) );
            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (8 * dat.Slope) );
            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (9 * dat.Slope) );
            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (10 * dat.Slope) );
            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (11 * dat.Slope) );
            //    Console.WriteLine("Trend Y = {0:#.##}", dat.Intercept + (12 * dat.Slope));
            //    Console.WriteLine("(B)Slope: {0}", dat.Slope);

            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine("Dia: {0}, Num {1} {2}", DateTime.Now.AddDays(i), DateTime.Now.AddDays(i).DayOfWeek, (int)DateTime.Now.AddDays(i).DayOfWeek);
            }
            Console.ReadLine();
        }
   }
}

