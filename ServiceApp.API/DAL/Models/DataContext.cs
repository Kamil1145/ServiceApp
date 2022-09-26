using Microsoft.EntityFrameworkCore;
using ServiceApp.API.Models.EntityConfiguration;
using ServiceApp.Models;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            new RoleEntityConfiguration().Configure(modelBuilder.Entity<Role>());
            new DepartmentEntityConfiguration().Configure(modelBuilder.Entity<Department>());
            new UserEntityConfiguration().Configure(modelBuilder.Entity<User>());
            new TicketEntityConfiguration().Configure(modelBuilder.Entity<Ticket>());
            new CommentEntityConfiguration().Configure(modelBuilder.Entity<Comment>());
        }
    }   
}
