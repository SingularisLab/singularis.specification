namespace Singularis.Specification.Domain.QueryParameters
{
    internal class TakeQueryParameter : QueryParameter
    {
        public int Count { get; set; }

        public override QueryParameter Clone()
        {
            return new TakeQueryParameter
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