namespace kursovaya.Server.DTOs
{
    public class ToDoListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ToDoDto> Items { get; set; } = new List<ToDoDto>();
    }

    public class CreateToDoListDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateToDoListDto
    {
        public string Name { get; set; } = string.Empty;
    }
}



