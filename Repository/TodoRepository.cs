using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Repository
{
    public interface ITodoRepository
    {
        Task<List<TodoItem>> GetTodoItemsAsync();
        Task<TodoItem> GetTodoItemAsync(long id);
        Task<TodoItem> CreateTodoItemAsync(TodoItem item);
        Task<TodoItem> UpdateTodoItemAsync(TodoItem item);
        Task<bool> DeleteTodoItemAsync(TodoItem item);
    }


    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;

        public TodoRepository(TodoContext context)
        {
            _context = context;
            if (_context.TodoItems.Count() == 0)
            {
                // create some default items
                _context.TodoItems.Add(new TodoItem { Description = "Something to be done" });
                _context.TodoItems.Add(new TodoItem { Description = "Another item on the list" });
                _context.TodoItems.Add(new TodoItem { Description = "Knock it out of the park", IsComplete = true });
                _context.SaveChanges();
            }

        }

        public async Task<List<TodoItem>> GetTodoItemsAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem> GetTodoItemAsync(long id)
        {
            return await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TodoItem> CreateTodoItemAsync(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TodoItem> UpdateTodoItemAsync(TodoItem item)
        {
            _context.TodoItems.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteTodoItemAsync(TodoItem item)
        {
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
