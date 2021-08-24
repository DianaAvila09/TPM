using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Atk_wsCatWCenter
{
   [RunInstaller(true)]
   public partial class ProjectInstaller : System.Configuration.Install.Installer
   {
      public ProjectInstaller()
      {
         InitializeComponent();
      }
   }
}
