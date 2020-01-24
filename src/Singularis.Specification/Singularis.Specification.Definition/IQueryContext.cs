using System.Linq;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition
{
	public interface IQueryContext
	{
		IQueryable<T> GetQueryResult<T>(IQuery query);
	}
}