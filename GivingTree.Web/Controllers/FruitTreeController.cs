using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using GivingTree.Web.DAL;
using GivingTree.Web.Models;
using File = GivingTree.Web.Models.File;

namespace GivingTree.Web.Controllers
{
	public class FruitTreeController : Controller
    {
	    private GivingTreeDbContext _db = new();

	    // GET: FruitTree
	    [Authorize]
	    [HttpGet]
	    public ViewResult Index()
	    {
		    ViewBag.Markers = GetMapsMarkers();
		    IQueryable<FruitTree> trees = from s in _db.FruitTrees
			    select s;

		    return View(trees);
	    }

	    // GET: FruitTrees/Details/5
	    [Authorize]
	    [HttpGet]
        public ActionResult Details(int? id)
        {
	        if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			
			// We need to use the Include method in the LINQ query that fetches the tree from the database to bring back related files. The Include() method does not support filtering, so it fetches ALL files associated with the tree, regardless of type.
			FruitTree tree = _db.FruitTrees.Include(s => s.Files).SingleOrDefault(s => s.Id == id);

			if (tree == null)
			{
				return HttpNotFound();
			}

			return View(tree);
        }


        // GET: FruitTrees/Create
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
	        ViewBag.Markers = GetMapsMarkers();
            return View();
        }

        // POST: FruitTrees/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, Fruit, Description, Latitude, Longitude, LastUpdated")]FruitTree tree, HttpPostedFileBase upload)
        {
	        try
            {
	            if (ModelState.IsValid)
	            {
		            // Extracts the binary data from the request and populates a new File entity instance which is ready to be added to the FruitTree's File collection. If the user adds a new FruitTree without uploading a file, the upload parameter will equate to null, which is why we check for the null condition before attempting to access the HttpPostedFileBase ContentLength property. If we reference the ContentLength property before doing this check when no file has been uploaded, an exception will be raised. If a user uploads an empty file, it will have a ContentLength of 0, and we don't need to bother storing this data, either. 
		            if (upload != null && upload.ContentLength > 0)
		            {
			            // create the new File object and assign the properties given by the upload parameter
			            File photo = new File
			            {
				            FileName = System.IO.Path.GetFileName(upload.FileName),
				            FileType = FileType.Photo,
				            ContentType = upload.ContentType
			            };

			            // the binary data is obtained from the InputStream property of the uploaded file, and a BinaryReader object is used to read that data into the Content property of the File object. 
			            using (BinaryReader reader = new System.IO.BinaryReader(upload.InputStream))
			            {
				            photo.Content = reader.ReadBytes(upload.ContentLength);
			            }

			            // add the new file object to the tree object
			            tree.Files = new List<File> { photo };
		            }

		            // save to database
		            _db.FruitTrees.Add(tree); 
					_db.SaveChanges();
					return RedirectToAction("Index");
	            }
            }
	        catch (RetryLimitExceededException  dex )
	        {
		        ModelState.AddModelError("Unable to save changes. Maybe it's because you're a big giant retard?", dex);
            }

            return View(tree);
        }

        // GET: FruitTrees/Edit/5
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
	        ViewBag.Markers = GetMapsMarkers();

	        if (id == null)
	        {
		        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        }

	        // Returns all of the file types associated with the given id
	        FruitTree tree = _db.FruitTrees.Include(s => s.Files).SingleOrDefault(s => s.Id == id);

	        if (tree == null)
	        {
		        return HttpNotFound();
	        }

	        return View(tree);
        }

        // POST: FruitTrees/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, HttpPostedFileBase upload)
        {
	        if (id == null)
	        {
		        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        }

	        FruitTree treeToUpdate = _db.FruitTrees.Find(id);

	        if (TryUpdateModel(treeToUpdate, "",
		        new string[] { "Name", "Fruit", "Description", "Latitude", "Longitude", "LastUpdated" }))
	        {
		        try
		        {
			        if (upload != null && upload.ContentLength > 0)
			        {
				        if (treeToUpdate.Files.Any(f => f.FileType == FileType.Photo))
				        {
					        _db.Files.Remove(treeToUpdate.Files.First(f => f.FileType == FileType.Photo));
				        }

				        File photo = new File
				        {
					        FileName = System.IO.Path.GetFileName(upload.FileName),
					        FileType = FileType.Photo,
					        ContentType = upload.ContentType
				        };

				        using (BinaryReader reader = new System.IO.BinaryReader(upload.InputStream))
				        {
					        photo.Content = reader.ReadBytes(upload.ContentLength);
				        }

				        treeToUpdate.Files = new List<File> { photo };
			        }

			        _db.Entry(treeToUpdate).State = EntityState.Modified;
			        _db.SaveChanges();

			        return RedirectToAction("Index");
		        }
		        catch (RetryLimitExceededException dex)
		        {
			        ModelState.AddModelError("Unable to save changes. Try again, and if the problem persists, you're a giant baby banana. ", dex);
		        }
	        }
	        return View(treeToUpdate);
        }


        // GET: Student/Delete/5
        [Authorize]
        [HttpGet]
	    public ActionResult Delete(int? id, bool? saveChangesError = false)
	    {
		    if (id == null)
		    {
			    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		    }
		    if (saveChangesError.GetValueOrDefault())
		    {
			    ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists apply lotion to the affected areas two to three times daily";
		    }
		    FruitTree tree = _db.FruitTrees.Find(id);
		    if (tree == null)
		    {
			    return HttpNotFound();
		    }
		    return View(tree);
	    }



		// POST: Student/Delete/5
		[Authorize]
		[HttpPost]
	    [ValidateAntiForgeryToken]
	    public ActionResult Delete(int id)
	    {
		    try
		    {
			    FruitTree tree = _db.FruitTrees.Find(id);
			    _db.FruitTrees.Remove(tree);
			    _db.SaveChanges();
		    }
		    catch (RetryLimitExceededException dex )
		    {
			    ModelState.AddModelError("Failed to delete the model. GREAT. JUST GREAAAAT.", dex);
			    return RedirectToAction("Delete", new { id = id, saveChangesError = true });
		    }
		    return RedirectToAction("Index");
	    }


	    protected override void Dispose(bool disposing)
	    {
		    if (disposing)
		    {
			    _db.Dispose();
		    }
		    base.Dispose(disposing);
	    }


	    private string GetMapsMarkers()
        {
	        IEnumerable<FruitTree> model = _db.FruitTrees.OrderBy(t => t.LastUpdated);

	        string markers = "[";
	        foreach (var m in model)
	        {
		        string formattedName = m.Name.Replace("\"", "").Replace("\'", "");
		        string formattedDescription = m.Description.Replace("\"", "").Replace("\'", "");

		        markers += "{";
		        markers += $"'Id': '{m.Id}', ";
		        markers += $"'Name': '{formattedName}', ";
		        markers += $"'Type': '{m.Fruit}', ";
		        markers += $"'Description': '{formattedDescription}', ";
		        markers += $"'Icon': '/Content/Images/Fruit/Shine/{m.Fruit}/{m.Fruit}.ico', ";
		        markers += $"'Latitude': '{m.Latitude}', ";
		        markers += $"'Longitude': '{m.Longitude}' ";
		        markers += "},";
	        }
	        markers += "];";
	        return (markers);
        }
    }
}

