namespace Singularis.Specification.Definition.Query
{
	internal enum QueryType
	{
		Where,
		Projection,
		OrderBy,
		OrderByDescending,
		ThenBy,
		ThenByDescending,
		Skip,
		Take,
		SkipWhile,
		TakeWhile,
		GroupBy,
		Fetch,
		ThenFetch,
		Join,
		Empty
	}
}