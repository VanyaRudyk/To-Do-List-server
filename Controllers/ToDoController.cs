using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kursovaya.Server.Data;
using kursovaya.Server.DTOs;
using kursovaya.Server.Models;

namespace kursovaya.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/ToDo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoDto>>> GetToDos([FromQuery] int? toDoListId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            
            IQueryable<ToDo> query = _context.ToDos.Include(t => t.ToDoList);
            
            if (toDoListId.HasValue)
            {
                query = query.Where(t => t.ToDoListId == toDoListId.Value);
            }

            if (!isAdmin)
            {
                // User бачить лише записи зі своїх списків
                query = query.Where(t => t.ToDoList!.UserId == user.Id);
            }

            var toDos = await query.ToListAsync();
            return Ok(toDos.Select(t => new ToDoDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                ToDoListId = t.ToDoListId
            }));
        }

        // GET: api/ToDo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoDto>> GetToDo(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            
            var toDo = await _context.ToDos
                .Include(t => t.ToDoList)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (toDo == null)
            {
                return NotFound();
            }

            // Перевірка прав доступу
            if (!isAdmin && toDo.ToDoList?.UserId != user.Id)
            {
                return Forbid();
            }

            return Ok(new ToDoDto
            {
                Id = toDo.Id,
                Title = toDo.Title,
                Description = toDo.Description,
                IsCompleted = toDo.IsCompleted,
                ToDoListId = toDo.ToDoListId
            });
        }

        // POST: api/ToDo
        [HttpPost]
        public async Task<ActionResult<ToDoDto>> CreateToDo([FromBody] CreateToDoDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            
            var toDoList = await _context.ToDoLists.FindAsync(dto.ToDoListId);
            if (toDoList == null)
            {
                return NotFound(new { message = "Список не знайдено" });
            }

            // Перевірка прав доступу
            if (!isAdmin && toDoList.UserId != user.Id)
            {
                return Forbid();
            }

            var toDo = new ToDo
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                ToDoListId = dto.ToDoListId
            };

            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDo), new { id = toDo.Id }, new ToDoDto
            {
                Id = toDo.Id,
                Title = toDo.Title,
                Description = toDo.Description,
                IsCompleted = toDo.IsCompleted,
                ToDoListId = toDo.ToDoListId
            });
        }

        // PUT: api/ToDo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDo(int id, [FromBody] UpdateToDoDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            
            var toDo = await _context.ToDos
                .Include(t => t.ToDoList)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (toDo == null)
            {
                return NotFound();
            }

            // Перевірка прав доступу
            if (!isAdmin && toDo.ToDoList?.UserId != user.Id)
            {
                return Forbid();
            }

            toDo.Title = dto.Title;
            toDo.Description = dto.Description;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/ToDo/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateToDoStatus(int id, [FromBody] UpdateToDoStatusDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            
            var toDo = await _context.ToDos
                .Include(t => t.ToDoList)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (toDo == null)
            {
                return NotFound();
            }

            // Перевірка прав доступу
            if (!isAdmin && toDo.ToDoList?.UserId != user.Id)
            {
                return Forbid();
            }

            toDo.IsCompleted = dto.IsCompleted;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ToDo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            
            var toDo = await _context.ToDos
                .Include(t => t.ToDoList)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (toDo == null)
            {
                return NotFound();
            }

            // Проверка прав доступа
            if (!isAdmin && toDo.ToDoList?.UserId != user.Id)
            {
                return Forbid();
            }

            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}



