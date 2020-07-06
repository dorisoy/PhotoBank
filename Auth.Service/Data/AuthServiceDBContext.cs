using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace PhotoBank.Auth.Service.Data
{
    public class AuthServiceDBContext : DbContext
    {
        public AuthServiceDBContext(string connectionString)
        {
            ConnectionString = connectionString;
            base.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }

        public DbSet<UserPoco> Users { get; set; }

        public string ConnectionString { get; }
    }
}
