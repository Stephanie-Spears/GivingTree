using System.ComponentModel.DataAnnotations;

namespace GivingTree.Data.Models
{
    public class Review
    { 
	    public long Id { get; set; }
	    public string UserId { get; set; }
	 //   public string UserName { get; set; }

	    public string TreeSKU { get; set; }

	    [Range(minimum: 1, maximum: 5)]
	    public int Rating { get; set; }

	    [DataType(DataType.MultilineText)]
	    public string Comments { get; set; }
    }
}
