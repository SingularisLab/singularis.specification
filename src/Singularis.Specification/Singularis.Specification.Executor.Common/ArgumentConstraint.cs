using System;
using System.Collections.Generic;

namespace Singularis.Specification.Executor.Common
{
	internal class ArgumentConstraint
	{
		private readonly List<ArgumentConstraint> _genericTypes;

		public IReadOnlyCollection<ArgumentConstraint> GenericTypes => _genericTypes;

		public Type Type { get; }

		public ArgumentConstraint(Type argumentType, int depth)
		{
			Type = argumentType;
			_genericTypes = new List<ArgumentConstraint>();

			if (depth == 1)
				return;

			if (argumentType.IsGenericType)
			{
				var arguments = argumentType.GetGenericArguments();
				foreach (var argument in arguments)
					_genericTypes.Add(new ArgumentConstraint(argument, depth - 1));
			}
		}
	}
}