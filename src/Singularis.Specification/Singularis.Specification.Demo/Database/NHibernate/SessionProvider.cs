using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace Singularis.Specification.Demo.Database.NHibernate
{
	class SessionProvider
	{
		private readonly ISessionFactory _sessionFactory;

		public SessionProvider()
		{
			var configuration = new Configuration();

			configuration
				.DataBaseIntegration(c =>
				{
					c.Dialect<MsSql2012Dialect>();
					c.ConnectionString = "Server=cis;Database=test;Trusted_Connection=True;";
					c.LogSqlInConsole = true;
					c.LogFormattedSql = true;
				});

			configuration.CurrentSessionContext<ThreadStaticSessionContext>();

			var mapper = new ModelMapper();
			mapper.AddMappings(typeof(Program).Assembly.GetExportedTypes());

			configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
			var validator = new SchemaValidator(configuration);
			validator.Validate();

			_sessionFactory = configuration.BuildSessionFactory();
		}

		public ISession OpenSession()
		{
			return _sessionFactory.OpenSession();
		}
	}
}
