using companyappbasic.Data.Context;
using companyappbasic.Data.Entity;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace companyappbasic.Services.BackgroundServices
{
    public class BackgroundServi
    {
        private readonly ApplicationDBContext _context;

        public BackgroundServi(ApplicationDBContext context)
        {
            _context = context;
        }

        public void CheckAndUpdateRecords()
        {
            var loginLogs = _context.LoginLogss.ToList();

            foreach (var loginLog in loginLogs)
            {
                var backgroundJobRecord = _context.RecordOfBackgroungJobss
                    .FirstOrDefault(r => r.UserId == loginLog.UserId);

                if (backgroundJobRecord == null)
                {
                    _context.RecordOfBackgroungJobss.Add(new RecordOfBackgroungJobs
                    {
                        UserId = loginLog.UserId,
                        UserName = loginLog.UserName,
                        Email = loginLog.Email,
                        LoginTime = loginLog.LoginTime,
                        NumberOfBackjob = 0 
                    });
                }
                else if (backgroundJobRecord.NumberOfBackjob != loginLog.NumberOfLogin)
                {
                    backgroundJobRecord.NumberOfBackjob = loginLog.NumberOfLogin;
                }
            }

            // Değişiklikleri veritabanına kaydet
            _context.SaveChanges();
        }
        public async Task AddLoginLogAsync(string userName, string email, string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                var loginLog = await _context.LoginLogss.SingleOrDefaultAsync(l => l.UserId == userId);

                if (loginLog != null)
                {

                    loginLog.NumberOfLogin++;
                    loginLog.LoginTime = DateTime.UtcNow;
                }
                else
                {
                    loginLog = new LoginLog
                    {
                        UserId = userId,
                        UserName = userName,
                        Email = email,
                        NumberOfLogin = 1,
                        LoginTime = DateTime.UtcNow
                    };
                    _context.LoginLogss.Add(loginLog);
                }

                await _context.SaveChangesAsync();

            }

        }
    }
}




