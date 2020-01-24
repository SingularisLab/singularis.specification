using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.EF.Mapping
{
	public class ItemMapping : IEntityTypeConfiguration<Item>
	{
		public void Configure(EntityTypeBuilder<Item> builder)
		{
			builder.ToTable("Items");
			builder.HasKey(x => x.Id);
			builder
				.Property(x => x.Name)
				.HasColumnName("Name");

			builder
				.Property(x => x.Name)
				.HasColumnName("Name");

			builder
				.HasMany(x => x.Runes)
				.WithOne(x => x.Item)
				.HasForeignKey(x => x.ItemId);
		}
	}
}