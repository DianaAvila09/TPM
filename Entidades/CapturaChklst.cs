using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class CapturaChklst
   {
      public CheckListEqEnc ChklsxEq { get; set; }
      public List<CheckListEqDt> lstChckActEq { get; set; }
      public int ResultSave { get; set; }
      public string Mensaje { get; set; }
   }
}
