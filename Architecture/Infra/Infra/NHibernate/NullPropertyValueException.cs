using System;
using NHibernate;

namespace Infra.NHibernate
{
    public class NullPropertyValueException : PropertyValueException
    {
        public override string Message
        {
            get
            {
                return String.Format("O campo '{0}' da tabela '{1}' não pode ser nulo.", this.PropertyName, this.EntityName);
            }
        }

        public NullPropertyValueException(string entityName, string propertyName)
            : base(null, entityName, propertyName)
        {
        }
    }
}
