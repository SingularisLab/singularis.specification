namespace Singularis.Specification.Domain.QueryParameters
{
    internal class OrderByDescendingQueryParameter : OrderQueryParameter
    {
        public override QueryParameter Clone()
        {
            return new OrderByDescendingQueryParameter
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