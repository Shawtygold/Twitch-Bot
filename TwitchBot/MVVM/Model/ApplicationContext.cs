using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.MVVM.Model
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Command> Commands => Set<Command>();
        public DbSet<Timer> Timers => Set<Timer>();

        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=databases.db");
        }
    }
}
