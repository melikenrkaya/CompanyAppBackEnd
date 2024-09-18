using companyappbasic.Data.Entity;
using companyappbasic.Data.Models;

namespace companyappbasic.Common.Extensions
{
    public static class AdminExten
    {
        public static AdminDto ToAdminDto(this Admin AdminModel)
        {
            return new AdminDto
            {
                Id = AdminModel.Id,
                UserName = AdminModel.UserName,
                Password = AdminModel.Password,
                Role = AdminModel.Role,
            };
        }
        public static Data.Entity.Admin ToAdminFromCreatedDTO(this CreateAdminRequestDto createadminDto, int Employees)
        {
            return new Admin
            {
                UserName = createadminDto.UserName,
                Password = createadminDto.Password,
                Role = createadminDto.Role,
            };
        }
    }
}
