using companyappbasic.Data.Context;
using companyappbasic.Data.Entity;
using companyappbasic.Data.Models;
using Microsoft.EntityFrameworkCore;




namespace companyappbasic.Services.EmployeeServices
{
    public class EmployeeServi : IEmployee
    {
        private readonly ApplicationDBContext _context;
        public EmployeeServi(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.Include(e => e.AssignedTasks).ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee?> CreateAsync(Employee employeesModel)
        {
            await _context.Employees.AddAsync(employeesModel);
            await _context.SaveChangesAsync();
            return employeesModel;
        }

        public async Task<Employee?> UpdateAsync(int id, UpdateEmployeesRequestDto employeesDto)
        {
            var existingEmployees = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (existingEmployees == null)
            {
                return null;
            }
            existingEmployees.FirstName = employeesDto.FirstName;
            existingEmployees.LastName = employeesDto.LastName;
            existingEmployees.Email = employeesDto.Email;
            existingEmployees.PhoneNumber = employeesDto.PhoneNumber;
            existingEmployees.Department = employeesDto.Department;

            await _context.SaveChangesAsync();
            return existingEmployees;
        }
        public async Task<Employee?> DeleteAsync(int id)
        {
            var employeesModel = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employeesModel == null)
            {
                return null;
            }
            _context.Employees.Remove(employeesModel);
            await _context.SaveChangesAsync();
            return employeesModel;
        }

        
    }
}
