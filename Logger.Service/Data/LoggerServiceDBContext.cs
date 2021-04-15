using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace PhotoBank.Logger.Service.Data
{
    public class LoggerServiceDBContext : DbContext
    {
        public LoggerServiceDBContext(string connectionString)
        {
            ConnectionString = connectionString;
            base.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }

        public DbSet<LogPoco> Logs { get; set; }

        public string ConnectionString { get; }
    }
}
