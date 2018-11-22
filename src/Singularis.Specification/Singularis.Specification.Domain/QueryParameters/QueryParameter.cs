using System;
using System.Linq.Expressions;
using Singularis.Specification.Domain.Query;

namespace Singularis.Specification.Domain.QueryParameters
{
    internal class QueryParameter
    {
        public Type InType { get; set; }
        public Type OutType { get; set; }
        public Expression Expression { get; set; }
        public EQueryType Type { get; set; }

        public virtual QueryParameter Clone()
        {
            return new QueryParameter
            {
                InType = InType,
                OutType = OutType,
                Expression = Expression,
                Type = Type
            };
        }
    }
}