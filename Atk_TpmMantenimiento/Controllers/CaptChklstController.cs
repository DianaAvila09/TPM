using Atk_TpmMantenimiento.Properties;
using BusinessLogic;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Atk_TpmMantenimiento.Controllers
{
    public class CaptChklstController : Controller
    {
        #region Variables

        static string SrvSql = Settings.Default.SrvSql;
        static string BD = Settings.Default.BD;
        static string UserSql = Settings.Default.UserSql;
        static string PwdSql = Settings.Default.PwdSql;
        static string cnxSqlMT = "Data Source=" + SrvSql + ";Initial Catalog=" + BD + ";User ID=" + UserSql + ";Password=" + PwdSql;

        static string tituArea = "";
        static string tituCC = "";
        static string tituCA = "";
        static string rutalog = "";
        static string cCostos = "";
        static string pCostos = "";
        static string cCostosSap = "";
        static string pDepto = "";
        static int plantaSat = 0;
        static string cnxSqlRefec = "";
        static string rutaLog = "";

        BL_TPM tpm = new BL_TPM();
        BL_CapturaChklst bl_CapchkxEquipo = new BL_CapturaChklst();
        BL_Usuarios blUsu = new BL_Usuarios();

        #endregion
        public ActionResult CatCapChkxEquipo()
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            tituCC = "";
            pDepto = "";
            if (cCostos != "")
            {
               tituCC = config.TituCC;
                pDepto = config.Depto;
                pCostos = config.CtroCtosSap;
            }

            tituArea = config.TituArea;            
            tituCA = config.TituCA;
            rutalog = config.RutaLog;
            plantaSat = config.Planta;

            List<CheckListEqEnc> lstChkxEq = new List<CheckListEqEnc>();
            lstChkxEq = bl_CapchkxEquipo.GetCatChkxEq(cnxSqlMT, plantaSat, pDepto, cCostos, true);

            ViewBag.Message = "Checklist de equipos activos";
            ViewBag.Editar = false;
            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(lstChkxEq);
        }

        [HttpGet]
        public ActionResult Generar(int Id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            CapturaChklst chkxEq = new CapturaChklst();

            chkxEq.ChklsxEq = bl_CapchkxEquipo.GetDatosChkxEq(cnxSqlMT, Id, pDepto);
            chkxEq.lstChckActEq = bl_CapchkxEquipo.GetDatosChkxEqDet(cnxSqlMT, Id, pDepto);
            chkxEq.ResultSave = 99;
            chkxEq.Mensaje = "";

            ViewBag.Message = "Actividades de Checklist ";
            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.SumPonder = 0;
            ViewBag.captura = true;
            ViewBag.SumPonder = chkxEq.lstChckActEq.Sum(x => x.Ponderacion);

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(chkxEq);
        }

        [HttpPost]
        public ActionResult Generar(CapturaChklst newchklst)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            string usuario = Session["UserName"].ToString();
            string claveUsuario = Session["UserId"].ToString();
            cCostos = Session["costos"].ToString();

            ViewBag.Message = "Actividades de Checklist asignado a un equipo";
            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.SumPonder = 0;
            ViewBag.captura = false;
            var temp = newchklst.ChklsxEq.Observaciones;

            newchklst.ResultSave = 1;
            foreach (var i in newchklst.lstChckActEq)
            {
                if (i.TipoOperacion == "V") // Visual
                {
                    if (i.ResultVisual != null)
                    {
                        i.ResultActiv = (bool)i.ResultVisual;
                    }
                    else
                    {
                        newchklst.ResultSave = 0;
                        newchklst.Mensaje = "Error al Guardar los Datos, revice la informacion del checklist";
                    }

                }
                else if (i.TipoOperacion == "M") // Medible
                {
                    if (i.ResultMedible == null || i.ResultMedible.ToString() == "")
                    {
                        newchklst.ResultSave = 0;
                        newchklst.Mensaje = "Error al Guardar los Datos, revice la informacion del checklist";
                    }
                }
            }

            ViewBag.SumPonder = 0;
            

            if (newchklst.ResultSave != 0)
            {
                // Obtener datos del encabezado
                newchklst.ChklsxEq = bl_CapchkxEquipo.GetDatosChkxEq(cnxSqlMT, newchklst.ChklsxEq.IdChkEquipo, pDepto);

                newchklst.ChklsxEq.FchEjecucion = DateTime.Now;
                newchklst.ChklsxEq.UserEjecuto = usuario;
                newchklst.ChklsxEq.Observaciones = temp;
                newchklst.ChklsxEq.CentroCostos = cCostos;
                newchklst.ChklsxEq.CodDepartamento = pDepto;
                newchklst.ChklsxEq.Planta = plantaSat;

                // Guardamos el detalle de las actividades
                newchklst = bl_CapchkxEquipo.Guardar(cnxSqlMT, newchklst, rutalog, plantaSat, pDepto, usuario, cCostos, claveUsuario);


                if (newchklst.ResultSave == 1)
                {
                    newchklst.Mensaje = "Registro Guardado";
                    usuario = Session["UserName"].ToString();
                }
                else
                {
                    newchklst.ResultSave = 0;
                    newchklst.Mensaje = "Error al Guardar los Datos, revice la informacion del checklist";
                }


                ViewBag.SumPonder = newchklst.lstChckActEq.Sum(x => x.Ponderacion);
            }
            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(newchklst);
        }


        public ActionResult ChklistProgram(string pCtroCostos = null, string codEqui = null)
        {

            if (Session["costos"] == null && pCtroCostos == "")
            {
                return RedirectToAction("FinSesion", "Usuarios");
            }

            if (pCtroCostos != null)
            {
                Session["costos"] = pCtroCostos;
                cCostos = pCtroCostos;
            }
            else
               if (Session["costos"] != null)
            {
                cCostos = Session["costos"].ToString();
            }
            else
            {
                return RedirectToAction("FinSesion", "Usuarios");
            }

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            tituCC = "";
            pDepto = "";
            if (cCostos != "")
            {
                tituCC = config.TituCC;
                pDepto = config.Depto;
                pCostos = config.CtroCtosSap;
            }

            tituArea = config.TituArea;
            
            tituCA = config.TituCA;
            rutalog = config.RutaLog;
           

           
            plantaSat = config.Planta;

            List<CheckListEqEnc> lstChkxEq = new List<CheckListEqEnc>();
            lstChkxEq = bl_CapchkxEquipo.GetChkxEqProg(cnxSqlMT, pDepto, plantaSat, codEqui, cCostos);
            BL_CatalogosSap bl_CatSap = new BL_CatalogosSap();
            var eq = bl_CatSap.GetDatosEquipo(cnxSqlMT, codEqui);

            ViewBag.Editar = false;
            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.Costos = pCtroCostos;
            ViewBag.codEqui = codEqui;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            ViewBag.Message = "Relacion de Checklist Programados";

            if (codEqui != null)
            {
                ViewBag.Message = ViewBag.Message + ": [" + eq.WorkCenter + "] " + eq.DescripTechnical;
            }

            return View(lstChkxEq);
        }

        [HttpGet]
        public ActionResult Ejecutar(int Id)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            cCostos = Session["costos"].ToString();
            string usuario = Session["UserName"].ToString();

            // Leemos la configuracion de acuerdo al Centro de costos que venga como parametro
            DatosConfig config = new DatosConfig();
            config = tpm.LeeConfig(cnxSqlMT, cCostos);

            cnxSqlRefec = "Data Source=" + config.SrvSqlEmpl.Trim() + ";Initial Catalog=" + config.BdEmpl.Trim() + "; User ID=" + config.UserEmpl.Trim() + "; Password=" + config.PwdEmpl.Trim();
            pDepto = config.Depto;
            rutaLog = config.RutaLog;
            tituArea = config.TituArea;
            tituCC = config.TituCC;
            tituCA = config.TituCA;
            plantaSat = config.Planta;
            rutaLog = config.RutaLog;
            cCostosSap = config.CtroCtosSap;

            ViewBag.Title = config.TituArea;
            ViewBag.TitleCC = config.TituCC;
            ViewBag.Message = "Checklist Programado";
            ViewBag.SumPonder = 0;
            ViewBag.captura = true;

            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            CapturaChklst chkxEq = new CapturaChklst();

            chkxEq.ChklsxEq = bl_CapchkxEquipo.GetDatosChkEncb(cnxSqlMT, Id, pDepto, plantaSat);
            chkxEq.lstChckActEq = bl_CapchkxEquipo.GetDatosChkActProg(cnxSqlMT, Id, pDepto);
            chkxEq.ResultSave = 99;
            ViewBag.SumPonder = chkxEq.lstChckActEq.Sum(x => x.Ponderacion);

            return View(chkxEq);
        }


        [HttpPost]
        public ActionResult Ejecutar(CapturaChklst newchklst)
        {
            if (Session["costos"] == null) { return RedirectToAction("FinSesion", "Usuarios"); }
            string usuario = Session["UserName"].ToString();
            string claveUsuario = Session["UserId"].ToString();
            cCostos = Session["costos"].ToString();

            ViewBag.Message = "Checklist Programado";
            ViewBag.Title = tituArea;
            ViewBag.TitleCC = tituCC;
            ViewBag.SumPonder = 0;
            ViewBag.captura = false;
            var temp = newchklst.ChklsxEq.Observaciones;



            newchklst.ResultSave = 1;
            foreach (var i in newchklst.lstChckActEq)
            {
                if (i.TipoOperacion == "V") // Visual
                {
                    if (i.ResultVisual != null)
                    {
                        i.ResultActiv = (bool)i.ResultVisual;
                    }
                    else
                    {
                        newchklst.ResultSave = 0;
                        newchklst.Mensaje = "Error al Guardar los Datos, revice la informacion del checklist";
                    }

                }
                else if (i.TipoOperacion == "M") // Medible
                {
                    if (i.ResultMedible == null || i.ResultMedible.ToString() == "")
                    {
                        newchklst.ResultSave = 0;
                        newchklst.Mensaje = "Error al Guardar los Datos, revice la informacion del checklist";
                    }
                }
            }

            // Obtener datos del encabezado  
            newchklst.ChklsxEq = bl_CapchkxEquipo.GetDatosChkEncb(cnxSqlMT, newchklst.ChklsxEq.IdChkEquipo, pDepto, plantaSat);


            if (newchklst.ResultSave != 0)
            {
               
                newchklst.ChklsxEq.FchEjecucion = DateTime.Now;
                newchklst.ChklsxEq.UserEjecuto = usuario;
                newchklst.ChklsxEq.Observaciones = temp;
                // Actualizamos datos del checklist
                newchklst = bl_CapchkxEquipo.Update(cnxSqlMT, newchklst, rutalog, cCostos, usuario, claveUsuario);

                if (newchklst.ResultSave == 1)
                {
                    newchklst.Mensaje = "Registro Guardado";
                    usuario = Session["UserName"].ToString();
                }
                else
                {
                    newchklst.ResultSave = 0;
                    newchklst.Mensaje = "Error al Guardar los Datos";
                }

                ViewBag.SumPonder = newchklst.lstChckActEq.Sum(x => x.Ponderacion);
            }
            #region Combo Centro de Costos            
            ViewBag.Opciones = blUsu.GetUsuarioCtroCostos(cnxSqlMT, Convert.ToInt32(Session["IdUsuario"].ToString()), cCostos);
            #endregion

            return View(newchklst);
        }
    }
}