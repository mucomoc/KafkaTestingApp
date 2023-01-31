using System.Reflection;
using KafkaTestingApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace KafkaTestingApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
    }
    
    public class TemporaryDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private readonly IConfiguration _configuration;
        public TemporaryDbContextFactory() { }
        
        public TemporaryDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public AppDbContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = configBuilder.Build();
            
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(AppDbContext).GetTypeInfo().Assembly.GetName().Name));
            return new AppDbContext(builder.Options);
        }
    }
}