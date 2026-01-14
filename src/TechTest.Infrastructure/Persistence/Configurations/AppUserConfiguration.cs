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
    public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> e)
        {
            e.ToTable("app_user");

            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id");

            e.Property(x => x.FullName)
                .HasColumnName("full_name")
                .HasMaxLength(120)
                .IsRequired();

            e.Property(x => x.Phone)
                .HasColumnName("phone")
                .HasMaxLength(20)
                .IsRequired();

            e.Property(x => x.Address)
                .HasColumnName("address")
                .HasMaxLength(200)
                .IsRequired();

            e.Property(x => x.MunicipalityId)
                .HasColumnName("municipality_id")
                .IsRequired();

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            e.HasOne(x => x.Municipality)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.MunicipalityId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.MunicipalityId);
        }
    }
}
