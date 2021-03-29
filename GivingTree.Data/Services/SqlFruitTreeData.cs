using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
			return _db.FruitTrees.OrderBy(t => t.LastUpdated);
		}

		public void Update(FruitTree fruitTree)
		{
			// Optimistic concurrency in case there are multiple users editing the same object

			// passing the fruitTree object here tells Entity that this object should already exist in the database
			// "entry" here tells Entity to start keeping track of this object
			DbEntityEntry<FruitTree> entry = _db.Entry(fruitTree);
			// tells Entity that the object is in a modified state, and that the changes need to be persisted. Entity Framework will issue an update statement for this fruitTree record and make sure that it matches the data in the database.
			entry.State = EntityState.Modified;
			_db.SaveChanges();
		}






		//public void AddImage(File file)
		//{
		//	_db.Files.Add(file);
		//	_db.SaveChanges();
		//}

		//public void DeleteImage(int? id)
		//{
		//	_db.Files.Remove(_db.Files.First(f => f.FileType == FileType.Avatar));
		//	_db.SaveChanges();
		//}

		public FruitTree GetImage(int? id)
		{
			return _db.FruitTrees.Include(t => t.Files).SingleOrDefault(t => t.Id == id);
		}

		//public IEnumerable<File> GetAllImages()
		//{
		//	return _db.Files.Select(t => t);
		//}

		//public void UpdateImage(File file)
		//{
		//	DbEntityEntry<File> entry = _db.Entry(file);
		//	entry.State = EntityState.Modified;
		//	_db.SaveChanges();
		//}


	}
}