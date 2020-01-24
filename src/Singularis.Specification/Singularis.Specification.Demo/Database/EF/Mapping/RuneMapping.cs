using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.EF.Mapping
{
	public class RuneMapping : IEntityTypeConfiguration<Rune>
	{
		public void Configure(EntityTypeBuilder<Rune> builder)
		{
			builder.ToTable("Runes");
			builder.HasKey(x => x.Id);
			builder
				.Property(x => x.TargetAttribute)
				.HasColumnName("TargetAttribute");

			builder
				.Property(x => x.Modifier)
				.HasColumnName("Modifier");
		}
	}
}