using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServiceApp.API.DAL.Interfaces;
using ServiceApp.API.Models;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.DAL
{
    public class CommentRepository : ICommentRepository
    {
        internal DataContext Context;
        internal DbSet<Comment> DbSet;

        public CommentRepository(DataContext context)
        {
            Context = context;
            DbSet = context.Set<Comment>();
        }
        public virtual IEnumerable<Comment> GetAll(
            Expression<Func<Comment, bool>>? filter = null,
            Func<IQueryable<Comment>, IOrderedQueryable<Comment>>? orderBy = null,
            Expression<Func<Comment, object>>[]? includes = null)
        {
            IQueryable<Comment> query = DbSet;

            if (includes is not null)
            {
                foreach (Expression<Func<Comment, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return orderBy(query).ToList();


            return query.ToList();
        }

        public Comment GetComment(
            Expression<Func<Comment, bool>>? filter,
            Expression<Func<Comment, object>>[]? includes = null)
        {
            IQueryable<Comment> query = DbSet;

            if (includes is not null)
            {
                foreach (Expression<Func<Comment, object>> include in includes)
                    query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.FirstOrDefault();
        }

        public virtual void Insert(Comment entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            Comment entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(Comment entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(Comment entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

    }
}

