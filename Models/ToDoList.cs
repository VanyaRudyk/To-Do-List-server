namespace kursovaya.Server.Models
{
    public class ToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public List<ToDo> Items { get; set; }
        public ToDoList() {
            Items = new List<ToDo>();
        }
    }
}
