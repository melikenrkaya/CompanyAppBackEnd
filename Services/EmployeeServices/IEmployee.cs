using companyappbasic.Data.Entity;
using companyappbasic.Data.Models;

namespace companyappbasic.Services.EmployeeServices
{
    public interface IEmployee
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task<Employee?> CreateAsync(Employee employeesModel);
        Task<Employee?> UpdateAsync(int id, UpdateEmployeesRequestDto employeesDto);
        Task<Employee?> DeleteAsync(int id);
    }
}
