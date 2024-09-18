using companyappbasic.Data.Entity;
using System.Security.Claims;

namespace companyappbasic.Services.AppUserServices
{
    public interface IToken
    {
        string CreateToken(AppUser user);
    }
}
