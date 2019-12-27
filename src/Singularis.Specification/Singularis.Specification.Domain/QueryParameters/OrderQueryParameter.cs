using System;

namespace Singularis.Specification.Domain.QueryParameters
{
    internal class OrderQueryParameter: QueryParameter
    {
        public Type KeyType { get; set; }

        public override QueryParameter Clone()
        {
            return new OrderQueryParameter
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