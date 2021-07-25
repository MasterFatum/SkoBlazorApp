using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SkoBlazorApp
{
    public class SkoContext : DbContext
    {
        public SkoContext()
        {
        }

        public SkoContext(DbContextOptions<SkoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=VANHOME;Database=SOKO;Trusted_Connection=True;");
            }
        }
    }
}
