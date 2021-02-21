using System.ComponentModel.DataAnnotations;

namespace GivingTree.Data.Models
{
	// This is our entity/model that represents the information that will be persisted into the database. 
	// Entities don't always make the best models for a view because quite often a view will need more information than you can have in a single entity. In such situations, it is a good idea to build a View Model, whose only responsibility is to provide all the information and behavior that is needed for one specific view. 
	// Entity Framework will look at these data annotations and add them as constraints in the database that it's trying to create ([Required] would mean not nullable)
	public class FruitTree
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Display(Name="Type of Fruit")]
		[Required]
		public FruitType Fruit { get; set; }

		public string Description { get; set; }

		[Required]
		public double Latitude { get; set; }

		[Required]
		public double Longitude { get; set; }

/*		[Required]
		public DbGeography Location { get; set; }*/

		[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
		public byte[] Image { get; set; }
	}

	public enum FruitType
	{
		Apple,
		Fig,
		Pear,
		Persimmon,
		Cherry,
		Plum
	}
}



// A View Model is a class whose only purpose is to encapsulate and carry all the information that is needed to render a view. Some of the models in the application may serve a dual purpose. 


/*using (var context = new FruitTreeContext ())
{
	context.FruitTrees.Add(new FruitTree()
	{
		Name = "Lorem Ipsum",
		Location = DbGeography.FromText("POINT(-122.336106 47.605049)"),
	});

	context.FruitTrees.Add(new FruitTree()
	{
		Name = "Lorem Ipsum 2",
		Location = DbGeography.FromText("POINT(-122.335197 47.646711)"),
	});

	context.SaveChanges();

	var myLocation = DbGeography.FromText("POINT(-122.296623 47.640405)");

	var FruitTree = (from t in context.FruitTrees
		orderby t.Location.Distance(myLocation)
		select t).FirstOrDefault();

	Console.WriteLine(
		"The closest FruitTree to you is: {0}.",
		FruitTree.Name);
}*/