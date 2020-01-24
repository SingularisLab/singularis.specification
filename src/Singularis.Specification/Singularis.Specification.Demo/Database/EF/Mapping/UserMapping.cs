using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Database.EF.Mapping
{
	public class UserMapping : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");
			builder.HasKey(x => x.Id);
			builder
				.Property(x => x.Email)
				.HasColumnName("Email");

			builder
				.Property(x => x.Firstname)
				.HasColumnName("Firstname");

			builder
				.Property(x => x.Lastname)
				.HasColumnName("Lastname");

			builder
				.HasMany(x => x.Characters)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId);
		}
	}
}