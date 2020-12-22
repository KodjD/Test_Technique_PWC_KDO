using Microsoft.Data.Sqlite;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Test_Technique_PWC.Model;

namespace Test_Technique_PWC.DataContext
{
    public class DataContext_DataTest: DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DataContext_DataTest() :
            base(new SqliteConnection()
            {
                ConnectionString = new SqliteConnectionStringBuilder() { DataSource = "C:\\Users\\user\\source\\repos\\Test_Technique_PWC_KDO\\Test_Technique_PWC_Solution\\Test_Technique_PWC\\Db", ForeignKeys = true }.ConnectionString
            }, true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

        }
    }
}
