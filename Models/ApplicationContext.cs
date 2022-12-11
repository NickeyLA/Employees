using Microsoft.EntityFrameworkCore;

namespace Employees_Test.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Employee> Employees_Test { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
