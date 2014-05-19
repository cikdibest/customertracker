using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using CustomerTracker.Web.Models.Entities;

namespace CustomerTracker.Web.Infrastructure.Repository
{
    public interface IRepositoryType : IDisposable
    {
        IQueryable SelectAll();

        BaseEntity Find(params object[] keys);

        BaseEntity Create(BaseEntity t);

        int Delete(BaseEntity t);

        int Update(BaseEntity t);

        void CreateOrUpdate(BaseEntity t);

        int Delete(int id);
    }

    public class RepositoryType : IRepositoryType
    {
        private readonly Type _type;

        protected DbContext Context = null;

        protected DbSet DbSet
        {
            get
            {
                return Context.Set(_type);
            }
        }
         
        public RepositoryType(DbContext context, Type type)
        {
            Context = context;

            _type = type;
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }

        public virtual IQueryable SelectAll()
        {
            return DbSet.AsQueryable();
        }

        public virtual BaseEntity Find(params object[] keys)
        {
            return (BaseEntity)DbSet.Find(keys);
        }

        public virtual BaseEntity Create(BaseEntity TObject)
        {
            var newEntry = DbSet.Add(TObject);

            return (BaseEntity)newEntry;
        }

        public void CreateOrUpdate(BaseEntity model)
        {
            if ((model as BaseEntity).Id == 0)
            {
                Create(model);
            }
            else
            {
                Update(model);
            }

        }

        public virtual int Delete(BaseEntity TObject)
        {
            DbSet.Remove(TObject);

            return 0;
        }

        public virtual int Delete(int id)
        {
            var entity = Find(id);

            return Delete(entity);
        }

        public virtual int Update(BaseEntity TObject)
        {
            var entry = Context.Entry(TObject);

            if (entry.State == EntityState.Detached)
            {
                var attachedEntity = DbSet.Find(entry.Entity.Id);

                if (attachedEntity != null)
                {
                    var attachedEntry = Context.Entry(attachedEntity);

                    attachedEntry.CurrentValues.SetValues(TObject);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }

            return 0;
        }

    }
}