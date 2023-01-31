using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KafkaTestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load appsettings.json configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            // Configure SQLite database
            var optionsBuilder = new DbContextOptionsBuilder<Data.AppDbContext>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            // Create the database and seed data
            using (var context = new Data.AppDbContext(optionsBuilder.Options))
            {
                context.Database.EnsureCreated();

                if (context.People.CountAsync().Result == 0)
                {
                    context.People.Add(new Models.Person { Name = "John Doe" });
                    context.SaveChanges();
                }
                
                var records = context.People.ToList();
                
                foreach (var record in records)
                {
                    Console.WriteLine("Id: " + record.Id);
                    Console.WriteLine("Name: " + record.Name);
                    Console.WriteLine("==============================");
                }
            }
        }
    }
}