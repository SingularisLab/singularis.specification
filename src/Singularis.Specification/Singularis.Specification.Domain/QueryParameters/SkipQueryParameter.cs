namespace Singularis.Specification.Domain.QueryParameters
{
    internal class SkipQueryParameter : QueryParameter
    {
        public int Count { get; set; }

        public override QueryParameter Clone()
        {
            return new SkipQueryParameter
            {
                Count = Count,
                Expression = Expression,
                InType = InType,
                OutType = OutType,
                Type = Type
            };
        }
    }
}