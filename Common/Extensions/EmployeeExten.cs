using companyappbasic.Data.Entity;
using companyappbasic.Data.Models;

namespace companyappbasic.Common.Extensions
{
    public static class EmployeeExten
    {
        public static EmployeeDto ToEmployeesDto(this Employee EmployeesModel)
        {
            return new EmployeeDto
            {
                Id = EmployeesModel.Id,
                FirstName = EmployeesModel.FirstName,
                LastName = EmployeesModel.LastName,
                Email = EmployeesModel.Email,
                PhoneNumber = EmployeesModel.PhoneNumber,
                Department = EmployeesModel.Department,
                //NAVIGASYON OZELLIKLERI
             //   AssignedTasks = EmployeesModel.AssignedTasks?.Select(t => t.ToTaskDto()).ToList(),
                
            };
        }


        public static Data.Entity.Employee ToEmployeesFromCreateDTO(this CreateEmployeesRequestDto employeesDto)
        {
            return new Employee
            {
                FirstName = employeesDto.FirstName,
                LastName = employeesDto.LastName,
                Email = employeesDto.Email,
                PhoneNumber = employeesDto.PhoneNumber,
                Department = employeesDto.Department,
            };
        }
    }
}
