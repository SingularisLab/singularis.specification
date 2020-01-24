using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.NHibernate.Mapping
{
	public class UserMapping : ClassMapping<User>
	{
		public UserMapping()
		{
			SchemaAction(global::NHibernate.Mapping.ByCode.SchemaAction.Validate);
			Table("Users");

			Id(x => x.Id, x => x.Generator(Generators.Increment));

			Property(x => x.Email);
			Property(x => x.Firstname);
			Property(x => x.Lastname);

			Set(
				x => x.Characters,
				x =>
				{
					x.Cascade(Cascade.All);
					x.Key(k => k.Column("UserId"));
				},
				x => x.OneToMany());
		}
	}	
}