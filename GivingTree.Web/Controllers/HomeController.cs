﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GivingTree.Web.DAL;
using GivingTree.Web.Models;

namespace GivingTree.Web.Controllers
{
	[RequireHttps]
	[Authorize]
	public class HomeController : Controller
	{ 
		
		private GivingTreeDbContext _db = new GivingTreeDbContext();
		

/*		public HomeController(GivingTreeDbContext db)
		{
			this._db = db;
		}*/

		public ActionResult Index()
		{
			IEnumerable<FruitTree> model = _db.FruitTrees.OrderBy(t => t.LastUpdated);
			return View(model);
		}

		public ActionResult About()
		{

			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		protected override void Dispose(bool disposing)
		{
			_db.Dispose();
			base.Dispose(disposing);
		}
	}
}


// We can rely on dependency injection to provide our controllers with the dependencies it needs. So instead of hard-coding that we need the InMemoryFruitTreeData here inside of our HomeController, we instead want to inject something that implements IFruitTreeData and simply let the HomeController depend on that object that implements this interface. It's going to be given to us in the constructor. That way the HomeController doesn't have to have low-level knowledge of specific components like InMemoryFruitTreeData. We can just instead depend on the abstraction, which is IFruitTreeData.

// In order to implement this, we need a container (specifically an Inversion of Control container), which is something that knows how to analyze objects like the HomeController to build objects, and how to analyze objects like HomeController to understand what dependencies are required, and it's the container that can figure out what to inject when we have a constructor that says "give me something that implements IFruitTreeData, and we can then take whatever is given to us and assign it to the private field for IFruitTreeData. 