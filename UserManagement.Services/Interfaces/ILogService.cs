using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{
    IEnumerable<LogViewModel> GetAllLogs();
    IEnumerable<LogViewModel> GetLogsByUserId(long userId);
    LogViewModel? GetLogById(long id);
    void CreateLog(LogViewModel log);
}
