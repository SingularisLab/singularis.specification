namespace Singularis.Specification.Domain.QueryParameters
{
    internal class ThenByDescendingQueryParameter : OrderQueryParameter
    {
        public override QueryParameter Clone()
        {
            return new ThenByDescendingQueryParameter
            {
                Expression = Expression,
                InType = InType,
                KeyType = KeyType,
                OutType = OutType,
                Type = Type
            };
        }
    }
}