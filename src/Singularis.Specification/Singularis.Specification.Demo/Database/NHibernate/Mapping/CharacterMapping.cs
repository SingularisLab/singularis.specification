using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.NHibernate.Mapping
{
	public class CharacterMapping : ClassMapping<Character>
	{
		public CharacterMapping()
		{
			SchemaAction(global::NHibernate.Mapping.ByCode.SchemaAction.Validate);
			Table("Characters");

			Id(x => x.Id, x => x.Generator(Generators.Increment));
			Property(x => x.CreatedAt);
			Property(x => x.Name);

			ManyToOne(
				x => x.User, 
				x => x.Column("UserId"));

			Set(
				x => x.Items,
				x =>
				{
					x.Cascade(Cascade.All);
					x.Key(k => k.Column("CharacterId"));
				},
				x => x.OneToMany());
		}
	}
}
