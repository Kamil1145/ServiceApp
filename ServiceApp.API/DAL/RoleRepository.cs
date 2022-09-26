using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServiceApp.API.DAL.Interfaces;
using ServiceApp.API.Models;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.DAL
{
    public class RoleRepository : IRoleRepository
    {
        internal DataContext Context;
        internal DbSet<Role> DbSet;

        public RoleRepository(DataContext context)
        {
            Context = context;
            DbSet = context.Set<Role>();
        }
        public virtual IEnumerable<Role> GetAllRoles(
            Expression<Func<Role, bool>>? filter = null,
            Func<IQueryable<Role>, IOrderedQueryable<Role>>? orderBy = null,
            Expression<Func<Role, object>>[]? includes = null)
        {
            IQueryable<Role> query = DbSet;

            if (includes is not null)
            {
                foreach (Expression<Func<Role, object>> include in includes)
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


        public virtual void Insert(Role entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            Role entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(Role entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }


        public virtual void Update(Role entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

    }
}

