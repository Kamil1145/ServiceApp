using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServiceApp.API.DAL.Interfaces;
using ServiceApp.API.Models;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.DAL
{
    public class UserRepository : IUserRepository
    {
        internal DataContext Context;
        internal DbSet<User> DbSet;

        public UserRepository(DataContext context)
        {
            Context = context;
            DbSet = context.Set<User>();
        }

        public virtual IEnumerable<User> GetAllUsers(
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            Expression<Func<User, object>>[]? includes = null)
        {
            IQueryable<User> query = DbSet;

            if (includes is not null)
            {
                foreach (Expression<Func<User, object>> include in includes)
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

        public User GetUser(
            Expression<Func<User, bool>>? filter,
            Expression<Func<User, object>>[]? includes = null)
        {
            IQueryable<User> query = DbSet;

            if (includes is not null)
            {
                foreach (Expression<Func<User, object>> include in includes)
                    query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.FirstOrDefault();
        }

        public virtual void Insert(User entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            User entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(User entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }


        public virtual void Update(User entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

    }
}

