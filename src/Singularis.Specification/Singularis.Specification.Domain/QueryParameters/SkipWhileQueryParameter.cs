namespace Singularis.Specification.Domain.QueryParameters
{
    internal class SkipWhileQueryParameter : QueryParameter
    {
        public override QueryParameter Clone()
        {
            return new SkipWhileQueryParameter
            {
                Expression = Expression,
                InType = InType,
                OutType = OutType,
                Type = Type
            };
        }
    }
}