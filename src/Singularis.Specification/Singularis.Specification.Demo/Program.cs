using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Singularis.Specification.Definition;
using Singularis.Specification.Demo.Database.EF;
using Singularis.Specification.Demo.Database.NHibernate;
using Singularis.Specification.Demo.Models;
using Singularis.Specification.Demo.Specifications;
using Singularis.Specification.Executor.Common;

namespace Singularis.Specification.Demo
{
	class Program
	{
		static void Main(string[] args)
		{
			GetById().Wait();
			List().Wait();
			Fetch().Wait();
			CompositeFilter().Wait();
			Subquery().Wait();
		}
		
		static async Task GetById()
		{
			var settings = new RepositorySettings(IsolationLevel.ReadUncommitted, TimeSpan.FromSeconds(1));

			using (var ctx = new Context())
			{
				var repository = new Executor.EntityFramework.Repository<int>(ctx, settings);
				var users = await repository.GetAsync<User>(1);
			}

			using (var session = new SessionProvider().OpenSession())
			{
				var repository = new Executor.Nhibernate.Repository<int>(session, settings);
				var users = await repository.GetAsync<User>(1);
			}
		}

		static async Task List()
		{
			var settings = new RepositorySettings(IsolationLevel.ReadUncommitted, TimeSpan.FromSeconds(1));

			using (var ctx = new Context())
			{
				var repository = new Executor.EntityFramework.Repository<int>(ctx, settings);
				var users = await repository.ListAsync(new All());
			}

			using (var session = new SessionProvider().OpenSession())
			{
				var repository = new Executor.Nhibernate.Repository<int>(session, settings);
				var users = await repository.ListAsync(new All());
			}
		}

		static async Task Fetch()
		{
			var settings = new RepositorySettings(IsolationLevel.ReadUncommitted, TimeSpan.FromSeconds(1));

			using (var ctx = new Context())
			{
				var repository = new Executor.EntityFramework.Repository<int>(ctx, settings);
				var users = await repository.ListAsync(new UserWithRelatedObjects());
			}

			using (var session = new SessionProvider().OpenSession())
			{
				var repository = new Executor.Nhibernate.Repository<int>(session, settings);
				var users = await repository.ListAsync(new UserWithRelatedObjects());
			}
		}

		static async Task CompositeFilter()
		{
			var settings = new RepositorySettings(IsolationLevel.ReadUncommitted, TimeSpan.FromSeconds(1));

			using (var ctx = new Context())
			{
				var repository = new Executor.EntityFramework.Repository<int>(ctx, settings);
				var characters = await repository.ListAsync(new CreatedAfter(new DateTime(2019, 1, 1)).Combine(new CreatedBefore(new DateTime(2019, 7, 1))));
			}

			using (var session = new SessionProvider().OpenSession())
			{
				var repository = new Executor.Nhibernate.Repository<int>(session, settings);
				var characters = await repository.ListAsync(new CreatedAfter(new DateTime(2019, 1, 1)).Combine(new CreatedBefore(new DateTime(2019, 7, 1))));
			}
		}

		static async Task Subquery()
		{
			var settings = new RepositorySettings(IsolationLevel.ReadUncommitted, TimeSpan.FromSeconds(1));

			using (var ctx = new Context())
			{
				var repository = new Executor.EntityFramework.Repository<int>(ctx, settings);
				var characters = await repository.ListAsync(new CharactersForUserWithEmailDomain("@inmagna.ca"));
			}

			using (var session = new SessionProvider().OpenSession())
			{
				var repository = new Executor.Nhibernate.Repository<int>(session, settings);
				var characters = await repository.ListAsync(new CharactersForUserWithEmailDomain("@inmagna.ca"));
			}
		}
	}
}
