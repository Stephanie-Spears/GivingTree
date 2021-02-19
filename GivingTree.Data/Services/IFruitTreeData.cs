using System.Collections.Generic;
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
	}
}
