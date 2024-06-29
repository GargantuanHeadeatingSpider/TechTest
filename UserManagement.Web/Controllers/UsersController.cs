using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;


using System;

namespace UserManagement.WebMS.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogService _logService;

        public UsersController(IUserService userService, ILogService logService)
        {
            _userService = userService;
            _logService = logService;
        }

        [HttpGet]
        public IActionResult List(bool filterActive, bool activeStatus)
        {
            var items = (!filterActive) ? GetAll() : FilterByActive(activeStatus);
            var model = new UserListViewModel
            {
                Items = items.ToList()
            };
            return View(model);
        }

        public IEnumerable<UserListItemViewModel> GetAll()
        {
            return _userService.GetAll().Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth
            });
        }

        public IEnumerable<UserListItemViewModel> FilterByActive(bool isActive)
        {
            return _userService.FilterByActive(isActive).Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth
            });
        }

        [HttpGet("details/{id}")]
        public IActionResult UserDetails(long id)
        {
            var user = _userService.GetAll().FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var model = MakeUserListItemViewModel(user);
            var logs = _logService.GetLogsByUserId(id);
            ViewData["Logs"] = logs;
            return View(model);
        }

        [HttpGet("edit/{id}")]
        public IActionResult EditUser(long id)
        {
            var user = _userService.GetAll().FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var model = MakeUserListItemViewModel(user);
            return View(model);
        }

        [HttpPost("edit/{id}")]
        public IActionResult EditUser(UserListItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetAll().FirstOrDefault(x => x.Id == model.Id);

                if (user == null)
                {
                    return NotFound();
                }

                UpdateUser(user, model);
                _logService.CreateLog(new LogViewModel
                {
                    UserId = model.Id,
                    Title = "User Edited",
                    LogType = "Edit",
                    Description = $"User {model.Forename} {model.Surname} was edited.",
                    DateCreated = DateTime.UtcNow
                });

                return RedirectToAction("List");
            }

            return View(model);
        }

        public UserListItemViewModel MakeUserListItemViewModel(User user)
        {
            return new UserListItemViewModel
            {
                Id = user.Id,
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email,
                IsActive = user.IsActive,
                DateOfBirth = user.DateOfBirth
            };
        }

        public void UpdateUser(User user, UserListItemViewModel model)
        {
            user.Forename = model.Forename ?? user.Forename;
            user.Surname = model.Surname ?? user.Surname;
            user.Email = model.Email ?? user.Email;
            user.IsActive = model.IsActive;
            user.DateOfBirth = model.DateOfBirth;

            _userService.UpdateUser(user);
        }

        [HttpGet("add")]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost("add")]
        public IActionResult AddUser(UserListItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Forename = model.Forename ?? string.Empty,
                    Surname = model.Surname ?? string.Empty,
                    Email = model.Email ?? string.Empty,
                    IsActive = model.IsActive,
                    DateOfBirth = model.DateOfBirth
                };

                _userService.CreateUser(user);
                _logService.CreateLog(new LogViewModel
                {
                    UserId = user.Id,
                    Title = "User Created",
                    LogType = "Create",
                    Description = $"User {user.Forename} {user.Surname} was created.",
                    DateCreated = DateTime.UtcNow
                });

                return RedirectToAction("List");
            }

            return View(model);
        }

        [HttpPost("delete")]
        public IActionResult Delete(long id)
        {
            _userService.DeleteUser(id);
            _logService.CreateLog(new LogViewModel
            {
                UserId = id,
                Title = "User Deleted",
                LogType = "Delete",
                Description = $"User with ID {id} was deleted.",
                DateCreated = DateTime.UtcNow
            });

            return RedirectToAction("List");
        }
    }
}
