using Microsoft.EntityFrameworkCore;
using PE.DataManager.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PE.DataManager
{
    public class SqLiteDbContext : DbContext, ISqLiteDbContext
    {
        public SqLiteDbContext()
        {
            //Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=PEDataBase.db");
    }
}
