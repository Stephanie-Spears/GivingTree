using System.ComponentModel.DataAnnotations;

namespace GivingTree.Data.Models
{
	// This is our entity/model that represents the information that will be persisted into the database. 
	// Entities don't always make the best models for a view because quite often a view will need more information than you can have in a single entity. In such situations, it is a good idea to build a View Model, whose only responsibility is to provide all the information and behavior that is needed for one specific view. 
	// Entity Framework will look at these data annotations and add them as constraints in the database that it's trying to create ([Required] would mean not nullable)
	public class FruitTree
	{
		public int Id { get; set; }

		// regex specifies letters only, first letter must be uppercase, no whitespace, numbers, or special characters allowed.
		// [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
		[Required]
		public string Name { get; set; }

		[Display(Name="Type of Fruit")]
		[Required]
		public FruitType Fruit { get; set; }

		// regex specifies first char must be uppercase, allows subsequent special characters and numbers
		// [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
		[Required]
		public string Description { get; set; }

		[Required]
		public double Latitude { get; set; }

		[Required]
		public double Longitude { get; set; }

/*		[RegularExpression()]
		public string StarRating { get; set; }*/

/*		[Required]
		public DbGeography Location { get; set; }

		[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
		public byte[] Image { get; set; }
*/

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

	/* todo: set up star rating system, connect user as creators of FruitTree posts (only creator can edit/delete their own posts), track who rates which locations, set regex for input validations, allow users to upload photos of locations */
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