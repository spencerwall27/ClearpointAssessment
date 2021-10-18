
using Cp.ToDoList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cp.ToDoList.Domain.Repositories
{
    public interface IToDoItemRepository
    {
        Task<List<ToDoItem>> GetToDoItemsAsync();

        Task<ToDoItem> GetToDoItemAsync(Guid id);

        Task CreateToDoItemAsync(ToDoItem todoItem);

        Task UpdateToDoItemAsync(ToDoItem todoItem);

        bool ToDoItemIdExists(Guid id);

        bool ToDoItemDescriptionExists(string description);

    }
}
