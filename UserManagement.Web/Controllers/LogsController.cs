using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.WebMS.Controllers
{
    [Route("logs")]
    public class LogsController : Controller
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var logs = _logService.GetAllLogs();
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
}
