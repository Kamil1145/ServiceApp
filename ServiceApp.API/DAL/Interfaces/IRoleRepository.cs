using ServiceApp.Models.Entities;
using System.Linq.Expressions;

namespace ServiceApp.API.DAL.Interfaces
{
    public interface IRoleRepository 
    {
        IEnumerable<Role> GetAllRoles(
            Expression<Func<Role, bool>>? filter = null,
            Func<IQueryable<Role>, IOrderedQueryable<Role>>? orderBy = null,
            Expression<Func<Role, object>>[]? includes = null);

        void Insert(Role entity);

        void Delete(object entity);
    }
}