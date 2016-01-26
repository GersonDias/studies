using System;

namespace Infra.NHibernate
{
    public class ReferenceConstraintException : Exception
    {
        public ReferenceConstraintException(string message)
            : base(message)
        {
        }
    }
}
