using NHibernate.Mapping.ByCode.Conformist;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.NHibernate.Mapping
{
	public class ReadCharacterMapping : ClassMapping<ReadCharacter>
	{
		public ReadCharacterMapping()
		{
			Table("ReadCharacters");
			Property(x => x.Name);
			Property(x => x.UserId);
		}
	}
}