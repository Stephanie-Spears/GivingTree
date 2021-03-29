using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GivingTree.Data.Models;

namespace GivingTree.Data.Services
{
	// Our class here will derive from the DbContext class in Entity Framework
	// Now GivingTreeDbContext becomes our gateway to the database. We can configure what database we want this to point to just by adding a connectionString in the Web.config file. So we can have a single DbContext that points to different databases when this application gets deployed.
	// We can use a single DbContext to encapsulate everything that we might need to pull from a database (for instance a very large database with thousands of tables can be used with a single encapsulating DbContext defined here). 
	// We can also break up a single database into multiple DbContexts. For example, we might have a SalesDbContext, which points to the same database as the AccountingDbContext, or the OrdersDbContext. So ultimately all those tables live inside the same database and maybe even inside the same schema, but in order to break things down into single simple objects, we don't want one giant DbContext that handles everything. 
	// Any time we have a public property on a DbContext that is of type DbSet<T> (where <T> represents a generic type), DbSet tells Entity Framework that we are working with a table of data that should have a shape of a particular object that we have defined by the generic type parameter set for DbSet... So if we say that we have a DbSet of type "Restaurant" (DbSet<Restaurant>), that tells the Entity Framework that somewhere in the database (pointed to by my connectionString), there is a table that contains data that can populate Restaurant objects...so somewhere there is a table that contains records with an Id, Name, CuisineType property. And when I query that table, I can map the data that comes back into this class's DbSet<Restaurant>
	// There's lots of different features we can use from Entity Framework which would allow us to customize the configuration, and to point to different tables, or to rename columns, etc. but we won't be using those advanced features in this simple project. 

	// By default, if we query data from the SQL Server and bring it back as a result set that includes columns with names that match what we've added already, it will automatically map it to the corresponding property (Id, Name, Cuisine)
	public class GivingTreeDbContext : DbContext
	{
		// Setting this name to FruitTrees here will make Entity Framework assume that there is a table in the database with the name FruitTrees
		public DbSet<FruitTree> FruitTrees { get; set; }
		public DbSet<File> Files { get; set; }
	}
}
