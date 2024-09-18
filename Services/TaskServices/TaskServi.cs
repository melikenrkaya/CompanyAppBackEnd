using companyappbasic.Data.Context;
using companyappbasic.Data.Entity;
using companyappbasic.Data.Models;
using Microsoft.EntityFrameworkCore;
using companyappbasic.Services.TaskServices;

namespace companyappbasic.Services.TaskServices
{
    public class TaskServi : ITask
    {
        private readonly ApplicationDBContext _context;
        public TaskServi(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Tasks>> GetAllAsync()
        {
            return await _context.Taskss.Include(e => e.AssignedEmployee).ToListAsync();
        }
        public async Task<Tasks?> CreateAsync(Tasks tasksModel)
        {
            await _context.Taskss.AddAsync(tasksModel);
            await _context.SaveChangesAsync();
            return tasksModel;
        }
        public async Task<Tasks?> GetByIdAsync(int id)
        {
            return await _context.Taskss.Include(e => e.AssignedEmployee).FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<Tasks?> UpdateAsync(int id, UpdateTaskRequestDto updatetasksDto)
        {
            var existingTasks = await _context.Taskss.FirstOrDefaultAsync(x => x.Id == id);
            if (existingTasks == null)
            {
                return null;
            }
            existingTasks.Title = updatetasksDto.Title;
            existingTasks.Description = updatetasksDto.Description;
            existingTasks.AssignedToEmployeeId = updatetasksDto.AssignedToEmployeeId;

            await _context.SaveChangesAsync();
            return existingTasks;
        }

        public async Task<Tasks?> DeleteAsync(int id)
        {
            var tasksModel = await _context.Taskss.FirstOrDefaultAsync(x => x.Id == id);
            if (tasksModel == null)
            {
                return null;
            }
            _context.Taskss.Remove(tasksModel);
            await _context.SaveChangesAsync();
            return tasksModel;
        }

       
    }
}
