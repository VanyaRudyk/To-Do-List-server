namespace kursovaya.Server.DTOs
{
    public class ToDoDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int ToDoListId { get; set; }
    }

    public class CreateToDoDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ToDoListId { get; set; }
    }

    public class UpdateToDoDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateToDoStatusDto
    {
        public bool IsCompleted { get; set; }
    }
}



