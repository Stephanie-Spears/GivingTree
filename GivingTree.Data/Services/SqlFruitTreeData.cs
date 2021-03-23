using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GivingTree.Data.Models;

namespace GivingTree.Data.Services
{
	public class SqlFruitTreeData : IFruitTreeData
	{
		// We need to construct a new instance of a DbContext here. In order to do this, though, we need an instance of GivingTreeDbContext inside of this class. We can either create a private instance of the DbContext and use it in this class, or we can ask the application to provide us with the DbContext from the environment by passing it in as a parameter. 

		private readonly GivingTreeDbContext _db;

		public SqlFruitTreeData(GivingTreeDbContext db)
		{
			_db = db;
		}

		public void Add(FruitTree fruitTree)
		{
			// Every time we insert an item into the database here, Entity will generate the Id and assign it to the object
			_db.FruitTrees.Add(fruitTree);
			_db.SaveChanges();
		}

		public void Delete(int id)
		{
			var fruitTree = _db.FruitTrees.Find(id);
			if (fruitTree != null) _db.FruitTrees.Remove(fruitTree);
			_db.SaveChanges();
		}

		public FruitTree Get(int id)
		{
			return _db.FruitTrees.FirstOrDefault(t => t.Id == id);
		}

		public IEnumerable<FruitTree> GetAll()
		{
			return from t in _db.FruitTrees
				orderby t.LastUpdated
				select t;
		}

		public void Update(FruitTree fruitTree)
		{
			// Optimistic concurrency in case there are multiple users editing the same object

			// passing the fruitTree object here tells Entity that this object should already exist in the database
			// "entry" here tells Entity to start keeping track of this object
			var entry = _db.Entry(fruitTree);
			// tells Entity that the object is in a modified state, and that the changes need to be persisted. Entity Framework will issue an update statement for this fruitTree record and make sure that it matches the data in the database.
			entry.State = EntityState.Modified;
			_db.SaveChanges();
		}


		//
		//public DbSet<FruitTree> FruitTrees { get; set; }

		//public DbSet<Review> Reviews { get; set; }

		//public DbSet<Image> Images { get; set; }

		//public FruitTree FindFruitTreeBySku(string treeSku)
		//	=> FruitTrees.FirstOrDefault(x => x.TreeSKU == treeSku);

		//public FruitTreeRating GetFruitTreeRating(string treeSku)
		//{
		//	IQueryable<Review> reviews = Reviews.Where(x => x.TreeSKU == treeSku);

		//	return new FruitTreeRating
		//	{
		//		TreeSKU = treeSku,
		//		Rating = reviews.Average(x => (double?)x.Rating),
		//		ReviewCount = reviews.Count(),
		//	};
		//}

		//public IQueryable<FruitTreeRating> GetFruitTreeRatings(IEnumerable<string> skus)
		//{
		//	return
		//		Reviews
		//			.Where(x => skus.Distinct().Contains(x.TreeSKU))
		//			.GroupBy(x => x.TreeSKU)
		//			.Select(reviews => new FruitTreeRating
		//			{
		//				TreeSKU = reviews.Key,
		//				Rating = reviews.Average(x => x.Rating),
		//				ReviewCount = reviews.Count(),
		//			});
		//}
	}
}