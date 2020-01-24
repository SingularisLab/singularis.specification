using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.EF.Mapping
{
	public class ItemToRuneMapping : IEntityTypeConfiguration<ItemToRune>
	{
		public void Configure(EntityTypeBuilder<ItemToRune> builder)
		{
			builder.ToTable("ItemsToRunes");
			builder.HasKey(x => new {x.ItemId, x.RuneId});

			builder
				.HasOne(x => x.Item)
				.WithMany(x => x.Runes)
				.HasForeignKey(x => x.ItemId);

			builder
				.HasOne(x => x.Rune)
				.WithMany(x => x.Items)
				.HasForeignKey(x => x.RuneId);
		}
	}
}