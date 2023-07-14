using Microsoft.EntityFrameworkCore;

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
