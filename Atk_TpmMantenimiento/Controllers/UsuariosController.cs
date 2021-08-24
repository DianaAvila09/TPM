using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace Atk_TpmMantenimiento.Controllers
{
    public class UsuariosController : Controller
    {
        #region Variables para conexion a SQL
        static string SrvSql = Settings.Default.SrvSql;
        static string BD = Settings.Default.BD;
        static string UserSql = Settings.Default.UserSql;
        static string PwdSql = Settings.Default.PwdSql;
        static string cnxSqlMT = "Data Source=" + Settings.Default.SrvSql + ";Initial Catalog=" + Settings.Default.BD + ";User ID=" + Settings.Default.UserSql + ";Password=" + Settings.Default.PwdSql;
        #endregion

        #region variables 
        static string cnxSqlRefec = "";
        static string depto = "";
        static string rutaLog = "";
        static string tituArea = "";
        static string tituCC = "";
        static string tituCA = "";
        static int plantaSat = 0;
        static string ctroCostos = "";
        static string rutalog = "";
        static string cCostos = "";
        #endregion

        BusinessLogic.BL_Usuarios blUsuarios = new BL_Usuarios();
        BusinessLogic.BL_TPM tpm = new BL_TPM();
        BLCatDeptos blCatDeptos = new BLCatDeptos();
        BL_Usuarios blUsu = new BL_Usuarios();

        // GET: Usuarios
        public ActionResult Login()
        {
            //if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            if (Session["costos"] != null) 
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            cnxSqlRefec = "Data Source=" + config.SrvSqlEmpl.Trim() + ";Initial Catalog=" + config.BdEmpl.Trim() + "; User ID=" + config.UserEmpl.Trim() + "; Password=" + config.PwdEmpl.Trim();
            depto = config.Depto;
            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            plantaSat = config.Planta;
            ctroCostos = config.CtroCtosSap;

            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.Message = "Iniciar sesión en el sistema";
            ViewBag.Result = null;
            UsuarioAcceso usuario = new UsuarioAcceso();
            return PartialView("_Login", usuario);
        }

        [HttpPost]
        public ActionResult Login(UsuarioAcceso usuario)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();


            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            cnxSqlRefec = "Data Source=" + config.SrvSqlEmpl.Trim() + ";Initial Catalog=" + config.BdEmpl.Trim() + "; User ID=" + config.UserEmpl.Trim() + "; Password=" + config.PwdEmpl.Trim();

            if (cCostos != "")
                depto = config.Depto;

            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            plantaSat = config.Planta;
            ctroCostos = cCostos;

            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.Message = "Iniciar sesión en el sistema";
            ViewBag.Result = true;

            if (!ModelState.IsValid)
            {
                ViewBag.Result = false;

                //return View(usuario);
                return PartialView("_Login", usuario);

            }

            usuario = blUsuarios.ValidoAcceso(cnxSqlMT, usuario, ctroCostos);

            if (usuario.NumControl != "" && usuario.Nombre != null)
            {
                Session["IdUsuario"] = usuario.Id.ToString();
                Session["UserId"] = usuario.NumControl.ToString();
                Session["UserName"] = usuario.Nombre.ToString();
                Session["CatTpm"] = usuario.CatTpm;
                Session["EditarTicket"] = usuario.EditarTicket;
                Session["CatChecklist"] = usuario.CatChecklist;
                Session["CapturaChecklist"] = usuario.CapturaChecklist;
                Session["Costos"] = "";
                ViewBag.Result = true;

                return Json(new { success = true });
            }

            else
            {
                Session["IdUsuario"] = "0";
                Session["UserId"] = "";
                Session["UserName"] = "";
                Session["CatTpm"] = false;
                Session["EditarTicket"] = false;
                Session["CatChecklist"] = false;
                Session["CapturaChecklist"] = false;
                Session["Costos"] = "";
                ViewBag.Result = false;
            }

            return PartialView("_Login", usuario);

        }

        public ActionResult LogOut()
        {
           // if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
           // cCostos = Session["costos"].ToString();


            UsuarioAcceso usuario = new UsuarioAcceso();

            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.Message = "Iniciar sesion en el sistema";

            Session["IdUsuario"] = "0";
            Session["UserId"] = "";
            Session["UserName"] = "";
            Session["CatTpm"] = false;
            Session["EditarTicket"] = false;
            Session["CatChecklist"] = false;
            Session["CapturaChecklist"] = false;
            Session["Costos"] = "";

            return PartialView("_Logout");
        }

        public ActionResult FinSesion()
        {

            //Session["IdUsuario"] = "0";
            //Session["UserId"] = "";
            //Session["UserName"] = "";
            //Session["CatTpm"] = false;
            //Session["EditarTicket"] = false;
            //Session["CatChecklist"] = false;
            //Session["CapturaChecklist"] = false;
            //Session.Remove("Costos");

            return Json(new { success = true });

        }

        public ActionResult FinSesion2()
        {

            return Json(new { success = true });

        }

        [HttpGet]
        public ActionResult CatUsuarios()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            depto = config.Depto;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            rutalog = config.RutaLog;
            ctroCostos = config.CtroCtosSap;
            plantaSat = config.Planta;

            List<UsuarioTpm> lstDatos = blUsuarios.GetCatUsuarios(cnxSqlMT, ctroCostos);

            ViewBag.Message = "Catálogo de usuarios del TPM, del centro de costos: " + tituCC;
            ViewBag.Editar = false;
            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(lstDatos);
        }

        [HttpGet]
        public ActionResult Nuevo()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            rutalog = config.RutaLog;

            AltaUsuario usuarioNew = new AltaUsuario();
            string cnxSqlRefec = "Data Source=" + config.SrvSqlEmpl.Trim() + ";Initial Catalog=" + config.BdEmpl.Trim() + "; User ID=" + config.UserEmpl.Trim() + "; Password=" + config.PwdEmpl.Trim();

            usuarioNew.lstDeptos = blCatDeptos.DatosCatalogo(cnxSqlMT);
            usuarioNew.lstRoles = blUsuarios.GetCatRolles(cnxSqlMT);
            usuarioNew.lstEmpleados = blUsuarios.GetCatEmpleados(cnxSqlMT, cnxSqlRefec, rutalog);

            usuarioNew.Usuario = new UsuarioTpm();

            usuarioNew.Usuario.FecAlta = DateTime.Now;
            usuarioNew.Usuario.CentroCostos = ctroCostos;
            usuarioNew.Usuario.Agrego = Session["UserId"].ToString();
            usuarioNew.Usuario.StatusEmpTpm = false;
            usuarioNew.Usuario.CatTpm = false;
            usuarioNew.Usuario.EditarTicket = false;
            usuarioNew.Usuario.CatChecklist = false;
            usuarioNew.Usuario.CapturaChecklist = false;
            usuarioNew.Usuario.Password = "";
            usuarioNew.Usuario.NumControl = "";

            ViewBag.Message = "Alta de usuario";
            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return PartialView("_Nuevo", usuarioNew);
        }

        [HttpPost]
        public ActionResult Nuevo(AltaUsuario usuarioNew)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            bool existe = false;

            ViewBag.Message = "Alta de usuario";
            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            if (!ModelState.IsValid)
            {
                usuarioNew.lstDeptos = blCatDeptos.DatosCatalogo(cnxSqlMT);
                usuarioNew.lstRoles = blUsuarios.GetCatRolles(cnxSqlMT);
                usuarioNew.lstEmpleados = blUsuarios.GetCatEmpleados(cnxSqlMT, cnxSqlRefec, rutalog);

                return PartialView("_Nuevo", usuarioNew);
            }

            existe = blUsuarios.ValidoUsuario(cnxSqlMT, usuarioNew.Usuario);
            if (existe)
            {
                usuarioNew.lstDeptos = blCatDeptos.DatosCatalogo(cnxSqlMT);
                usuarioNew.lstRoles = blUsuarios.GetCatRolles(cnxSqlMT);
                usuarioNew.lstEmpleados = blUsuarios.GetCatEmpleados(cnxSqlMT, cnxSqlRefec, rutalog);
                ModelState.AddModelError("Usuario.NumControl", "Ya Existe en el centro de costos");
                return PartialView("_Nuevo", usuarioNew);
            }

            usuarioNew.Usuario.FecAlta = DateTime.Now;
            int result = blUsuarios.GuardarUsuario(cnxSqlMT, usuarioNew.Usuario);

            return Json(new { success = true });
        }

        public ActionResult Editar(int Id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            rutalog = config.RutaLog;

            AltaUsuario usuarioNew = new AltaUsuario();
            string cnxSqlRefec = "Data Source=" + config.SrvSqlEmpl.Trim() + ";Initial Catalog=" + config.BdEmpl.Trim() + "; User ID=" + config.UserEmpl.Trim() + "; Password=" + config.PwdEmpl.Trim();

            usuarioNew.lstDeptos = blCatDeptos.DatosCatalogo(cnxSqlMT);
            usuarioNew.lstRoles = blUsuarios.GetCatRolles(cnxSqlMT);
            usuarioNew.lstEmpleados = blUsuarios.GetCatEmpleados(cnxSqlMT, cnxSqlRefec, rutalog);

            usuarioNew.Usuario = blUsuarios.GetDatosUsuario(cnxSqlMT, cnxSqlRefec, rutalog, Id);

            ViewBag.Message = "Edición de datos de usuario";
            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return PartialView("_Editar", usuarioNew);
        }

        [HttpPost]
        public ActionResult Editar(AltaUsuario usuarioNew)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            ViewBag.Message = "Edición de datos de usuario";
            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            if (!ModelState.IsValid)
            {
                usuarioNew.lstDeptos = blCatDeptos.DatosCatalogo(cnxSqlMT);
                usuarioNew.lstRoles = blUsuarios.GetCatRolles(cnxSqlMT);
                usuarioNew.lstEmpleados = blUsuarios.GetCatEmpleados(cnxSqlMT, cnxSqlRefec, rutalog);

                return PartialView("_Editar", usuarioNew);
            }

            int result = blUsuarios.UpdateUsuario(cnxSqlMT, usuarioNew.Usuario);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult CambiaCtroCosto(string pCtroCosto)
        {

            // if (Session["costos"] == null) { return PartialView("_Logout"); }


            Session["costos"] = pCtroCosto;
            ViewBag.Result = true;
            return Json(new { success = true });
        }
    }
}