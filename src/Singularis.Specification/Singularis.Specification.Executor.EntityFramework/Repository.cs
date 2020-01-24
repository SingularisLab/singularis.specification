using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Services;
using Singularis.Specification.Executor.Common;

namespace Singularis.Specification.Executor.EntityFramework
{
	public class Repository
	{
		protected static readonly MethodInfo ToListMethodInfo;
		protected static readonly MethodInfo FirstOrDefaultMethodInfo;
		protected static readonly MethodInfo CountMethodInfo;

		static Repository()
		{
			ToListMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EntityFrameworkQueryableExtensions),
				nameof(EntityFrameworkQueryableExtensions.ToListAsync),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(CancellationToken), 1));

			FirstOrDefaultMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EntityFrameworkQueryableExtensions),
				nameof(EntityFrameworkQueryableExtensions.FirstOrDefaultAsync),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(CancellationToken), 1));

			CountMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EntityFrameworkQueryableExtensions),
				nameof(EntityFrameworkQueryableExtensions.CountAsync),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(CancellationToken), 1));
		}
	}

	public class Repository<TId> : Repository, IRepository<TId>
	{
		private readonly DbContext _dbContext;
		private readonly RepositorySettings _settings;
		private readonly SpecificationExecutor _specificationExecutor;

		public Repository(
			DbContext dbContext,
			RepositorySettings settings)
		{
			_dbContext = dbContext;
			_settings = settings;
			_specificationExecutor = new SpecificationExecutor();
		}

		public async Task<object> GetAsync(TId id, Type entityType, CancellationToken cancellationToken = default(CancellationToken))
		{
			var value = await _dbContext.FindAsync(entityType, new object[]{id}, cancellationToken);
			return value;
		}

		public async Task<T> GetAsync<T>(TId id, CancellationToken cancellationToken = default(CancellationToken))
			where T : class
		{
			var value = await _dbContext.FindAsync<T>(new object[]{id}, cancellationToken);
			return value;
		}

		public async Task<object> GetAsync(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = _specificationExecutor.ExecuteSpecification(_dbContext, specification);
			var firstOrDefault = FirstOrDefaultMethodInfo.MakeGenericMethod(specification.ResultType);
			dynamic task = firstOrDefault.Invoke(query, new object[] { query, cancellationToken });

			await task;

			return task.Result;
		}

		public async Task<T> GetAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default(CancellationToken))
			where T : class
		{
			var item = await GetAsync((ISpecification)specification, cancellationToken);
			return (T)item;
		}

		public async Task<T> GetAsync<T>(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
			where T : class
		{
			var item = await GetAsync(specification, cancellationToken);
			return (T)item;
		}

		public async Task<ICollection> ListAsync(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = _specificationExecutor.ExecuteSpecification(_dbContext, specification);
			var toList = ToListMethodInfo.MakeGenericMethod(specification.ResultType);
			dynamic task = toList.Invoke(query, new object[] { query, cancellationToken });

			await task;

			return task.Result;
		}

		public Task<IReadOnlyCollection<T>> ListAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default(CancellationToken))
			where T : class
		{
			return ListAsync<T>((ISpecification)specification, cancellationToken);
		}

		public async Task<IReadOnlyCollection<T>> ListAsync<T>(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
			where T : class
		{
			var items = await _specificationExecutor
				.ExecuteSpecification<T>(_dbContext, specification)
				.ToListAsync(cancellationToken);

			return items.AsReadOnly();
		}

		public Task<int> CountAsync(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = _specificationExecutor.ExecuteSpecification(_dbContext, specification);
			var countMethod = CountMethodInfo.MakeGenericMethod(specification.ResultType);
			var task = (Task<int>)countMethod.Invoke(query, new object[] { query, cancellationToken });

			return task;
		}

		public async Task SaveAsync(object entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var scope = new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = _settings.TransactionTimeout,
					IsolationLevel = _settings.TransactionIsolationLevel
				}))
			{
				_dbContext.Update(entity);
				await _dbContext.SaveChangesAsync(cancellationToken);
				scope.Complete();
			}
		}

		public async Task SaveAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var scope = new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = _settings.TransactionTimeout,
					IsolationLevel = _settings.TransactionIsolationLevel
				}))
			{
				_dbContext.UpdateRange(entities);
				await _dbContext.SaveChangesAsync(cancellationToken);
				scope.Complete();
			}
		}

		public async Task DeleteAsync(object entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var scope = new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = _settings.TransactionTimeout,
					IsolationLevel = _settings.TransactionIsolationLevel
				}))
			{
				_dbContext.Remove(entity);
				await _dbContext.SaveChangesAsync(cancellationToken);

				scope.Complete();
			}
		}

		public async Task DeleteAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var scope = new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = _settings.TransactionTimeout,
					IsolationLevel = _settings.TransactionIsolationLevel
				}))
			{
				_dbContext.RemoveRange(entities);
				await _dbContext.SaveChangesAsync(cancellationToken);

				scope.Complete();
			}
		}

		public Task DeleteAsync(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotSupportedException();
		}
	}
}