using System;
using System.Linq;
using System.Reflection;

namespace Singularis.Specification.Executor.Nhibernate
{
	static class ReflectionHelper
	{
		public static MethodInfo FindMethod(BindingFlags flags, Type ownerType, string name, params ArgumentConstraint[] constraints)
		{
			var candidates = ownerType
				.GetMethods(flags)
				.Where(x => x.Name == name)
				.ToArray();

			foreach (var candidate in candidates)
			{
				var parameters = candidate.GetParameters();
				if (parameters.Length != constraints.Length)
					continue;

				var isMatch = true;

				foreach (var itm in Enumerable.Zip(parameters, constraints, (p, c) => new { Parameter = p, Constraint = c }))
				{
					if (itm.Constraint.GenericTypes.Count == 0)
					{
						if (itm.Parameter.ParameterType.IsGenericType)
							isMatch &= itm.Parameter.ParameterType.GetGenericTypeDefinition() == itm.Constraint.Type;
						else
							isMatch &= itm.Parameter.ParameterType == itm.Constraint.Type;
					}
					else
						isMatch &= CheckConstraints(itm.Parameter.ParameterType, itm.Constraint);
				}

				if (isMatch)
					return candidate;
			}

			throw new MissingMethodException(ownerType.FullName, name);
		}

		private static bool CheckConstraints(Type parameterType, ArgumentConstraint constraint)
		{
			var parameters = parameterType.GetGenericArguments();
			
			if (parameters.Length != constraint.GenericTypes.Count)
				return false;

			var isMatch = true;

			foreach (var itm in Enumerable.Zip(parameters, constraint.GenericTypes, (p, c) => new {Parameter = p, Constraint = c}))
			{
				if (itm.Constraint.GenericTypes.Any())
					isMatch &= CheckConstraints(itm.Parameter, itm.Constraint);
				else if (itm.Parameter.IsGenericType)
					isMatch &= itm.Parameter.GetGenericTypeDefinition() == itm.Constraint.Type;
				else
					isMatch &= itm.Parameter == itm.Constraint.Type;
			}

			return isMatch;
		}
	}
}