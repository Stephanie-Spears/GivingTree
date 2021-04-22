using System.Web.Mvc;
using GivingTree.Web.DAL;
using GivingTree.Web.Models;

namespace GivingTree.Web.Controllers
{
    public class FileController : Controller
    {
	     private GivingTreeDbContext _db = new();

	    // This obtains the file based on the id value passed in the query string, then it returns the image to the browser as a FileResult
	    // GET: /File/
	    public ActionResult Index(int id)
	    {
		    File fileToRetrieve = _db.Files.Find(id);

		    return File(fileToRetrieve.Content, fileToRetrieve.ContentType);

	    }
    }
}