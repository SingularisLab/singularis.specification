using System.Collections.Generic;

namespace Singularis.Specification.Demo.Models
{
	public class User
	{
		public virtual int Id { get; set; }
		public virtual string Email { get; set; }
		public virtual string Firstname { get; set; }
		public virtual string Lastname { get; set; }

		public virtual ICollection<Character> Characters { get; set; }

		public User()
		{
			Characters = new List<Character>();
		}
	}
}