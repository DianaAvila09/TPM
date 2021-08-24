using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
   class Program
   {
      static void Main(string[] args)
      {

         // Creamos los item de los periodos
         int aIni = 2020;
         int mIni = 12;

         int aFin = 2020;         
         int mfin = 12;

         int x = aIni;
         int m = mIni;

         while (x <= aFin)
         {
            if (x == aFin)
            {

               while (m <= mfin)
               {
                  Console.WriteLine("Periodo: {0}", x.ToString().Trim() + m.ToString().Trim().PadLeft(2,'0'));
                  m = m + 1;
               }
            }
            else
            {
               while (m <= 12)
               {
                  Console.WriteLine("Periodo: {0}", x.ToString().Trim() + m.ToString().Trim().PadLeft(2, '0'));
                  m = m + 1;
               }
            }

            x = x + 1;
            m = 1;
         }

         Console.ReadLine();
      }
   }
}
