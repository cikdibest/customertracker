using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using CustomerTracker.Data;
using CustomerTracker.Data.Model.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Infrastructure.Repository
{
     
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryGeneric<TSet> GetRepository<TSet>() where TSet : BaseEntity;
         
        DbTransaction BeginTransaction();

        int Save();

        CustomerTrackerDataContext GetCurrentDataContext();
         
    }

    public class UnitOfWork : IUnitOfWork
    {
        private DbTransaction _transaction;

        private readonly Dictionary<Type, object> _repositoriesGeneric;

        private readonly Dictionary<Type, object> _repositoriesType;

        private readonly CustomerTrackerDataContext _ctx;

        //public UnitOfWork(JewelryDataContext ctx)
        //{
        //    _ctx = ctx;

        //    _repositoriesGeneric = new Dictionary<Type, object>();

        //    _repositoriesType = new Dictionary<Type, object>();
        //}

        public UnitOfWork()
        {
            _ctx = new CustomerTrackerDataContext();

            _repositoriesGeneric = new Dictionary<Type, object>();

            _repositoriesType = new Dictionary<Type, object>();
        }

        public IRepositoryGeneric<TSet> GetRepository<TSet>() where TSet : BaseEntity
        {
            if (_repositoriesGeneric.Keys.Contains(typeof(TSet)))
                return _repositoriesGeneric[typeof(TSet)] as IRepositoryGeneric<TSet>;

            var repository = new RepositoryGeneric<TSet>(_ctx);

            _repositoriesGeneric.Add(typeof(TSet), repository);

            return repository;
        }
         
        public DbTransaction BeginTransaction()
        {
            if (_transaction == null)
            {
                if (_ctx.Database.Connection.State != ConnectionState.Open)
                {
                    //_ctx.Database.Connection.Open();
                    ((IObjectContextAdapter)_ctx).ObjectContext.Connection.Open();
                }

                this._transaction = _ctx.Database.Connection.BeginTransaction();
            }

            return _transaction;
        }

        public int Save()
        {

            var addedEntries = this.GetCurrentDataContext().ChangeTracker.Entries().Where(q => q.State == EntityState.Added);

            foreach (var baseEntity in addedEntries.Select(entry => (BaseEntity)entry.Entity))
            {
                baseEntity.CreationDate = DateTime.Now;

                baseEntity.CreationPersonelId = ConfigurationHelper.CurrentUser != null
                                                    ? ConfigurationHelper.CurrentUser.UserId
                                                    : (int?)null;
            }

            var modifiedEntries = this.GetCurrentDataContext().ChangeTracker.Entries().Where(q => q.State == EntityState.Modified);

            foreach (var baseEntity in modifiedEntries.Select(entry => (BaseEntity)entry.Entity))
            {
                baseEntity.UpdatedDate = DateTime.Now;

                baseEntity.UpdatedPersonelId = ConfigurationHelper.CurrentUser != null
                                                   ? ConfigurationHelper.CurrentUser.UserId
                                                   : (int?)null;
            }

            var saveChanges = _ctx.SaveChanges();

            if (saveChanges>0)
            {
                if (_transaction != null)
                    _transaction.Commit();    
            }

            return saveChanges;


        }

        public CustomerTrackerDataContext GetCurrentDataContext()
        {
            return _ctx;
        }
         
        #region IDisposable Members

        public void Dispose()
        {
            if (null != _transaction)
            {
                _transaction.Dispose();
            }

            if (null != _ctx)
            {
                _ctx.Dispose();
            }

        }

        #endregion
    }

    
}