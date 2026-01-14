using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Domain.Entities;

namespace TechTest.Infrastructure.Persistence
{
    public sealed class TechTestDbContext : DbContext
    {
        public TechTestDbContext(DbContextOptions<TechTestDbContext> options) : base(options) { }

        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Municipality> Municipalities => Set<Municipality>();
        public DbSet<AppUser> Users => Set<AppUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones del assembly Infrastructure
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechTestDbContext).Assembly);
        }
    }
}
