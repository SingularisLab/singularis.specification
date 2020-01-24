using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.NHibernate.Mapping
{
	public class ItemMapping : ClassMapping<Item>
	{
		public ItemMapping()
		{
			SchemaAction(global::NHibernate.Mapping.ByCode.SchemaAction.Validate);
			Table("Items");

			Id(x => x.Id, x => x.Generator(Generators.Increment));

			Property(x => x.Name);

			ManyToOne(
				x => x.Character,
				x => x.Column("CharacterId"));

			Set(
				x => x.Runes,
				x =>
				{
					x.Cascade(Cascade.All);
					x.Key(k => k.Column("ItemId"));
				},
				x => x.OneToMany());
		}
	}
}