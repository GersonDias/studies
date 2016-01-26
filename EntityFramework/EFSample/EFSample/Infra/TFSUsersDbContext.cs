using EFSample.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSample.Infra
{
    public class TFSUsersDbContext : DbContext
    {
        public DbSet<TFSUsers> TfsUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            Database.SetInitializer<TFSUsersDbContext>(new CreateDatabaseIfNotExists<TFSUsersDbContext>());
        }

    }
}
