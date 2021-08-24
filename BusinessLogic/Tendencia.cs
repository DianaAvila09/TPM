using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
   public class Tendencia
   {
      public Trendline CalculateLinearRegression(string[] values)
      {
         var yAxisValues = new List<decimal>();
         var xAxisValues = new List<int>();

         for (int i = 0; i < values.Length; i++)
         {
            if (!string.IsNullOrEmpty(values[i]))
            {
               yAxisValues.Add(Convert.ToDecimal(values[i]));

               xAxisValues.Add(i + 1);
            }
         }
         return new Trendline(yAxisValues, xAxisValues);
      }
   }

   public class Trendline
   {
      private readonly List<int> xAxisValues;
      private readonly List<decimal> yAxisValues;
      private int count;
      private decimal xAxisValuesSum, xxSum, xySum, yAxisValuesSum;
      public decimal Slope, Intercept;

      public Trendline(List<decimal> yAxisValues, List<int> xAxisValues)
      {
         this.yAxisValues = yAxisValues;
         this.xAxisValues = xAxisValues;
         count = yAxisValues.Count;
         yAxisValuesSum = yAxisValues.Sum();
         xAxisValuesSum = xAxisValues.Sum();
         xxSum = 0;
         xySum = 0;

         for (int i = 0; i < this.count; i++)
         {
            xySum += (xAxisValues[i] * yAxisValues[i]);
            xxSum += (xAxisValues[i] * xAxisValues[i]);
         }

         Slope = CalculateSlope();
         Intercept = CalculateIntercept();
      }

      private decimal CalculateSlope()
      {
         try
         {
            return ((count * xySum) - (xAxisValuesSum * yAxisValuesSum)) / ((count * xxSum) - (xAxisValuesSum * xAxisValuesSum));
         }
         catch (DivideByZeroException)
         {
            return 0;
         }
      }

      private decimal CalculateIntercept()
      {
         return (yAxisValuesSum - (Slope * xAxisValuesSum)) / count;
      }
   }
}

