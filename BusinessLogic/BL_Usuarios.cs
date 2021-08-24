using Entidades;
using RepositorySql;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class BL_Usuarios
    {

        public UsuarioAcceso ValidoAcceso(string cnxSql, UsuarioAcceso usuario)
        {

            SqlRepository repoSql = new SqlRepository();
            DataRow dato = repoSql.GetDatosUsuario(cnxSql, usuario);

            if (dato != null)
            {
                if (usuario.Password.Trim() == dato["Password"].ToString().Trim())
                {
                    usuario.Id = (int)dato["Id"];
                    usuario.NumControl = dato["NumControl"].ToString();
                    usuario.Nombre = dato["Nombre"].ToString();
                    usuario.Password = dato["Password"].ToString();
                    usuario.CatTpm = (bool)dato["CatTpm"];
                    usuario.EditarTicket = (bool)dato["EditarTicket"];
                    usuario.CatChecklist = (bool)dato["CatChecklist"];
                    usuario.CapturaChecklist = (bool)dato["CapturaChecklist"];
                    usuario.cCostos = dato["CentroCostos"].ToString();
                }
            }
            else
            {
                usuario.NumControl = "";
                usuario.Password = "";
                usuario.Nombre = "VALOR NO VALIDO";
                usuario.CatTpm = false;
                usuario.EditarTicket = false;
                usuario.CatChecklist = false;
                usuario.CapturaChecklist = false;
            }

            return (usuario);
        }

        public UsuarioAcceso ValidoAcceso(string cnxSql, UsuarioAcceso usuario, string pCtroCostos)
        {

            SqlRepository repoSql = new SqlRepository();
            DataRow dato = repoSql.GetDatosUsuario(cnxSql, usuario, pCtroCostos);

            if (dato != null)
            {
                if (usuario.Password.Trim() == dato["Password"].ToString().Trim())
                {
                    usuario.Id = (int)dato["Id"];
                    usuario.NumControl = dato["NumControl"].ToString();
                    usuario.Nombre = dato["Nombre"].ToString();
                    usuario.Password = dato["Password"].ToString();
                    usuario.CatTpm = (bool)dato["CatTpm"];
                    usuario.EditarTicket = (bool)dato["EditarTicket"];
                    usuario.CatChecklist = (bool)dato["CatChecklist"];
                    usuario.CapturaChecklist = (bool)dato["CapturaChecklist"];
                }
            }
            else
            {
                usuario.NumControl = "";
                usuario.Password = "";
                usuario.Nombre = "VALOR NO VALIDO";
                usuario.CatTpm = false;
                usuario.EditarTicket = false;
                usuario.CatChecklist = false;
                usuario.CapturaChecklist = false;
            }

            return (usuario);
        }

        public List<UsuarioTpm> GetCatUsuarios(string cnxSql, string pCtroCostos)
        {
            SqlRepository repoSql = new SqlRepository();

            List<UsuarioTpm> lstCatUsuarios = repoSql.LeeCatUsuarios(cnxSql, pCtroCostos);
            return (lstCatUsuarios);
        }

        public List<RolAcceso> GetCatRolles(string cnxSql)
        {
            SqlRepository repoSql = new SqlRepository();

            List<RolAcceso> lstCatRolles = repoSql.LeeCatRolles(cnxSql);
            return (lstCatRolles);
        }

        public List<Empleado> GetCatEmpleados(string cnxSqlMt, string cnxSqlRefec, string rutalog)
        {
            SqlRepository repoSql = new SqlRepository();

            List<Empleado> lstemp = repoSql.LeeCatEmpleados(cnxSqlMt, cnxSqlRefec, rutalog);
            return (lstemp);


        }

        public int GuardarUsuario(string cnxSql, UsuarioTpm Usuario)
        {
            int result = 0;

            // Obtenemos el nombre
            SqlRepository repoSql = new SqlRepository();

            Usuario.Nombre = repoSql.GetNombreEmpl(cnxSql, Usuario.NumControl);

            result = repoSql.GuardarUsuarioTPM(cnxSql, Usuario);
            return result;
        }

        public int UpdateUsuario(string cnxSql, UsuarioTpm Usuario)
        {
            int result = 0;

            // Obtenemos el nombre
            SqlRepository repoSql = new SqlRepository();

            Usuario.Nombre = repoSql.GetNombreEmpl(cnxSql, Usuario.NumControl);

            result = repoSql.UpdateUsuarioTPM(cnxSql, Usuario);
            return result;
        }

        public bool ValidoUsuario(string cnxSql, UsuarioTpm usuario)
        {

            SqlRepository repoSql = new SqlRepository();
            DataRow dato = repoSql.GetDatosUsuario(cnxSql, usuario);

            if (dato == null)
            {
                return (false);
            }
            else
            {
                return (true);
            }


        }

        public UsuarioTpm GetDatosUsuario(string cnxSqlMT, string cnxSqlRefec, string rutalog, int id)
        {
            SqlRepository repoSql = new SqlRepository();

            UsuarioTpm datosUsuario = repoSql.LeeDatosUsuarioTpm(cnxSqlMT, id);
            return (datosUsuario);
        }

        public List<UsuarioCtroCostos> GetUsuarioCtroCostos(string cnxSqlMT, int IdUsuario, string pCtroCostos)
        {
            SqlRepository repoSql = new SqlRepository();

            List<UsuarioCtroCostos> datosUsuario = repoSql.GetUsuarioCtroCostos(cnxSqlMT, IdUsuario, pCtroCostos);
            return (datosUsuario.OrderBy(x => x.Orden).ToList());
        }
    }


}
