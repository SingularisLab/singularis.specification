using System.Collections.Generic;

namespace Singularis.Specification.Demo.Models
{
	public class Item
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual ICollection<ItemToRune> Runes { get; set; }

		public virtual Character Character { get; set; }
		public virtual int CharacterId { get; set; }

		public Item()
		{
			Runes = new List<ItemToRune>();
		}
	}
}