using System;
using System.Collections.Generic;

namespace Singularis.Specification.Demo.Models
{
	public class Character
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual DateTime CreatedAt { get; set; }

		public virtual ICollection<Item> Items { get; set; }
		public virtual User User { get; set; }
		public virtual int UserId { get; set; }

		public Character()
		{
			Items = new List<Item>();
		}
	}
}