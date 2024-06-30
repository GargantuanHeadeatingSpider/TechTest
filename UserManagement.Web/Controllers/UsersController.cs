using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using System;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController(IUserService userService, ILogService logService) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly ILogService _logService = logService;

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

    private IEnumerable<UserListItemViewModel> GetAll()
    {
        return _userService.GetAll().Select(ToUserListItemViewModel);
    }

    private IEnumerable<UserListItemViewModel> FilterByActive(bool isActive)
    {
        return _userService.FilterByActive(isActive).Select(ToUserListItemViewModel);
    }

    [HttpGet("details/{id}")]
    public IActionResult UserDetails(long id)
    {
        var user = _userService.GetAll().FirstOrDefault(x => x.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        var model = ToUserListItemViewModel(user);
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

        var model = ToUserListItemViewModel(user);
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

            var previousForename = user.Forename;
            var previousSurname = user.Surname;
            var previousEmail = user.Email;
            var previousDateOfBirth = user.DateOfBirth;

            UpdateUser(user, model);
            _userService.UpdateUser(user);

            // Log the changes
            if (user.Forename != previousForename || user.Surname != previousSurname)
            {
                _logService.CreateLog(ToLogViewModel(user.Id, "User Name Updated", "Update",
                    $"User name changed from {previousForename} {previousSurname} to {user.Forename} {user.Surname}"));
            }

            if (user.Email != previousEmail)
            {
                _logService.CreateLog(ToLogViewModel(user.Id, "User Email Updated", "Update",
                    $"User email changed from {previousEmail} to {user.Email}"));
            }

            if (user.DateOfBirth != previousDateOfBirth)
            {
                _logService.CreateLog(ToLogViewModel(user.Id, "User Date of Birth Updated", "Update",
                    $"User date of birth changed from {previousDateOfBirth.ToShortDateString()} to {user.DateOfBirth.ToShortDateString()}"));
            }

            return RedirectToAction("List");
        }

        return View(model);
    }

    private UserListItemViewModel ToUserListItemViewModel(User user)
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

    private static LogViewModel ToLogViewModel(long userId, string title, string logType, string description)
    {
        return new LogViewModel
        {
            UserId = userId,
            Title = title,
            LogType = logType,
            Description = description,
            DateCreated = DateTime.UtcNow
        };
    }

    private void UpdateUser(User user, UserListItemViewModel model)
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
            var user = ToUserEntity(model);

            _userService.CreateUser(user);
            _logService.CreateLog(ToLogViewModel(user.Id, "User Created", "Create", $"User {user.Forename} {user.Surname} was created."));

            return RedirectToAction("List");
        }

        return View(model);
    }

    private static User ToUserEntity(UserListItemViewModel model) => new()
    {
        Forename = model.Forename ?? string.Empty,
        Surname = model.Surname ?? string.Empty,
        Email = model.Email ?? string.Empty,
        IsActive = model.IsActive,
        DateOfBirth = model.DateOfBirth
    };

    [HttpPost("delete")]
    public IActionResult Delete(long id)
    {
        _userService.DeleteUser(id);
        _logService.CreateLog(ToLogViewModel(id, "User Deleted", "Delete", $"User with ID {id} was deleted."));

        return RedirectToAction("List");
    }
}
