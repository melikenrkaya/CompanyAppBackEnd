using companyappbasic.Data.Entity;
using companyappbasic.Data.Models;

namespace companyappbasic.Services.AdminServices
{
    public interface IAdmin
    {
        Task<List<Admin>> GetAllAsync();
        Task<Admin?> GetByIdAsync(int id);
        Task<Admin?> CreateAsync(Admin AdminModel);
        Task<Admin?> UpdateAsync(int id, UpdateAdminRequestDto AdminDto);
        Task<Admin?> DeleteAsync(int id);
    }
}
