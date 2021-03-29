using System.Web.Mvc;
using GivingTree.Data.Services;

namespace GivingTree.Web.Controllers
{
    public class FileController : Controller
    {
		// This obtains the correct file based on the id value passed in the query string, then it returns the image to the browser as a FileResult
	    private readonly GivingTreeDbContext _db = new();
	    //
	    // GET: /File/
	    public ActionResult Index(int id)
	    {
		    var fileToRetrieve = _db.Files.Find(id);
		    return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
	    }
    }
}