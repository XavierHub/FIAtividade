using FI.WebAtividadeEntrevista.App_Start;
using System;
using System.Web.Optimization;

namespace FI.WebAtividadeEntrevista
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Clear();
            var version = DateTime.Now.Ticks.ToString();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js").WithVersion());

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js").WithVersion());

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*").WithVersion());

            bundles.Add(new ScriptBundle("~/bundles/jquerymask").Include(
                        "~/Scripts/jquery.mask.js").WithVersion());

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*").WithVersion());

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js").WithVersion());

            bundles.Add(new ScriptBundle("~/bundles/jtable").Include(
                      "~/Scripts/jtable/jquery.jtable.min.js",
                      "~/Scripts/jtable/localization/jquery.jtable.pt-BR.js").WithVersion());

            bundles.Add(new ScriptBundle("~/bundles/clientes").Include(
                      "~/Scripts/Clientes/FI.Clientes.js").WithVersion());

            bundles.Add(new ScriptBundle("~/bundles/listClientes").Include(
                      "~/Scripts/Clientes/FI.ListClientes.js").WithVersion());

            bundles.Add(new ScriptBundle("~/bundles/altClientes").Include(
                      "~/Scripts/Clientes/FI.AltClientes.js").WithVersion());

            bundles.Add(new ScriptBundle("~/bundles/beneficiario").Include(
                      "~/Scripts/Clientes/FI.Beneficiario.js").WithVersion());

            bundles.Add(new ScriptBundle("~/bundles/global").Include(
                     "~/Scripts/Global/FI.Global.js").WithVersion());

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css").WithVersion());

            bundles.Add(new StyleBundle("~/Content/jtable").Include(
                      "~/Scripts/jtable/themes/metro/darkgray/jtable.css").WithVersion());

        }
    }

    public static class BundleExtensions
    {
        public static Bundle WithVersion(this Bundle bundle)
        {
            bundle.Transforms.Add(new CustomBundleTransform());
            return bundle;
        }
    }
}
