using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Domain.Entities;

namespace TechTest.Infrastructure.Persistence.Configurations
{
    public sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> e)
        {
            e.ToTable("country");

            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id");

            e.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.Iso2)
                .HasColumnName("iso2")
                .HasMaxLength(2);

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            e.HasIndex(x => x.Name).IsUnique();
            e.HasIndex(x => x.Iso2).IsUnique();
        }
    }
}
