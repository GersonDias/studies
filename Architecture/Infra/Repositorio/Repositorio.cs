using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominio;
using Infra.NHibernate;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;

namespace Repositorio
{
    public class Repositorio<T>
    {
        public ISession Session
        {
            get { return SessionManager.CurrentSession; }
        }

        public virtual IEnumerable<T> PesquisarProc(string procName)
        {
            var oBaileTodo = Session.CreateSQLQuery("exec " + procName)
                .SetResultTransformer(Transformers.AliasToBean<T>())
                .List<T>();

            return oBaileTodo;
        }

        public void Salvar(Pessoa pessoa)
        {
            Session.SaveOrUpdate(pessoa);
        }

        public IEnumerable<T> Pesquisar()
        {
            return Session.Query<T>().ToList();
        }
    }
}
