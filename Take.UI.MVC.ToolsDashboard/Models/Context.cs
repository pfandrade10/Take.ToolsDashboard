using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Take.UI.MVC.ToolsDashboard.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<UserModel> User { get; set; }
        public DbSet<Tool> Tool { get; set; }


    }

    /// <summary>
    /// Factory class for EmployeesContext
    /// </summary>

    public static class ContextFactory
    {
        public static Context Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            //Ensure database creation
            var context = new Context(optionsBuilder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
