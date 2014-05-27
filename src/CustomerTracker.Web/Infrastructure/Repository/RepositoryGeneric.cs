using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using CustomerTracker.Web.Models.Entities;

namespace CustomerTracker.Web.Infrastructure.Repository
{
    public interface IRepositoryGeneric<T> : IDisposable where T : BaseEntity
    {
        IQueryable<T> SelectAll();

        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

        IQueryable<T> Filter<TKey>(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);

        bool Contains(Expression<Func<T, bool>> predicate);

        T Find(params object[] keys);

        T Find(Expression<Func<T, bool>> wherePredicate);

        T Create(T t);

        int Delete(T t);

        int Delete(Expression<Func<T, bool>> predicate);

        int Update(T t);

        int Count { get; }

        void CreateOrUpdate(T t);

        int Delete(int id);
    }

    public class RepositoryGeneric<TObject> : IRepositoryGeneric<TObject> where TObject : BaseEntity
    {
        private readonly DbContext _context = null;

        private DbSet<TObject> DbSet
        {
            get
            {
                return _context.Set<TObject>();
            }
        }

        public RepositoryGeneric(DbContext context)
        {
            _context = context;

        }

        public virtual IQueryable<TObject> SelectAll()
        {
            return DbSet.AsQueryable().Where(q => !q.IsDeleted);
        }
         
        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return SelectAll().Where(predicate).AsQueryable<TObject>();
        }

        public virtual IQueryable<TObject> Filter<TKey>(Expression<Func<TObject, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var resetSet = filter != null ? SelectAll().Where(filter).AsQueryable() :
                SelectAll().AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) :
                resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        public virtual TObject Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual TObject Find(Expression<Func<TObject, bool>> wherePredicate)
        { 
            return SelectAll().SingleOrDefault(wherePredicate);
        }

        public virtual TObject Create(TObject TObject)
        {
            var newEntry = DbSet.Add(TObject);

            return newEntry;
        }

        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public void CreateOrUpdate(TObject model)
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

        public virtual int Delete(TObject TObject)
        {
            DbSet.Remove(TObject);

            return 0;
        }

        public virtual int Delete(int id)
        {
            var entity = Find(id);

            return Delete(entity);
        }

        public virtual int Update(TObject TObject)
        {
            var entry = _context.Entry(TObject);

            if (entry.State == EntityState.Detached)
            {
                var attachedEntity = DbSet.Find(entry.Entity.Id);

                if (attachedEntity != null)
                {
                    var attachedEntry = _context.Entry(attachedEntity);

                    attachedEntry.CurrentValues.SetValues(TObject);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }

            return 0;
        }

        public virtual int Delete(Expression<Func<TObject, bool>> predicate)
        {
            var objects = Filter(predicate);

            foreach (var obj in objects)
                DbSet.Remove(obj);

            return 0;
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
    }

}