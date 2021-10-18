using Cp.ToDoList.Domain.Entities;
using Cp.ToDoList.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cp.ToDoList.Repositories
{
    public class ToDoItemRepository : IToDoItemRepository, IDisposable
    {
        private readonly ToDoContext _toDoDataContext;

        public ToDoItemRepository(ToDoContext toDoDataContext)
        {
            _toDoDataContext = toDoDataContext;
        }
        

        public async Task<List<ToDoItem>> GetToDoItemsAsync()
        {
            return await _toDoDataContext.ToDoItems.Where(x => !x.IsCompleted).ToListAsync();
        }

        public async Task<ToDoItem> GetToDoItemAsync(Guid id)
        {
            return await _toDoDataContext.ToDoItems.FindAsync(id);
        }

        public async Task CreateToDoItemAsync(ToDoItem todoItem)
        {
            _toDoDataContext.ToDoItems.Add(todoItem);
            await _toDoDataContext.SaveChangesAsync();
        }

        public async Task UpdateToDoItemAsync(ToDoItem todoItem)
        {
            _toDoDataContext.Entry(todoItem).State = EntityState.Modified;
            await _toDoDataContext.SaveChangesAsync();
        }

        public bool ToDoItemIdExists(Guid id)
        {
            return _toDoDataContext.ToDoItems.Any(x => x.Id == id);
        }

        public bool ToDoItemDescriptionExists(string description)
        {
            return _toDoDataContext.ToDoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _toDoDataContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
