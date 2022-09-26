using ServiceApp.Models.Entities;
using System.Linq.Expressions;

namespace ServiceApp.API.DAL.Interfaces;

public interface ICommentRepository
{
    IEnumerable<Comment> GetAll(
        Expression<Func<Comment, bool>>? filter = null,
        Func<IQueryable<Comment>, IOrderedQueryable<Comment>>? orderBy = null,
        Expression<Func<Comment, object>>[]? includes = null);

    void Insert(Comment entity);

    void Delete(object entity);
}
