using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GivingTree.Data.Models;

namespace GivingTree.Data.Services
{
	public class SqlFruitTreeData : IFruitTreeData
	{
		// We need to construct a new instance of a DbContext here. In order to do this, though, we need an instance of TheGivingTreeDbContext inside of this class. We can either create a private instance of the DbContext and use it in this class, or we can ask the application to provide us with the DbContext from the environment by passing it in as a parameter. 

		private readonly GivingTreeDbContext db;

		public SqlFruitTreeData(GivingTreeDbContext db)
		{
			this.db = db;
		}

		public void Add(FruitTree fruitTree)
		{
			// Every time we insert an item into the database here, Entity will generate the Id and assign it to the object
			db.FruitTrees.Add(fruitTree);
			db.SaveChanges();
		}

		public void Delete(int id)
		{
			var fruitTree = db.FruitTrees.Find(id);
			db.FruitTrees.Remove(fruitTree);
			db.SaveChanges();
		}

		public FruitTree Get(int id)
		{
			return db.FruitTrees.FirstOrDefault(t => t.Id == id);
		}

		public IEnumerable<FruitTree> GetAll()
		{
			return from t in db.FruitTrees
				orderby t.Name
				select t;
		}

		public void Update(FruitTree fruitTree)
		{
			// Optimistic concurrency in case there are multiple users editing the same object

			// passing the fruitTree object here tells Entity that this object should already exist in the database
			// "entry" here tells Entity to start keeping track of this object
			var entry = db.Entry(fruitTree);
			// tells Entity that the object is in a modified state, and that the changes need to be persisted. Entity Framework will issue an update statement for this fruitTree record and make sure that it matches the data in the database.
			entry.State = EntityState.Modified;
			db.SaveChanges();
		}
	}
}