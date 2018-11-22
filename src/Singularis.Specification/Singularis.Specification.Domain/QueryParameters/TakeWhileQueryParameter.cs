namespace Singularis.Specification.Domain.QueryParameters
{
    internal class TakeWhileQueryParameter : QueryParameter
    {
        public override QueryParameter Clone()
        {
            return new TakeWhileQueryParameter
            {
                Expression = Expression,
                InType = InType,
                OutType = OutType,
                Type = Type
            };
        }
    }
}