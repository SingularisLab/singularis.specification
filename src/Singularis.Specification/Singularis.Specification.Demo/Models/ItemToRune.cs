namespace Singularis.Specification.Demo.Models
{
	public class ItemToRune
	{
		public virtual Item Item { get; set; }
		public virtual int ItemId { get; set; }

		public virtual Rune Rune { get; set; }
		public virtual int RuneId { get; set; }

		protected bool Equals(ItemToRune other)
		{
			return ItemId == other.ItemId && RuneId == other.RuneId;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((ItemToRune)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (ItemId * 397) ^ RuneId;
			}
		}
	}
}