using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace RPA_Alura
{
    public class AppDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "db", "courses.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}