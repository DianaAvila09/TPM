using System.Web;
using System.Web.Optimization;

namespace Atk_TpmMantenimiento
{
   public class BundleConfig
   {
      // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
      public static void RegisterBundles(BundleCollection bundles)
      {

         BundleTable.EnableOptimizations = true;

         bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     "~/Scripts/jquery-{version}.js"));

         bundles.Add(new ScriptBundle("~/bundles/modalform").Include(
                 "~/Scripts/modalform.js"));

         bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                     "~/Scripts/jquery.validate*"));

         //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
         //           "~/Scripts/jquery.unobtrusive*"));

         bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                     "~/Scripts/modernizr-*"));

         bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                   "~/Scripts/bootstrap.js",
                   "~/Scripts/respond.js")); 

         // Boorstrap dropdown select
         bundles.Add(new ScriptBundle("~/bundles/bootstrap-select").Include(
                               "~/Scripts/bootstrap-select.js",
                               "~/Scripts/bootstrap-select.min.js",
                               "~/Scripts/script-bootstrap-select.js"));

         bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/bootstrap.css",
                   "~/Content/site.css",
                   "~/Content/TpmManto.css",
                   "~/Content/Radios.css"
                   ));

         // Bootstrap dropdown select
         bundles.Add(new StyleBundle("~/Content/Bootstrap-Select/css").Include(
                              "~/Content/style/bootstrap-select.css",
                              "~/Content/style/bootstrap-select.min.css"));
      
      }
   }
}
