namespace Singularis.Specification.Domain.QueryParameters
{
    internal class OrderByQueryParameter: OrderQueryParameter
    {
        public override QueryParameter Clone()
        {
            return new OrderByQueryParameter
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