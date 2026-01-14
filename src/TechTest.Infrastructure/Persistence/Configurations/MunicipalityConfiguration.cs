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
    public sealed class MunicipalityConfiguration : IEntityTypeConfiguration<Municipality>
    {
        public void Configure(EntityTypeBuilder<Municipality> e)
        {
            e.ToTable("municipality");

            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id");

            e.Property(x => x.DepartmentId)
                .HasColumnName("department_id")
                .IsRequired();

            e.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(120)
                .IsRequired();

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            e.HasOne(x => x.Department)
                .WithMany(x => x.Municipalities)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.DepartmentId);
            e.HasIndex(x => new { x.DepartmentId, x.Name }).IsUnique();
        }
    }
}
