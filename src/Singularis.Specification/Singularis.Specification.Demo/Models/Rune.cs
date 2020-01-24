using System.Collections.Generic;

namespace Singularis.Specification.Demo.Models
{
	public class Rune
	{
		public virtual int Id { get; set; }
		public virtual string TargetAttribute { get; set; }
		public virtual int Modifier { get; set; }
		public virtual ICollection<ItemToRune> Items { get; set; }
		
		public Rune()
		{
			Items = new List<ItemToRune>();
		}
	}
}
