using System.ComponentModel.DataAnnotations;

namespace GivingTree.Web.Models
{
	public class File
	{
		public int FileId { get; set; }

		[StringLength(255)]
		public string FileName { get; set; }

		[StringLength(100)]
		public string ContentType { get; set; }

		public byte[] Content { get; set; }

		public FileType FileType { get; set; }

		public int FruitTreeId { get; set; }

		// using virtual here is one part of establishing a one-to-many relationship with the FruitTree class
		public virtual FruitTree FruitTree { get; set; }
	}
}
