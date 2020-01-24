namespace Singularis.Specification.Definition.Query
{
	internal class FetchedQuery<TEntity, TFetch> : Query<TEntity>, IFetchedQuery<TEntity, TFetch>
	{
		public FetchedQuery(Query parent)
			: base(parent)
		{
		}
	}
}