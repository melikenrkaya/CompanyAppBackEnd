using companyappbasic.Data.Entity;
using companyappbasic.Data.Models;

namespace companyappbasic.Services.TaskServices
{
    public interface ITask
    {
        Task<List<Tasks>> GetAllAsync();
        Task<Tasks?> GetByIdAsync(int id);
        Task<Tasks?> CreateAsync(Tasks tasksModel);
        Task<Tasks?> UpdateAsync(int id, UpdateTaskRequestDto updatetasksDto);
        Task<Tasks?> DeleteAsync(int id);
    }
}
