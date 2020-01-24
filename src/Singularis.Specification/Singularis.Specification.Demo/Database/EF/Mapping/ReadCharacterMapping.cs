using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.EF.Mapping
{
	public class ReadCharacterMapping : IEntityTypeConfiguration<ReadCharacter>
	{
		public void Configure(EntityTypeBuilder<ReadCharacter> builder)
		{
			builder.ToTable("ReadCharacters");
			builder.HasKey(x => x.Id);
			builder
				.Property(x => x.Name)
				.HasColumnName("Name");

			builder
				.Property(x => x.UserId)
				.HasColumnName("UserId");
		}
	}
}