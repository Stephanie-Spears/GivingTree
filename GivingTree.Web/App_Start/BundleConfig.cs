using System.Web.Optimization;

namespace GivingTree.Web
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
				"~/Scripts/jquery.validate*"));

			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
				"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
				"~/Scripts/bootstrap.js",
				"~/Scripts/site.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/bootstrap.css",
				"~/Content/site.css"));

			bundles.Add(new ScriptBundle("~/bundles/polyfill",
				"https://polyfill.io/v3/polyfill.min.js?features=default").Include(
				"~/Scripts/polyfill.js"));

			BundleTable.EnableOptimizations = true;
		}
	}
}
