namespace Singularis.Specification.Domain.QueryParameters
{
    internal class ThenByQueryParameter : OrderQueryParameter
    {
        public override QueryParameter Clone()
        {
            return new ThenByQueryParameter
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