using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService(IDataContext dataAccess) : IUserService
{
    private readonly IDataContext _dataAccess = dataAccess;

    public IEnumerable<User> FilterByActive(bool isActive)
    {
        var users = GetAll();
        return users.Where(x => x.IsActive == isActive);
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public void CreateUser(User user)
    {
        _dataAccess.Create(user);
    }

    public void UpdateUser(User user)
    {
        _dataAccess.Update(user);
    }

    public void DeleteUser(long id)
    {
        var userToDelete = GetAll().FirstOrDefault(x => x.Id == id);
        if (userToDelete != null)
        {
            _dataAccess.Delete(userToDelete);
        }
    }
}
