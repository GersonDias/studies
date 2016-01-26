using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFSample.Infra;
using EFSample.Model;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace EFSample
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Se_a_base_de_dados_nao_existir_ela_deve_ser_criada()
        {
            using (var context = new TFSUsersDbContext())
            {
                context.Database.Delete();

                Assert.IsFalse(context.Database.Exists());

                context.Database.CreateIfNotExists();

                Assert.IsTrue(context.Database.Exists());
            } 
        }

        [TestMethod]
        public void Devo_Conseguir_Fazer_Um_Select_Na_TblCommand()
        {
            using (var context = new TblCommandDbContext("Data Source=.;Initial Catalog=TFS_DefaultCollection;Integrated Security=SSPI;"))
            {
                var sql = new StringBuilder()
                .AppendLine("select UserName, UserAgent, convert(date, StartTime) StartTime, count(UserName) Quantity")
                .AppendLine("from tbl_Command")
                .AppendLine("group by UserName, UserAgent, convert(date, StartTime)").ToString();

                var lista = context.Database.SqlQuery<tblCommand>(sql).ToList();

                Assert.IsNotNull(lista);
                Assert.IsTrue(lista.Count() > 0);
            }
        }

        [TestMethod]
        public void Devo_Conseguir_Inserir_Registros()
        {
            List<tblCommand> tblCommand;

            using (var context = new TblCommandDbContext("Data Source=.;Initial Catalog=TFS_DefaultCollection;Integrated Security=SSPI"))
            {
                var sql = new StringBuilder()
                .AppendLine("select UserName, UserAgent, convert(date, StartTime) StartTime, count(UserName) Quantity")
                .AppendLine("from tbl_Command")
                .AppendLine("group by UserName, UserAgent, convert(date, StartTime)").ToString();

                tblCommand = context.Database.SqlQuery<tblCommand>(sql).ToList();

                Assert.IsNotNull(tblCommand);
                Assert.IsTrue(tblCommand.Count() > 0);
            }

            using (var context = new TFSUsersDbContext())
            {
                context.Database.CreateIfNotExists();

                tblCommand.ForEach(x => context.TfsUsers.Add(new TFSUsers(x.UserName, x.UserAgent, x.StartTime, x.Quantity)));

                context.SaveChanges();

                Assert.IsTrue(context.TfsUsers.Count() > 0);
            }
        
        }

    }
}
