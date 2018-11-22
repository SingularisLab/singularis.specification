using System.Collections.Generic;
using System.Linq;
using Singularis.Specification.Domain.Query;

namespace Singularis.Specification.Domain
{
	class QueryContext: IQueryContext
	{
		private readonly Dictionary<string, IQuery> _sources;

		public QueryContext()
		{
			_sources = new Dictionary<string, IQuery>();
		}

		public void RegisterSource(string name, IQuery query)
		{
			_sources.Add(name, query);
		}

		public IQueryable<T> GetSource<T>(string name)
		{
			return null;
		}

		public IQuery GetQuery(string name)
		{
			return _sources[name];
		}
		
		public void CopyTo(IQueryContext context)
		{
			var contestSource = ((QueryContext) context)._sources;
			foreach(var itm in _sources)
				contestSource.Add(itm.Key, itm.Value);
		}
	}
}