using companyappbasic.Data.Entity;
using companyappbasic.Data.Models;

namespace companyappbasic.Common.Extensions
{
  
        public static class TaskExten
        {
            public static TasksDto ToTaskDto(this Tasks TasksModel)
            {
                return new TasksDto
                {
                    Id = TasksModel.Id,
                    Title = TasksModel.Title,
                    Description = TasksModel.Description,
                    AssignedToEmployeeId = TasksModel.AssignedToEmployeeId,
                };
            }
            public static Data.Entity.Tasks ToTasksFromCreateDTO(this CreateTaskRequestDto createtaskDto)
            {
                return new Tasks
                {
                    Title = createtaskDto.Title,
                    Description = createtaskDto.Description,
                    AssignedToEmployeeId = createtaskDto.AssignedToEmployeeId,

                };
            }
        }
    }


