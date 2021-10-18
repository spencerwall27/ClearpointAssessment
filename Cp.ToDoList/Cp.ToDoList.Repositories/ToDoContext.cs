using Cp.ToDoList.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cp.ToDoList.Repositories
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
    : base(options)
        {
        }

        public ToDoContext()
        {

        }

        public virtual DbSet<ToDoItem> ToDoItems { get; set; }

    }
}
