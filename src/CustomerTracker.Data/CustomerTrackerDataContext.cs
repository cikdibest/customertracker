using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using CustomerTracker.Data.Model.Entities;

namespace CustomerTracker.Data
{
    public class CustomerTrackerDataContext : DbContext
    {
        public CustomerTrackerDataContext()
            : base("CustomerTrackerDataContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        //public JewelryDataContext(string connectionString)
        //    : base(new SqlCeConnection(connectionString), true)
        //{
        //    this.Configuration.LazyLoadingEnabled = false;
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Communication> Communications { get; set; }
        public DbSet<RemoteMachine> RemoteMachines { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Department> Titles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SocialAccount> SocialAccounts { get; set; }
        public DbSet<DataMaster> DataMasters { get; set; }
        public DbSet<DataDetail> DataDetails { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Trouble> Troubles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);

        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException != null && dbUpdateException.InnerException.InnerException != null && dbUpdateException.InnerException.InnerException is SqlException)
                {
                    var sqlException = dbUpdateException.InnerException.InnerException as SqlException;
                    if (sqlException.Number == 547)
                    {
                        return -547;
                    }
                }


                throw;
            }
        }

    }
}


