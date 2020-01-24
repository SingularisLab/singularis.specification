using System.Collections.Generic;

namespace Singularis.Specification.Test.Singularis.Specification.Definition.Models
{
	class User
	{
		public Person Person { get; set; }
		public ICollection<Order> Orders { get; set; }
		public int Id { get; set; }
	}
}