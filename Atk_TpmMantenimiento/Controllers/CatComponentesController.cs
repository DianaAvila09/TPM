using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CatComponentesController : Controller
    {
        #region Variables

        static string SrvSql = Settings.Default.SrvSql;
        static string BD = Settings.Default.BD;
        static string UserSql = Settings.Default.UserSql;
        static string PwdSql = Settings.Default.PwdSql;
        static string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

        static string TituArea = "";
        static string TituCC = "";
        static string TituCA = "";
        static string rutalog = "";
        static string cCostos = "";
        static string pCostos = "";

        static string pDepto = "";
        static int PlantaSat = 0;


        BL_TPM tpm = new BL_TPM();
        BL_CatCompon BlCompo = new BL_CatCompon();
        BL_Usuarios blUsu = new BL_Usuarios();
        #endregion

        public ActionResult CatComponentes()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            pDepto = "";
            TituCC = "";
            if (cCostos != "")
            {
                pDepto = config.Depto;
                TituCC = config.TituCC;
            }

                TituArea = config.TituArea;
           
            TituCA = config.TituCA;
            rutalog = config.RutaLog;

            PlantaSat = config.Planta;

            List<Componente> lstCompo = new List<Componente>();
            lstCompo = BlCompo.GetCatCompo(cnxSqlMT, pDepto, cCostos);

            ViewBag.Message = "Catálogo de Componentes de Sistemas, Area de " + TituArea;
            ViewBag.Editar = false;
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(lstCompo);
        }

        [HttpGet]
        public ActionResult Nuevo(string mensaje, string ResultOperacion)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);
            pDepto = config.Depto;

            TituArea = config.TituArea;
            TituCC = config.TituCC;
            TituCA = config.TituCA;
            rutalog = config.RutaLog;
            pDepto = config.Depto;
            pCostos = config.CtroCtosSap;
            PlantaSat = config.Planta;

            AltaCompo newCompo = new AltaCompo();

            BLSistemasEquipo blSist = new BLSistemasEquipo();

            newCompo.compo = new Componente();
            newCompo.compo.DescripCompo = "";
            newCompo.compo.CodDepartamento = pDepto;
            newCompo.compo.CodSistema = "";
            newCompo.lstSisManto = blSist.DatosCatSistManto(cnxSqlMT, pDepto).OrderBy(x => x.Sistema).ToList();
            newCompo.compo.StatusCompo = true;
            newCompo.compo.Usuario = usuario;
            newCompo.compo.CentroCostos = cCostos;
            newCompo.compo.FchModif = DateTime.Now;
            newCompo.compo.FchAlta = DateTime.Now;

            ViewBag.Message = "Alta del componente del sistema";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.msg = mensaje;
            ViewBag.result = ResultOperacion;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View("Nuevo", newCompo);
        }

        [HttpPost]
        public ActionResult Nuevo(AltaCompo newComp)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            ViewBag.Message = "Alta de componente del sistema";
            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            newComp.compo.CentroCostos = cCostos;
            // Validamos la clave que no este repetida
            if (BlCompo.ValidaClave(cnxSqlMT, newComp.compo, rutalog))
            {
                ModelState.AddModelError("compo.DescripCompo", "Clave ya existe");
            }



            if (!ModelState.IsValid)
            {
                BLSistemasEquipo blSist = new BLSistemasEquipo();
                newComp.lstSisManto = blSist.DatosCatSistManto(cnxSqlMT, pDepto).OrderBy(x => x.Sistema).ToList();
                return View("Nuevo", newComp);
            }

            newComp.compo.FchModif = DateTime.Now;
            newComp.compo.FchAlta = DateTime.Now;

            int result = BlCompo.Guardar(cnxSqlMT, newComp.compo);
            string msj = "";
            string valorResult = "";
            if (result == 1)
            {
                msj = "Registro Guardado";
                string usuario = Session["UserName"].ToString();
                valorResult = "B";
            }
            else
            {
                msj = "Error al Guardar";
                valorResult = "M";
            }
            return RedirectToAction("Nuevo", new { mensaje = msj, ResultOperacion = valorResult });
        }


        [HttpGet]
        public ActionResult Editar(int id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            AltaCompo editCompo = new AltaCompo();
            editCompo.compo = BlCompo.DatosCompo(cnxSqlMT, id);

            BLSistemasEquipo blSist = new BLSistemasEquipo();
            editCompo.lstSisManto = blSist.DatosCatSistManto(cnxSqlMT, pDepto).OrderBy(x => x.Sistema).ToList();

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición de componente del sistema";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View("Editar", editCompo);
        }


        [HttpPost]
        public ActionResult Editar(AltaCompo editCompo)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            ViewBag.Title = TituArea;
            ViewBag.TitleCC = TituCC;
            ViewBag.Message = "Edición de componente del sistema";

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            if (!ModelState.IsValid)
            {
                BLSistemasEquipo blSist = new BLSistemasEquipo();
                editCompo.lstSisManto = blSist.DatosCatSistManto(cnxSqlMT, pDepto).OrderBy(x => x.Sistema).ToList();
                return View("Editar", editCompo);
            }

            editCompo.compo.FchModif = DateTime.Now;
            editCompo.compo.Usuario = usuario;

            int result = BlCompo.Update(cnxSqlMT, editCompo.compo, rutalog);
            if (result == 1)
            {
                ViewBag.msg = "Registro Guardado";
            }
            else
            {
                ViewBag.msg = "Error al Guardar";
            }
            return RedirectToAction("CatComponentes");

        }


    }
}