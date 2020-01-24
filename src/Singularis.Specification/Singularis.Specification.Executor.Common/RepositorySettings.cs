using System;
using System.Transactions;

namespace Singularis.Specification.Executor.Common
{
	public class RepositorySettings
	{
		public IsolationLevel TransactionIsolationLevel { get; }
		public TimeSpan TransactionTimeout { get; }

		public RepositorySettings(IsolationLevel transactionIsolationLevel, TimeSpan transactionTimeout)
		{
			TransactionIsolationLevel = transactionIsolationLevel;
			TransactionTimeout = transactionTimeout;
		}
	}
}