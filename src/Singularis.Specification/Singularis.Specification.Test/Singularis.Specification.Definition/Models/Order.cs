using System;
using System.Collections.Generic;

namespace Singularis.Specification.Test.Singularis.Specification.Definition.Models
{
	class Order
	{
		public DateTime Date { get; set; }
		public Address Address { get; set; }

		public ICollection<Good> Goods { get; set; }
	}
}