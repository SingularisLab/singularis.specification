using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.NHibernate.Mapping
{
	public class RuneMapping : ClassMapping<Rune>
	{
		public RuneMapping()
		{
			SchemaAction(global::NHibernate.Mapping.ByCode.SchemaAction.Validate);
			Table("Runes");

			Id(x => x.Id, x => x.Generator(Generators.Increment));

			Property(x => x.Modifier);
			Property(x => x.TargetAttribute);

			Set(
				x => x.Items,
				x =>
				{
					x.Cascade(Cascade.All);
					x.Key(k => k.Column("RuneId"));
				},
				x => x.OneToMany());
		}
	}
}