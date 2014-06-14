using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using CustomerTracker.Data;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Migrations;

namespace CustomerTracker.Web.App_Start
{
    public class DatabaseConfiguration
    {
        public static void StartMigration()
        {
            var configuration = new Configuration
            {
                AutomaticMigrationsEnabled = true,
                ContextType = typeof(CustomerTrackerDataContext),
                AutomaticMigrationDataLossAllowed = true,
                TargetDatabase = new DbConnectionInfo("CustomerTrackerDataContext"),//("Data Source=|DataDirectory|myblogdb.sdf", "System.Data.SqlServerCe.4.0"),

            };

            var migratorScriptingDecorator = new MigratorScriptingDecorator(new DbMigrator(configuration));

            IEnumerable<string> pendingMigrations = migratorScriptingDecorator.GetPendingMigrations();

            var dbMigrator = new DbMigrator(configuration);

            foreach (var pendingMigration in pendingMigrations)
            {
                dbMigrator.Update(pendingMigration);
            }

            //var migratorScriptingDecorator = new MigratorScriptingDecorator(dbMigrator);

            //IEnumerable<string> databaseMigrations = migratorScriptingDecorator.GetDatabaseMigrations();

            //IEnumerable<string> localMigrations = migratorScriptingDecorator.GetLocalMigrations();

            //IEnumerable<string> pendingMigrations = migratorScriptingDecorator.GetPendingMigrations();

            //var script = migratorScriptingDecorator.ScriptUpdate(sourceMigration: null, targetMigration:null);
        }
    }
}