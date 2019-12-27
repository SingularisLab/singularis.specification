using System.Linq;
using Singularis.Specification.Domain.Query;

namespace Singularis.Specification.Domain
{
	public interface IQueryContext
	{
		void RegisterSource(string name, IQuery query);
		IQueryable<T> GetSource<T>(string name);
		IQuery GetQuery(string name);
	}
}