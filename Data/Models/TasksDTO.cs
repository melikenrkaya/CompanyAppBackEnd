using companyappbasic.Data.Entity;

namespace companyappbasic.Data.Models
{
    public class TasksDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int AssignedToEmployeeId { get; set; }
        public Employee? AssignedEmployee { get; set; }
    }
    public class CreateTaskRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int AssignedToEmployeeId { get; set; }
     
    }
    public class UpdateTaskRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int AssignedToEmployeeId { get; set; }
        
    }


}
