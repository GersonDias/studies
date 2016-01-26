using System;
using System.Configuration;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cache;

namespace Infra.NHibernate
{
    public class SessionManagerInstance : IDisposable
    {
        public ISessionFactory SessionFactory { get; private set; }
        public ISession CurrentSession { get; set; }

        public ISession OpenSession()
        {
            return OpenSession(true);
        }

        public ISession OpenSession(bool openTransaction)
        {
            if (SessionFactory == null)
                SessionFactory = BuildSessionFactory();

            CurrentSession = SessionFactory.OpenSession();

            if (openTransaction)
                CurrentSession.BeginTransaction();

            return CurrentSession;
        }

        public void CloseSessionFactory()
        {
            CloseSessionFactory(SessionFactory);
        }

        public void CloseSessionFactory(ISessionFactory sessionFactory)
        {
            CloseSession();

            if (sessionFactory != null)
            {
                if (!sessionFactory.IsClosed)
                {
                    sessionFactory.Close();
                }

                sessionFactory = null;
            }
        }

        public void CloseSession()
        {
            CloseSession(CurrentSession);
        }

        public void CloseSession(ISession currentSession)
        {
            try
            {
                if (currentSession != null)
                {
                    if (currentSession.IsOpen)
                    {
                        currentSession.Close();
                    }
                }

                currentSession = null;
            }
            catch (Exception)
            {
                if (currentSession != null)
                {
                    if (currentSession.IsOpen && currentSession.Transaction.IsActive)
                    {
                        currentSession.Transaction.Rollback();
                    }
                }

                throw;
            }
        }

        public void CommitChanges()
        {
            CommitTransaction();
            BeginTransaction();
        }

        public void BeginTransaction()
        {
            CurrentSession.Transaction.Begin();
        }

        public void RollbackTransaction()
        {
            if (CurrentSession == null) return;

            if (CurrentSession.Transaction.IsActive)
                CurrentSession.Transaction.Rollback();
        }

        public void RollbackChanges()
        {
            RollbackTransaction();
            BeginTransaction();
        }

        public void CommitTransaction()
        {
            try
            {
                if (CurrentSession != null)
                {
                    if (CurrentSession.IsOpen && CurrentSession.Transaction.IsActive)
                    {
                        CurrentSession.Transaction.Commit();
                    }
                }
            }
            catch (Exception)
            {
                CurrentSession.Transaction.Rollback();
                throw;
            }
        }

        public ISessionFactory BuildSessionFactory()
        {
            var pathAssemblyMappings = ConfigurationManager.AppSettings["PathAssemblyMappings"];

            if (pathAssemblyMappings == null)
                throw new ArgumentException("Path Assembly Mappings não definido no Web/App Config!");

            var assemblyToMap = Assembly.LoadFile(pathAssemblyMappings);

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            var factory = Fluently.Configure()
                                .Cache(c => c.UseQueryCache().ProviderClass<HashtableCacheProvider>())
                                .Database(MsSqlConfiguration.MsSql2008
                                                .ConnectionString(connectionString)
                                                .ShowSql())
                                .Mappings(m => m.FluentMappings
                                                .AddFromAssembly(assemblyToMap)
                                                .Conventions.Add<CustomPrimaryKeyConvention>()
                                                .Conventions.Add<CustomForeignKeyConvention>()
                                                .Conventions.Add<CustomHasManyConvention>()
                                                .Conventions.Add<CustomHasManyToManyConvention>()
                                                .Conventions.Add<CustomReferenceConvention>()
                                                .Conventions.Add<CustomIndexManyToManyConvention>()
                                                .Conventions.Add<EnumConvention>()
                                          )
                                .BuildSessionFactory();

            assemblyToMap = null;

            SessionFactory = factory;

            return factory;
        }

        public void Dispose()
        {
            SessionFactory.Close();
            SessionFactory = null;
        }
    }
}
