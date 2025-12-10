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
    public class ToDoListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDoListController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/ToDoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoListDto>>> GetToDoLists()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            
            IQueryable<ToDoList> query = _context.ToDoLists.Include(t => t.Items);
            
            if (!isAdmin)
            {
                // User бачить лише свої списки
                query = query.Where(t => t.UserId == user.Id);
            }
            // Admin бачить усі списки

            var lists = await query.ToListAsync();
            return Ok(lists.Select(l => new ToDoListDto
            {
                Id = l.Id,
                Name = l.Name,
                Items = l.Items.Select(i => new ToDoDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    IsCompleted = i.IsCompleted,
                    ToDoListId = i.ToDoListId
                }).ToList()
            }));
        }

        // GET: api/ToDoList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoListDto>> GetToDoList(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            
            var toDoList = await _context.ToDoLists
                .Include(t => t.Items)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (toDoList == null)
            {
                return NotFound();
            }

            // Проверка прав доступа
            if (!isAdmin && toDoList.UserId != user.Id)
            {
                return Forbid();
            }

            return Ok(new ToDoListDto
            {
                Id = toDoList.Id,
                Name = toDoList.Name,
                Items = toDoList.Items.Select(i => new ToDoDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    IsCompleted = i.IsCompleted,
                    ToDoListId = i.ToDoListId
                }).ToList()
            });
        }

        // POST: api/ToDoList
        [HttpPost]
        public async Task<ActionResult<ToDoListDto>> CreateToDoList([FromBody] CreateToDoListDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var toDoList = new ToDoList
            {
                Name = dto.Name,
                UserId = user.Id
            };

            _context.ToDoLists.Add(toDoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDoList), new { id = toDoList.Id }, new ToDoListDto
            {
                Id = toDoList.Id,
                Name = toDoList.Name,
                Items = new List<ToDoDto>()
            });
        }

        // PUT: api/ToDoList/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoList(int id, [FromBody] UpdateToDoListDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            
            var toDoList = await _context.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return NotFound();
            }

            // Перевірка прав доступу
            if (!isAdmin && toDoList.UserId != user.Id)
            {
                return Forbid();
            }

            toDoList.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ToDoList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoList(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            
            var toDoList = await _context.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return NotFound();
            }

            // Перевірка прав доступу
            if (!isAdmin && toDoList.UserId != user.Id)
            {
                return Forbid();
            }

            _context.ToDoLists.Remove(toDoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}



