namespace Singularis.Specification.Domain.QueryParameters
{
    internal class WhereQueryParameter : QueryParameter
    {
        public override QueryParameter Clone()
        {
            return new WhereQueryParameter
            {
                Expression = Expression,
                InType = InType,
                OutType = OutType,
                Type = Type
            };
        }
    }
}