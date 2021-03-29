using System;
using System.Collections.Generic;
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

		[Required]
		[DataType(DataType.Text)]
		public string Description { get; set; }

		[Required]
		public double Latitude { get; set; }

		[Required]
		public double Longitude { get; set; }

		[Required]
		public string CreatedByUserId { get; set; }

		[Required]
		public string CreatedByUserName { get; set; }

		[Required]
		[Display(Name = "Last Updated Timestamp")]
		public DateTime LastUpdated { get; set; }

		// this completes the association of a one-to-many relationship with the File entity
		public virtual ICollection<File> Files { get; set; }

	}

	/* todo: set up star rating system, connect user as creators of FruitTree posts (only creator can edit/delete their own posts), track who rates which locations, set regex for input validations, allow users to upload photos of locations */
}
