using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace Singularis.Specification.Demo.Database.EF
{
	class Context : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddLogging(builder =>
				builder.AddConsole()
					.AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information)
					.AddFilter(DbLoggerCategory.Infrastructure.Name, LogLevel.None)
					.AddFilter(DbLoggerCategory.Database.Connection.Name, LogLevel.None)
					.AddFilter(l => l == LogLevel.Information));
			var loggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();

			optionsBuilder
				.ConfigureWarnings(x => x.Throw(RelationalEventId.QueryClientEvaluationWarning))
				.UseLoggerFactory(loggerFactory)
				.EnableSensitiveDataLogging()
				.UseLazyLoadingProxies()
				.UseSqlServer("Server=cis;Database=test;Trusted_Connection=True;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
		}
	}
}
