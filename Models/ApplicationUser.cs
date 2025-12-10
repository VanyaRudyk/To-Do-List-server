using Microsoft.AspNetCore.Identity;

namespace kursovaya.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<ToDoList> ToDoLists { get; set; } = new List<ToDoList>();
    }
}



