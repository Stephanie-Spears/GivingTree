using System.Web.Mvc;
using GivingTree.Data.Models;
using GivingTree.Data.Services;

namespace GivingTree.Web.Controllers
{
    public class FruitTreesController : Controller
    {
	    private readonly IFruitTreeData _db;

	    public FruitTreesController(IFruitTreeData db)
	    {
		    this._db = db;
	    }


        // GET: FruitTrees
        [HttpGet]
        public ActionResult Index()
        { 
	        var model = _db.GetAll();
	        return View(model);
        }

        // GET: FruitTrees/Details/5
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
        //[Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: FruitTrees/Create
        //[Authorize]
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
        //[Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
	        var model = _db.Get(id);
	        if(model == null)
	        {
		        return HttpNotFound();
	        }
	        return View(model);
        }

        // POST: FruitTrees/Edit/5
        //[Authorize]
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
        //[Authorize]
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
        //[Authorize]
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
    }
}
