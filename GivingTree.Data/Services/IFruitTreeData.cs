using System.Collections.Generic;
using System.Linq;
using GivingTree.Data.Models;

namespace GivingTree.Data.Services
{
	public interface IFruitTreeData
	{
		void Add(FruitTree fruitTree);
		void Delete(int id);
		FruitTree Get(int id);
		IEnumerable<FruitTree> GetAll();
		void Update(FruitTree fruitTree);


		//void AddImage(File file);
		//void DeleteImage(int? id);
		FruitTree GetImage(int? id);
		//IEnumerable<File> GetAllImages();
		//void UpdateImage(File file);
		
	}
}
