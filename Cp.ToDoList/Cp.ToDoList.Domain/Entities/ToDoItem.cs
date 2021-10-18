using System;

namespace Cp.ToDoList.Domain.Entities
{
    public class ToDoItem
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

    }
}
