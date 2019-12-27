namespace Singularis.Specification.Domain.QueryParameters
{
    internal class SelectQueryParameter: QueryParameter
    {
        public override QueryParameter Clone()
        {
            return new SelectQueryParameter
            {
                Expression = Expression,
                InType = InType,
                OutType = OutType,
                Type = Type
            };
        }
    }
}