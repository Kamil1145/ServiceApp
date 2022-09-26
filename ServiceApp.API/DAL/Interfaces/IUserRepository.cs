using ServiceApp.Models.Entities;
using System.Linq.Expressions;

namespace ServiceApp.API.DAL.Interfaces;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers(
        Expression<Func<User, bool>>? filter = null,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
        Expression<Func<User, object>>[]? includes = null);

    User GetUser(
        Expression<Func<User, bool>>? filter,
        Expression<Func<User, object>>[]? includes = null);

    void Insert(User entity);
    void Update(User entity);
    void Delete(object entity);
}
