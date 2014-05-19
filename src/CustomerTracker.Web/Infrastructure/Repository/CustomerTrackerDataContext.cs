using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Infrastructure.Repository
{
    public class CustomerTrackerDataContext : DbContext
    {
        public CustomerTrackerDataContext()
            : base("CustomerTrackerDataContext")
        {

        }

        //public JewelryDataContext(string connectionString)
        //    : base(new SqlCeConnection(connectionString), true)
        //{
        //    this.Configuration.LazyLoadingEnabled = false;
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Communication> Communications { get; set; }
        public DbSet<RemoteComputer> RemoteComputers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SocialAccount> SocialAccounts { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
 
            base.OnModelCreating(modelBuilder);
             
        }

        public override int SaveChanges()
        {
            var addedEntries = this.ChangeTracker.Entries().Where(q => q.State == EntityState.Added);

            foreach (var baseEntity in addedEntries.Select(entry => (BaseEntity)entry.Entity))
            {
                baseEntity.CreationDate = DateTime.Now;

                baseEntity.CreationPersonelId = ConfigurationHelper.CurrentUser != null
                                                    ? ConfigurationHelper.CurrentUser.UserId
                                                    : (int?)null;
            }

            var modifiedEntries = this.ChangeTracker.Entries().Where(q => q.State == EntityState.Modified);

            foreach (var baseEntity in modifiedEntries.Select(entry => (BaseEntity)entry.Entity))
            {
                baseEntity.UpdatedDate = DateTime.Now;

                baseEntity.UpdatedPersonelId = ConfigurationHelper.CurrentUser != null
                                                   ? ConfigurationHelper.CurrentUser.UserId
                                                   : (int?)null;
            }

            return base.SaveChanges();

        }

    }
}


