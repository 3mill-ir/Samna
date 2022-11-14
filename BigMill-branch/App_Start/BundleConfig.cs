using System.Web;
using System.Web.Optimization;

namespace BigMill
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                    "~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/UserContent/css").Include(
                   "~/Content/UserContent/css/style.css",
                   "~/Content/UserContent/css/icons.css",
                   "~/Content/UserContent/css/animate.css",
                   "~/Content/UserContent/css/shortcodes.css",
                   "~/Content/UserContent/css/custom-styles.css",
                   "~/Content/UserContent/css/rtl.css"));

            bundles.Add(new StyleBundle("~/Content/pipoCSS").Include(
               "~/Content/UserContent/css/style.css",new CssRewriteUrlTransform()).Include(
               "~/Content/UserContent/css/icons.css",new CssRewriteUrlTransform()).Include(
               "~/Content/UserContent/css/animate.css",new CssRewriteUrlTransform()).Include(
               "~/Content/UserContent/css/shortcodes.css",new CssRewriteUrlTransform()).Include(
               "~/Content/UserContent/css/custom-styles.css", new CssRewriteUrlTransform()).Include(
               "~/Content/UserContent/css/rtl.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/Content/pipoJS").Include(
                    "~/Content/UserContent/js/jquery/jquery.js", new CssRewriteUrlTransform()).Include(
                    "~/Content/UserContent/js/themetor.js", new CssRewriteUrlTransform()).Include(
                    "~/Content/UserContent/js/tt_composer_front.js", new CssRewriteUrlTransform()).Include(
                    "~/Content/UserContent/js/jquery.prettyPhoto.js", new CssRewriteUrlTransform()).Include(
                    "~/Content/UserContent/js/jquery.flexslider-min.js", new CssRewriteUrlTransform()).Include(
                    "~/Content/UserContent/js/jquery.jplayer.min.js", new CssRewriteUrlTransform()).Include(
                    "~/Content/UserContent/js/custom.js", new CssRewriteUrlTransform()).Include(
                    "~/Content/UserContent/js/jquery.li-scroller.1.0.js", new CssRewriteUrlTransform()));
        }
    }
}
