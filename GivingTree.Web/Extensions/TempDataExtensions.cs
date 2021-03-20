// ReSharper disable once CheckNamespace
namespace GivingTree.Web
{
    public static class TempDataExtensions
    {
	    // to add behavior to the TempData property on both controllers and in views, the method has to extend the System.Web.Mvc.TempDataDictionary class, which is the type of the temp data property on both controllers and views.
		public static void Message(this System.Web.Mvc.TempDataDictionary tempData, string message)
		{
			tempData["Message"] = message;
		}

		public static string Message(this System.Web.Mvc.TempDataDictionary tempData)
		{
			// since the TempDataDictionary just returns objects, we need to cast it to a string here
			return tempData["Message"] as string;
		}
    }
}
