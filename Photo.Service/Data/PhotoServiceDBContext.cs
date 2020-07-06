using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace PhotoBank.Photo.Service.Data
{
    public class PhotoServiceDBContext : DbContext
    {
        public PhotoServiceDBContext(string connectionString)
        {
            ConnectionString = connectionString;
            base.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }

        public DbSet<PhotoPoco> Photos { get; set; }

        public string ConnectionString { get; }
    }
}
