using System;
using System.Configuration;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Infra.NHibernate
{
    public class SessionManager : IDisposable
    {
        private const string SESSION_NAME = "CurrentSession";
        private const string FACTORY_NAME = "SessionFactory";

        private static bool IsWebContext
        {
            get { return HttpContext.Current != null; }
        }

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (IsWebContext)
                    return HttpContext.Current.Application[FACTORY_NAME] as ISessionFactory;

                return System.Threading.Thread.GetDomain().GetData(FACTORY_NAME) as ISessionFactory;
            }
            set
            {
                if (IsWebContext)
                {
                    if (HttpContext.Current.Application[FACTORY_NAME] != null)
                        HttpContext.Current.Application.Remove(FACTORY_NAME);

                    HttpContext.Current.Application.Add(FACTORY_NAME, value);
                }
                else
                {
                    System.Threading.Thread.GetDomain().SetData(FACTORY_NAME, value);
                }
            }
        }

        public static ISession OpenSession()
        {
            return OpenSession(true);
        }

        public static ISession OpenSession(bool openTransaction)
        {
            if (SessionFactory == null)
                SessionFactory = BuildSessionFactory();

            CurrentSession = SessionFactory.OpenSession();

            if (openTransaction)
            {
                CurrentSession.BeginTransaction();
            }

            return CurrentSession;
        }

        public static ISession CurrentSession
        {
            get
            {
                if (IsWebContext)
                    return HttpContext.Current.Items[SESSION_NAME] as ISession;

                return System.Threading.Thread.GetDomain().GetData(SESSION_NAME) as ISession;
            }

            set
            {
                if (IsWebContext)
                {
                    if (HttpContext.Current.Items[SESSION_NAME] != null)
                        HttpContext.Current.Items.Remove(SESSION_NAME);

                    HttpContext.Current.Items.Add(SESSION_NAME, value);
                    HttpContext.Current.Items[SESSION_NAME] = value;
                }
                else
                {
                    System.Threading.Thread.GetDomain().SetData(SESSION_NAME, value);
                }
            }
        }

        public static void CloseSessionFactory()
        {
            CloseSessionFactory(SessionFactory);
        }

        public static void CloseSessionFactory(ISessionFactory sessionFactory)
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

        public static void CloseSession()
        {
            CloseSession(CurrentSession);
        }

        public static void CloseSession(ISession currentSession)
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

        public static void CommitChanges()
        {
            CommitTransaction();
            BeginTransaction();
        }

        public static void BeginTransaction()
        {
            if (CurrentSession == null)
                OpenSession();
            else
            {
                CurrentSession.Transaction.Begin();
            }
        }

        public static void RollbackTransaction()
        {
            if (CurrentSession == null) return;

            if (CurrentSession.Transaction.IsActive)
            {
                CurrentSession.Transaction.Rollback();
            }
        }

        public static void RollbackChanges()
        {
            RollbackTransaction();
            BeginTransaction();
        }

        public static void CommitTransaction()
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
        
        public static ISessionFactory BuildSessionFactory()
        {
            var assemblyMappings = ConfigurationManager.AppSettings["AssemblyMappings"];

            if (assemblyMappings == null)
                throw new ArgumentException("Assembly Mappings não definido no Web/App Config!");

            var tipo = Type.GetType(assemblyMappings);
            if (tipo == null)
                throw new NullReferenceException(string.Format("Type '{0}' do assembly mappings não encontrado!", assemblyMappings));

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            var factory = Fluently.Configure()
                                .Database(MsSqlConfiguration.MsSql2008
                                                .ConnectionString(connectionString)
                                                .UseReflectionOptimizer()
                                                .ShowSql())
                                .Mappings(m => m.FluentMappings
                                                .AddFromAssembly(tipo.Assembly)
                                                .Conventions.Add<CustomPrimaryKeyConvention>()
                                                .Conventions.Add<CustomForeignKeyConvention>()
                                                .Conventions.Add<CustomHasManyConvention>()
                                                .Conventions.Add<CustomHasManyToManyConvention>()
                                                .Conventions.Add<CustomReferenceConvention>()
                                                .Conventions.Add<CustomIndexManyToManyConvention>()
                                                .Conventions.Add<EnumConvention>())
                                .BuildSessionFactory();

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
