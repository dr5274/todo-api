using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TodoApi.Repository;

namespace TodoApi.Controllers
{
    [Route("api/todo")]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger _logger;

        public TodoController(ITodoRepository todoRepository, ILogger<TodoController> logger)
        {
            _todoRepository = todoRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetTodoItems()
        {
            _logger.LogDebug("Getting all items...");
            var items = await _todoRepository.GetTodoItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public async Task<ActionResult> GetTodoItems(long id)
        {
            _logger.LogDebug("Getting item {ID}", id);
            var item = await _todoRepository.GetTodoItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TodoItem item)
        {
            if (item == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var todo = await _todoRepository.CreateTodoItemAsync(item);
            return CreatedAtRoute("GetTodo", new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = await _todoRepository.GetTodoItemAsync(item.Id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Description = item.Description;

            await _todoRepository.UpdateTodoItemAsync(todo);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var todo = await _todoRepository.GetTodoItemAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            await _todoRepository.DeleteTodoItemAsync(todo);
            return new NoContentResult();
        }
    }
}