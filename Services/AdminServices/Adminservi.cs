using companyappbasic.Data.Context;
using Microsoft.EntityFrameworkCore;
using companyappbasic.Data.Entity;
using companyappbasic.Data.Models;

namespace companyappbasic.Services.AdminServices
{
    public class Adminservi: IAdmin 
    {
    private readonly ApplicationDBContext _context;

    public Adminservi(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<List<Admin>> GetAllAsync()
    {
        return await _context.Admins.ToListAsync();
    }
    public async Task<Admin?> GetByIdAsync(int id)
    {
        return await _context.Admins.FirstOrDefaultAsync(e => e.Id == id);
    }
    public async Task<Admin?> CreateAsync(Admin AdminModel)
    {
        await _context.Admins.AddAsync(AdminModel);
        await _context.SaveChangesAsync();
        return AdminModel;
    }

    public async Task<Admin?> UpdateAsync(int id, UpdateAdminRequestDto AdminDto)
    {
        var existingAdmin = await _context.Admins.FindAsync(id);
        if (existingAdmin == null)
        {
            return null;
        }
        existingAdmin.UserName = AdminDto.UserName;
        existingAdmin.Password = AdminDto.Password;
        existingAdmin.Role = AdminDto.Role;
        await _context.SaveChangesAsync();
        return existingAdmin;
    }

    public async Task<Admin?> DeleteAsync(int id)
    {
        var adminModel = await _context.Admins.FirstOrDefaultAsync(x => x.Id == id);
        if (adminModel == null)
        {
            return null;
        }
        _context.Admins.Remove(adminModel);
        await _context.SaveChangesAsync();
        return adminModel;
    }

       
    }
}


