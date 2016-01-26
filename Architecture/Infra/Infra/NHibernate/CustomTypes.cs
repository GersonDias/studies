using NHibernate.SqlTypes;
using NHibernate.Type;

namespace Infra.NHibernate
{
    public class CharToBoolean : CharBooleanType
    {
        public CharToBoolean() : base(new AnsiStringFixedLengthSqlType(1)) { }

        protected override string FalseString
        {
            get { return "0"; }
        }

        protected override string TrueString
        {
            get { return "1"; }
        }
    }
}
