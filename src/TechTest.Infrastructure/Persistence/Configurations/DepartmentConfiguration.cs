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
    public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> e)
        {
            e.ToTable("department");

            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id");

            e.Property(x => x.CountryId)
                .HasColumnName("country_id")
                .IsRequired();

            e.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            e.HasOne(x => x.Country)
                .WithMany(x => x.Departments)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.CountryId);
            e.HasIndex(x => new { x.CountryId, x.Name }).IsUnique();
        }
    }
}
