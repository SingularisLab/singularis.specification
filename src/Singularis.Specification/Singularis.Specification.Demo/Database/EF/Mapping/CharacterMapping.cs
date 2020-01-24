using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.EF.Mapping
{
	public class CharacterMapping : IEntityTypeConfiguration<Character>
	{
		public void Configure(EntityTypeBuilder<Character> builder)
		{
			builder.ToTable("Characters");
			builder.HasKey(x => x.Id);
			builder
				.Property(x => x.CreatedAt)
				.HasColumnName("CreatedAt");

			builder
				.Property(x => x.Name)
				.HasColumnName("Name");

			builder
				.HasMany(x => x.Items)
				.WithOne(x => x.Character)
				.HasForeignKey(x => x.CharacterId);
		}
	}
}
