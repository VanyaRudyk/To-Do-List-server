using System.ComponentModel.DataAnnotations;

namespace kursovaya.Server.DTOs
{
    public class AssignRoleDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}



