
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class LogService(IDataContext dataAccess) : ILogService
{
    private readonly IDataContext _dataAccess = dataAccess;

    public IEnumerable<LogViewModel> GetAllLogs()
    {
        return _dataAccess.GetAll<Log>().Select(log => MakeLogViewModel(log));
    }

    public IEnumerable<LogViewModel> GetLogsByUserId(long userId)
    {
        return _dataAccess.GetAll<Log>().Where(log => log.UserId == userId).Select(log => MakeLogViewModel(log));
    }

    public LogViewModel? GetLogById(long id)
    {
        var log = _dataAccess.GetAll<Log>().FirstOrDefault(log => log.Id == id);
        if (log == null)
            return null;
        return MakeLogViewModel(log);
    }

   public static LogViewModel MakeLogViewModel(Log log) =>
            new()
            {
                Id = log.Id,
                UserId = log.UserId,
                Title = log.Title,
                LogType = log.LogType,
                Description = log.Description,
                DateCreated = log.DateCreated
            };

    public void CreateLog(LogViewModel log)
    {
        var logEntry = new Log
        {
            UserId = log.UserId,
            Title = log.Title,
            LogType = log.LogType,
            Description = log.Description,
            DateCreated = log.DateCreated
        };
        _dataAccess.Create(logEntry);
    }
}
