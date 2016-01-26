using EFSample.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSample.Infra
{
    public class TblCommandDbContext : DbContext
    {
        public TblCommandDbContext(string connectionString) : base(connectionString) { }

        public DbQuery<tblCommand> Commands
        {
            get
            { 
                return Set<tblCommand>().AsNoTracking();
            }
        }

        public override int SaveChanges()
        {
            throw new InvalidOperationException("This context is read only");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<tblCommand>().ToTable("tbl_Command");

            Database.SetInitializer<TblCommandDbContext>(null);
        }

    }
}
