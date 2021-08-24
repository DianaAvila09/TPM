using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class AltaUsuario
   {
      public UsuarioTpm Usuario { get; set; }
      public List<Departamento> lstDeptos { get; set; }
      public List<RolAcceso> lstRoles { get; set; }
      public List<Empleado> lstEmpleados { get; set; }
   }
}
