using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Colors.API.Controllers
{
    /// <summary>
    /// An example controller performs CRUD operations on TodoItems
    /// </summary>
    [ApiController]
    [Produces("application/json", "text/plain")]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private static readonly List<TodoItem> TodoItems = new List<TodoItem>
        {
            new TodoItem
            {
                Id = 1,
                Name = "feed fish",
                IsComplete = true
            },
            new TodoItem
            {
                Id = 2,
                Name = "walk dog",
                IsComplete = true
            }
        };

        /// <summary>
        /// Gets a TodoItem by ID
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     GET /api/todos/1
        ///
        /// Sample response:
        ///
        ///     {
        ///         "id": 1,
        ///         "name": "feed fish",
        ///         "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The ID of a TodoItem.</param>
        /// <returns></returns>
        /// <response code="200">Returns the item with the specified ID</response>
        /// <response code="404">If the item is not found.</response>  
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TodoItem> GetTodoItem(int id)
        {
            var todoItem = TodoItems.FirstOrDefault(x => x.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/todos
        ///     {
        ///        "id": 3,
        ///        "name": "push up",
        ///        "isComplete": false
        ///     }
        ///
        /// Sample response body:
        /// 
        ///     {
        ///        "id": 3,
        ///        "name": "push up",
        ///        "isComplete": false
        ///     }
        /// 
        /// Sample response header:
        /// 
        ///     content-type: application/json; charset=utf-8 
        ///     date: Wed, 01 Apr 2020 20:30:05 GMT
        ///     location: http://localhost:5000/api/Todos/3 
        ///     server: Kestrel
        ///     transfer-encoding: chunked
        /// 
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TodoItem> Create(TodoItem item)
        {
            TodoItems.Add(item);
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        /// <summary>
        /// Modifies a ToDoItem
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todoItem"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult PutTodoItem(int id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            var todo = TodoItems.FirstOrDefault(x => x.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.Name = todoItem.Name;
            todo.IsComplete = todoItem.IsComplete;

            return NoContent();
        }

        /// <summary>
        /// Deletes a TodoItem
        /// </summary>
        /// <remarks>
        ///
        /// # Header 1
        ///
        /// ## Header 2
        /// 
        /// 1. Item 1
        /// 1. Item 2
        ///
        /// * Item 1
        /// * Item 2
        ///
        /// inline `code`
        ///
        /// [link](#)
        ///
        /// ### Table
        ///
        /// Column 1 | Column 2 | Column 2
        /// -------- | -------- | ---------
        /// Value 1  | Value 2  | Value 3
        ///
        /// <input type="text"></input>
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult<TodoItem> DeleteTodoItem(int id)
        {
            var todoItem = TodoItems.FirstOrDefault(x => x.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            TodoItems.Remove(todoItem);
            return todoItem;
        }
    }

    /// <summary>
    /// A TodoItem tracks a task
    /// </summary>
    public class TodoItem
    {
        /// <summary>
        /// The ID of the TodoItem
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// The task name.
        /// </summary>
        /// <example>
        ///     feed fish
        /// </example>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The task is completed or not. Default: <code>false</code>.
        /// </summary>
        /// <example>false</example>
        public bool IsComplete { get; set; }
    }
}
