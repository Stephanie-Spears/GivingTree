using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;



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

		[Required]
		public string Description { get; set; }

		[Required]
		public double Latitude { get; set; }

		[Required]
		public double Longitude { get; set; }


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


/*
 
		[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.svg)$", ErrorMessage = "Only Image files allowed.")]
		public byte[] FruitTreeImage { get; set; }
		public string ImageUrl { get; set; }

		[RegularExpression("([0-9]+)")]
		public int UpVote { get; set; }

		[RegularExpression("([0-9]+)")]
		public int DownVote { get; set; }

 */

// A View Model is a class whose only purpose is to encapsulate and carry all the information that is needed to render a view. Some of the models in the application may serve a dual purpose. 

/* GENERAL DEBUGGING NOTES
 
 * Autos and Locals Windows*
 * The Autos and Locals windows show variable values while you are debugging. The windows are only available during a debugging session. The Autos window shows variables used around the current breakpoint. The Locals window shows variables defined in the local scope, which is usually the current function or method.
 

 * Immediate Window *
Use the Immediate window to debug and evaluate expressions, execute statements, and print variable values. The Immediate window evaluates expressions by building and using the currently selected project.

To display the Immediate window, open a project for editing, and then choose Debug > Windows > Immediate or press Ctrl+Alt+I. You can also enter Debug.Immediate in the Command window.

Typing this into the immediate window gives the same results, the '?' form is shorthand. Both return "Google":
	>Debug.Print loginProviders[0].Caption
	? loginProviders[0].Caption

 */




// STAR RATING 
// in FruitTree class:
/*		public int StarRateCount
		{
			get { return StarRatings.Count; }
		}

		public int StarRateTotal
		{
			get
			{
				return (StarRatings.Sum(m => m.StarRate));
			}
		}

		public virtual ICollection<StarRating> StarRatings { get; set; }
*/
// in StarRating.cs:
/*
 
 	public class StarRating
	{
		[Key]
		public int StarRateId { get; set; }

		public int StarRate { get; set; }

		public int FruitTreeId { get; set; }

		[ForeignKey("FruitTreeId")]
		public virtual FruitTree FruitTree { get; set; }
}
 */