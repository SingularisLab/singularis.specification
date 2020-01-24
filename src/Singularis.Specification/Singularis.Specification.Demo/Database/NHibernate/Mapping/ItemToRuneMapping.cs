using NHibernate.Mapping.ByCode.Conformist;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.NHibernate.Mapping
{
	public class ItemToRuneMapping : ClassMapping<ItemToRune>
	{
		public ItemToRuneMapping()
		{
			SchemaAction(global::NHibernate.Mapping.ByCode.SchemaAction.Validate);
			Table("ItemsToRunes");

			ComposedId(x =>
			{
				x.Property(e => e.RuneId);
				x.Property(e => e.ItemId);
			});

			ManyToOne(
				x => x.Item,
				x =>
				{
					x.Insert(false);
					x.Update(false);
					x.Column("ItemId");
				});

			ManyToOne(
				x => x.Rune,
				x =>
				{
					x.Insert(false);
					x.Update(false);
					x.Column("RuneId");
				});
		}
	}
}