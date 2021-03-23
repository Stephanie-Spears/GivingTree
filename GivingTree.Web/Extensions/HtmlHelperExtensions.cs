/*using System.Web;
using System.Web.Mvc.Html;
using GivingTree.Data.Models;

// ReSharper disable once CheckNamespace
namespace GivingTree.Web
{
    public static class HtmlHelperExtensions
    { 
	    public static IHtmlString Rating(this System.Web.Mvc.HtmlHelper html, FruitTreeRating rating)
	    {
		    if (rating == null || rating.ReviewCount == 0)
		    {
			    return new System.Web.Mvc.MvcHtmlString("<span><em>No Rating</em></span>");
		    }

		    return html.Partial("_Rating", rating);
	    }

    }
}
*/