using System;
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

//		[Required]
//		public string TreeSKU { get; set; }

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


		//public long? ThumbnailImageId { get; set; }
		//public virtual Image ThumbnailImage { get; set; }

		//public virtual ICollection<Image> Images { get; private set; }

		//public FruitTree()
		//{
		//	Images = new List<Image>();
		//}



/*		// list of all reviews for tree -> each review contains the user id, rating id, tree id, star rating, user comments
		public ICollection<Review> ReviewList { get; set; }

		// calculated star rating average
		public double? StarRatingAverage
		{
			get
			{
				//double starRatingTotal = ReviewList.Sum(review => review.StarRating);
				//double starRatingAverage = (starRatingTotal) / (ReviewList.Count());
				//return starRatingAverage;

				return ReviewList.Average(review => review.StarRating);

			}
		}*/

		// number of total ratings
/*		public int ReviewCount
		{
			get
			{
				return ReviewList.Count();
			}
		}*/
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