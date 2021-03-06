using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using GivingTree.Data.Models;
using GivingTree.Data.Services;

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
	        IEnumerable<FruitTree> model = _db.GetAll();

	        ViewBag.Markers = GetMapsMarkers();

	        return View(model);
        }

        // GET: FruitTrees/Details/5
        [Authorize]
        [HttpGet]
        public ActionResult Details(int id)
        {
	        var model = _db.Get(id);
	        if(model == null)
	        {
		        return View("NotFound");
	        }
	        return View(model);
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
        public ActionResult Create(FruitTree fruitTree)
        {
            try
            {
	            if (ModelState.IsValid)
	            {
                    _db.Add(fruitTree);
                    return RedirectToAction("Details", new { id = fruitTree.Id });
	            }
	            return View();
            }
            catch
            {
	            TempData["Message"] = "There was an error with saving the tree";
                return View();
            }
        }

        // GET: FruitTrees/Edit/5
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
	        var model = _db.Get(id);
	        if(model == null)
	        {
		        return HttpNotFound();
	        }

	        ViewBag.Markers = GetMapsMarkers();

	        return View(model);
        }

        // POST: FruitTrees/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FruitTree fruitTree)
        {
            try
            {
	            if(ModelState.IsValid)
	            {
		            _db.Update(fruitTree);
		            TempData["Message"] = "You have saved the tree!";
		            return RedirectToAction("Details", new { id = fruitTree.Id });
	            }
	            return View(fruitTree);
            }
            catch
            {
	            TempData["Message"] = "There was an error with saving the tree";
	            return View(fruitTree);
            }
        }

        // GET: FruitTrees/Delete/5
        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
	        var model = _db.Get(id);
	        if(model == null)
	        {
		        return View("NotFound");
	        }
	        return View(model);
        }

        // POST: FruitTrees/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection form)
        {
            try
            {
	            _db.Delete(id);
	            return RedirectToAction("Index");
            }
            catch
            {
	            TempData["Message"] = "There was an error with deleting the tree";
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
		        markers += $"'Icon': '/Content/Images/Optimized/Compressed/{m.Fruit}.ico', ";
		        markers += $"'Latitude': '{m.Latitude}', ";
		        markers += $"'Longitude': '{m.Longitude}' ";
		        markers += "},";
	        }
	        markers += "];";
	        return (markers);
        }
    }
}
