using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServiceApp.API.DAL.Interfaces;
using ServiceApp.API.Models;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.DAL
{
    public class TicketRepository: ITicketRepository
    {
        internal DataContext Context;
        internal DbSet<Ticket> DbSet;

        public TicketRepository(DataContext context)
        {
            Context = context;
            DbSet = context.Set<Ticket>();
        }
        public virtual IEnumerable<Ticket> GetAllTickets(
            Expression<Func<Ticket, bool>>? filter = null,
            Func<IQueryable<Ticket>, IOrderedQueryable<Ticket>>? orderBy = null,
            Expression<Func<Ticket, object>>[]? includes = null)
        {
            IQueryable<Ticket> query = DbSet;

            if (includes is not null)
            {
                foreach (Expression<Func<Ticket, object>> include in includes)
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

        public Ticket GetTicket(
            Expression<Func<Ticket, bool>>? filter,
            Expression<Func<Ticket, object>>[]? includes = null)
        {
            IQueryable<Ticket> query = DbSet;

            if (includes is not null)
            {
                foreach (Expression<Func<Ticket, object>> include in includes)
                    query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.FirstOrDefault();
        }

        public virtual void Insert(Ticket entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            Ticket entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(Ticket entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }


        public virtual void Update(Ticket entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

    }
}

