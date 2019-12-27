namespace Singularis.Specification.Domain.Query
{
    internal enum EQueryType
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
        TakeWhile
    }
}