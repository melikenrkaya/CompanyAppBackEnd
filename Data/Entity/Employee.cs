using companyappbasic.Data.Models;

namespace companyappbasic.Data.Entity
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime HireDate { get; set; } = DateTime.Now;
        public ICollection <Tasks> AssignedTasks { get; set; } = [];



    }
}
