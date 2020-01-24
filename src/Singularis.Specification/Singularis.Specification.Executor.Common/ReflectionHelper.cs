using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Singularis.Specification.Executor.Common
{
	internal static class ReflectionHelper
	{
		class HandlerCacheKey
		{
			protected bool Equals(HandlerCacheKey other)
			{
				return Equals(BaseMethod, other.BaseMethod) && Equals(GenericTypes, other.GenericTypes);
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != this.GetType()) return false;
				return Equals((HandlerCacheKey)obj);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					return ((BaseMethod != null ? BaseMethod.GetHashCode() : 0) * 397) ^ (GenericTypes != null ? GenericTypes.GetHashCode() : 0);
				}
			}

			public MethodInfo BaseMethod { get; }
			public Type[] GenericTypes { get; }

			public HandlerCacheKey(MethodInfo baseMethod, params Type[] genericTypes)
			{
				BaseMethod = baseMethod;
				GenericTypes = genericTypes;
			}
		}

		private static readonly ConcurrentDictionary<HandlerCacheKey, ReflectionHelper.InvokeHandler> Handlers;

		static ReflectionHelper()
		{
			Handlers = new ConcurrentDictionary<HandlerCacheKey, InvokeHandler>();
		}

		public delegate object InvokeHandler(object target, object[] parameters);

		public static ReflectionHelper.InvokeHandler GetHandler(MethodInfo baseMethod, params Type[] entityTypes)
		{
			var key = new HandlerCacheKey(baseMethod, entityTypes);
			if (!Handlers.TryGetValue(key, out var handler))
			{
				var methodDefinition = baseMethod;
				if (entityTypes != null && entityTypes.Length > 0)
					methodDefinition = methodDefinition.MakeGenericMethod(entityTypes);

				handler = ReflectionHelper.CreateInvoker(methodDefinition);
				Handlers.TryAdd(key, handler);
			}

			return handler;
		}

		public static InvokeHandler CreateInvoker(MethodInfo methodInfo)
		{
			var instanceParameter = Expression.Parameter(typeof(object), "target");
			var argumentsParameter = Expression.Parameter(typeof(object[]), "arguments");

			Expression callExpression;
			if (methodInfo.IsStatic)
				callExpression = Expression.Call(methodInfo, CreateParameterExpressions(methodInfo, argumentsParameter));
			else
				callExpression = Expression.Call(
					Expression.Convert(instanceParameter, methodInfo.DeclaringType),
					methodInfo,
					CreateParameterExpressions(methodInfo, argumentsParameter));

			Expression bodyExpression;
			if (methodInfo.ReturnType == typeof(void))
			{
				var returnTargetExpression = Expression.Label(typeof(object));
				var returnExpression = Expression.Return(returnTargetExpression, Expression.Constant(new object()), typeof(object));
				var returnLabelExpression = Expression.Label(returnTargetExpression, Expression.Constant(new object()));
				bodyExpression = Expression.Block(
					typeof(object),
					callExpression,
					returnExpression,
					returnLabelExpression);
			}
			else
				bodyExpression = Expression.Convert(callExpression, typeof(object));

			var lambda = Expression.Lambda<InvokeHandler>(
				bodyExpression,
				instanceParameter,
				argumentsParameter);

			return lambda.Compile();
		}

		public static MethodInfo FindMethod(
			BindingFlags flags,
			Type ownerType,
			string name,
			bool? genericMethod,
			params ArgumentConstraint[] constraints)
		{
			var candidates = ownerType
				.GetMethods(flags)
				.Where(x => x.Name == name)
				.Where(x => !genericMethod.HasValue || genericMethod.Value == x.IsGenericMethod)
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

		private static Expression[] CreateParameterExpressions(MethodInfo method, Expression argumentsParameter)
		{
			return method
				.GetParameters()
				.Select((parameter, index) => Expression.Convert(
					Expression.ArrayIndex(argumentsParameter, Expression.Constant(index)),
					parameter.ParameterType))
				.ToArray();
		}

		private static bool CheckConstraints(Type parameterType, ArgumentConstraint constraint)
		{
			var parameters = parameterType.GetGenericArguments();

			if (parameters.Length != constraint.GenericTypes.Count)
				return false;

			var isMatch = true;

			foreach (var itm in Enumerable.Zip(parameters, constraint.GenericTypes, (p, c) => new { Parameter = p, Constraint = c }))
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