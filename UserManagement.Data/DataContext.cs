using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
        [
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = new DateTime(1998,04,30)},
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth = new DateTime(1992,05,13)},
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false, DateOfBirth = new DateTime(1974,11,12)},
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true, DateOfBirth = new DateTime(1965,01,01)},
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true, DateOfBirth = new DateTime(1998,05,13)},
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true, DateOfBirth = new DateTime(1989,03,23)},
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false, DateOfBirth = new DateTime(1992,05,13)},
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false, DateOfBirth =  new DateTime(1901,01,01)},
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false, DateOfBirth =  new DateTime(1991,04,19)},
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true, DateOfBirth =  new DateTime(1998,11,30)},
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true, DateOfBirth =  new DateTime(1986,07,08)},
        ]);

        modelBuilder.Entity<Log>().HasData(
        [
            new Log { Id = 1, UserId = 1, Title = "User Name Updated", LogType = "Update", Description = "User name changed from Peter Pan to Peter Loew", DateCreated = DateTime.UtcNow.AddDays(-30) },
            new Log { Id = 2, UserId = 2, Title = "User Email Updated", LogType = "Update", Description = "User email changed from ben@example.com to bfgates@example.com", DateCreated = DateTime.UtcNow.AddDays(-25) },
            new Log { Id = 3, UserId = 3, Title = "User Date of Birth Updated", LogType = "Update", Description = "User date of birth changed from 1974-11-10 to 1974-11-12", DateCreated = DateTime.UtcNow.AddDays(-20) },
            new Log { Id = 4, UserId = 4, Title = "User Name Updated", LogType = "Update", Description = "User name changed from Memphis Monroe to Memphis Raines", DateCreated = DateTime.UtcNow.AddDays(-15) },
            new Log { Id = 5, UserId = 5, Title = "User Email Updated", LogType = "Update", Description = "User email changed from stantheman@example.com to sgodspeed@example.com", DateCreated = DateTime.UtcNow.AddDays(-10) },
            new Log { Id = 6, UserId = 6, Title = "User Date of Birth Updated", LogType = "Update", Description = "User date of birth changed from 1989-03-20 to 1989-03-23", DateCreated = DateTime.UtcNow.AddDays(-5) },
            new Log { Id = 7, UserId = 7, Title = "User Name Updated", LogType = "Update", Description = "User name changed from Edgar Allan Poe to Cameron Poe", DateCreated = DateTime.UtcNow.AddDays(-2) },
            new Log { Id = 8, UserId = 8, Title = "User Email Updated", LogType = "Update", Description = "User email changed from eddyboii@example.com to emalus@example.com", DateCreated = DateTime.UtcNow.AddDays(-1) },
            new Log { Id = 9, UserId = 9, Title = "User Date of Birth Updated", LogType = "Update", Description = "User date of birth changed from 1901-04-18 to 1991-04-19", DateCreated = DateTime.UtcNow },
            new Log { Id = 10, UserId = 10, Title = "User Name Updated", LogType = "Update", Description = "User name changed from Johnny Joestar to Johnny Blaze", DateCreated = DateTime.UtcNow.AddDays(-48) },
            new Log { Id = 11, UserId = 123456789, Title = "User Deleted", LogType = "Delete", Description = "User with ID 123456789 was deleted.", DateCreated = DateTime.UtcNow.AddDays(-45) }
        ]);

    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Log>? Logs { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public void Create<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        SaveChanges();
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        SaveChanges();
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        SaveChanges();
    }
}
