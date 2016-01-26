using System;
using System.Web;

namespace Infra.NHibernate
{
    public class SessionManagerModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;
            context.Error += context_ErrorRequest;
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(VirtualPathUtility.GetExtension(HttpContext.Current.Request.FilePath))) return;

            SessionManager.OpenSession();
            SessionManager.BeginTransaction();
        }

        private void context_EndRequest(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(VirtualPathUtility.GetExtension(HttpContext.Current.Request.FilePath))) return;

            SessionManager.CommitTransaction();
            SessionManager.CloseSession();
        }

        private void context_ErrorRequest(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(VirtualPathUtility.GetExtension(HttpContext.Current.Request.FilePath))) return;
            SessionManager.RollbackTransaction();
            SessionManager.CloseSession();
        }

        public void Dispose()
        {
        }
    }
}