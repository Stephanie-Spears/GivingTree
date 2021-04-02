using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using GivingTree.Data.Models;
using GivingTree.Data.Services;
using Microsoft.AspNet.Identity;

namespace GivingTree.Web.Controllers
{
    public class FruitTreesController : Controller
    {
	    private readonly IFruitTreeData _db;

	    public FruitTreesController(IFruitTreeData db)
	    {
		    _db = db;
	    }

        // GET: FruitTrees
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        { 
	        IEnumerable<FruitTree> trees = _db.GetAll();

	        ViewBag.Markers = GetMapsMarkers();

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
			// We use IFruitDataTree and SqlFruitTreeData to interact with the database directly, so GetImage() is implemented in those files.
			FruitTree tree = _db.GetImage(id);

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
        public ActionResult Create([Bind(Include = "Name, Fruit, Description, Latitude, Longitude, CreatedByUserId, CreatedByUserName, LastUpdated")]FruitTree tree, HttpPostedFileBase upload)
        {
            try
            {
	            if (ModelState.IsValid)
	            {
					// Extracts the binary data from the request and populates a new File entity instance which is ready to be added to the FruitTree's File collection. If the user adds a new FruitTree without uploading a file, the upload parameter will equate to null, which is why we check for the null condition before attempting to access the HttpPostedFileBase ContentLength property. If we reference the ContentLength property before doing this check when no file has been uploaded, an exception will be raised. If a user uploads an empty file, it will have a ContentLength of 0, and we don't need to bother storing this data, either. 
		            if (upload != null && upload.ContentLength > 0)
		            {
						// create the new File object and assign the properties given by the upload parameter
			            var avatar = new File
			            {
				            FileName = System.IO.Path.GetFileName(upload.FileName),
				            FileType = FileType.Avatar,
				            ContentType = upload.ContentType
			            };
						// the binary data is obtained from the InputStream property of the uploaded file, and a BinaryReader object is used to read that data into the Content property of the File object. 
			            using (var reader = new System.IO.BinaryReader(upload.InputStream))
			            {
				            avatar.Content = reader.ReadBytes(upload.ContentLength);
			            }
						// add the new file object to the tree object
			            tree.Files = new List<File> { avatar };
		            }
					// save to database
		            _db.Add(tree);
                    return RedirectToAction("Details", new { id = tree.Id });
	            }
	            return View();
            }
            catch
            {
	            TempData.Message("There was an error with saving the tree");
                return View();
            }
        }

        // GET: FruitTrees/Edit/5
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
			//todo: do i need to check post creator authorization to edit here?
	        if (id == null)
	        {
		        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        }
			// Uses the GetImage() method to interact with the database and return all of the file types associated with the given id
	        FruitTree tree = _db.GetImage(id);
	        if (tree == null)
	        {
		        return HttpNotFound();
	        }
	        return View(tree);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HttpPostedFileBase upload)
        {

			// this right?
	        var treeToUpdate = _db.Get(id);
	        if (TryUpdateModel(treeToUpdate, "",
		        new string[] { "Name", "Fruit", "Description", "Latitude", "Longitude", "CreatedByUserId", "CreatedByUserName", "LastUpdated" }))
	        {
		        try
		        {
			        if (upload != null && upload.ContentLength > 0)
			        {
				        if (treeToUpdate.Files.Any(f => f.FileType == FileType.Avatar))
				        {
					        treeToUpdate.Files.Remove(treeToUpdate.Files.First(f => f.FileType == FileType.Avatar));
				        }
				        var avatar = new File
				        {
					        FileName = System.IO.Path.GetFileName(upload.FileName),
					        FileType = FileType.Avatar,
					        ContentType = upload.ContentType
				        };
				        using (var reader = new System.IO.BinaryReader(upload.InputStream))
				        {
					        avatar.Content = reader.ReadBytes(upload.ContentLength);
				        }
				        treeToUpdate.Files = new List<File> { avatar };
			        }

			        _db.Update(treeToUpdate);

			        return RedirectToAction("Index");
		        }
		        catch (RetryLimitExceededException /* dex */)
		        {
			        //Log the error (uncomment dex variable name and add a line here to write a log.
			        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
		        }
	        }
	        return View(treeToUpdate);
        }

/*        // POST: FruitTrees/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FruitTree tree)
        {				
	        try
            {
	            if(ModelState.IsValid && tree.CreatedByUserId == User.Identity.GetUserId())
	            {
		            _db.Update(tree);
		            TempData.Message("You have saved the tree!");
		            return RedirectToAction("Details", new { id = tree.Id });
	            }
	            return View(tree);
            }
            catch
            {
	            TempData.Message("There was an error with saving the tree");
	            return View(tree);
            }
        }*/

        // GET: FruitTrees/Delete/5
        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
	        var tree = _db.Get(id);
	        if(tree == null || tree.CreatedByUserId != User.Identity.GetUserId())
	        {
		        return View("NotFound");
	        }
	        return View(tree);
        }

        // POST: FruitTrees/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection form)
        {
            try
            {
	            var tree = _db.Get(id);
	            if (tree.CreatedByUserId == User.Identity.GetUserId())
	            { 
		            _db.Delete(id);
	            }
	            return RedirectToAction("Index");
            }
            catch
            {
	            TempData.Message("There was an error with deleting the tree");
                return View();
            }
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
	        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
	        IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

	        IEnumerable<SelectListItem> items =
		        values.Select(value => new SelectListItem
		        {
			        Text = value.ToString(),
			        Value = value.ToString(),
			        Selected = value.Equals(metadata.Model)
		        });

	        return htmlHelper.DropDownListFor(
		        expression,
		        items
	        );
        }

        private string GetMapsMarkers()
        {
	        IEnumerable<FruitTree> model = _db.GetAll();

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
