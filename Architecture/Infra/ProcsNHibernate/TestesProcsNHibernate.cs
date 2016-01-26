using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dominio;
using Infra.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositorio;

namespace ProcsNHibernate
{
    [TestClass]
    public class TestesProcsNHibernate
    {
        private int numeroDeRegistros = 1000;

        [ClassInitialize]
        public static void Inicializacoes(TestContext testContext)
        {
            log4net.Config.XmlConfigurator.Configure();
            LimparBanco();
        }

        [ClassCleanup]
        public static void Finalizacao()
        {
            //LimparBanco();
        }

        [TestInitialize]
        public void Setup()
        {
        }

        private static void LimparBanco()
        {
            using (SessionManager.OpenSession(false))
            {
                SessionManager.CurrentSession.CreateSQLQuery("truncate table pessoa").ExecuteUpdate();
            }
        }

        [TestMethod]
        public void NHibernateInsereAtravezDeProcs()
        {
            var listaDePessoas = new List<Pessoa>(numeroDeRegistros);

            for (int i = 0; i < numeroDeRegistros; i++)
            {
                listaDePessoas.Add(new Pessoa(){Nome="teste" + i, Idade = i});
            }
            
            var watch = new Stopwatch();
            var repositorio = new Repositorio<Pessoa>();

            using (SessionManager.OpenSession())
            {
                watch.Start();
                listaDePessoas.ForEach(repositorio.Salvar);
                SessionManager.CommitTransaction();
                watch.Stop();
                Console.WriteLine("Tempo de execução para a inserção de registros: " + watch.ElapsedMilliseconds + "ms");
            }

            listaDePessoas.ForEach(x => Assert.IsTrue(x.Id > 0));

            //listaDePessoas.ForEach(x => Console.WriteLine(string.Format("Id: {0} - Nome: {1} - Idade {2}", x.Id, x.Nome, x.Idade)));
        }


        [TestMethod]
        public void nHibernate_Pode_Fazer_O_Select_De_Um_Registro()
        {
            IEnumerable<Pessoa> listaDePessoas;
            var watch = new Stopwatch();
            using (SessionManager.OpenSession(true))
            {
                //var repositorio = new Repositorio<Pessoa>();

                //watch.Start();
                //listaDePessoas = repositorio.PesquisarProc("oBaileTodo");
                //watch.Stop();

                //Console.WriteLine("Tempo de execução para retornar registros via procs: " + watch.ElapsedMilliseconds + "ms. Quantidade de Registros: " + listaDePessoas.Count());

                watch = new Stopwatch();
                var repositorio = new Repositorio<Pessoa>();
                watch.Start();
                listaDePessoas = null;
                listaDePessoas = repositorio.Pesquisar();
                watch.Stop();

                Console.WriteLine("Tempo de execução para retornar registros via select: " + watch.ElapsedMilliseconds + "ms. Quantidade de Registros: " + listaDePessoas.Count());
            }

            Assert.IsTrue(listaDePessoas.Any());
            //listaDePessoas.ToList().ForEach(pessoa => Console.WriteLine(string.Format("{0}-{1}anos", pessoa.Nome, pessoa.Idade)));
        }
    }
}
