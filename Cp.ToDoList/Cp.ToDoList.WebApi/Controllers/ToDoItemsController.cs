using Cp.ToDoList.Domain.Entities;
using Cp.ToDoList.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cp.ToDoList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ILogger<ToDoItemsController> _logger;
        private readonly IToDoItemRepository _toDoItemRepository;

        public ToDoItemsController(IToDoItemRepository toDoItemRepository, ILogger<ToDoItemsController> logger)
        {
            _toDoItemRepository = toDoItemRepository;
            _logger = logger;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<IActionResult> GetToDoItems()
        {
            var results = await _toDoItemRepository.GetToDoItemsAsync();
            return Ok(results);
        }

        // GET: api/ToDoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoItem(Guid id)
        {
            var result = await _toDoItemRepository.GetToDoItemAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/ToDoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(Guid id, ToDoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            if (!_toDoItemRepository.ToDoItemIdExists(id))
                return NotFound();

            await _toDoItemRepository.UpdateToDoItemAsync(todoItem);

            return NoContent();
        }

        // POST: api/ToDoItems 
        [HttpPost]
        public async Task<IActionResult> PostToDoItem(ToDoItem todoItem)
        {
            if (string.IsNullOrEmpty(todoItem?.Description))
            {
                return BadRequest("Description is required");
            }
            else if (_toDoItemRepository.ToDoItemDescriptionExists(todoItem.Description))
            {
                return BadRequest("Description already exists");
            }

            await _toDoItemRepository.CreateToDoItemAsync(todoItem);

            return CreatedAtAction(nameof(PostToDoItem), new { id = todoItem.Id }, todoItem);
        }

    }
}
