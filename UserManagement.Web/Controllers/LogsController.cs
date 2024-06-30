using UserManagement.Services.Domain.Interfaces;
using UserManagement.Models;
using System.Linq;
using System;

namespace UserManagement.WebMS.Controllers;

[Route("logs")]
public class LogsController(ILogService logService) : Controller
{
    private readonly ILogService _logService = logService;

    [HttpGet]
    public IActionResult Index(string logType, DateTime? startDate, DateTime? endDate)
    {
        IEnumerable<LogViewModel> logs = _logService.GetAllLogs();

        if (!string.IsNullOrEmpty(logType))
        {
            logs = logs.Where(log => log.LogType == logType);
        }

        if (startDate.HasValue)
        {
            if (endDate.HasValue)
            {
                logs = logs.Where(log => log.DateCreated >= startDate.Value && log.DateCreated <= endDate.Value);
            }
            else
            {
                logs = logs.Where(log => log.DateCreated.Date == startDate.Value.Date);
            }
        }
        ViewData["StartDate"] = startDate;
        ViewData["EndDate"] = endDate;

        return View(logs);
    }

    [HttpGet("details/{id}")]
    public IActionResult Details(long id)
    {
        var log = _logService.GetLogById(id);
        if (log == null)
        {
            return NotFound();
        }

        return View(log);
    }
}
