using companyappbasic.Data.Models;

namespace companyappbasic.Data.Entity
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int AssignedToEmployeeId { get; set; }
        public Employee? AssignedEmployee { get; set; }

        
    }
}
